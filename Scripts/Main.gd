# The main entryway into the game program
# and the root of all other nodes.
class_name Main extends Control


# The node that this script is attached to.
static var node: Control

# The world space containing game objects.
static var game_world: Node

# The world space containing text.
static var text_world: Node

# A tool for managing the music.
static var music_box: MusicBox = MusicBox.new()

# A tool for managing the locale.
static var atlas: Atlas = Atlas.new()

# The current state of the game.
static var game_state: GameState = GameState.new()


# Called when the node enters the scene tree for the first time.
func _ready():
	node = self
	game_world = $GameWindow/GameViewport/GameWorld
	text_world = $TextWindow/TextViewport/TextWorld
	Storage.load_app_data()
	load_display()
	load_title_scene()

# Called when the node is about to leave the scene tree.
func _exit_tree():
	Storage.save_app_data()


# Loads the resolutions and positions of the viewports.
func load_display():
	# Get viewports.
	var game_window: SubViewportContainer = $GameWindow
	var text_window: SubViewportContainer = $TextWindow
	# Get resolutions.
	var screen_size: Vector2i = get_viewport().size
	var game_size: Vector2i = Vector2i(264, 576)
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
	game_window.position.x = roundf((-508 * game_min_scale) + (screen_size.x - display_size.x) / 2.0)
	game_window.position.y = roundf((-352 * game_min_scale) + (screen_size.y - display_size.y) / 2.0)
	text_window.position.x = game_window.position.x
	text_window.position.y = game_window.position.y


# Loads and displays the title scene.
func load_title_scene():
	var title_scene: Node = preload("res://Scenes/Title.tscn").instantiate()
	game_world.add_child(title_scene)
	music_box.play_music(Music.Kind.MENU)
