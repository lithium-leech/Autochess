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

# The icon displayed in the speed button.
var speed_icon: TextureRect

# The speed icons to rotate through.
var speed_sprites: Array[CompressedTexture2D] = [
	preload("res://Sprites/GameIcons/SmallSpeed1.png"),
	preload("res://Sprites/GameIcons/SmallSpeed2.png"),
	preload("res://Sprites/GameIcons/SmallSpeed3.png")
]

# The available game speeds.
var speeds: Array[float] = [2.0, 1.0, 0.5]

# The index of the speed currently being used.
var speed_index: int = 0

# Called when the node enters the scene tree for the first time.
func _ready():
	concede_button = $Concede
	speed_button = $Speed
	fight_button = $Fight
	level_text = $Level/Label
	speed_icon = $Speed/Icon


# Called when the speed button is pressed.
func _on_speed_pressed():
	speed_index += 1
	if (speed_index >= speeds.size()):
		speed_index = 0
	Main.game_state.turn_pause = speeds[speed_index]
	speed_icon.texture = speed_sprites[speed_index]
