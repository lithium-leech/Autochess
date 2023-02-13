/// <summary>
/// Represents one stage of gameplay
/// </summary>
public interface IStage
{
    /// <summary>Runs once when the stage starts</summary>
    public void Start();

    /// <summary>Runs repeadetley while the player is in this stage</summary>
    public void During();

    /// <summary>Runs once when the stage ends</summary>
    public void End();
}
