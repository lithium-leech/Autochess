# A tamerlane chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.GIRAFFE


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Construct the initial paths.
	var uru: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(1, -1), \
							  space.coordinates + Vector2i(1, -2)]
	var urr: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(1, -1), \
							  space.coordinates + Vector2i(2, -1)]
	var drr: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(1, 1), \
							  space.coordinates + Vector2i(2, 1)]
	var drd: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(1, 1), \
							  space.coordinates + Vector2i(1, 2)]
	var dld: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(-1, 1), \
							  space.coordinates + Vector2i(-1, 2)]
	var dll: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(-1, 1), \
							  space.coordinates + Vector2i(-2, 1)]
	var ull: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(-1, -1), \
							  space.coordinates + Vector2i(-2, -1)]
	var ulu: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(-1, -1), \
							  space.coordinates + Vector2i(-1, -2)]
	# Get the possible moves and captures this piece can make.
	var moves: Array = []
	var captures: Array = []
	if (is_valid_path(uru, false)):
		add_linear_paths(uru, 0, -1, 100, false, moves, captures)
	if (is_valid_path(urr, false)):
		add_linear_paths(urr, 1, 0, 100, false, moves, captures)
	if (is_valid_path(drr, false)):
		add_linear_paths(drr, 1, 0, 100, false, moves, captures)
	if (is_valid_path(drd, false)):
		add_linear_paths(drd, 0, 1, 100, false, moves, captures)
	if (is_valid_path(dld, false)):
		add_linear_paths(dld, 0, 1, 100, false, moves, captures)
	if (is_valid_path(dll, false)):
		add_linear_paths(dll, -1, 0, 100, false, moves, captures)
	if (is_valid_path(ull, false)):
		add_linear_paths(ull, -1, 0, 100, false, moves, captures)
	if (is_valid_path(ulu, false)):
		add_linear_paths(ulu, 0, -1, 100, false, moves, captures)
	# Capture a piece if possible.
	if (captures.size() > 0):
		_path = captures[randi_range(0, captures.size() - 1)]
	# Otherwise, move if possible.
	elif (moves.size() > 0):
		_path = moves[randi_range(0, moves.size() - 1)]
	# Move to the new space.
	start_turn(_path)
