# An equipment that protects its wearer for a duration.
extends Equipment


# The number of remaining rounds of protection.
var protection_rounds: int = 6


# Must be implemented by inheriting classes.
# The kind of item this is.
func get_kind() -> Kind:
	return Kind.SHIELD


# Performs additional behaviors when equipped.
func on_equip():
	space.board.game.on_round_finish.connect(lower_duration)


# Performs additional behaviors when unequipped.
func on_unequip():
	space.board.game.on_round_finish.disconnect(lower_duration)


# Checks if the equipped piece is protected from the given piece.
# 	piece: The attacking piece.
# 	return: True if the equipped piece is protected.
func is_protected(_piece: Piece):
	return true


# Lowers the remaining rounds of protection.
func lower_duration():
	if (protection_rounds > 0):
		protection_rounds -= 1
	if (protection_rounds < 1):
		space.board.game.on_move_finish.connect(self.destroy, CONNECT_ONE_SHOT)
