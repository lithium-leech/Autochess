# This script is attached to the Main node
extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready():
	# Load the locale of the local environment
	GameState.current_locale = TranslationServer.get_locale().split("_")[0]
	GameState.available_locales = TranslationServer.get_loaded_locales()
	# Load and display the title menu
	var title_menu: Node = preload("res://Scenes/TitleMenu.tscn").instantiate()
	title_menu.transform = Transform2D()
	add_child(title_menu)
