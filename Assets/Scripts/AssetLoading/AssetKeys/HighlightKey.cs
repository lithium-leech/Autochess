/// <summary>
/// A key to attach to highlight assets in order to load them properly
/// </summary>
public class HighlightKey : AssetKey
{
    // Properties to set in unity interface
    public AssetGroup.Highlight Identity;

    public override int ID { get => (int)Identity; set => Identity = (AssetGroup.Highlight)value; }
}
