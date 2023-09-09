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

    /// <summary>A sequence of coordinates to move through</summary>
    private IList<Vector2Int> Path { get; set; }

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
                transform.position = Board.ToPosition(Path.Last()) + GameState.MovingPieceZ;
            }
            else
            {
                int segment = Mathf.FloorToInt(LerpIncrement * (Path.Count - 1));
                float segmentIncrement = (LerpIncrement * (Path.Count - 1)) - segment;
                transform.position = Vector3.Lerp(Board.ToPosition(Path[segment]) + GameState.MovingPieceZ, Board.ToPosition(Path[segment+1]) + GameState.MovingPieceZ, segmentIncrement);
            }
            if (Vector3.Distance(transform.position, Board.ToPosition(Path.Last()) + GameState.MovingPieceZ) < 0.001f) IsMoving = false;
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

    /// <summary>Checks if the given path is a valid path this piece can traverse</summary>
    /// <param name="path">The path to check</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <returns>True if the path is traversable</returns>
    protected bool IsValidPath(IList<Vector2Int> path, bool jump)
    {
        // Check that the path starts from the current location
        if (path.Count < 1) return false;
        if (path[0] != Space.Coordinates) return false;

        // Check that the rest of the path is traversable
        for (int i = 1; i < path.Count; i++)
        {
            if (!Board.OnBoard(path[i])) return false;
            Space space = Board.GetSpace(path[i]);
            if (!jump && space == null) return false;
            if (jump && space == null) continue;
            if (!jump && !space.IsEnterable(this)) return false;
            if (jump && !space.IsEnterable(this)) continue;
        }

        // The path is valid if nothing proved it invalid
        return true;
    }

    /// <summary>Gets possible move and capture paths in the specified direction</summary>
    /// <param name="start">The starting location to find paths from</param>
    /// <param name="xi">x increment</param>
    /// <param name="yi">y increment</param>
    /// <param name="steps">The number of times to increment</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <param name="moves">A list of possible move paths to add to</param>
    /// <param name="captures">A list of possible capture paths to add to</param>
    protected void AddLinearPaths(IList<Vector2Int> start, int xi, int yi, int steps, bool jump, IList<IList<Vector2Int>> moves, IList<IList<Vector2Int>> captures)
    {
        IList<Vector2Int> path = new List<Vector2Int>(start);
        Vector2Int pointer = path.Last();
        for (int i = 0; i < steps; i++)
        {
            // Take one step using the given increment
            pointer.x += xi;
            pointer.y += yi;
            if (!Board.OnBoard(pointer)) break;
            path.Add(pointer);

            // Determine possible paths for this step
            Space space = Board.GetSpace(pointer);
            if (!jump && space == null) break;
            if (jump && space == null) continue;
            if (space.HasCapturable(this)) { captures.Add(new List<Vector2Int>(path)); }
            if (!jump && !space.IsEnterable(this)) break;
            if (jump && !space.IsEnterable(this)) continue;
            moves.Add(new List<Vector2Int>(path));
        }
    }

    /// <summary>Gets possible horizontal paths</summary>
    /// <param name="start">The starting location to find paths from</param>
    /// <param name="i">The size of each step</param>
    /// <param name="steps">The number of steps to take</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <param name="moves">A list of possible move paths to add to</param>
    /// <param name="captures">A list of possible capture paths to add to</param>
    protected void AddHorizontalPaths(IList<Vector2Int> start, int i, int steps, bool jump, IList<IList<Vector2Int>> moves, IList<IList<Vector2Int>> captures)
    {
        AddLinearPaths(start, i, 0, steps, jump, moves, captures);
        AddLinearPaths(start, -i, 0, steps, jump, moves, captures);
    }

    /// <summary>Gets possible vertical paths</summary>
    /// <param name="start">The starting location to find paths from</param>
    /// <param name="i">The size of each step</param>
    /// <param name="steps">The number of steps to take</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <param name="moves">A list of possible move paths to add to</param>
    /// <param name="captures">A list of possible capture paths to add to</param>
    protected void AddVerticalPaths(IList<Vector2Int> start, int i, int steps, bool jump, IList<IList<Vector2Int>> moves, IList<IList<Vector2Int>> captures)
    {
        AddLinearPaths(start, 0, i, steps, jump, moves, captures);
        AddLinearPaths(start, 0, -i, steps, jump, moves, captures);
    }

    /// <summary>Gets possible vertical and horizontal paths</summary>
    /// <param name="start">The starting location to find paths from</param>
    /// <param name="i">The size of each step</param>
    /// <param name="steps">The number of steps to take</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <param name="moves">A list of possible move paths to add to</param>
    /// <param name="captures">A list of possible capture paths to add to</param>
    protected void AddOrthogonalPaths(IList<Vector2Int> start, int i, int steps, bool jump, IList<IList<Vector2Int>> moves, IList<IList<Vector2Int>> captures)
    {
        AddVerticalPaths(start, i, steps, jump, moves, captures);
        AddHorizontalPaths(start, i, steps, jump, moves, captures);
    }

    /// <summary>Gets possible diagonal paths</summary>
    /// <param name="start">The starting location to find paths from</param>
    /// <param name="i">The size of each step</param>
    /// <param name="steps">The number of steps to take</param>
    /// <param name="jump">True if obstacles can be jumped over</param>
    /// <param name="moves">A list of possible move paths to add to</param>
    /// <param name="captures">A list of possible capture paths to add to</param>
    protected void AddDiagonalPaths(IList<Vector2Int> start, int i, int steps, bool jump, IList<IList<Vector2Int>> moves, IList<IList<Vector2Int>> captures)
    {
        AddLinearPaths(start, i, i, steps, jump, moves, captures);
        AddLinearPaths(start, i, -i, steps, jump, moves, captures);
        AddLinearPaths(start, -i, -i, steps, jump, moves, captures);
        AddLinearPaths(start, -i, i, steps, jump, moves, captures);
    }

    /// <summary>The piece carries out its turn</summary>
    /// <param name="path">The path to move through</param>
    protected void EnactTurn(IList<Vector2Int> path)
    {
        Path = path;
        Space destination = Board.GetSpace(path.Last());
        if (destination != null && destination != Space)
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
