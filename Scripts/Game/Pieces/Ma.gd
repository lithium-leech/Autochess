# A chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.MA


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Construct the initial paths.
	var u: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(0, -1)]
	var d: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(0, 1)]
	var l: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(-1, 0)]
	var r: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(1, 0)]
	# Get the possible moves and captures this piece can make.
	var moves: Array = []
	var captures: Array = []
	if (is_valid_path(u, false)):
		add_linear_paths(u, -1, -1, 1, false, moves, captures)
		add_linear_paths(u, 1, -1, 1, false, moves, captures)
	if (is_valid_path(d, false)):
		add_linear_paths(d, -1, 1, 1, false, moves, captures)
		add_linear_paths(d, 1, 1, 1, false, moves, captures)
	if (is_valid_path(l, false)):
		add_linear_paths(l, -1, -1, 1, false, moves, captures)
		add_linear_paths(l, -1, 1, 1, false, moves, captures)
	if (is_valid_path(r, false)):
		add_linear_paths(r, 1, -1, 1, false, moves, captures)
		add_linear_paths(r, 1, 1, 1, false, moves, captures)
	# Capture a piece if possible.
	if (captures.size() > 0):
		_path = captures[randi_range(0, captures.size() - 1)]
	# Otherwise, move if possible.
	elif (moves.size() > 0):
		_path = moves[randi_range(0, moves.size() - 1)]
	# Move to the new space.
	start_turn(_path)
