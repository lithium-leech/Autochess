/// <summary>
/// The equipped piece cannot be captured during the first three turns
/// </summary>
public class Shield : Equipment
{
    /// <summary>The number of remaining turns that the shield is effective</summary>
    private int RoundsOfProtection = 6;

    public override void Initialize(Game game, bool player, bool white)
    {
        base.Initialize(game, player, white);
        Game.OnRoundFinish.AddListener(LowerProtection);
    }

    public override void Destroy()
    {
        Game.OnRoundFinish.RemoveListener(LowerProtection);
        base.Destroy();
    }

    public override bool IsProtected(Piece piece) => true;

    /// <summary>Lowers the remaining rounds of protection by one</summary>
    private void LowerProtection()
    {
        if (RoundsOfProtection > 0)
        {
            RoundsOfProtection--;
            if (RoundsOfProtection < 1) Destroy();
        }
    }
}
 