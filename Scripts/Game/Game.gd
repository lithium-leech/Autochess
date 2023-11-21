# A class for managing the state of the game.
class_name Game extends Control


# The in-game controls.
var in_game_menu: InGameMenu

# The concede pop-up.
var concede_menu: ConcedeMenu

# The level up menu.
var choice_menu: ChoiceMenu

# The game over menu.
var game_over_menu: GameOverMenu

# The current stage of the game being run.
var current_stage: Stage

# The next stage of the game to run.
#   remark: null when the current stage should keep running.
var next_stage: Stage = null

# The enemy's roster of pieces.
var enemy_pieces: Array[Piece.Kind] = []

# The enemy's collection of items.
var enemy_items: Array[Item.Kind] = []

# The player's placed pieces and items.
var player_placements: Array[Placement] = []

# The current set of pieces being played with.
var current_set: Set

# The kind of board currently being used as the game board.
var current_board: Board.Kind

# The board that fights take place on.
var game_board: Board

# The board that holds the player's available pieces.
var side_board: Board

# The board that displays the enemy's powers.
var enemy_power_board: Board

# The board that displays the player's powers.
var player_power_board: Board

# A signal that triggers when one combat round is finished.
signal on_round_finish

# A signal that triggers when piece movement is finished.
signal on_move_finish


# Called when the node enters the scene tree for the first time.
func _ready():
	# Add nodes from scene.
	in_game_menu = $InGame/Menu
	concede_menu = $Concede/Menu
	choice_menu = $Choice/Menu
	game_over_menu = $GameOver/Menu
	# Go to the first level.
	Main.game_state.turn_pause = 2.0
	Main.game_state.is_player_white = false
	Main.game_state.level = 1
	in_game_menu.set_level(Main.game_state.level)
	# Create the static boards.
	side_board = SideBoardBuilder.new(self).build()
	enemy_power_board = PowerBoardBuilder.new(self, false).build()
	player_power_board = PowerBoardBuilder.new(self, true).build()
	# Create the starting map.
	game_board = BoardBuilder.get_board_builder(Main.game_state.start_board, self).build()
	current_set = Set.get_set(Main.game_state.start_set)
	var start_piece: Piece.Kind = current_set.get_starting_piece()
	var empty: Vector2i = side_board.get_first_empty_space().coordinates
	enemy_pieces.append(start_piece)
	player_placements.append(Placement.record_new_piece(start_piece, empty, false))
	# Start the planning phase
	next_stage = PlanningStage.new(self)


# Called every frame.
# 	delta: The elapsed time since the previous frame.
func _process(delta: float):
	# Check if a new stage has been queued.
	if (next_stage != null):
		# Replace the current stage with the next one.
		if (current_stage != null):
			current_stage.end()
		current_stage = next_stage
		next_stage = null
		current_stage.start()
	else:
		# Otherwise just run the current stage.
		if (current_stage != null):
			current_stage.during(delta)
