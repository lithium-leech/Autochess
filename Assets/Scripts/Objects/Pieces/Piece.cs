using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A single game piece
/// </summary>
public abstract class Piece : ChessObject
{
    /// <summary>A piece to transform into</summary>
    public AssetGroup.Piece Transform { get; set; } = AssetGroup.Piece.None;

    /// <summary>The kind of piece this is</summary>
    public abstract AssetGroup.Piece Kind { get; }

    /// <summary>A captured piece waiting to be destroyed</summary>
    public Piece Captured { get; set; }

    /// <summary>True if the object is moving towards a target location</summary>
    public bool IsMoving { get; private set; } = false;

    /// <summary>True in the frame immediately following a finished move</summary>
    private bool IsDoneMoving { get; set; } = false;

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
                transform.position = Path.Last().Position + GameState.PieceZTop;
            }
            else
            {
                int segment = Mathf.FloorToInt(LerpIncrement * (Path.Count - 1));
                float segmentIncrement = (LerpIncrement * (Path.Count - 1)) - segment;
                transform.position = Vector3.Lerp(Path[segment].Position + GameState.PieceZTop, Path[segment+1].Position + GameState.PieceZTop, segmentIncrement);
            }
            if (Vector3.Distance(transform.position, Path.Last().Position + GameState.PieceZTop) < 0.001f) IsDoneMoving = true;
        }

        // Finish moving once the move is done
        if (IsDoneMoving)
        {
            IsMoving = false;
            IsDoneMoving = false;

            // Destroy the captured piece
            if (Captured != null) Captured.Destroy();

            // Move back to the stationary layer
            transform.position = Board.ToPosition(Space.Coordinates) + GameState.PieceZBottom;

            // Transform if possible
            if (Transform != AssetGroup.Piece.None)
            {
                Space.Exit(this);
                Board.AddPiece(Transform, IsPlayer, IsWhite, Space);
                Destroy();
            }
        }
    }

    public override bool IsPlaceable(Space space) => space.InPlayerZone();

    public override void Destroy()
    {
        if (Board != null)
        {
            Space.RemoveObject(this);
            if (IsPlayer) Board.PlayerPieces.Remove(this);
            else Board.EnemyPieces.Remove(this);
        }
        GameObject.Destroy(gameObject);
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
            if (space.HasEnemy(IsPlayer)) { possibleCaptures.Add(new List<Space>(path)); break; }
            if (!space.IsEnterable(this)) break;
            possibleMoves.Add(new List<Space>(path));
        }
    }

    /// <summary>The piece carries out its turn</summary>
    /// <param name="path">The path to move through</param>
    public void EnactTurn(IList<Space> path)
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
    }
}
