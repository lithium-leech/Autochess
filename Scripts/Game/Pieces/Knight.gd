# A classic chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.KNIGHT


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Construct the initial paths.
	var u: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(0, -1), \
							  space.coordinates + Vector2i(0, -2)]
	var d: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(0, 1), \
							  space.coordinates + Vector2i(0, 2)]
	var l: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(-1, 0), \
							  space.coordinates + Vector2i(-2, 0)]
	var r: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(1, 0), \
							  space.coordinates + Vector2i(2, 0)]
	# Get the possible moves and captures this piece can make.
	var moves: Array = []
	var captures: Array = []
	if (is_valid_path(u, true)):
		add_horizontal_paths(u, 1, 1, true, moves, captures)
	if (is_valid_path(d, true)):
		add_horizontal_paths(d, 1, 1, true, moves, captures)
	if (is_valid_path(l, true)):
		add_vertical_paths(l, 1, 1, true, moves, captures)
	if (is_valid_path(r, true)):
		add_vertical_paths(r, 1, 1, true, moves, captures)
	# Capture a piece if possible.
	if (captures.size() > 0):
		_path = captures[randi_range(0, captures.size() - 1)]
	# Otherwise, move if possible.
	elif (moves.size() > 0):
		_path = moves[randi_range(0, moves.size() - 1)]
	# Move to the new space.
	start_turn(_path)
