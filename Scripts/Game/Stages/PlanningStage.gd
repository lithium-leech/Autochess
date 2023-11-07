# The stage of the game where the player chooses where to put their pieces.
class_name PlanningStage extends Stage


# Creates a new instance of a planning stage.
#   game: The game to run the stage in.
func _init(_game: Game):
	game = _game


# The game this is being run in.
var game: Game

# The object currently held by the player.
var held: GameObject

# A node containing sprites highlighting the different placement zones.
var highlight_node: Node2D


# Runs once when the stage starts.
func start():
	# Start the planning music.
	Main.music_box.play_music(Music.Kind.PLANNING)
	# Set up the boards.
	game.game_board.clear()
	game.side_board.clear()
	set_up_player()
	PlacementAI.set_up_enemy(game)
	# Add highlights around the different the placement zones.
	Highlighter.new(game.game_board).add_highlights()
	Highlighter.new(game.side_board).add_highlights()
	# Activate the concede button.
	# Activate the fight button.


# Runs repeatedly while the player is in this stage.
func during():
	# Get the current mouse position.
	# Check if the position is inside a board.
	# Get the current space.
	# Grab objects on click.
	# Drag grabbed objects.
	# Drop objects on release.
	pass


# Runs once when the stage ends.
func end():
	# Deactivate the fight button.
	# Deactivate the concede button.
	# Remove highlights.
	# Save the player's placement set up.
	pass


# Starts the fight sequence.
func start_fight():
	# Don't start the fight if there are no pieces on the board.
	# Deactivate the fight button.
	# Queue the combat stage.
	pass


# Displays the concede menu to the player.
func start_concede():
	pass


# The player concedes and the fight is lost.
func confirm_concede():
	pass


# Removes the concede menu and resets concede logic.
func cancel_concede():
	pass


# Sets up the player's pieces on the game board and side board.
func set_up_player():
	# Place the player's pieces on the game board.
	# Place the player's pieces on the side board.
	# Place the player's objects on the side board.
	pass
