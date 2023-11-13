# Contains the behaviors of the title menu's front-most controls
class_name FrontMenu extends Menu


# Gets the label group containing this menu's text.
# 	return: A label group.
func get_text() -> LabelGroup:
	return $".."


# Called when the play button is pressed.
func _on_play_button_pressed():
	# Delete the current contents of the game world.
	for child in Main.game_world.get_children():
		child.queue_free()
	# Add the new game scene.
	var game_scene: Control = load("res://Scenes/Game.tscn").instantiate()
	Main.game_world.add_child(game_scene)
	Main.menu_manager.initialize(game_scene.get_node("InGame/Menu"))
	Main.music_box.play_music(Music.Kind.PLANNING)


# Called when the settings button is pressed.
func _on_settings_button_pressed():
	var menu: Menu = $"../../Settings/Menu"
	Main.menu_manager.open_overlay(menu)


# Called when the quit button is pressed.
func _on_quit_button_pressed():
	get_tree().quit()
