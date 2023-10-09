# This script is attached to the TitleMenu node
extends Node2D


# Called when the settings button is pressed
func _on_settings_button_pressed():
	get_node("SettingsMenu").visible = true


# Called when the quit button is pressed
func _on_quit_button_pressed():
	get_tree().quit()	
	
