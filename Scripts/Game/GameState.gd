# A class that defines the current state of the game.
class_name GameState


# An enumeration containing each Z index.
enum ZIndex {
	DEFAULT = 0,
	TERRAIN = 1,
	PIECE = 2,
	EQUIPMENT = 3,
	HIGHLIGHT = 4,
	DYNAMIC = 5,
	UPGRADE = 11
}


# The board to load when a game starts.
var start_board: Board.Kind = Board.Kind.CLASSIC

# The set to load when a game starts.
var start_set: Set.Kind = Set.Kind.WESTERN

# The current level of the game.
var level: int = 0

# The highest level ever reached by the player.
var high_score: int = 0

# The amount of time between the player and enemy moving their pieces.
var turn_pause: float = 2.0

# True if the player is controlling white pieces.
var is_player_white: bool = false

# True after rounds where at least one piece has moved.
var is_active_round: bool = false
