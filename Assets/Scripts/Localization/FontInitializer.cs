using TMPro;
using UnityEngine;

/// <summary>Loads the desired fonts into static fields to be accessed across a scene</summary>
public class FontInitializer : MonoBehaviour
{
    /// Properties to set using Unity interface
    public TMP_FontAsset ArabicFont;
    public TMP_FontAsset ChineseFont;
    public TMP_FontAsset EnglishFont;
    public TMP_FontAsset FrenchFont;
    public TMP_FontAsset GermanFont;
    public TMP_FontAsset HindiFont;
    public TMP_FontAsset IndonesianFont;
    public TMP_FontAsset JapaneseFont;
    public TMP_FontAsset KoreanFont;
    public TMP_FontAsset PortugueseFont;
    public TMP_FontAsset RussianFont;
    public TMP_FontAsset SpanishFont;

    void Awake()
    {
        Fonts.ArabicFont = ArabicFont;
        Fonts.ChineseFont = ChineseFont;
        Fonts.EnglishFont = EnglishFont;
        Fonts.FrenchFont = FrenchFont;
        Fonts.GermanFont = GermanFont;
        Fonts.HindiFont = HindiFont;
        Fonts.IndonesianFont = IndonesianFont;
        Fonts.JapaneseFont = JapaneseFont;
        Fonts.KoreanFont = KoreanFont;
        Fonts.PortugueseFont = PortugueseFont;
        Fonts.RussianFont = RussianFont;
        Fonts.SpanishFont = SpanishFont;
    }
}
