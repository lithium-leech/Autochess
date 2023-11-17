# The stage of the game after the player loses.
class_name DefeatStage extends Stage


# Creates a new instance of a combat stage.
#   game: The game to run the stage in.
func _init(_game: Game):
	game = _game


# The game this is being run in.
var game: Game


# Runs once when the stage starts.
func start():
	# stop all music.
	Main.music_box.play_music(Music.Kind.NONE)
	# Show the game over screen.
	Main.menu_manager.add_active_menu(game.game_over_menu)
	# Add button signals.
	game.game_over_menu.retry_button.pressed.connect(_on_retry_pressed)


# Runs once when the stage ends.
func end():
	# Remove button signals.
	game.game_over_menu.retry_button.pressed.disconnect(_on_retry_pressed)
	# Hide the game over menu.
	Main.menu_manager.remove_active_menu(game.game_over_menu)


# Called when the retry button is pressed.
func _on_retry_pressed():
	game.next_stage = PlanningStage.new(game)
