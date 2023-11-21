# A game board that pieces can move on.
class_name Board


# An enumeration containing each kind of game board.
enum Kind {
	CLASSIC,
	HOURGLASS,
	ZIG_ZAG,
	CENTER_CROSS,
	SMALL
}

# A collection of board icons.
static var images: Dictionary = {
	Kind.CLASSIC: "res://Sprites/Boards/Classic.png",
	Kind.HOURGLASS: "res://Sprites/Boards/Hourglass.png",
	Kind.ZIG_ZAG: "res://Sprites/Boards/ZigZag.png",
	Kind.CENTER_CROSS: "res://Sprites/Boards/CenterCross.png",
	Kind.SMALL: "res://Sprites/Boards/Small.png"
}

# Gets an icon for the requested game board.
# 	kind: The kind of game board to get an icon for.
# 	return: An icon.
static func get_icon(kind: Kind) -> CompressedTexture2D:
	return load(images[kind])


# The Game that this board exists in.
var game: Game

# The number of spaces going horizontally.
var width: int

# The number of spaces going vertically.
var height: int

# The board spaces and the objects occupying them.
# The intended type is Array[Array[Space]].
var spaces

# The number of rows (starting from the bottom) that the player can use.
var player_rows: int

# The number of rows (starting from the top) that the enemy can use.
var enemy_rows: int

# The pieces controlled by the player.
var player_pieces: Array[Piece]

# The pieces controlled by the enemy.
var enemy_pieces: Array[Piece]

# The game world position for this board's top-left corner.
var corner_tl: Vector2i

# The game world position for this board's bottom-right corner.
var corner_br: Vector2i

# A node containing tile sprites making the board visible.
var tile_node: Node2D


# Adds an object at the given space.
# 	object: The object to add.
# 	space: The space to place the object at.
func add_object(object: GameObject, space: Space):
	if (space != null):
		space.add_object(object)


# Destroys all objects on the board.
func clear():
	for x in range(0, width):
		for y in range(0, height):
			var space: Space = get_space(Vector2i(x, y))
			if (space != null): space.clear()


# Checks if there are any moving pieces on the board.
# 	return: True if any pieces on the board are actively moving.
func are_pieces_moving() -> bool:
	for piece in player_pieces:
		if (piece.is_moving):
			return true
	for piece in enemy_pieces:
		if (piece.is_moving):
			return true
	return false


# Gets the first empty space on the board.
# 	return: A board space.
# 	remark: Returns null if there is no empty space.
func get_first_empty_space() -> Space:
	if (Main.atlas.in_rtl):
		for y in range(height-1, -1, -1):
			for x in range(width-1, -1, -1):
				if (spaces[x][y].is_empty()):
					return spaces[x][y]
	else:
		for y in range(height-1, -1, -1):
			for x in range(0, width):
				if (spaces[x][y].is_empty()):
					return spaces[x][y]
	return null


# Checks if a set of coordinates is on the board.
# 	coordinates: The coordinates to check.
# 	return: True if the coordinates are on the board.
func on_board(coordinates: Vector2i) -> bool:
	return coordinates.x >= 0 and coordinates.x < width and coordinates.y >= 0 and coordinates.y < height


# Gets a space on the board.
# 	coordinates: The desired board space's coordinates.
# 	return: The desired space, or null if the coordinates are invalid.
func get_space(coordinates: Vector2i) -> Space:
	if (on_board(coordinates)):
		return spaces[coordinates.x][coordinates.y]
	else:
		return null


# Gets a board space's coordinates from a given game world position.
# 	position: The game world position to get coordinates for.
# 	return: A board space's coordinates.
func to_coordinates(position: Vector2i) -> Vector2i:
	return Vector2i(floor((position.x - corner_tl.x) / 32.0), floor((position.y - corner_tl.y) / 32.0))


# Gets the game world position for a set of coordinates.
# 	coordinates: The coordinates to get a position for.
# 	returns: A game world position.
func to_position(coordinates: Vector2i) -> Vector2i:
	return Vector2i(corner_tl.x + (32 * coordinates.x) + 16, corner_tl.y + (32 * coordinates.y) + 16)


# Destroys this game board.
func destroy():
	clear()
	tile_node.queue_free()
