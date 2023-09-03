/// <summary>
/// A key to attach to map assets in order to load them properly
/// </summary>
public class MapKey : AssetKey
{
    // Properties to set in unity interface
    public AssetGroup.Map Identity;

    public override int ID => (int)Identity;
}
