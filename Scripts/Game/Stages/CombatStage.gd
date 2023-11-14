# The stage of the game where pieces fight.
class_name CombatStage extends Stage


# Creates a new instance of a combat stage.
#   game: The game to run the stage in.
func _init(_game: Game):
	game = _game


# The game this is being run in.
var game: Game

# The time elapsed so far, in between turns.
var time_waited: float

# True if it is currently the player's turn.
var is_player_turn: bool

# True until the first turn has been taken.
var is_first_turn: bool = true

# True if the concede menu is currently being displayed.
var is_concede_shown: bool

# True while pieces are moving.
var are_pieces_moving: bool = false

# The number of rounds until the player should be prompted to concede.
var rounds_to_concede: int

# The number of rounds that have occurred with no pieces moving.
var rounds_static: int


# Runs once when the stage starts.
func start():
	# Initialize battle start.
	Main.music_box.play_music(Music.Kind.NONE)
	time_waited = 0.0
	is_player_turn = Main.game_state.is_player_white
	# Activate buttons.


# Runs repeatedly while the player is in this stage.
# 	delta: The elapsed time since the previous frame.
func during(delta: float):
	# Pause between turns.
	time_waited += delta
	if (time_waited < Main.game_state.turn_pause / 2.0):
		return
	elif (are_pieces_moving and game.game_board.are_pieces_moving()):
		return
	elif (are_pieces_moving):
		are_pieces_moving = false
		game.on_move_finish.emit()
	if (time_waited < Main.game_state.turn_pause):
		return
	# Check if the battle is over.
	if (game.game_board.player_pieces.size() < 1):
		game.next_stage = DefeatStage.new(game)
		return
	elif (game.game_board.enemy_pieces.size() < 1):
		game.next_stage = UpgradeStage.new()
		return
	# Start the fight music on the first turn.
	if (is_first_turn):
		is_first_turn = false
		Main.music_box.play_music(Music.Kind.BATTLE)
	# Move the current player's pieces.
	var pieces: Array[Piece]
	if (is_player_turn):
		pieces = game.game_board.player_pieces
	else:
		pieces = game.game_board.enemy_pieces
	Main.game_state.is_active_round = false
	run_round(pieces)
	if (Main.game_state.is_active_round):
		rounds_static = 0
	else:
		rounds_static += 1
	are_pieces_moving = true
	# Check if the player should be prompted to concede.
	if (not is_concede_shown):
		if (rounds_static > 1 or rounds_to_concede < 1):
			start_concede()
	# Go to the next turn.
	rounds_to_concede -= 1
	is_player_turn = not is_player_turn
	time_waited = 0.0


# Runs once when the stage ends.
func end():
	# Remove any remaining pieces.
	game.game_board.clear()
	game.side_board.clear()
	# Deactivate buttons.
	cancel_concede()


# Runs the battle operations for one set of pieces.
# 	pieces: The pieces to move.
func run_round(pieces: Array[Piece]):
	for piece in pieces:
		piece.take_turn()
	game.on_round_finish.emit()


# Displays the concede menu to the player.
func start_concede():
	pass


# The player concedes and the fight is lost.
func confirm_concede():
	pass


# Removes the concede menu and resets concede logic.
func cancel_concede():
	pass
