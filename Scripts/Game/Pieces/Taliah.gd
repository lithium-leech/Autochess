# A tamerlane chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.TALIAH


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Construct the initial paths.
	var ur: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(1, -1)]
	var dr: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(1, 1)]
	var dl: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(-1, 1)]
	var ul: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(-1, -1)]
	# Get the possible moves and captures this piece can make.
	var moves: Array = []
	var captures: Array = []
	if (is_valid_path(ur, false)):
		add_linear_paths(ur, 1, -1, 100, false, moves, captures)
	if (is_valid_path(dr, false)):
		add_linear_paths(dr, 1, 1, 100, false, moves, captures)
	if (is_valid_path(dl, false)):
		add_linear_paths(dl, -1, 1, 100, false, moves, captures)
	if (is_valid_path(ul, false)):
		add_linear_paths(ul, -1, -1, 100, false, moves, captures)
	# Capture a piece if possible.
	if (captures.size() > 0):
		_path = captures[randi_range(0, captures.size() - 1)]
	# Otherwise, move if possible.
	elif (moves.size() > 0):
		_path = moves[randi_range(0, moves.size() - 1)]
	# Move to the new space.
	start_turn(_path)
