# A military chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.COLONEL


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Get the possible moves and captures this piece can make.
	var moves_1: Array = []
	var moves_2: Array = []
	var moves_3: Array = []
	var moves_4: Array = []
	var captures: Array = []
	add_orthogonal_paths(_path, 1, 1, false, moves_1, captures)
	for move in moves_1:
		add_orthogonal_paths(move, 1, 1, false, moves_2, captures)
	for move in moves_2:
		add_orthogonal_paths(move, 1, 1, false, moves_3, captures)
	for move in moves_3:
		add_orthogonal_paths(move, 1, 1, false, moves_4, captures)
	var moves: Array = moves_1 + moves_2 + moves_3 + moves_4
	# Capture a piece if possible.
	if (captures.size() > 0):
		_path = captures[randi_range(0, captures.size() - 1)]
	# Otherwise, move if possible.
	elif (moves.size() > 0):
		_path = moves[randi_range(0, moves.size() - 1)]
	# Move to the new space.
	start_turn(_path)
