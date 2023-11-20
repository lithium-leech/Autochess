# An item that applies additional properties to a single piece.
class_name Equipment extends Item


# The sprite shown when this is unequipped.
var item_sprite: String

# The sprite shown when this is equipped.
var equip_sprite: String

# The piece this is equipped to.
var piece: Piece


# Called every frame.
# 	delta: The elapsed time since the previous frame.
func _process(_delta: float):
	# Follow the equipped piece.
	if (piece != null):
		warp_to(piece.position)


# Determines if this object can be placed in a given space.
#   space: The space to check.
#   return: True if this object can be placed in the given space.
func is_placeable(new_space: Space) -> bool:
	return new_space.has_ally(self.is_player)


# Equips this to a given piece.
# 	piece: The piece to equip this to.
func equip(_piece: Piece):
	# Attach this to the given piece.
	piece = _piece
	piece.equipment = self
	on_equip()
	# Replace the item sprite with the equipped sprite.
	texture = load(equip_sprite) as CompressedTexture2D


# Unequips this from its equipped piece.
func unequip():
	# Remove this from the equipped piece.
	on_unequip()
	piece.equipment = null
	piece = null
	# Replace the equipped sprite with the item sprite.
	texture = load(item_sprite) as CompressedTexture2D


# Performs additional behaviors when equipped.
func on_equip():
	pass


# Performs additional behaviors when unequipped.
func on_unequip():
	pass


# Removes this object from the entire game.
func destroy():
	if (piece != null):
		unequip()
	if (space != null):
		space.remove_object(self)
	queue_free()


# Checks if the equipped piece is protected from the given piece.
# 	piece: The attacking piece.
# 	return: True if the equipped piece is protected.
func is_protected(_piece: Piece):
	return false
