public abstract class Piece
{
    public Board Board; // The board this piece is on
    public int X; // The piece's x coordinate on the board
    public int Y; // The piece's y coordinate on the board

    // The logic for how this piece moves on its turn
    public abstract void Move();

    // Destroys this piece
    public void Destroy()
    {
    
    }
}
