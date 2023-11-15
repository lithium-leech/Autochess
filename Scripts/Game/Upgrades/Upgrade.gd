# Contains a set of upgrades to choose from after completing a level.
class_name Upgrade


# Must be called in the constructor of inheriting classes.
# Creates a new instance of an upgrade.
# 	game: The game to show choices in.
func _base_init(_game: Game):
	game = _game
	n_choices = 3


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
# Displays info about the selected upgrade choice.
func show_info():
	pass


# Must be implemented by inheriting classes.
# Applies the given choice to the game.
# 	choice: The index of the selected choice.
func apply_choice(_choice: int):
	pass


# The game to show choices in.
var game: Game

# The number of available choices.
var n_choices: int

# A node containing the sprites that don't change.
var static_node: Node2D

# A node containing the sprites that do change.
var dynamic_node: Node2D


# Creates the display for this set of upgrade choices.
func create_display():
	create_panels()
	show_choices()


# Creates the panels displayed behind the various sprites.
func create_panels():
	pass


# Removes the display for this set of upgrade choices.
func remove_display():
	remove_panels_and_choices()
	remove_info()


# Removes the background panels and choice sprites.
func remove_panels_and_choices():
	if (static_node != null):
		static_node.queue_free()


# Removes the info sprites and text.
func remove_info():
	if (dynamic_node != null):
		dynamic_node.queue_free()
	pass


# Gets the position of a desired choice to display.
# 	choice: The index of the choice to display.
# 	player: True if this is the player's half of the choice.
# 	return: A game world position.
func get_choice_position(choice: int, player: bool) -> Vector2i:
	var x: int
	match choice:
		0:
			x = 100
		1:
			x = 200
		2:
			x = 300
	var y: int = 100 if player else 200
	return Vector2i(x, y)


# Gets the position of a desired choice to show info for.
# 	player: True if this is the player's half of the choice.
# 	return: A game world position.
func get_info_position(player: bool) -> Vector2i:
	var x: int = 100 if player else 200
	var y: int = 400
	return Vector2i(x, y)
