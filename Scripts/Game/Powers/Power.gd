# A single power up.
class_name Power extends GameObject


# An enumeration containing each kind of power.
enum Kind {
	FIRST,
	ROW,
	MINE,
	WALL,
	SHIELD
}


# Must be implemented by inheriting classes.
# The kind of power this is.
func get_kind() -> Kind:
	return Kind.FIRST


# Applies this power to the current game.
func activate():
	pass


# Unapplies this power from the current game.
func deactivate():
	pass
