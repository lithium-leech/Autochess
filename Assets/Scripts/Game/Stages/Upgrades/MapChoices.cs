using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Holds information about the choices a player has when they're
/// choosing a map and piece set for the next part of the game
/// </summary>
public class MapChoices : UpgradeChoices
{
    /// <summary>Creates a new set of map choices</summary>
    public MapChoices(Game game) : base(game)
    {
        Sets = GetRandomSets(NumberOfChoices);
        Maps = GetRandomMaps(NumberOfChoices);
    }

    /// <summary>Creates a new set of map choices</summary>
    public MapChoices(Game game, AssetGroup.Map map, AssetGroup.Set set) : base(game)
    {
        Sets = new AssetGroup.Set[1];
        Maps = new AssetGroup.Map[1];
        Sets[0] = set;
        Maps[0] = map;
    }

    public override string TitleText { get; } = "ChooseMap";

    /// <summary>The available maps</summary>
    private AssetGroup.Map[] Maps { get; }

    /// <summary>The available sets</summary>
    private AssetGroup.Set[] Sets { get; }

    protected override void ShowChoices()
    {
        for (int i = 0; i < NumberOfChoices; i++)
        {
            CreateSprite(i, true, false);
            CreateSprite(i, false, false);
        }
    }

    public override void ShowInfo(int choice)
    {
        // Erase previous info
        RemoveInfo();

        // Create new sprites
        GameObject map = CreateSprite(choice, true, true);
        GameObject set = CreateSprite(choice, false, true);

        // Change text
        Game.PlayerChoiceNameText.StringReference = new LocalizedString("MapNames", $"{set.name}");
        Game.PlayerChoiceInfoText.StringReference = new LocalizedString("MapInfo", $"{set.name}");
        Game.EnemyChoiceNameText.StringReference = new LocalizedString("SetNames", $"{map.name}");
        Game.EnemyChoiceInfoText.StringReference = new LocalizedString("SetInfo", $"{map.name}");
    }

    public override void ApplyChoice(int choice)
    {
        // Reset pieces
        Game.EnemyPieces.Clear();
        Game.PlayerGameBoard.Clear();
        Game.PlayerSideBoard.Clear();
            
        // Reset powers
        IList<Power> powers = new List<Power>();
        foreach (Power power in Game.EnemyPowers) powers.Add(power);
        foreach (Power power in Game.PlayerPowers) powers.Add(power);
        foreach (Power power in powers) power.Deactivate();

        // Remove the old map
        Game.GameBoard?.Destroy();

        // Log the chosen map and set
        AssetGroup.Map map = Maps[choice];
        AssetGroup.Set set = Sets[choice];
        Debug.Log($"Chosen Map: {map}");
        Debug.Log($"Chosen Set: {set}");

        // Add the new map
        BoardBuilder boardBuilder = map switch
        {
            AssetGroup.Map.Classic => new ClassicBoardBuilder(Game),
            AssetGroup.Map.Hourglass => new HourglassBoardBuilder(Game),
            AssetGroup.Map.ZigZag => new ZigZagBoardBuilder(Game),
            AssetGroup.Map.CenterCross => new CenterCrossBoardBuilder(Game),
            AssetGroup.Map.Small => new SmallBoardBuilder(Game),
            _ => throw new System.Exception("Map kind not recognized")
        };
        Game.GameBoard = boardBuilder.Build();

        // Add the new piece set
        PieceSet pieceSet = set switch
        {
            AssetGroup.Set.Western => new WesternPieceSet(),
            AssetGroup.Set.Military => new MilitaryPieceSet(),
            AssetGroup.Set.Tamerlane => new TamerlanePieceSet(),
            AssetGroup.Set.Shogi => new ShogiPieceSet(),
            AssetGroup.Set.Xiangqi => new XiangqiPieceSet(),
            _ => throw new System.Exception("Set kind not recognized")
        };
        Game.CurrentSet = pieceSet;

        // Add the starting pieces
        PiecePair starters = Game.CurrentSet.GetStartingPieces();
        Game.EnemyPieces.Add(starters.Enemy);
        Game.PlayerGameBoard.Add(new PiecePositionRecord(starters.Player, null));
    }

    /// <summary>Create a set or map sprite</summary>
    /// <param name="choice">The choice to create the sprite for</param>
    /// <param name="map">True if this is the map half of the choice</param>
    /// <param name="info">True if this is going to be shown in the info menu</param>
    /// <returns>A new sprite</returns>
    private GameObject CreateSprite(int choice, bool map, bool info)
    {
        Vector3 position = info ? InfoPosition(map, false, false) : ChoicePosition(choice, map, false);
        GameObject sprite = map ? Game.CreateMap(Maps[choice], position) : Game.CreateSet(Sets[choice], position);
        if (info) InfoSprites.Add(sprite);
        else Sprites.Add(sprite);
        return sprite;
    }

    /// <summary>Gets a collection of unique random maps</summary>
    /// /// <param name="n">The number of maps to get</param>
    /// <returns>A collection of AssetGroup.Map</returns>
    private AssetGroup.Map[] GetRandomMaps(int n)
    {
        AssetGroup.Map[] maps = new AssetGroup.Map[n];
        IList<AssetGroup.Map> mapOptions = System.Enum.GetValues(typeof(AssetGroup.Map)).Cast<AssetGroup.Map>().ToList();
        for (int i = 0; i < n; i++)
        {
            int r = UnityEngine.Random.Range(1, mapOptions.Count);
            AssetGroup.Map map = mapOptions[r];
            maps[i] = map;
            mapOptions.RemoveAt(r);
        }
        return maps;
    }

    /// <summary>Gets a collection of unique random sets</summary>
    /// <param name="n">The number of sets to get</param>
    /// <returns>A collection of AssetGroup.Set</returns>
    private AssetGroup.Set[] GetRandomSets(int n)
    {
        AssetGroup.Set[] sets = new AssetGroup.Set[n];
        IList<AssetGroup.Set> setOptions = System.Enum.GetValues(typeof(AssetGroup.Set)).Cast<AssetGroup.Set>().ToList();
        for (int i = 0; i < n; i++)
        {
            int r = UnityEngine.Random.Range(1, setOptions.Count);
            AssetGroup.Set set = setOptions[r];
            sets[i] = set;
            setOptions.RemoveAt(r);
        }
        return sets;
    }
}
