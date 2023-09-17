using System.Collections.Generic;

/// <summary>
/// A set containing the pieces from a variant of chess played in Japan
/// </summary>
public class ShogiPieceSet : PieceSet
{    
    protected override IList<IList<AssetGroup.Piece>> Pieces { get; } = new List<IList<AssetGroup.Piece>>() {
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Fuhyo},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Ginsho, AssetGroup.Piece.Keima},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Osho, AssetGroup.Piece.Kinsho, AssetGroup.Piece.Narigin, AssetGroup.Piece.Narikei, AssetGroup.Piece.Narikyo, AssetGroup.Piece.Tokin},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Kakugyo, AssetGroup.Piece.Ryuma},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Kyosha, AssetGroup.Piece.Hisha, AssetGroup.Piece.Ryuo},
    };
}
