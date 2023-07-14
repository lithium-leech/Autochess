/// <summary>
/// A type of terrain that explodes when touched
/// </summary>
public class Mine : Terrain
{
    public override AssetGroup.Object Kind => AssetGroup.Object.Mine;

    public override bool IsEnterable(ChessObject obj) => obj.IsPlayer != IsPlayer;

    public override void OnEnter(Piece piece) { }

    public override void OnExit(Piece piece) { }
}
