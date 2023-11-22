# A shogi chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.GINSHO


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Determine facing.
	var yi: int = -1 if is_player else 1
	# Get the possible moves and captures this piece can make.
	var moves: Array = []
	var captures: Array = []
	add_diagonal_paths(_path, 1, 1, false, moves, captures)
	add_linear_paths(_path, 0, yi, 1, false, moves, captures)
	# Capture a piece if possible.
	if (captures.size() > 0):
		_path = captures[randi_range(0, captures.size() - 1)]
	# Otherwise, move if possible.
	elif (moves.size() > 0):
		_path = moves[randi_range(0, moves.size() - 1)]
	# Move to the new space.
	start_turn(_path)
	# Check for promotion.
	var destination: Space = space.board.get_space(_path[_path.size() - 1])
	if ((is_player and destination.is_player_promotion) or \
		(!is_player and destination.is_enemy_promotion)):
		promotion = Kind.NARIGIN
