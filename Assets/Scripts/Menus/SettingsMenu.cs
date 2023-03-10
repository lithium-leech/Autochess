using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    /// Properties to set using Unity interface
    public AudioSource[] Music;
    public TextMeshProUGUI VolumeValue;
    public Button VolumeLeftButton;
    public Button VolumeRightButton;
    public Button LanguageLeftButton;
    public Button LanguageRightButton;
    public Button CloseButton;

    /// <summary>The available locale identifiers</summary>
    private IList<Locale> Locales;

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

        // Add button handlers
        VolumeLeftButton.onClick.AddListener(VolumeDown);
        VolumeRightButton.onClick.AddListener(VolumeUp);
        LanguageLeftButton.onClick.AddListener(LanguageDown);
        LanguageRightButton.onClick.AddListener(LanguageUp);
        CloseButton.onClick.AddListener(Close);
    }

    /// <summary>Decreases the volume</summary>
    public void VolumeDown()
    {
        foreach (AudioSource source in Music)
            source.volume -= 0.1f;
        UpdateVolume();
    }

    /// <summary>Increases the volume</summary>
    public void VolumeUp()
    {
        foreach (AudioSource source in Music)
            source.volume += 0.1f;
        UpdateVolume();
    }

    /// <summary>Updates the volume value to the current volume</summary>
    private void UpdateVolume()
    {
        float tens = Music[0].volume * 10.0f;
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

    /// <summary>Closes the settings menu</summary>
    public void Close() => gameObject.SetActive(false);
}
