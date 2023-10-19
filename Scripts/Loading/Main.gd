# This script is attached to the main node.
extends Node


# Called when the node enters the scene tree for the first time.
func _ready():
	GameState.game_world = $GameWindow/GameViewport/GameWorld
	GameState.text_world = $TextWindow/TextViewport/TextWorld
	load_display()
	load_locale()
	load_title_scene()


# Loads the resolutions and positions of the viewports.
func load_display():
	# Get viewports.
	var game_window: SubViewportContainer = $GameWindow
	var text_window: SubViewportContainer = $TextWindow
	# Get resolutions.
	var screen_size: Vector2i = get_viewport().size
	var game_size: Vector2i = Vector2i(288, 576)
	# Determine scaling for game viewport.
	var game_scaling: Vector2i = screen_size / game_size
	var game_min_scale: int = floor(min(game_scaling.x, game_scaling.y))
	if (game_min_scale < 1): game_min_scale = 1
	game_scaling = Vector2i(game_min_scale, game_min_scale)
	game_window.scale = game_scaling
	# Determine scaling for text viewport.
	var text_min_scale: float = float(game_min_scale) / 4
	var text_scaling: Vector2 = Vector2(text_min_scale, text_min_scale)
	text_window.scale = text_scaling
	# Determine viewport positions.
	var display_size: Vector2i = game_size * game_scaling
	game_window.position.x = roundf((-496 * game_min_scale) + (screen_size.x - display_size.x) / 2.0)
	game_window.position.y = roundf((-352 * game_min_scale) + (screen_size.y - display_size.y) / 2.0)
	text_window.position.x = game_window.position.x
	text_window.position.y = game_window.position.y


# Loads the locale of the local environment.
func load_locale():
	GameState.current_locale = TranslationServer.get_locale().split("_")[0]
	GameState.available_locales = TranslationServer.get_loaded_locales()


# Loads and displays the title scene.
func load_title_scene():
	var title_scene: Node = preload("res://Scenes/Title.tscn").instantiate()
	GameState.game_world.add_child(title_scene)
