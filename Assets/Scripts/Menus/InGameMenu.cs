using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    /// Properties to set using Unity interface
    public Game Game;
    public Image Image;
    public Sprite[] SpeedSprites;

    /// <summary>The available game speeds</summary>
    private float[] Speeds { get; } = new float[3] { 2.0f, 1.0f, 0.5f };
    
    /// <summary>The index of the speed currently being used</summary>
    private int SpeedIndex { get; set; } = 0;

    public void Awake() => Image.sprite = SpeedSprites[0];

    /// <summary>Increases the game speed</summary>
    public void ToggleSpeed()
    {
        SpeedIndex++;
        if (SpeedIndex >= Speeds.Length) SpeedIndex = 0;
        Debug.Log($"Changed speed to {2.0f/Speeds[SpeedIndex]}x.");
        GameState.TurnPause = Speeds[SpeedIndex];
        Image.sprite = SpeedSprites[SpeedIndex];
    }
}
