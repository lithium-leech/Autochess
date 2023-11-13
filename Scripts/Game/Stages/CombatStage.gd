# The stage of the game where pieces fight.
class_name CombatStage extends Stage


# Creates a new instance of a combat stage.
#   game: The game to run the stage in.
func _init(_game: Game):
	game = _game


# The game this is being run in.
var game: Game
