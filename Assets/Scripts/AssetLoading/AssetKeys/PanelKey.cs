/// <summary>
/// A key to attach to panel assets in order to load them properly
/// </summary>
public class PanelKey : AssetKey
{
    // Properties to set in unity interface
    public AssetGroup.Panel Identity;

    public override int ID => (int)Identity;
}
