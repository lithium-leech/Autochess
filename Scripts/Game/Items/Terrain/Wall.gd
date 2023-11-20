# A terrain that blocks movement.
extends Terrain


# Must be implemented by inheriting classes.
# The kind of item this is.
func get_kind() -> Kind:
	return Kind.WALL


# Determines if this terrain can be entered by the given object.
# 	object: The object to enter the terrain.
# 	return: True if this terrain can be entered.
func is_enterable(_object: GameObject) -> bool:
	return false
