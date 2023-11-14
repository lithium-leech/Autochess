# A class containing methods for the enemy's decision making during piece placement.
class_name PlacementAI


# Sets up the enemy's pieces on the game board.
# 	game: The game to set up the enemy's pieces in.
static func set_up_enemy(game: Game):
	var enemy_pieces: Array[Piece.Kind] = game.enemy_pieces.duplicate(true)
	var enemy_items: Array[Item.Kind] = game.enemy_items.duplicate(true)
	enemy_pieces.reverse()
	enemy_items.reverse()
	for piece in enemy_pieces:
		place_piece_randomly(game.game_board, piece)
	for item in enemy_items:
		pass


# Places a single piece on a random space.
# 	board: The board to place the piece on.
# 	kind: The kind of piece to place.
static func place_piece_randomly(board: Board, kind: Piece.Kind):
	board.add_piece(kind, false, get_random_enemy_space(board))


# Gets a random empty space in the enemy placement zone.
# 	board: The board to get a space from.
# 	return: A space.
static func get_random_enemy_space(board: Board):
	var empty_spaces: Array[Space] = []
	for x in range(board.width):
		for y in range(board.enemy_rows):
			var space: Space = board.get_space(Vector2i(x, y))
			if (space != null and space.is_empty()):
				empty_spaces.append(space)
	if (empty_spaces.size() < 1):
		return null
	else:
		return empty_spaces[randi_range(0, empty_spaces.size() - 1)]
