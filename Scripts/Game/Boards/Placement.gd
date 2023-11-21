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
			record.item = object.equipment.kind
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
		# Create the piece's equipment
		if (item != Item.Kind.NONE):
			pass
	# Create the recorded item.
	else:
		object = Item.create_item(item, white)
		object.is_player = is_player
		object.is_white = white
		object.is_grabable = is_player
		Main.game_world.add_child(object)
	# Place the object on the recorded space.
	var space: Space
	if (is_game):
		space = game.game_board.get_space(Vector2i(x, y))
	else:
		space = game.side_board.get_space(Vector2i(x, y))
	space.add_object(object)
