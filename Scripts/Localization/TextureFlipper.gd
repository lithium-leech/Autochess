# This script is attached to images that flip when an RTL locale is in use.
extends TextureRect


# The last locale detected by this script.
var detected_locale: String = ""


# Called every frame.
# 	delta: The elapsed time since the previous frame.
func _process(_delta: float):
	# Check if the locale has changed.
	if (detected_locale != GameState.current_locale):
		detected_locale = GameState.current_locale
		if (detected_locale == "ar"):
			# Modify image for right-to-left.
			flip_h = true
		else:
			# Modify image for left-to-right.
			flip_h = false
