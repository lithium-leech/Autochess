using TMPro;
using UnityEngine.Localization;

/// <summary>Static container for fonts</summary>
public static class Fonts
{
    /// The font to use for each locale
    public static TMP_FontAsset ArabicFont;
    public static TMP_FontAsset ChineseFont;
    public static TMP_FontAsset EnglishFont;
    public static TMP_FontAsset FrenchFont;
    public static TMP_FontAsset GermanFont;
    public static TMP_FontAsset HindiFont;
    public static TMP_FontAsset IndonesianFont;
    public static TMP_FontAsset JapaneseFont;
    public static TMP_FontAsset KoreanFont;
    public static TMP_FontAsset PortugueseFont;
    public static TMP_FontAsset RussianFont;
    public static TMP_FontAsset SpanishFont;

    /// <summary>Gets the font for the given locale</summary>
    /// <param name="locale">The locale to retrieve a font for</param>
    /// <returns>A font</returns>
    public static TMP_FontAsset GetFont(Locale locale)
    {
        return locale.Identifier.Code switch
        {
            "ar" => ArabicFont,
            "zh" => ChineseFont,
            "en" => EnglishFont,
            "fr" => FrenchFont,
            "de" => GermanFont,
            "hi" => HindiFont,
            "id" => IndonesianFont,
            "ja" => JapaneseFont,
            "ko" => KoreanFont,
            "pt" => PortugueseFont,
            "ru" => RussianFont,
            "es" => SpanishFont,
            _ => EnglishFont,
        };
    }
}
