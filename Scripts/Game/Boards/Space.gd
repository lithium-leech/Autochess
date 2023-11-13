# A single space on the game board.
class_name Space


# Creates a new instance of a Space.
#   board: The board that this space is a part of.
#   x: The x coordinate of this space.
#   y: The y coordinate of this space.
func _init(_board: Board, _x: int, _y: int):
	board = _board
	x = _x
	y = _y


# The board that this space is a part of.
var board: Board

# This space's x coordinate.
var x: int

# This space's y coordinate.
var y: int

# This space's coordinates.
var coordinates: Vector2i:
	get:
		return Vector2i(x, y)

# This space's world position.
var position: Vector2:
	get:
		return board.to_position(coordinates)

# True if enemy pieces can be promoted on this space.
var is_enemy_promotion: bool = false

# True if player pieces can be promoted on this space.
var is_player_promotion: bool = false

# The object occupying this space (null when there is none).
var object: GameObject = null

# The terrain applied to this space (null when there is none).
var terrain: Terrain = null


# Goes through the process of a piece entering this space.
#   piece: The piece that will enter this space.
func enter(piece: Piece):
	# Capture the existing piece.
	if (object != null):
		piece.captured = object
	# Enter the terrain.
	if (terrain != null):
		terrain.enter(piece)
	# Add the piece to this space.
	add_object(piece)


# Goes through the process of a piece exiting this space.
#   piece: The piece that will exit this space.
func exit(piece: Piece):
	# Check that the given piece is on this space.
	if (object != piece):
		return
	# Exit the terrain.
	if (terrain != null):
		terrain.exit(piece)
	# Remove the piece from this space.
	remove_object(piece)


# Picks up an object from this space.
#   return: The first grabable object, or null if there is none.
func grab() -> GameObject:
	var grabbed: GameObject = null
	# Grab any equipment.
	# if (object is Piece and object.equipment != null):
	#	return object.unequip()
	# Grab any game object.
	if (object != null):
		grabbed = object
	# Grab any terrain.
	elif (terrain != null):
		grabbed = terrain
	# Remove and return the grabbed object
	remove_object(grabbed)
	return grabbed


# Adds an object to this space.
#   object: The object to add.
func add_object(_object: GameObject):
	# Check if the object can be added.
	if (not is_enterable(_object)):
		return false
	# Update the object.
	_object.space = self
	# Add the object as a piece.
	if (_object is Piece):
		if (_object.is_player):
			board.player_pieces.append(_object)
		else:
			board.enemy_pieces.append(_object)
		object = _object;
		_object.warp_to(position)
	# Add the object as an equipment.
	elif (_object is Equipment and object is Piece and object.equipment == null):
		_object.equip_to(object)
	# Add the object as a terrain.
	elif (_object is Terrain):
		terrain = _object
		_object.warp_to(position)
	# Add the object as is.
	else:
		object = _object
		_object.warp_to(position)


# Removes an object from this space.
#   object: The object to remove.
func remove_object(_object: GameObject):
	# Remove a piece.
	if (_object is Piece):
		if (object != _object):
			return
		if (_object.is_player):
			board.player_pieces.erase(_object)
		else:
			board.enemy_pieces.erase(_object)
		object = null
	# Remove an equipment.
	elif (_object is Equipment and object is Piece):
		if (object.equipment != _object):
			return
		object.equipment = null;
	# Remove a terrain.
	elif (_object is Terrain):
		if (terrain != _object):
			return
		terrain = null;
	# Remove an object.
	else:
		if (object != _object):
			return
		object = null


# Destroys all objects on this space.
func clear():
	# Clear objects.
	if (object != null):
		object.destroy()
		object = null
	# Clear terrain.
	if (terrain != null):
		terrain.destroy()
		terrain = null;


# Checks if this space is empty.
#   return: True if this space is empty.
func is_empty() -> bool:
	return object == null and terrain == null


# Checks if this space can be entered by a given object.
#   object: The object to check.
#   return: True if the given object can enter this space.
func is_enterable(_object: GameObject):
	if (object != null and _object is Equipment and object is Piece and object.equipment == null):
		return true
	elif (object != null):
		return false
	elif (terrain != null and _object is Terrain):
		return false
	elif (terrain != null and not terrain.is_enterable(_object)):
		return false
	else:
		return true


# Checks if this space has a piece on it.
#   return: True if this space has a piece on it.
func has_piece() -> bool:
	if (object == null):
		return false
	elif (object is Piece):
		return true
	else:
		return false


# Checks if this space has an ally piece on it.
#   player: True if player pieces are allies.
#   return: True if this space has an ally piece on it.
func has_ally(player: bool) -> bool:
	if (not has_piece()):
		return false
	elif (object.is_player == player):
		return true
	else:
		return false


# Checks if this space has an enemy piece on it.
#   player: True if non-player pieces are enemies.
#   return: True if this space has an enemy piece on it.
func has_enemy(player: bool) -> bool:
	if (not has_piece()):
		return false
	elif (object.is_player == player):
		return false
	else:
		return true


# Checks if this space has a capturable piece.
#   piece: The piece that wants to find a capture.
#   return: True if this space has a capturable piece.
func has_capturable(piece: Piece):
	if (not has_enemy(piece.is_player)):
		return false
	else:
		return object.is_capturable(piece)


# Checks if this space is in the player zone.
#   return: True if this space is in the player zone.
func in_player_zone() -> bool:
	return board.height - y  <= board.player_rows

# Checks if this space is in the enemy zone.
#   return: True if this space is in the enemy zone.
func in_enemy_zone() -> bool:
	return y < board.enemy_rows

# Checks if this space is in the neutral zone.
#   return: True if this space is in the neutral zone.
func in_neutral_zone() -> bool:
	return y >= board.enemy_rows and board.height - y > board.player_rows

# Gets a space in a position relative to this one.
#   dx: The relative horizontal distance.
#   dy: The relative vertical distance.
#   return: A space relative to this one on the same board.
func get_relative_space(dx: int, dy: int) -> Space:
	return board.get_space(coordinates + Vector2i(dx, dy))
