# Contains a set of upgrades to choose from after completing a level.
class_name Upgrade


# Must be called in the constructor of inheriting classes.
# Creates a new instance of an upgrade.
# 	game: The game to show choices in.
func _base_init(_game: Game):
	game = _game


# The number of available choices.
const N_CHOICES = 3

# The game to show choices in.
var game: Game

# A node containing the sprites that don't change.
var static_node: Node2D

# A node containing the sprites that do change.
var dynamic_node: Node2D


# Must be implemented by inheriting classes.
# Gets the title text to be displayed with this upgrade.
# 	return: A string.
func get_title_text():
	return null


# Must be implemented by inheriting classes.
# Displays the offered upgrade choices.
func show_choices():
	pass


# Must be implemented by inheriting classes.
# Displays information about the selected upgrade choice.
# 	choice: The choice to display information about.
func show_info(_choice: int):
	pass


# Must be implemented by inheriting classes.
# Applies the given choice to the game.
# 	choice: The index of the selected choice.
func apply_choice(_choice: int):
	pass


# Removes the choice sprites.
func remove_choices():
	if (static_node != null):
		static_node.queue_free()


# Removes the info sprites and text.
func remove_info():
	if (dynamic_node != null):
		dynamic_node.queue_free()
	game.choice_menu.set_info_text("", "", "", "")


# Gets the position of a desired choice to display.
# 	choice: The index of the choice to display.
# 	player: True if this is the player's half of the choice.
# 	return: A game world position.
func get_choice_position(choice: int, player: bool) -> Vector2i:
	var button: TextureButton = game.choice_menu.choice_buttons[choice]
	var position: Vector2 = button.global_position
	var x: int = int(position.x) + 24
	var y: int = int(position.y) + (56 if player else 24)
	return Vector2i(x, y)


# Gets the position of a desired choice to show info for.
# 	player: True if this is the player's half of the choice.
# 	return: A game world position.
func get_info_position(player: bool) -> Vector2i:
	var button: TextureRect = game.choice_menu.info_button
	var position: Vector2 = button.global_position
	var x: int = int(position.x) + (24 if player else 56)
	var y: int = int(position.y) + 24
	return Vector2i(x, y)
