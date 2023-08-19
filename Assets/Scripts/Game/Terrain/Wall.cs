/// <summary>
/// A type of terrain that simply sits still an blocks a space
/// </summary>
public class Wall : Terrain
{
    public override AssetGroup.Object Kind => AssetGroup.Object.Wall;

    public override bool IsEnterable(ChessObject obj) => false;
}
