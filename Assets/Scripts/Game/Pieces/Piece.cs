using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A single game piece
/// </summary>
public abstract class Piece : ChessObject
{
    /// <summary>The kind of piece this is</summary>
    public abstract AssetGroup.Piece Kind { get; }

    /// <summary>A piece to transform into</summary>
    public AssetGroup.Piece Transform { get; set; } = AssetGroup.Piece.None;

    /// <summary>The equipment this piece is wearing</summary>
    public Equipment Equipment { get; set; }

    /// <summary>A captured piece waiting to be destroyed</summary>
    public Piece Captured { get; set; }

    /// <summary>True if the piece is moving towards a target location</summary>
    public bool IsMoving { get; private set; } = false;

    /// <summary>A sequence of spaces to move through</summary>
    private IList<Space> Path { get; set; }

    /// <summary>The incrementor used for lerp movement</summary>
    private float LerpIncrement { get; set; } = 0;

    /// <summary>Takes the piece's turn</summary>
    public abstract void TakeTurn();

    public override void Update()
    {
        // Move towards the target when moving is activated
        if (IsMoving)
        {
            LerpIncrement = Mathf.Clamp01(LerpIncrement + (Time.deltaTime * 2.0f / GameState.TurnPause));
            if (LerpIncrement == 1.0f)
            {
                transform.position = Path.Last().Position + GameState.MovingPieceZ;
            }
            else
            {
                int segment = Mathf.FloorToInt(LerpIncrement * (Path.Count - 1));
                float segmentIncrement = (LerpIncrement * (Path.Count - 1)) - segment;
                transform.position = Vector3.Lerp(Path[segment].Position + GameState.MovingPieceZ, Path[segment+1].Position + GameState.MovingPieceZ, segmentIncrement);
            }
            if (Vector3.Distance(transform.position, Path.Last().Position + GameState.MovingPieceZ) < 0.001f) IsMoving = false;
        }
    }

    public override void Destroy()
    {
        if (Board != null)
        {
            Space?.RemoveObject(this);
            if (IsPlayer) Board.PlayerPieces.Remove(this);
            else Board.EnemyPieces.Remove(this);
        }
        if (Equipment != null) Equipment.Destroy();
        GameObject.Destroy(gameObject);
    }

    /// <summary>Checks if this can be captured by a given piece</summary>
    /// <param name="piece">The piece attempting to capture this</param>
    /// <returns>True if this can be captured by the given piece</returns>
    public bool IsCapturable(Piece piece)
    {
        if (piece.IsPlayer == IsPlayer) return false;
        if (Equipment != null && Equipment.IsProtected(piece)) return false;
        return true;
    }

    /// <summary>Gets possible move and capture paths in the specified direction</summary>
    /// <param name="xi">x increment</param>
    /// <param name="yi">y increment</param>
    /// <param name="steps">The number of times to increment</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <param name="moves">A list of possible move paths to add to</param>
    /// <param name="captures">A list of possible capture paths to add to</param>
    protected void AddPaths(int xi, int yi, int steps, bool jump, IList<IList<Space>> moves, IList<IList<Space>> captures)
    {
        // Create a new path to build
        IList<Space> path = new List<Space>() { Space };
        Vector2Int pointer = Space.Coordinates;
        for (int i = 0; i < steps; i++)
        {
            // Take one step using the given increment
            Space space = Space;
            int xj = Math.Abs(xi);
            int yj = Math.Abs(yi);
            while (xj > 0 || yj > 0)
            {
                if (xj > yj) { xj--; pointer.x += Math.Abs(xi) / xi; } 
                else if (yj > xj) { yj--; pointer.y += Math.Abs(yi) / yi; }
                else { xj--; yj--; pointer.x += Math.Abs(xi) / xi; pointer.y += Math.Abs(yi) / yi; }
                if (!Board.OnBoard(pointer)) break;
                else space = Board.GetSpace(pointer);
                if (space != null) path.Add(space);
                if (!jump && space == null) break;
                if (!jump && !space.IsEnterable(this)) break;
            }

            // There are no more paths if the step couldn't be finished
            if (xj > 0 || yj > 0) break;

            // Determine possible paths for this step
            if (!Board.OnBoard(pointer)) break;
            if (!jump && space == null) break;
            if (jump && space == null) continue;
            if (space.HasCapturable(this)) { captures.Add(new List<Space>(path)); break; }
            if (!jump && !space.IsEnterable(this)) break;
            if (jump && !space.IsEnterable(this)) continue;
            moves.Add(new List<Space>(path));
        }
    }

    /// <summary>Gets possible vertical and horizontal paths</summary>
    /// <param name="i">The size of each step</param>
    /// <param name="steps">The number of steps to take</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <param name="moves">A list of possible move paths to add to</param>
    /// <param name="captures">A list of possible capture paths to add to</param>
    protected void AddRookPaths(int i, int steps, bool jump, IList<IList<Space>> moves, IList<IList<Space>> captures)
    {
        AddPaths(i, 0, steps, jump, moves, captures);
        AddPaths(0, -i, steps, jump, moves, captures);
        AddPaths(-i, 0, steps, jump, moves, captures);
        AddPaths(0, i, steps, jump, moves, captures);
    }

    /// <summary>Gets possible diagonal paths</summary>
    /// <param name="i">The size of each step</param>
    /// <param name="steps">The number of steps to take</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <param name="moves">A list of possible move paths to add to</param>
    /// <param name="captures">A list of possible capture paths to add to</param>
    protected void AddBishopPaths(int i, int steps, bool jump, IList<IList<Space>> moves, IList<IList<Space>> captures)
    {
        AddPaths(i, i, steps, jump, moves, captures);
        AddPaths(i, -i, steps, jump, moves, captures);
        AddPaths(-i, -i, steps, jump, moves, captures);
        AddPaths(-i, i, steps, jump, moves, captures);
    }

    /// <summary>Gets possible L-shaped paths</summary>
    /// <param name="x">The length of one L-shaped step</param>
    /// <param name="y">The height of one L-shaped step</param>
    /// <param name="steps">The number of steps to take</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <param name="moves">A list of possible move paths to add to</param>
    /// <param name="captures">A list of possible capture paths to add to</param>
    protected void AddKnightPaths(int x, int y, int steps, bool jump, IList<IList<Space>> moves, IList<IList<Space>> captures)
    {
        AddPaths(y, x, steps, jump, moves, captures);
        AddPaths(y, -x, steps, jump, moves, captures);
        AddPaths(x, -y, steps, jump, moves, captures);
        AddPaths(-x, -y, steps, jump, moves, captures);
        AddPaths(-y, -x, steps, jump, moves, captures);
        AddPaths(-y, x, steps, jump, moves, captures);
        AddPaths(-x, y, steps, jump, moves, captures);
        AddPaths(x, y, steps, jump, moves, captures);
    }

    /// <summary>The piece carries out its turn</summary>
    /// <param name="path">The path to move through</param>
    protected void EnactTurn(IList<Space> path)
    {
        Path = path;
        Space destination = path.Last();
        if (destination != Space)
        {
            GameState.IsActiveRound = true;
            IsMoving = true;
            LerpIncrement = 0;
            Space.Exit(this);
            destination.Enter(this);
        }
        Game.OnMoveFinish.AddListener(FinishMoving);
    }

    /// <summary>Carries out final actions once this piece is done moving</summary>
    private void FinishMoving()
    {
        // Destroy the captured piece
        if (Captured != null) Captured.Destroy();

        // Move back to the stationary layer
        transform.position = Board.ToPosition(Space.Coordinates) + GameState.StillPieceZ;

        // Transform if possible
        if (Transform != AssetGroup.Piece.None)
        {
            Space.Exit(this);
            Board.AddPiece(Transform, IsPlayer, IsWhite, Space);
            Destroy();
        }

        // Remove the listener for this move
        Game.OnMoveFinish.RemoveListener(FinishMoving);
    }
}
