# A xiangqi chess piece.
extends Piece


# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.PAO


# Takes the piece's turn.
func take_turn():
	# Assume initially that the piece cannot move.
	var _path: Array[Vector2i] = [space.coordinates]
	# Get the possible moves this piece can make.
	var moves: Array = []
	add_orthogonal_paths(_path, 1, 100, false, moves, [])
	# Get the possible captures this piece can make.
	var suspects: Array = []
	add_orthogonal_paths(_path, 1, 100, true, [], suspects)
	# Determine the valid captures.
	var captures: Array = []
	for suspect in suspects:
		var screen: bool = false
		var target: bool = false
		for i in range(1, suspect.size()):
			var coordinates: Vector2i = suspect[i]
			var _space: Space = space.board.get_space(coordinates)
			if (_space != null):
				if (target):
					target = false
					break
				if (screen and _space.has_enemy(is_player)):
					target = true
				if (not screen and not _space.is_enterable(self)):
					screen = true
			elif (not screen):
				screen = true
			else:
				target = false
				break
		if (screen and target):
			captures.append(suspect)
	# Capture a piece if possible.
	if (captures.size() > 0):
		_path = captures[randi_range(0, captures.size() - 1)]
	# Otherwise, move if possible.
	elif (moves.size() > 0):
		_path = moves[randi_range(0, moves.size() - 1)]
	# Move to the new space.
	start_turn(_path)
