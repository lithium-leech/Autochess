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
	game.in_game_menu.concede_button.pressed.connect(start_concede)
	game.in_game_menu.concede_button.disabled = false
	# Remove the concede menu.
	end_concede()


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
		game.next_stage = UpgradeStage.new(game)
		return
	# Start the fight music on the first turn.
	if (is_first_turn):
		is_first_turn = false
		Main.music_box.play_music(Music.Kind.BATTLE)
	# Move the current player's pieces.
	var pieces: Array[Piece]
	if (is_player_turn):
		pieces = game.game_board.player_pieces.duplicate()
	else:
		pieces = game.game_board.enemy_pieces.duplicate()
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
	# call a final round/movement end.
	game.on_move_finish.emit()
	game.on_round_finish.emit()
	# Remove any remaining pieces.
	game.game_board.clear()
	game.side_board.clear()
	# Deactivate buttons.
	game.in_game_menu.concede_button.pressed.disconnect(start_concede)
	game.in_game_menu.concede_button.disabled = true
	# Remove the concede menu.
	end_concede()


# Runs the battle operations for one set of pieces.
# 	pieces: The pieces to move.
func run_round(pieces: Array[Piece]):
	for piece in pieces:
		piece.take_turn()
	game.on_round_finish.emit()


# Displays the concede menu to the player.
func start_concede():
	Main.menu_manager.add_active_menu(game.concede_menu)
	if (not game.concede_menu.confirm_button.pressed.is_connected(confirm_concede)):
		game.concede_menu.confirm_button.pressed.connect(confirm_concede)
	if (not game.concede_menu.cancel_button.pressed.is_connected(end_concede)):
		game.concede_menu.cancel_button.pressed.connect(end_concede)


# The player concedes and the game is lost.
func confirm_concede():
	game.next_stage = DefeatStage.new(game)
	end_concede()


# Removes the concede menu.
func end_concede():
	Main.menu_manager.remove_active_menu(game.concede_menu)
	if (game.concede_menu.confirm_button.pressed.is_connected(confirm_concede)):
		game.concede_menu.confirm_button.pressed.disconnect(confirm_concede)
	if (game.concede_menu.cancel_button.pressed.is_connected(end_concede)):
		game.concede_menu.cancel_button.pressed.disconnect(end_concede)
	rounds_to_concede = 40
	rounds_static = 0
	is_concede_shown = false
