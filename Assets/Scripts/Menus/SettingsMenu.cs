using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/// <summary>
/// Adds behaviors needed for changing game settings
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    /// Properties to set using Unity interface
    public Game Game;
    public TextMeshProUGUI VolumeValue;
    public TextMeshProUGUI SpeedValue;

    /// <summary>The available locale identifiers</summary>
    private IList<Locale> Locales { get; set; }

    /// <summary>The available game speeds</summary>
    private float[] Speeds { get; } = new float[3] { 1.0f, 2.0f, 4.0f };
    
    /// <summary>The index of the speed currently being used</summary>
    private int SpeedIndex { get; set; } = 0;

    /// <summary>The index of the currently selected locale</summary>
    private int LocaleIndex;

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

    /// <summary>Decreases the volume</summary>
    public void VolumeDown()
    {
        GameState.MusicBox.Volume -= 0.1f;
        UpdateVolume();
    }

    /// <summary>Increases the volume</summary>
    public void VolumeUp()
    {
        GameState.MusicBox.Volume += 0.1f;
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
    }

    /// <summary>Switches to the next locale</summary>
    public void LanguageUp()
    {
        LocaleIndex++;
        if (LocaleIndex >= Locales.Count) LocaleIndex = 0;
        PlayerPrefs.SetString("PreferredLocale", Locales[LocaleIndex].Identifier.Code);
        LocalizationSettings.SelectedLocale = Locales[LocaleIndex];
    }

    /// <summary>Decreases the game speed</summary>
    public void SpeedDown()
    {
        SpeedIndex--;
        if (SpeedIndex < 0) SpeedIndex = 0;
        UpdateSpeed();
    }

    /// <summary>Increases the game speed</summary>
    public void SpeedUp()
    {
        SpeedIndex++;
        if (SpeedIndex >= Speeds.Length) SpeedIndex = Speeds.Length - 1;
        UpdateSpeed();
    }

    /// <summary>Updates game speed to the speed value</summary>
    private void UpdateSpeed()
    {
        GameState.TurnPause = 2.0f / Speeds[SpeedIndex];
        SpeedValue.text = $"{Speeds[SpeedIndex]}x";
    }

    /// <summary>Concedes the stage, taking the player straight to defeat</summary>
    public void Concede()
    {
        gameObject.SetActive(false);
        Game.NextStage = new DefeatStage(Game);
    }

    /// <summary>Closes the settings menu</summary>
    public void Close() => gameObject.SetActive(false);
}
