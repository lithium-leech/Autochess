# A class that holds global properties that define the current state of the game.
class_name GameState


# The world space containing game objects.
static var game_world: Node

# The world space containing text.
static var text_world: Node

# The current locale being used by the player.
static var current_locale: String = ""

# The locales available in game.
static var available_locales: Array = []

# The amount of time between the player and enemy moving their pieces.
static var turn_pause: float = 2.0

# The current level of the game.
static var level: int = 0

# The highest level ever reached by the player.
static var high_score: int = 0
