# This script is attached to the settings menu node.
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
	set_volume(Main.music_box.volume - 1)


# Called when the right volume button is pressed.
func _on_volume_right_pressed():
	set_volume(Main.music_box.volume + 1)


# Called when the left language button is pressed.
func _on_language_left_pressed():
	# Get the index of the previous locale.
	var index: int = Main.atlas.available_locales.find(Main.atlas.current_locale) - 1
	if (index < 0): index = Main.atlas.available_locales.size() - 1
	# Set the new locale.
	set_locale(index)


# Called when the right language button is pressed.
func _on_language_right_pressed():
	# Get the index of the next locale.
	var index: int = Main.atlas.available_locales.find(Main.atlas.current_locale) + 1
	if (index >= Main.atlas.available_locales.size()): index = 0
	# Set the new locale.
	set_locale(index)


# Called when the exit button is pressed.
func _on_exit_button_pressed():
	$"..".visible_text = false
	self.visible = false
	$"../../FrontGroup".visible_text = true


# Sets the volume level.
# 	level: The level to set the volume to [0:10].
func set_volume(level: int):
	# Set the music volume.
	level = clamp(level, 0, 10)
	Main.music_box.set_volume(level)
	# Update the volume text.
	$"..".labels[$Volume/Value].text = "{volume}%".format({
		"volume": level * 10
	})
	

# Sets the current locale to the given index.
# 	index: The index to switch to.
func set_locale(index: int):
	Main.atlas.current_locale = Main.atlas.available_locales[index]
	TranslationServer.set_locale(Main.atlas.current_locale)
	Main.atlas.on_locale_change.emit()
