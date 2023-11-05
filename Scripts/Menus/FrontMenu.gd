# Contains the behaviors of title menu's front-most controls
extends Control


# Called when the play button is pressed.
func _on_play_button_pressed():
	# Delete the current contents of the game world.
	for child in Main.game_world.get_children():
		child.queue_free()
	# Add the new game scene.
	var game_scene: Node = preload("res://Scenes/Game.tscn").instantiate()
	Main.game_world.add_child(game_scene)
	Main.music_box.play_music(Music.Kind.PLANNING)


# Called when the settings button is pressed.
func _on_settings_button_pressed():
	$"..".visible_text = false
	$"../../SettingsGroup/SettingsMenu".visible = true
	$"../../SettingsGroup".visible_text = true


# Called when the quit button is pressed.
func _on_quit_button_pressed():
	get_tree().quit()
