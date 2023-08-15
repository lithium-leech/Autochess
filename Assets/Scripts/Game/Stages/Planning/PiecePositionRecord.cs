/// <summary>An object for recording the position of a piece</summary>
public class PiecePositionRecord
{
    /// <summary>Creates a new instance of a PiecePositionRecord</summary>
    /// <param name="kind">The kind of piece</param>
    /// <param name="space">The piece's space</param>
    public PiecePositionRecord(AssetGroup.Piece kind, Space space)
    {
        Kind = kind;
        Space = space;
    }

    /// <summary>The kind of piece</summary>
    public AssetGroup.Piece Kind;

    /// <summary>The piece's space</summary>
    public Space Space;
}
