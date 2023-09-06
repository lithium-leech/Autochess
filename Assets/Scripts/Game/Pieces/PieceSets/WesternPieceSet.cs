using System.Collections.Generic;

/// <summary>
/// A set containing the standard pieces seen in the variant of chess
/// played throughout the "western" world
/// </summary>
public class WesternPieceSet : PieceSet
{    
    protected override IList<IList<AssetGroup.Piece>> Pieces { get; } = new List<IList<AssetGroup.Piece>>() {
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Pawn},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.King},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Knight},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Bishop},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Rook},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Queen}
    };
}
