# A terrain that explodes when touched.
extends Terrain


# The piece that touches the mine.
var victim: Piece = null


# Must be implemented by inheriting classes.
# The kind of item this is.
func get_kind() -> Kind:
	return Kind.MINE


# Determines if this terrain can be entered by the given object.
# 	object: The object to enter the terrain.
# 	return: True if this terrain can be entered.
func is_enterable(object: GameObject) -> bool:
	return object.is_player != self.is_player


# Performs the effects of entering this terrain.
# 	piece: The piece entering.
func on_enter(piece: Piece):
	victim = piece
	space.board.game.on_move_finish.connect(explode, CONNECT_ONE_SHOT)


# The mine explodes and destroys the piece that touched it.
func explode():
	victim.destroy()
	self.destroy()
