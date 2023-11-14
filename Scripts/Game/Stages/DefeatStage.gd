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


# Runs repeatedly while the player is in this stage.
# 	delta: The elapsed time since the previous frame.
func during(_delta: float):
	pass


# Runs once when the stage ends.
func end():
	# Hide the game over menu.
	Main.menu_manager.remove_active_menu(game.game_over_menu)
