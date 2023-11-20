# Contains a set of maps to choose from.
class_name MapUpgrade extends Upgrade


# Create a new instance of a map upgrade.
func _init(_game: Game):
	_base_init(_game)
	# Pick random boards and sets.
	var all_boards: Array = Board.Kind.values()
	var all_sets: Array = Set.Kind.values()
	all_boards.erase(game.current_board)
	all_sets.erase(game.current_set.get_kind())
	for i in range(N_CHOICES):
		var board: Board.Kind = all_boards[randi_range(0, all_boards.size() - 1)]
		var _set: Set.Kind = all_sets[randi_range(0, all_sets.size() - 1)]
		boards.append(board)
		sets.append(_set)
		all_boards.erase(board)
		all_sets.erase(_set)


# The available boards to choose from.
var boards: Array[Board.Kind] = []

# The available sets to choose from.
var sets: Array[Set.Kind] = []


# Gets the title text to be displayed with this upgrade.
# 	return: A string.
func get_title_text():
	return "Text.ChooseMap"


# Displays the offered upgrade choices.
func show_choices():
	# Set the sprites for each choice.
	for i in range(N_CHOICES):
		game.choice_menu.player_choices[i].texture = Board.get_icon(boards[i])
		game.choice_menu.enemy_choices[i].texture = Set.get_icon(sets[i])


# Displays info about the selected upgrade choice.
# 	choice: The choice to display information about.
func show_info(choice: int):
	# Erase previously displayed info.
	remove_info()
	# Set the info board and set.
	game.choice_menu.player_info.texture = Board.get_icon(boards[choice])
	game.choice_menu.enemy_info.texture = Set.get_icon(sets[choice])
	# Change the text.
	var b_key: String = Board.Kind.keys()[boards[choice]].to_pascal_case()
	var s_key: String = Set.Kind.keys()[sets[choice]].to_pascal_case()
	var b_name: String = "BoardName." + b_key
	var b_info: String = "BoardInfo." + b_key
	var s_name: String = "SetName." + s_key
	var s_info: String = "SetInfo." + s_key
	game.choice_menu.set_info_text(b_name, b_info, s_name, s_info)


# Applies the given choice to the game.
# 	choice: The index of the selected choice.
func apply_choice(choice: int):
	# Remove pieces.
	game.game_board.clear()
	game.side_board.clear()
	game.enemy_pieces.clear()
	game.enemy_items.clear()
	game.player_placements.clear()
	# Remove powers.
	game.enemy_power_board.clear()
	game.enemy_powers.clear()
	game.player_power_board.clear()
	game.player_powers.clear()
	# Remove the old board.
	game.game_board.destroy()
	# Create the new board.
	game.current_board = boards[choice]
	game.game_board = BoardBuilder.get_board_builder(game.current_board, game).build()
	# Get the new set.
	game.current_set = Set.get_set(sets[choice])
	# Add the starting pieces.
	var start_kind: Piece.Kind = game.current_set.get_starting_piece()
	var space: Space = game.side_board.get_first_empty_space()
	game.enemy_pieces.append(start_kind)
	game.player_placements.append(Placement.record_new_piece(start_kind, space.coordinates, false))
