# This script is attached to the front menu node.
extends Node


# Called when the settings button is pressed.
func _on_settings_button_pressed():
	$"..".visible_text = false
	$"../../SettingsGroup/SettingsMenu".visible = true
	$"../../SettingsGroup".visible_text = true


# Called when the quit button is pressed.
func _on_quit_button_pressed():
	get_tree().quit()
