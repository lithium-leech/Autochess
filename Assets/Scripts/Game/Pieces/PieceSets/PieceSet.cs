using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A set of pieces that can be offered to the player
/// </summary>
public abstract class PieceSet
{
    /// <summary>The pieces available in this set, ordered least to most valuable</summary>
    protected abstract IList<IList<AssetGroup.Piece>> Pieces { get; }

    /// <summary>Gets a pair of pieces to offer as a choice</summary>
    /// <returns>A random pair of pieces</returns>
    public PiecePair GetPieceChoice()
    {
        // Roll piece values for the player and enemy
        float strength = (((GameState.Level - 1) % GameState.MapRounds) + 1) / (float)GameState.MapRounds;
        int disparity = -2 + Mathf.FloorToInt(GameState.Level / GameState.MapRounds);
        int expected = Mathf.RoundToInt((Pieces.Count - 1) * strength);
        int playerRoll = Random.Range(0,Pieces.Count - 1) + Random.Range(0, Pieces.Count - 1) - disparity;
        int enemyRoll = Random.Range(0,Pieces.Count - 1) + Random.Range(0, Pieces.Count - 1) + disparity;
        int playerResult = ClampInt(expected + (playerRoll - (Pieces.Count - 1)), 0, Pieces.Count - 1);
        int enemyResult = ClampInt(expected + (enemyRoll - (Pieces.Count - 1)), 0, Pieces.Count - 1);

        // Return a pair of pieces with the rolled values
        AssetGroup.Piece player = Pieces[playerResult][Random.Range(0, Pieces[playerResult].Count)];
        AssetGroup.Piece enemy = Pieces[enemyResult][Random.Range(0, Pieces[enemyResult].Count)];
        return new PiecePair(player, enemy);
    }

    /// <summary>Gets a pair of pieces to start a new map with</summary>
    /// <returns>A pair of the least valuable pieces in the set</returns>
    public PiecePair GetStartingPieces()
    {
        IList<AssetGroup.Piece> weakestPieces = Pieces[0];
        AssetGroup.Piece player = weakestPieces[Random.Range(0, weakestPieces.Count)];
        AssetGroup.Piece enemy = weakestPieces[Random.Range(0, weakestPieces.Count)];
        return new PiecePair(player, enemy);
    }

    /// <summary>Get the value of a given kind of piece</summary>
    /// <param name="kind">The kind of piece to evaluate</param>
    /// <returns>An integer value</returns>
    public int GetPieceValue(AssetGroup.Piece kind)
    {
        for (int i = 0; i < Pieces.Count; i++)
        foreach (AssetGroup.Piece piece in Pieces[i])
            if (piece == kind) return i;
        throw new System.Exception($"Piece kind {kind} not found in set");
    }

    /// <summary>Clamps an integer value between two other given integers</summary>
    /// <param name="value">The value to clamp</param>
    /// <param name="min">The lower bound of the clamping range</param>
    /// <param name="max"><The upper bound of the clamping range/param>
    /// <returns>The given value now clamped</returns>
    private int ClampInt(int value, int min, int max)
    {
        if (value < min) return min;
        else if (value > max) return max;
        else return value;
    }
}
