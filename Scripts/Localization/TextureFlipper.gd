# An image that flips when an RTL locale is in use.
extends TextureRect


# Called when the node enters the scene tree for the first time.
func _ready():
	Main.atlas.on_locale_change.connect(fix_for_locale)


# Adjusts the image for the current locale.
func fix_for_locale():
	if (Atlas.is_rtl(Main.atlas.current_locale)):
		# Modify image for right-to-left.
		flip_h = true
	else:
		# Modify image for left-to-right.
		flip_h = false
