/// <summary>
/// A key to attach to scene assets in order to load them properly
/// </summary>
public class SceneKey : AssetKey
{
    // Properties to set in unity interface
    public AssetGroup.Scene Identity;

    public override int ID => (int)Identity;
}
