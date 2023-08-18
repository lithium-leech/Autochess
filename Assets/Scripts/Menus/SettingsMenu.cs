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

    /// <summary>The available locale identifiers</summary>
    private IList<Locale> Locales { get; set; }

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

    /// <summary>Closes the settings menu</summary>
    public void Close() => MenuManager.CloseOverlay();
}
