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


# Removes the info sprites and text.
func remove_info():
	game.choice_menu.player_info.texture = null
	game.choice_menu.enemy_info.texture = null
	game.choice_menu.set_info_text("", "", "", "")
