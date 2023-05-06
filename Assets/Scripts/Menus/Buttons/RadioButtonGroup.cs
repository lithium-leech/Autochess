using UnityEngine;

/// <summary>
/// An object for connecting a group of RadioButtons
/// </summary>
public class RadioButtonGroup : MonoBehaviour
{
    /// Properties to set using Unity interface
    [SerializeField] private RadioButton[] Buttons;
    public int SelectedIndex = -1;

    private void Awake()
    {
        int index = -1;
        foreach (RadioButton button in Buttons)
        {
            index++;
            button.Group = this;
            button.Index = index;
        }
    }

    /// <summary>Selects one of the RadioButtons</summary>
    /// <param name="index">The index of the RadioButton to select</param>
    public void SelectButton(int index)
    {
        SelectedIndex = index;
        foreach (RadioButton button in Buttons)
        {
            if (button.Index == SelectedIndex)
                button.Select();
            else
                button.Deselect();
        }
    }
}
