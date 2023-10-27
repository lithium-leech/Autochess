# A class for managing the locale.
class_name Atlas extends Node


# The current locale being used by the player.
var current_locale: String = ""

# The locales available in game.
var available_locales: Array = []

# A signal that triggers when the locale is changed.
signal on_locale_change
