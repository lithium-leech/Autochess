# The level up choices.
class_name ChoiceMenu extends Menu


# The confirm button.
var confirm_button: TextureButton

# The choice buttons.
var choice_buttons: Array[TextureButton]

# The choice backgrounds.
var choice_backgrounds: Array[TextureRect]

# The info backgrounds.
var info_backgrounds: Array[TextureRect]

# The icons for the player's half of the choices.
var player_choices: Array[TextureRect]

# The icons for the enemy's half of the choices.
var enemy_choices: Array[TextureRect]

# The icon for the player's half of the selected choice.
var player_info: TextureRect

# The icon for the enemy's half of the selected choice.
var enemy_info: TextureRect

# The name of the player's half of the selected choice.
var player_name_text: Label

# The information about enemy's half of the selected choice.
var player_info_text: Label

# The name of the enemy's half of the selected choice.
var enemy_name_text: Label

# The information about the enemy's half of the selected choice.
var enemy_info_text: Label


# Called when the node enters the scene tree for the first time.
func _ready():
	confirm_button = $Choice/Confirm
	choice_buttons = [
		$Choice/Choice1,
		$Choice/Choice2,
		$Choice/Choice3]
	choice_backgrounds = [
		$Choice/Choice1/Background,
		$Choice/Choice2/Background,
		$Choice/Choice3/Background]
	info_backgrounds = [
		$Info/Selected/Background,
		$Info/Background]
	player_choices = [
		$Choice/Choice1/PlayerIcon,
		$Choice/Choice2/PlayerIcon,
		$Choice/Choice3/PlayerIcon]
	enemy_choices = [
		$Choice/Choice1/EnemyIcon,
		$Choice/Choice2/EnemyIcon,
		$Choice/Choice3/EnemyIcon]
	player_info = $Info/Selected/PlayerIcon
	enemy_info = $Info/Selected/EnemyIcon
	player_name_text = $Info/PlayerName
	player_info_text = $Info/PlayerInfo
	enemy_name_text = $Info/EnemyName
	enemy_info_text = $Info/EnemyInfo


# Called when the menu is opened.
func on_open():
	# Determine how the background images should be flipped.
	var flip_v: bool = Main.game_state.is_player_white
	var flip_h: bool = flip_v
	if (Main.atlas.in_rtl):
		flip_h = not flip_v
	# Flip the choice backgrounds.
	for background in choice_backgrounds:
		background.flip_v = flip_v
	# Flip the info backgrounds.
	for background in info_backgrounds:
		background.flip_h = flip_h


# Sets the information text.
# 	p_name: The name of the player's half of the selected choice.
# 	p_info: The information about the player's half of the selected choice.
# 	e_name: The name of the enemy's half of the selected choice.
# 	e_info: The information about the enemy's half of the selected choice.
func set_info_text(p_name: String, p_info: String, e_name: String, e_info: String):
	get_label(player_name_text).text = p_name
	get_label(player_info_text).text = p_info
	get_label(enemy_name_text).text = e_name
	get_label(enemy_info_text).text = e_info
