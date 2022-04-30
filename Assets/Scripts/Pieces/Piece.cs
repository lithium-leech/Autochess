/// <summary>
/// A single game piece
/// </summary>
public abstract class Piece
{
    /// <summary>The board this piece is on</summary>
    public Board Board { get; set; }

    /// <summary>The piece's x-coordinate on the board</summary>
    public int X { get; set; }

    /// <summary>The piece's y-coordinate on the board</summary>
    public int Y { get; set; }

    /// <summary>Enacts this piece's move for a single turn</summary>
    public abstract void Move();

    /// <summary>Destroys this piece</summary>
    public void Destroy()
    {
    
    }
}
