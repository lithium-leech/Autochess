# This script is attached to the SettingsMenu node.
extends Control

	
# Called when the left board button is pressed.
func _on_board_left_pressed():
	pass # Replace with function body.


# Called when the right board button is pressed.
func _on_board_right_pressed():
	pass # Replace with function body.


# Called when the left set button is pressed.
func _on_set_left_pressed():
	pass # Replace with function body.


# Called when the right set button is pressed.
func _on_set_right_pressed():
	pass # Replace with function body.


# Called when the left volume button is pressed.
func _on_volume_left_pressed():
	pass # Replace with function body.


# Called when the right volume button is pressed.
func _on_volume_right_pressed():
	pass # Replace with function body.


# Called when the left language button is pressed.
func _on_language_left_pressed():
	# Get the index of the previous locale
	var index : int = GameState.available_locales.find(GameState.current_locale) - 1
	if index < 0 : index = GameState.available_locales.size() - 1
	# Set the new locale
	set_locale(index)


# Called when the right language button is pressed.
func _on_language_right_pressed():
	# Get the index of the next locale
	var index : int = GameState.available_locales.find(GameState.current_locale) + 1
	if index >= GameState.available_locales.size() : index = 0
	# Set the new locale
	set_locale(index)


# Sets the current locale to the given index.
#	index: The index to switch to
func set_locale(index: int):
	GameState.current_locale = GameState.available_locales[index]
	TranslationServer.set_locale(GameState.current_locale)


# Called when the exit button is pressed.
func _on_exit_button_pressed():
	visible = false
