/// <summary>
/// A type of terrain that simply sits still an blocks a space
/// </summary>
public class Wall : Terrain
{
    public override AssetGroup.Object Kind => AssetGroup.Object.Wall;

    public override void Update() { }
    
    public override bool IsPlaceable(Space space) => space.InPlayerZone() || space.InNeutralZone();

    public override bool IsEnterable(ChessObject obj) => false;

    public override void OnEnter(Piece piece) { }

    public override void OnExit(Piece piece) { }
}
