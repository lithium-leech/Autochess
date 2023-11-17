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
	for i in range(N_CHOICES):
		# Set this choice's player piece.
		var player_piece: Piece = create_piece(player_pieces[i], true)
		game.choice_menu.player_choices[i].texture = player_piece.texture
		# Set this choice's enemy piece.
		var enemy_piece: Piece = create_piece(enemy_pieces[i], false)
		game.choice_menu.enemy_choices[i].texture = enemy_piece.texture


# Displays info about the selected upgrade choice.
# 	choice: The choice to display information about.
func show_info(choice: int):
	# Erase previously displayed info.
	remove_info()
	# Set the info pieces.
	var player_piece: Piece = create_piece(player_pieces[choice], true)
	var enemy_piece: Piece = create_piece(enemy_pieces[choice], false)
	game.choice_menu.player_info.texture = player_piece.texture
	game.choice_menu.enemy_info.texture = enemy_piece.texture
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
func apply_choice(_choice: int):
	pass


# Creates a new piece displayed at a given position.
# 	kind: The kind of piece to create.
# 	player: True if this piece is for the player.
# 	position: The game world position to display the piece.
func create_piece(kind: Piece.Kind, player: bool) -> Piece:
	var white: bool = (Main.game_state.is_player_white and player) or \
					  (not Main.game_state.is_player_white and not player)
	return Piece.create_piece(kind, white)
