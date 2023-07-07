/// <summary>
/// A key to attach to object assets in order to load them properly
/// </summary>
public class ObjectKey : AssetKey
{
    // Properties to set in unity interface
    public AssetGroup.Object Identity;

    public override int ID => (int)Identity;
}
