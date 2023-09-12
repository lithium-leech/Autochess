/// <summary>
/// A key to attach to piece assets in order to load them properly
/// </summary>
public class PieceKey : AssetKey
{
    // Properties to set in unity interface
    public AssetGroup.Piece Identity;

    public override int ID { get => (int)Identity; set => Identity = (AssetGroup.Piece)value; }
}
