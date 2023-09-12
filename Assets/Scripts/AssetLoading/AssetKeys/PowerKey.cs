/// <summary>
/// A key to attach to power assets in order to load them properly
/// </summary>
public class PowerKey : AssetKey
{
    // Properties to set in unity interface
    public AssetGroup.Power Identity;

    public override int ID { get => (int)Identity; set => Identity = (AssetGroup.Power)value; }
}
