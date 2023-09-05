/// <summary>
/// A non-choice option
/// </summary>
public class NullChoices : UpgradeChoices
{
    public NullChoices(Game game) : base(game) { }

    public override string TitleText => "NothingToChoose";

    public override void ApplyChoice(int choice) { }

    public override void ShowInfo(int choice) { }

    protected override void ShowChoices() { }
}
