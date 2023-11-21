# A record of an object placed on a board.
class_name Placement


# Records the placement of a given object on a given space.
# 	object: The placed object.
# 	coordinates: The coordinates of the placed on space.
# 	is_game: True if this is a game board placement, false for the side board.
# 	return: A new placement record.
static func record_object(object: GameObject, coordinates: Vector2i, _is_game: bool) -> Placement:
	var record: Placement = Placement.new()
	# Record the object
	record.is_player = object.is_player
	if (object is Piece):
		record.is_piece = true
		record.piece = object.get_kind()
		if (object.equipment != null):
			record.item = object.equipment.get_kind()
	elif (object is Item):
		record.is_piece = false
		record.item = object.get_kind()
	# Record the space
	record.is_game = _is_game
	record.x = coordinates.x
	record.y = coordinates.y
	# Return the record
	return record


# Records the placement of a new piece on a given space.
# 	kind: The kind of piece to place.
# 	coordinates: The coordinates of the placed on space.
# 	is_game: True if this is a game board placement, false for the side board.
# 	return: A new placement record.
static func record_new_piece(kind: Piece.Kind, coordinates: Vector2i, _is_game: bool) -> Placement:
	var record: Placement = Placement.new()
	# Record the piece
	record.piece = kind
	# Record the space
	record.is_game = _is_game
	record.x = coordinates.x
	record.y = coordinates.y
	# Return the record
	return record


# Records the placement of a new item on a given space.
# 	kind: The kind of item to place.
# 	coordinates: The coordinates of the placed on space.
# 	is_game: True if this is a game board placement, false for the side board.
# 	return: A new placement record.
static func record_new_item(kind: Item.Kind, coordinates: Vector2i, _is_game: bool) -> Placement:
	var record: Placement = Placement.new()
	# Record the piece
	record.item = kind
	record.is_piece = false
	# Record the space
	record.is_game = _is_game
	record.x = coordinates.x
	record.y = coordinates.y
	# Return the record
	return record


# True if the placed object belongs to the player.
var is_player: bool = true

# True if the placed object is a piece, false for an item.
var is_piece: bool = true

# True if this is a game board placement, false for the side board.
var is_game: bool = true

# The kind of piece.
var piece: Piece.Kind = Piece.Kind.NONE

# The kind of item.
var item: Item.Kind = Item.Kind.NONE

# The x coordinate of the placed on space.
var x: int = 0

# The y coordinate of the placed on space.
var y: int = 0


# Places the recorded object onto its recorded space.
# 	game_board: The game to place the object in.
func place_recorded_object(game: Game):
	# Get the recorded space.
	var space: Space = null
	if (is_game):
		space = game.game_board.get_space(Vector2i(x, y))
	else:
		space = game.side_board.get_space(Vector2i(x, y))
	# Check that the space is empty.
	if (space != null and space.is_empty()):
		# Create the recorded piece.
		var object: GameObject
		var white: bool = (Main.game_state.is_player_white and is_player) or \
						(not Main.game_state.is_player_white and not is_player)
		if (is_piece):
			object = Piece.create_piece(piece, white)
			object.is_player = is_player
			object.is_white = white
			object.is_grabable = is_player
			Main.game_world.add_child(object)
			space.add_object(object)
			# Create the piece's equipment
			if (item != Item.Kind.NONE):
				var equip: Equipment = Item.create_item(item, white)
				equip.is_player = is_player
				equip.is_white = white
				equip.is_grabable = is_player
				Main.game_world.add_child(equip)
				space.add_object(equip)
		# Create the recorded item.
		else:
			object = Item.create_item(item, white)
			object.is_player = is_player
			object.is_white = white
			object.is_grabable = is_player
			Main.game_world.add_child(object)
			space.add_object(object)


# Gets the first space on the side board that isn't taken by another placement record.
# 	game: The game to look for an empty space in.
# 	return: The first free space.
static func get_first_empty_side_board_space(game: Game) -> Space:
	# Create a boolean mapping which shows the free spaces.
	var width: int = game.side_board.width
	var height: int = game.side_board.height
	var free: Array = []
	for _x in range(width):
		free.append([])
		for _y in range(height):
			var space: Space = game.side_board.get_space(Vector2i(_x, _y))
			free[_x].append(space != null)
	# Go through the given records and mark taken spaces.
	for record in game.player_placements:
		if (not record.is_game):
			free[record.x][record.y] = false
	# Return the first free space.
	if (Main.atlas.in_rtl):
		for _y in range(height-1, -1, -1):
			for _x in range(width-1, -1, -1):
				if (free[_x][_y]):
					return game.side_board.get_space(Vector2i(_x, _y))
	else:
		for _y in range(height-1, -1, -1):
			for _x in range(0, width):
				if (free[_x][_y]):
					return game.side_board.get_space(Vector2i(_x, _y))
	return null
