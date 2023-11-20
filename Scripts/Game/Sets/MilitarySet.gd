# A set containing custom pieces based on U.S. military ranks.
class_name MilitarySet extends Set


# The pieces available in this set.
var pieces: Array = [
	[Piece.Kind.PRIVATE],
	[Piece.Kind.GENERAL],
	[Piece.Kind.SERGEANT],
	[Piece.Kind.LIEUTENANT],
	[Piece.Kind.CAPTAIN],
	[Piece.Kind.COLONEL]
]


# The kind of set this is.
func get_kind() -> Kind:
	return Kind.MILITARY


# Gets the pieces available in this set.
func get_pieces() -> Array:
	return pieces


# Gets the kind of piece that begins a map using this set.
# 	return: A kind of piece in this set.
func get_starting_piece() -> Piece.Kind:
	return Piece.Kind.PRIVATE
