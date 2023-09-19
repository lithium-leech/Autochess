/// <summary>
/// A class for menus that don't need extra behaviors
/// </summary>
public class NullMenu : Menu
{
    protected override void OnClose() { /*Do nothing*/ }

    protected override void OnOpen() { /*Do nothing*/ }
}
