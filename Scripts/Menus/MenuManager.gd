# A class for managing menus.
class_name MenuManager


# The active menu.
var active_menus: Array[Menu]

# An additional menu displayed over the active menu.
var overlay_menu: Menu


# Initializes the menu manager for the given menu.
# 	menu: The menu that starts active.
func initialize(menu: Menu):
	if (overlay_menu != null):
		overlay_menu.close()
		overlay_menu = null
	for active_menu in active_menus:	
		active_menu.close()
	active_menus = [menu]
	active_menus[0].open()


# Adds a menu to the active menus.
# 	menu: The menu to make active.
func add_active_menu(menu: Menu):
	if (overlay_menu == null):
		menu.open()
	active_menus.append(menu)


# Removes a menu from the active menus.
# 	menu: The menu to deactivate.
func remove_active_menu(menu: Menu):
	menu.close()
	active_menus.erase(menu)


# Shows a new menu above the active menu.
# 	menu: The menu to overlay.
func open_overlay(menu: Menu):
	if (overlay_menu != null):
		overlay_menu.close()
	else:
		for active_menu in active_menus:
			active_menu.close()
	overlay_menu = menu
	overlay_menu.open()


# Closes the current overlay.
func close_overlay():
	overlay_menu.close()
	overlay_menu = null
	for active_menu in active_menus:
		active_menu.open()
