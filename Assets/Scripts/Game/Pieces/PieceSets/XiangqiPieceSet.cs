using System.Collections.Generic;

/// <summary>
/// A set containing the pieces from a variant of chess played in China
/// </summary>
public class XiangqiPieceSet : PieceSet
{    
    protected override IList<IList<AssetGroup.Piece>> Pieces { get; } = new List<IList<AssetGroup.Piece>>() {
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Zu},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Jiang, AssetGroup.Piece.Shi},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Ma, AssetGroup.Piece.Xiang},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Pao},
        new List<AssetGroup.Piece>() {AssetGroup.Piece.Ju},
    };
}
