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
		place_item_randomly(game.game_board, item)


# Places a single piece on a random space.
# 	board: The board to place the piece on.
# 	kind: The kind of piece to place.
static func place_piece_randomly(board: Board, kind: Piece.Kind):
	var piece: Piece = Piece.create_piece(kind, not Main.game_state.is_player_white)
	piece.is_player = false
	piece.is_white = not Main.game_state.is_player_white
	piece.is_grabable = false
	Main.game_world.add_child(piece)
	# Flip pieces with directional sprites.
	var flip = func():
		piece.flip_v = true
	match kind:
		Piece.Kind.FUHYO: flip.call()
		Piece.Kind.GINSHO: flip.call()
		Piece.Kind.HISHA: flip.call()
		Piece.Kind.KAKUGYO: flip.call()
		Piece.Kind.KEIMA: flip.call()
		Piece.Kind.KINSHO: flip.call()
		Piece.Kind.KYOSHA: flip.call()
		Piece.Kind.NARIGIN: flip.call()
		Piece.Kind.NARIKEI: flip.call()
		Piece.Kind.NARIKYO: flip.call()
		Piece.Kind.OSHO: flip.call()
		Piece.Kind.RYUMA: flip.call()
		Piece.Kind.RYUO: flip.call()
		Piece.Kind.TOKIN: flip.call()
		Piece.Kind.ZU: flip.call()
		Piece.Kind.JU: flip.call()
		Piece.Kind.MA: flip.call()
		Piece.Kind.XIANG: flip.call()
		Piece.Kind.JIANG: flip.call()
		Piece.Kind.SHI: flip.call()
		Piece.Kind.PAO: flip.call()
	board.add_object(piece, get_random_enemy_space(board))


# Places a single item on a random space.
# 	board: The board to place the item on.
# 	kind: The kind of item to place.
static func place_item_randomly(board: Board, kind: Item.Kind):
	var item: Item = Item.create_item(kind, not Main.game_state.is_player_white)
	item.is_player = false
	item.is_white = not Main.game_state.is_player_white
	item.is_grabable = false
	Main.game_world.add_child(item)
	match kind:
		Item.Kind.SHIELD:
			board.add_object(item, get_random_piece_space(board))
		_:
			board.add_object(item, get_random_non_player_space(board))


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

	
# Gets a random empty space in the enemy placement zone or neutral zone.
# 	board: The board to get a space from.
# 	return: A space.
static func get_random_non_player_space(board: Board):
	var empty_spaces: Array[Space] = []
	for x in range(board.width):
		for y in range(board.height - board.player_rows):
			var space: Space = board.get_space(Vector2i(x, y))
			if (space != null and space.is_empty()):
				empty_spaces.append(space)
	if (empty_spaces.size() < 1):
		return null
	else:
		return empty_spaces[randi_range(0, empty_spaces.size() - 1)]


# Gets a random space with an enemy piece.
# 	board: The board to get a space from.
# 	return: A space.
static func get_random_piece_space(board: Board):
	var empty_spaces: Array[Space] = []
	for x in range(board.width):
		for y in range(board.height):
			var space: Space = board.get_space(Vector2i(x, y))
			if (space != null and space.has_ally(false)):
				empty_spaces.append(space)
	if (empty_spaces.size() < 1):
		return null
	else:
		return empty_spaces[randi_range(0, empty_spaces.size() - 1)]
