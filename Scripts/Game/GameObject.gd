# A single game object.
class_name GameObject extends Node2D


# The space this object is placed on.
var space: Space

# True if the object is controlled by the player.
var is_player: bool

# True if the object is white.
var is_white: bool

# True if this object can be picked up by the player.
var is_grabable: bool


# Called every frame.
# 	delta: The elapsed time since the previous frame.
func _process(_delta: float):
	pass


# Determines if this object can be placed in a given space.
#   space: The space to check.
#   return: True if this object can be placed in the given space.
func is_placeable(new_space: Space) -> bool:
	return new_space.in_player_zone()


# Removes this object from the entire game.
func destroy():
	if (space != null):
		space.remove_object(self)
	queue_free()


# Warps the object to the specified position.
#   position: The game world position to warp to.
func warp_to(new_position: Vector2):
	position = new_position
