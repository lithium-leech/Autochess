# A class for managing the locale.
class_name Atlas extends Node


# The current locale being used by the player.
var current_locale: String = TranslationServer.get_locale().split("_")[0]

# The locales available in game.
var available_locales: Array = TranslationServer.get_loaded_locales()

# A signal that triggers when the locale is changed.
signal on_locale_change

# Fonts mapped to their locales.
var fonts: Dictionary = {
	"ar": load("res://Fonts/unixel.ttf"),
	"de": load("res://Fonts/Pixel Musketeer.otf"),
	"en": load("res://Fonts/Pixel Musketeer.otf"),
	"es": load("res://Fonts/Pixel Musketeer.otf"),
	"fr": load("res://Fonts/Pixel Musketeer.otf"),
	"hi": load("res://Fonts/NotoSansDevanagari.ttf"),
	"id": load("res://Fonts/Pixel Musketeer.otf"),
	"ja": load("res://Fonts/DotGothic16.ttf"),
	"ko": load("res://Fonts/dalmoori.ttf"),
	"pt": load("res://Fonts/Pixel Musketeer.otf"),
	"ru": load("res://Fonts/Greybeard.ttf"),
	"zh": load("res://Fonts/NotoSansSC.ttf")
}


# Determines if the given locale uses a right-to-left layout.
# 	locale: The locale to check.
static func is_rtl(locale: String) -> bool:
	match locale:
		"ar": return true
		_: return false
