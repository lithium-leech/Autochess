# An item that applies additional properties to a single space on the game board.
class_name Terrain extends Item


# Determines if this object can be placed in a given space.
#   space: The space to check.
#   return: True if this object can be placed in the given space.
func is_placeable(new_space: Space) -> bool:
	return new_space.in_player_zone() or new_space.in_neutral_zone()


# Determines if this terrain can be entered by the given object.
# 	object: The object to enter the terrain.
# 	return: True if this terrain can be entered.
func is_enterable(_object: GameObject) -> bool:
	return true


# Performs the effects of entering this terrain.
# 	piece: The piece entering.
func enter(_piece: Piece):
	pass


# Performs the effects of exiting this terrain.
# 	piece: The piece exiting.
func exit(_piece:Piece):
	pass
