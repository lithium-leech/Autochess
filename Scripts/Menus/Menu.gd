# A base class for all menu implementations.
class_name Menu extends Control


# Can be implemented by inheriting classes.
# Gets the label group containing this menu's text.
# 	return: A label group.
func get_text() -> LabelGroup:
	return null


# Can be implemented by inheriting classes.
# A method called when the menu is opened.
func on_open():
	pass


# Can be implemented by inheriting classes.
# A method called when the menu is closed.
func on_close():
	pass


# Displays the menu.
func open():
	visible = true
	var text: LabelGroup = get_text()
	if (text != null):
		text.visible_text = true
	on_open()


# Hides the menu.
func close():
	on_close()
	var text: LabelGroup = get_text()
	if (text != null):
		text.visible_text = false
	visible = false
