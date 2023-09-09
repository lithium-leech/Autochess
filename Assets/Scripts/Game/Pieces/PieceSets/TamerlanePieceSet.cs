using System.Collections.Generic;

/// <summary>
/// A set containing the pieces from a variant of chess played
/// in the Tamerlane region
/// </summary>
public class TamerlanePieceSet : PieceSet
{    
    protected override IList<IList<AssetGroup.Piece>> Pieces { get; } = new List<IList<AssetGroup.Piece>>() {
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Pawn},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.King, AssetGroup.Piece.Ferz, AssetGroup.Piece.Wazir, AssetGroup.Piece.Elephant, AssetGroup.Piece.Dabbaba},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Knight, AssetGroup.Piece.Camel, AssetGroup.Piece.Swordsman},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Taliah},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Rook, AssetGroup.Piece.Giraffe},
    };
}
