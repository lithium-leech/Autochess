# A shogi  chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.KEIMA


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Construct the initial paths.
	var u: Array[Vector2i] = [space.coordinates, \
							  space.coordinates + Vector2i(0, -1), \
							  space.coordinates + Vector2i(0, -2)]
	# Get the possible moves and captures this piece can make.
	var moves: Array = []
	var captures: Array = []
	if (is_valid_path(u, true)):
		add_horizontal_paths(u, 1, 1, true, moves, captures)
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
		promotion = Kind.NARIKEI
