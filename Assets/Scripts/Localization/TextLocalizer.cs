using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using TMPro;
using UnityEngine.Localization.Components;

/// <summary>
/// A behavior to attach to text objects, which makes them change fonts with the locale
/// </summary>
public class TextLocalizer : MonoBehaviour
{
    /// <summary>Adds an extra layer of localization to the target text</summary>
    public string Text
    {
        get
        {
            return GetComponent<TextMeshProUGUI>().text;
        }
        set
        {
            if (LocalizationSettings.SelectedLocale.Identifier.Code == "ar")
                GetComponent<TextMeshProUGUI>().text = ArabicSupport.ArabicFixer.Fix(value);
            else
                GetComponent<TextMeshProUGUI>().text = value;
        }
    }

    /// <summary>Makes the string reference accessible from this class</summary>
    public LocalizedString StringReference
    {
        get
        {
            return GetComponent<LocalizeStringEvent>().StringReference;
        }
        set
        {
            GetComponent<LocalizeStringEvent>().StringReference= value;
        }
    }

    private void Awake() => LocalizationSettings.SelectedLocaleChanged += SelectFont;

    private void Start() => SelectFont(LocalizationSettings.SelectedLocale);

    private void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= SelectFont;

    /// <summary>Changes the font to something appropriate for the given locale</summary>
    /// <param name="locale">The locale to select a font for</param>
    private void SelectFont(Locale locale) => GetComponent<TextMeshProUGUI>().font = Fonts.GetFont(locale);
}
