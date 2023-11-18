# Contains a set of pieces to choose from.
class_name PieceUpgrade extends Upgrade


# Create a new instance of a piece upgrade.
func _init(_game: Game):
	_base_init(_game)
	# Pick random pieces.
	for i in range(N_CHOICES):
		pass
	# TEMPORARILY: Set the piece choices
	player_pieces = [Piece.Kind.ROOK, Piece.Kind.PAWN, Piece.Kind.KNIGHT]
	enemy_pieces = [Piece.Kind.QUEEN, Piece.Kind.KING, Piece.Kind.BISHOP]


# The available player pieces to choose from.
var player_pieces: Array[Piece.Kind] = []

# The available enemy pieces to choose from.
var enemy_pieces: Array[Piece.Kind] = []


# Gets the title text to be displayed with this upgrade.
# 	return: A string.
func get_title_text():
	return "Text.ChoosePiece"


# Displays the offered upgrade choices.
func show_choices():
	# Set the sprites for each choice.
	var player_white: bool = Main.game_state.is_player_white
	for i in range(N_CHOICES):
		game.choice_menu.player_choices[i].texture = Piece.get_icon(player_pieces[i], player_white)
		game.choice_menu.enemy_choices[i].texture = Piece.get_icon(enemy_pieces[i], not player_white)


# Displays info about the selected upgrade choice.
# 	choice: The choice to display information about.
func show_info(choice: int):
	# Erase previously displayed info.
	remove_info()
	# Set the info pieces.
	var player_white: bool = Main.game_state.is_player_white
	game.choice_menu.player_info.texture = Piece.get_icon(player_pieces[choice], player_white)
	game.choice_menu.enemy_info.texture = Piece.get_icon(enemy_pieces[choice], not player_white)
	# Change the text.
	var p_key: String = Piece.Kind.keys()[player_pieces[choice]].to_pascal_case()
	var e_key: String = Piece.Kind.keys()[enemy_pieces[choice]].to_pascal_case()
	var p_name: String = "PieceName." + p_key
	var p_info: String = "PieceInfo." + p_key
	var e_name: String = "PieceName." + e_key
	var e_info: String = "PieceInfo." + e_key
	game.choice_menu.set_info_text(p_name, p_info, e_name, e_info)


# Applies the given choice to the game.
# 	choice: The index of the selected choice.
func apply_choice(choice: int):
	# Add the enemy piece.
	game.enemy_pieces.append(enemy_pieces[choice])
	# Add the player piece
	var space: Space = game.side_board.get_first_empty_space()
	if (space != null):
		var record: Placement = Placement.record_new_piece(player_pieces[choice], space.coordinates, false)
		game.player_placements.append(record)
