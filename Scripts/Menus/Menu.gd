# A base class for all menu implementations.
class_name Menu extends Control


# Gets this menu's label group.
# 	return: A label group.
func get_label_group() -> LabelGroup:
	return $".."


# Gets the text world label for a given game world label.
# 	label: A label from the game world space.
# 	return: A label from the text world space.
func get_label(label: Label) -> Label:
	return get_label_group().labels[label]


# Can be implemented by inheriting classes.
# Called when the menu is opened.
func on_open():
	pass


# Can be implemented by inheriting classes.
# Called when the menu is closed.
func on_close():
	pass


# Displays the menu.
func open():
	visible = true
	var text: LabelGroup = get_label_group()
	if (text != null):
		text.visible_text = true
	on_open()


# Hides the menu.
func close():
	on_close()
	var text: LabelGroup = get_label_group()
	if (text != null):
		text.visible_text = false
	visible = false
