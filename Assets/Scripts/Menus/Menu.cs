using UnityEngine;

/// <summary>An interface for classes representing menus</summary>
public abstract class Menu : MonoBehaviour
{
    /// <summary>A method called when the menu is opened</summary>
    protected abstract void OnOpen();

    /// <summary>A method called when the menu is closed</summary>
    protected abstract void OnClose();

    /// <summary>Displays the menu</summary>
    public void Open()
    {
        gameObject.SetActive(true);
        OnOpen();
    }

    /// <summary>Hides the menu</summary>
    public void Close()
    {
        OnClose();
        gameObject.SetActive(false);
    }
}
