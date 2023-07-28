using UnityEngine;
/// <summary>
/// A type of terrain that explodes when touched
/// </summary>
public class Mine : Terrain
{
    public override AssetGroup.Object Kind => AssetGroup.Object.Mine;

    /// <summary>A piece that moves onto the mine becomes the victim</summary>
    private Piece Victim { get; set; }

    public override bool IsEnterable(ChessObject obj) => obj.IsPlayer != IsPlayer;

    public override void OnEnter(Piece piece)
    {
        Victim = piece;
        Game.OnMoveFinish.AddListener(Explode);
        string controller = piece.IsPlayer ? "Player" : "Enemy";
        Debug.Log($"{controller}{piece.Kind} activated a land mine at [{Space.X},{Space.Y}]");
    }

    /// <summary>The mine explodes and destroys the piece that moved onto it</summary>
    private void Explode()
    {
        Game.OnMoveFinish.RemoveListener(Explode);
        Victim.Destroy();
        Destroy();
    }
}
