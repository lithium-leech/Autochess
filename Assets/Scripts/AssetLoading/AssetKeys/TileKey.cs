/// <summary>
/// A key to attach to tile assets in order to load them properly
/// </summary>
public class TileKey : AssetKey
{
    // Properties to set in unity interface
    public AssetGroup.Tile Identity;

    public override int ID => (int)Identity;
}
