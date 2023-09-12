/// <summary>
/// A key to attach to set assets in order to load them properly
/// </summary>
public class SetKey : AssetKey
{
    // Properties to set in unity interface
    public AssetGroup.Set Identity;

    public override int ID { get => (int)Identity; set => Identity = (AssetGroup.Set)value; }
}
