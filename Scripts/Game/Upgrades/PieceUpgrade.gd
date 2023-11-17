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
	player_pieces = [Piece.Kind.QUEEN, Piece.Kind.KING, Piece.Kind.BISHOP]


# The text to be displayed in the choice menu alongside these upgrades.
var title_text: String = "Text.ChoosePiece"

# The available player pieces to choose from.
var player_pieces: Array[Piece.Kind] = []

# The available enemy pieces to choose from.
var enemy_pieces: Array[Piece.Kind] = []


# Gets the title text to be displayed with this upgrade.
# 	return: A string.
func get_title_text():
	return title_text


# Displays the offered upgrade choices.
func show_choices():
	# Create a new collection of choice sprites.
	static_node = Node2D.new()
	for i in range(N_CHOICES):
		# Create this choice's player piece.
		var player_piece: Piece = create_piece(player_pieces[i], true, get_choice_position(i, true))
		static_node.add_child(player_piece)
		# Create this choice's enemy piece.
		var enemy_piece: Piece = create_piece(player_pieces[i], false, get_choice_position(i, false))
		static_node.add_child(enemy_piece)
	# Display the choice sprites in the game world.
	Main.game_world.add_child(static_node)


# Displays info about the selected upgrade choice.
# 	choice: The choice to display information about.
func show_info(choice: int):
	# Erase previously displayed info.
	remove_info()
	# Create new info sprites.
	dynamic_node = Node2D.new()
	var player_piece: Piece = create_piece(player_pieces[choice], true, get_info_position(true))
	var enemy_piece: Piece = create_piece(enemy_pieces[choice], false, get_info_position(false))
	dynamic_node.append(player_piece)
	dynamic_node.append(enemy_piece)
	# Change the text.
	var p_key: String = ""
	var e_key: String = ""
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
func create_piece(kind: Piece.Kind, player: bool, position: Vector2i) -> Piece:
	var white: bool = (Main.game_state.is_player_white and player) or \
					  (not Main.game_state.is_player_white and not player)
	var piece: Piece = Piece.create_piece(kind, white)
	piece.is_white = white
	piece.is_player = player
	piece.is_grabable = false
	piece.position = position
	return piece
