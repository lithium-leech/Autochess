# A classic chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.PAWN


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Get the possible moves and captures this piece can make.
	var moves: Array = []
	var captures: Array = []
	var move_space: Space = get_move_space()
	if (move_space != null and move_space.is_enterable(self)):
		var move_path: Array[Vector2i] = _path.duplicate()
		move_path.append(move_space.coordinates)
		moves.append(move_path)
	var left_attack: Space = get_left_attack_space()
	if (left_attack != null and left_attack.has_capturable(self)):
		var left_path: Array[Vector2i] = _path.duplicate()
		left_path.append(left_attack.coordinates)
		captures.append(left_path)
	var right_attack: Space = get_right_attack_space()
	if (right_attack != null and right_attack.has_capturable(self)):
		var right_path: Array[Vector2i] = _path.duplicate()
		right_path.append(right_attack.coordinates)
		captures.append(right_path)
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
		promotion = Kind.QUEEN


# Gets the space in front of the pawn.
# 	return: The space.
func get_move_space() -> Space:
	var coordinates: Vector2i
	if (is_player):
		coordinates = space.coordinates + Vector2i(0, -1)
	else:
		coordinates = space.coordinates + Vector2i(0, 1)
	return space.board.get_space(coordinates)


# Gets the left-diagonal space in front of the pawn.
# 	return: The space.
func get_left_attack_space() -> Space:
	var coordinates: Vector2i
	if (is_player):
		coordinates = space.coordinates + Vector2i(-1, -1)
	else:
		coordinates = space.coordinates + Vector2i(-1, 1)
	return space.board.get_space(coordinates)


# Gets the right-diagonal space in front of the pawn.
# 	return: The space.
func get_right_attack_space() -> Space:
	var coordinates: Vector2i
	if (is_player):
		coordinates = space.coordinates + Vector2i(1, -1)
	else:
		coordinates = space.coordinates + Vector2i(1, 1)
	return space.board.get_space(coordinates)
