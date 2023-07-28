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
            if (Space != null) Space.RemoveObject(this);
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

    /// <summary>Searches for possible choices using the given direction increments</summary>
    /// <param name="xi">x increment</param>
    /// <param name="yi">y increment</param>
    /// <param name="range">The range a piece can move</param>
    /// <param name="possibleMoves">A list of possible moves to add to</param>
    /// <param name="possibleCaptures">A list of possible captures to add to</param>
    protected void GetChoicesInDirection(int xi, int yi, int range, IList<IList<Space>> possibleMoves, IList<IList<Space>> possibleCaptures)
    {
        IList<Space> path = new List<Space>() { Space };
        Vector2Int pointer = Space.Coordinates;
        for (int i = 0; i < range; i++)
        {
            pointer.x += xi;
            pointer.y += yi;
            if (!Board.OnBoard(pointer)) break;
            Space space = Board.GetSpace(pointer);
            path.Add(space);
            if (space.HasCapturable(this)) { possibleCaptures.Add(new List<Space>(path)); break; }
            if (!space.IsEnterable(this)) break;
            possibleMoves.Add(new List<Space>(path));
        }
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
