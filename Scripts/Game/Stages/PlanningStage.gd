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


# A function that does a thing.
func start():
	pass
