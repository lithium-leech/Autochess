using System.Collections.Generic;
using UnityEngine;

/// <summary>A static class for managing menu states</summary>
public static class MenuManager
{
    /// <summary>The active menus</summary>
    private static IList<GameObject> ActiveMenus { get; set; }

    /// <summary>Another menu which is displayed above the active menus</summary>
    private static GameObject Overlay { get; set; }

    /// <summary>Initializes the menu manager for the given menu</summary>
    /// <param name="menu">The menu that starts active</param>
    public static void Initialize(GameObject menu)
    {
        ActiveMenus = new List<GameObject>() { menu };
        Overlay = null;
    }

    /// <summary>Adds a menu to the active menus</summary>
    /// <param name="menu">The menu to make active</param>
    public static void AddActiveMenu(GameObject menu)
    {
        if (Overlay == null) menu.SetActive(true);
        ActiveMenus.Add(menu);
    }

    /// <summary>Removes a menu from the active menus</summary>
    /// <param name="menu">The menu to deactivate</param>
    public static void RemoveActiveMenu(GameObject menu)
    {
        menu.SetActive(false);
        ActiveMenus.Remove(menu);
    }

    /// <summary>Shows an overlay above the active menus</summary>
    /// <param name="overlay">The overlay to show</param>
    public static void OpenOverlay(GameObject overlay)
    {
        // Hide the active menus
        foreach(GameObject menu in ActiveMenus)
            menu.SetActive(false);

        // Show the overlay
        Overlay = overlay;
        Overlay.SetActive(true);
    }

    /// <summary>Closes the current overlay</summary>
    public static void CloseOverlay()
    {
        // Hide the overlay
        Overlay.SetActive(false);
        Overlay = null;

        // Show the active menus
        foreach(GameObject menu in ActiveMenus)
            menu.SetActive(true);
    }
}
