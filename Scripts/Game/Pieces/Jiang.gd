# A chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.JIANG


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Get the possible moves and captures this piece can make.
	var moves: Array = []
	var captures: Array = []
	add_orthogonal_paths(_path, 1, 1, false, moves, captures)
	# Capture a piece if possible.
	if (captures.size() > 0):
		_path = captures[randi_range(0, captures.size() - 1)]
	# Otherwise, move if possible.
	elif (moves.size() > 0):
		_path = moves[randi_range(0, moves.size() - 1)]
	# Move to the new space.
	start_turn(_path)
