/// <summary>
/// A power which allows a land mine to be placed on the map
/// </summary>
public class Mine : Power
{
    public override AssetGroup.Power Kind { get { return AssetGroup.Power.Mine; } }

    public override void Activate()
    {
        // TODO:
    }
}
