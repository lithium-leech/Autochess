# A single game item.
class_name Item extends GameObject


# An enumeration containing each kind of item.
enum Kind {
	NONE,
	WALL,
	MINE,
	SHIELD
}


# Must be implemented by inheriting classes.
# The kind of item this is.
func get_kind() -> Kind:
	return Kind.NONE
