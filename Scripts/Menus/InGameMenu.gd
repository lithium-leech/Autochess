# The game's default user interface.
class_name InGameMenu extends Menu


# Gets the label group containing this menu's text.
# 	return: A label group.
func get_text() -> LabelGroup:
	return $".."

# A button for triggering the concede prompt.
var concede_button: TextureButton

# A button for changing the speed of a round.
var speed_button: TextureButton

# A button for starting a battle.
var fight_button: TextureButton

# The text showing the current level.
var level_text: Label


# Called when the node enters the scene tree for the first time.
func _ready():
	concede_button = $Concede
	speed_button = $Speed
	fight_button = $Fight
	level_text = $Level/Label
