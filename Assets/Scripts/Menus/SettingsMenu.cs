using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/// <summary>
/// Adds behaviors needed for changing game settings
/// </summary>
public class SettingsMenu : Menu
{
    /// Properties to set using Unity interface
    public Game Game;
    public TextMeshProUGUI VolumeValue;

    /// <summary>The available locale identifiers</summary>
    private IList<Locale> Locales { get; set; }

    /// <summary>The index of the currently selected locale</summary>
    private int LocaleIndex { get; set; }

    /// <summary>A sprite to show the starting map</summary>
    private GameObject MapSprite { get; set; } = null;

    /// <summary>A sprite to show the starting set</summary>
    private GameObject SetSprite { get; set; } = null;

    async void Start()
    {
        // Wait for localization settings to finish loading
        while (!LocalizationSettings.InitializationOperation.IsDone)
        {
            await Task.Yield();
        }

        // Load locale data
        Locales = LocalizationSettings.AvailableLocales.Locales;
        LocaleIndex = Locales.IndexOf(LocalizationSettings.SelectedLocale);

        // Load current settings
        UpdateVolume();
    }

    protected override void OnOpen()
    {
        UpdateBoard();
        UpdateSet();
    }

    protected override void OnClose()
    {
        Destroy(MapSprite);
        Destroy(SetSprite);
    }

    /// <summary>Decreases the volume</summary>
    public void VolumeDown()
    {
        GameState.MusicBox.Volume -= 0.1f;
        if (GameState.MusicBox.Volume < 0.0f) GameState.MusicBox.Volume = 0.0f;
        Debug.Log($"Lowered volume to {GameState.MusicBox.Volume:0.0}.");
        UpdateVolume();
    }

    /// <summary>Increases the volume</summary>
    public void VolumeUp()
    {
        GameState.MusicBox.Volume += 0.1f;
        if (GameState.MusicBox.Volume > 1.0f) GameState.MusicBox.Volume = 1.0f;
        Debug.Log($"Raised volume to {GameState.MusicBox.Volume:0.0}.");
        UpdateVolume();
    }

    /// <summary>Updates the volume value to the current volume</summary>
    private void UpdateVolume()
    {
        float tens = GameState.MusicBox.Volume * 10.0f;
        int rounded = Mathf.RoundToInt(tens);
        int percent = rounded * 10;
        VolumeValue.text = $"{percent}%";
    }

    /// <summary>Switches to the previous locale</summary>
    public void LanguageDown()
    {
        LocaleIndex--;
        if (LocaleIndex < 0) LocaleIndex = Locales.Count - 1;
        PlayerPrefs.SetString("PreferredLocale", Locales[LocaleIndex].Identifier.Code);
        LocalizationSettings.SelectedLocale = Locales[LocaleIndex];
        Debug.Log($"Changed locale to {LocalizationSettings.SelectedLocale.LocaleName}.");
    }

    /// <summary>Switches to the next locale</summary>
    public void LanguageUp()
    {
        LocaleIndex++;
        if (LocaleIndex >= Locales.Count) LocaleIndex = 0;
        PlayerPrefs.SetString("PreferredLocale", Locales[LocaleIndex].Identifier.Code);
        LocalizationSettings.SelectedLocale = Locales[LocaleIndex];
        Debug.Log($"Changed locale to {LocalizationSettings.SelectedLocale.LocaleName}.");
    }

    /// <summary>Switches to the previous board</summary>
    public void BoardDown()
    {
        GameState.StartMap--;
        if (GameState.StartMap <= AssetGroup.Map.None) GameState.StartMap = Enum.GetValues(typeof(AssetGroup.Map)).Cast<AssetGroup.Map>().Max();
        Debug.Log($"Changed starting board to {GameState.StartMap}.");
        UpdateBoard();
    }

    /// <summary>Switches to the next board</summary>
    public void BoardUp()
    {
        GameState.StartMap++;
        if (GameState.StartMap > Enum.GetValues(typeof(AssetGroup.Map)).Cast<AssetGroup.Map>().Max()) GameState.StartMap = AssetGroup.Map.Classic;
        Debug.Log($"Changed starting board to {GameState.StartMap}.");
        UpdateBoard();
    }

    /// <summary>Updates the board icon to the current board</summary>
    public void UpdateBoard()
    {
        if (MapSprite != null) Destroy(MapSprite);
        MapSprite = Game.CreateMap(GameState.StartMap, new Vector3(-2.0f, 3.75f, -62.0f));
    }

    /// <summary>Switches to the previous set</summary>
    public void SetDown()
    {
        GameState.StartSet--;
        if (GameState.StartSet <= AssetGroup.Set.None) GameState.StartSet = Enum.GetValues(typeof(AssetGroup.Set)).Cast<AssetGroup.Set>().Max();
        Debug.Log($"Changed starting set to {GameState.StartSet}.");
        UpdateSet();
    }

    /// <summary>Switches to the next set</summary>
    public void SetUp()
    {
        GameState.StartSet++;
        if (GameState.StartSet > Enum.GetValues(typeof(AssetGroup.Set)).Cast<AssetGroup.Set>().Max()) GameState.StartSet = AssetGroup.Set.Western;
        Debug.Log($"Changed starting set to {GameState.StartSet}.");
        UpdateSet();
    }

    /// <summary>Updates the set icon to the current set</summary>
    public void UpdateSet()
    {
        if (SetSprite != null) Destroy(SetSprite);
        SetSprite = Game.CreateSet(GameState.StartSet, new Vector3(2.0f, 3.75f, -62.0f));
    }

    /// <summary>Closes the settings menu</summary>
    public void Exit() => MenuManager.CloseOverlay();
}
