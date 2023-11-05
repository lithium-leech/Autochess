# A class for managing the state of the game.
class_name Game extends Control


# The current stage of the game being run.
var current_stage: Stage

# The next stage of the game to run.
#   remark: null when the current stage should keep running.
var next_stage: Stage = null

# The enemy's roster of pieces.
var enemy_pieces: Array[Piece.Kind] = []

# The enemy's collection of items.
var enemy_items: Array[Item.Kind] = []

# The enemy's acquired power ups.
var enemy_powers: Array[Power] = []

# The player's roster of pieces.
var player_pieces: Array[PlacementRecord] = []

# The players's collection of items.
var player_items: Array[PlacementRecord] = []

# The player's acquired power ups.
var player_powers: Array[Power] = []

# The current set of pieces being played with.
var current_set: Set

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
	# Initialize the menu manager.
	#MenuManager.Initialize(InGameMenu);
	# Go to the first level.
	Main.game_state.turn_pause = 2.0
	Main.game_state.is_player_white = false
	Main.game_state.level = 1
	#LevelText.text = GameState.Level.ToString();
	#PersistentVariablesSource pvs = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
	#IntVariable score = pvs["game"]["score"] as IntVariable;
	#IntVariable highscore = pvs["game"]["highscore"] as IntVariable;
	#using (PersistentVariablesSource.UpdateScope())
	#{
	#    score.Value = GameState.Level;
	#    highscore.Value = GameState.HighScore;
	#}
	# Create the static boards.
	#SideBoard = new SideBoardBuilder(this).Build();
	enemy_power_board = PowerBoardBuilder.new(self, false).build()
	player_power_board = PowerBoardBuilder.new(self, true).build()
	# Create the starting game board.
	#MapChoices choices = new(this, GameState.StartMap, GameState.StartSet);
	#choices.ApplyChoice(0);
	# Start the planning phase
	#NextStage = new PlanningStage(this);
