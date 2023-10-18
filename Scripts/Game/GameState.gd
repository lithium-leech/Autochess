# Holds global properties that define the current state of the game.
class_name GameState


# The world space containing game objects
static var game_world : Node

# The world space containing text
static var text_world : Node

# Current locale being used by the player.
static var current_locale : String = ""

# Locales available in game.
static var available_locales : Array = []

# Amount of time between the player and enemy moving their pieces.
static var turn_pause : float = 2.0

# Current level of the game.
static var level : int = 0

# Highest level ever reached by the player.
static var high_score : int = 0
