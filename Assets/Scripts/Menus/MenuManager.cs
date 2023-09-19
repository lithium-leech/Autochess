using System.Collections.Generic;

/// <summary>A static class for managing menu states</summary>
public static class MenuManager
{
    /// <summary>The active menus</summary>
    private static IList<Menu> ActiveMenus { get; set; }

    /// <summary>Another menu which is displayed above the active menus</summary>
    private static Menu Overlay { get; set; }

    /// <summary>Initializes the menu manager for the given menu</summary>
    /// <param name="menu">The menu that starts active</param>
    public static void Initialize(Menu menu)
    {
        ActiveMenus = new List<Menu>() { menu };
        Overlay = null;
    }

    /// <summary>Adds a menu to the active menus</summary>
    /// <param name="menu">The menu to make active</param>
    public static void AddActiveMenu(Menu menu)
    {
        if (Overlay == null) menu.Open();
        ActiveMenus.Add(menu);
    }

    /// <summary>Removes a menu from the active menus</summary>
    /// <param name="menu">The menu to deactivate</param>
    public static void RemoveActiveMenu(Menu menu)
    {
        menu.Close();
        ActiveMenus.Remove(menu);
    }

    /// <summary>Shows an overlay above the active menus</summary>
    /// <param name="overlay">The overlay to show</param>
    public static void OpenOverlay(Menu overlay)
    {
        // Hide the active menus
        foreach(Menu menu in ActiveMenus)
            menu.Close();

        // Show the overlay
        Overlay = overlay;
        Overlay.Open();
    }

    /// <summary>Closes the current overlay</summary>
    public static void CloseOverlay()
    {
        // Hide the overlay
        Overlay.Close();
        Overlay = null;

        // Show the active menus
        foreach(Menu menu in ActiveMenus)
            menu.Open();
    }
}
