/// <summary>
/// The equipped piece cannot be captured during the first three turns
/// </summary>
public class Shield : Equipment
{
    private int TurnsOfProtection = 3;
    private bool TurnStarted = false;

    public override void Update()
    {
        if (TurnsOfProtection > 0 && Piece != null) 
        {
            if (!TurnStarted && Piece.IsMoving)
            {
                TurnStarted = true;
            }
            else if (TurnStarted && !Piece.IsMoving)
            {
                TurnStarted = false;
                TurnsOfProtection--;
            }
        }
    }

    public override bool IsProtected(Piece piece) => TurnsOfProtection > 0;
}
 