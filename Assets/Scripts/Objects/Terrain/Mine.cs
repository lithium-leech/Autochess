/// <summary>
/// A type of terrain that explodes when touched
/// </summary>
public class Mine : Terrain
{
    public override AssetGroup.Object Kind => AssetGroup.Object.Mine;

    /// <summary>A piece that moves onto the mine becomes the victim</summary>
    private Piece Victim { get; set; }

    public override void Update()
    {
        if (Victim != null)
        {
            // Destroy the victim once they're done moving
            if (!Victim.IsMoving)
            {
                Victim.Destroy();
                Destroy();
            }
        }
    }

    public override bool IsEnterable(ChessObject obj) => obj.IsPlayer != IsPlayer;

    public override void OnEnter(Piece piece) => Victim = piece;
}
