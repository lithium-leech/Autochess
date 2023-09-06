using System.Collections.Generic;

/// <summary>
/// A set containing custom pieces based on U.S. military ranks
/// </summary>
public class MilitaryPieceSet : PieceSet
{    
    protected override IList<IList<AssetGroup.Piece>> Pieces { get; } = new List<IList<AssetGroup.Piece>>() {
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Private},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.General},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Sergeant},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Lieutenant},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Captain},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Colonel}
    };
}
