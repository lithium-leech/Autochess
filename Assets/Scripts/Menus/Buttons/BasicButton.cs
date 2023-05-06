using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A simple button implementation which changes the sprite based on its current state
/// </summary>
public class BasicButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    /// Properties to set using Unity interface
    [SerializeField] private Image Image;
    [SerializeField] private Sprite HighlightedSprite;
    [SerializeField] private Sprite PressedSprite;
    [SerializeField] private Sprite DisabledSprite;
    [SerializeField] private bool Enabled = true;
    [SerializeField] private bool GoneAfterClick = false;
    public UnityEvent onClick;

    /// <summary>A property to hold the sprite originally attached to the DefaultImage</summary>
    private Sprite DefaultSprite { get; set; }
    
    /// <summary>True if the button is currently hovered</summary>
    private bool IsHovered { get; set; } = false;

    /// <summary>True if the button is currently pressed down</summary>
    private bool IsPressed { get; set; } = false;
    
    void Awake()
    {
        DefaultSprite = Image.sprite;
        SetEnabled(Enabled);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Enabled)
        {
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                if (IsPressed)
                {
                    IsHovered = true;
                }
            }
            else
            {
                IsHovered = true;
                Image.sprite = HighlightedSprite;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Enabled)
        {
            IsHovered = false;
            if (!IsPressed)
            {
                Image.sprite = DefaultSprite;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Enabled)
        {
            IsHovered = true;
            IsPressed = true;
            Image.sprite = PressedSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Enabled && IsPressed)
        {
            IsPressed = false;
            if (IsHovered)
            {
                if (GoneAfterClick)
                {
                    Image.sprite = DefaultSprite;
                }
                else
                {
                    Image.sprite = HighlightedSprite;
                }
                onClick.Invoke();
            }
            else
            {
                Image.sprite = DefaultSprite;
            }
        }
    }

    /// <summary>Sets the buttons enabled state</summary>
    /// <param name="enabled">The enabled state to set the button to</param>
    public void SetEnabled(bool enabled)
    {
        Enabled = enabled;
        IsPressed = false;
        if (Enabled)
        {
            Image.sprite = DefaultSprite;
        }
        else
        {
            Image.sprite = DisabledSprite;
        }
    }
}
