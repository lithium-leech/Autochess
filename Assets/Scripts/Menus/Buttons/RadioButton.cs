using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A button for selecting one option out of a group
/// </summary>
public class RadioButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    /// Properties to set using Unity interface
    [SerializeField] private Image Image;
    [SerializeField] private Sprite HighlightedSprite;
    [SerializeField] private Sprite PressedSprite;
    [SerializeField] private Sprite SelectedSprite;

    /// <summary>The group that this RadioButton belongs to</summary>
    /// <remarks>Should only be set by the group object</remarks>
    public RadioButtonGroup Group { get; set; }

    /// <summary>The index of this button within its group</summary>
    /// <remarks>Should only be set by the group object</remarks>
    public int Index { get; set; }
    
    /// <summary>A property to hold the sprite originally attached to the DefaultImage</summary>
    private Sprite DefaultSprite { get; set; }

    /// <summary>True if the button is the selected option</summary>
    private bool Selected { get; set; } = false;

    /// <summary>True if the button is currently hovered</summary>
    private bool Hovered { get; set; } = false;

    /// <summary>True if the button is currently pressed down</summary>
    private bool Pressed { get; set; } = false;

    void Awake()
    {
        DefaultSprite = Image.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!Selected)
        {
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                if (Pressed)
                {
                    Hovered = true;
                }
            }
            else
            {
                Hovered = true;
                Image.sprite = HighlightedSprite;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!Selected)
        {
            Hovered = false;
            if (!Pressed)
            {
                Image.sprite = DefaultSprite;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Selected)
        {
            Hovered = true;
            Pressed = true;
            Image.sprite = PressedSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!Selected)
        {
            Pressed = false;
            if (Hovered)
            {
                Group.SelectButton(Index);
            }
            else
            {
                Image.sprite = DefaultSprite;
            }
        }
    }

    /// <summary>Selects this button out of the group</summary>
    /// <remarks>Should only be called by the group object</remarks>
    public void Select()
    {
        Selected = true;
        Image.sprite = SelectedSprite;
    }
    
    /// <summary>Deselects this button</summary>
    /// <remarks>Should only be called by the group object</remarks>
    public void Deselect()
    {
        Selected = false;
        Image.sprite = DefaultSprite;
    }
}
