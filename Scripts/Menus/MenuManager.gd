# A class for managing menus.
class_name MenuManager


# The active menu.
var active_menu: Menu

# An additional menu displayed over the active menu.
var overlay_menu: Menu


# Initializes the menu manager for the given menu.
# 	menu: The menu that starts active.
func initialize(menu: Menu):
	if (overlay_menu != null):
		overlay_menu.close()
		overlay_menu = null
	if (active_menu != null):
		active_menu.close()
	active_menu = menu
	active_menu.open()


# Shows a new menu above the active menu.
# 	menu: The menu to overlay.
func open_overlay(menu: Menu):
	if (overlay_menu != null):
		overlay_menu.close()
	else:
		active_menu.close()
	overlay_menu = menu
	overlay_menu.open()


# Closes the current overlay.
func close_overlay():
	overlay_menu.close()
	overlay_menu = null
	active_menu.open()
