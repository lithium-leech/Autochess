# Contains the behaviors of the title menu's setting controls.
class_name SettingsMenu extends Menu


# The icon representing the current starting board.
var board_icon: TextureRect

# The icon representing the current starting set.
var set_icon: TextureRect

# The label containing the volume value.
var volume_value: Label


# Called when the node enters the scene tree for the first time.
func _ready():
	board_icon = $StartingMap/Board/Socket/Icon
	set_icon = $StartingMap/Set/Socket/Icon
	set_starting_board(Main.game_state.start_board)
	set_starting_set(Main.game_state.start_set)
	volume_value = $Volume/Value
	volume_value.text = "{volume}%".format({ "volume": Main.music_box.volume * 10 })


# Called when the left board button is pressed.
func _on_board_left_pressed():
	# Get the index of the previous board.
	var index: int = Main.game_state.start_board - 1
	if (index < 0): index = Board.Kind.values().size() - 1
	# Set the new starting board.
	set_starting_board(index)


# Called when the right board button is pressed.
func _on_board_right_pressed():
	# Get the index of the next board.
	var index: int = Main.game_state.start_board + 1
	if (index >= Board.Kind.values().size()): index = 0
	# Set the new starting board.
	set_starting_board(index)


# Called when the left set button is pressed.
func _on_set_left_pressed():
	# Get the index of the previous set.
	var index: int = Main.game_state.start_set - 1
	if (index < 0): index = Set.Kind.values().size() - 1
	# Set the new starting set.
	set_starting_set(index)


# Called when the right set button is pressed.
func _on_set_right_pressed():
	# Get the index of the next set.
	var index: int = Main.game_state.start_set + 1
	if (index >= Set.Kind.values().size()): index = 0
	# Set the new starting set.
	set_starting_set(index)


# Called when the left volume button is pressed.
func _on_volume_left_pressed():
	set_volume(Main.music_box.volume - 1)


# Called when the right volume button is pressed.
func _on_volume_right_pressed():
	set_volume(Main.music_box.volume + 1)


# Called when the left language button is pressed.
func _on_language_left_pressed():
	# Get the index of the previous locale.
	var index: int = Main.atlas.available_locales.find(Main.atlas.current_locale) - 1
	if (index < 0): index = Main.atlas.available_locales.size() - 1
	# Set the new locale.
	set_locale(index)


# Called when the right language button is pressed.
func _on_language_right_pressed():
	# Get the index of the next locale.
	var index: int = Main.atlas.available_locales.find(Main.atlas.current_locale) + 1
	if (index >= Main.atlas.available_locales.size()): index = 0
	# Set the new locale.
	set_locale(index)


# Called when the exit button is pressed.
func _on_exit_button_pressed():
	Main.menu_manager.close_overlay()


# Sets the starting board to the given index.
# 	index: The index to switch to.
func set_starting_board(index: int):
	board_icon.texture = Board.get_icon(index)
	Main.game_state.start_board = index as Board.Kind


# Sets the starting set to the given index.
# 	index: The index to switch to.
func set_starting_set(index: int):
	set_icon.texture = Set.get_icon(index)
	Main.game_state.start_set = index as Set.Kind


# Sets the volume level.
# 	level: The level to set the volume to [0:10].
func set_volume(level: int):
	# Set the music volume.
	level = clamp(level, 0, 10)
	Main.music_box.set_volume(level)
	# Update the volume text.
	get_label(volume_value).text = "{volume}%".format({ "volume": level * 10 })


# Sets the current locale to the given index.
# 	index: The index to switch to.
func set_locale(index: int):
	var locale: String = Main.atlas.available_locales[index]
	Main.atlas.set_locale(locale)
