# A set containing the pieces seen in the variant of chess
# played throughout the "western" world.
class_name WesternSet extends Set


# The pieces available in this set.
var pieces: Array = [
	[Piece.Kind.PAWN],
	[Piece.Kind.KING],
	[Piece.Kind.KNIGHT],
	[Piece.Kind.BISHOP],
	[Piece.Kind.ROOK],
	[Piece.Kind.QUEEN]
]


# The kind of set this is.
func get_kind() -> Kind:
	return Kind.WESTERN


# Gets the pieces available in this set.
func get_pieces() -> Array:
	return pieces


# Gets the kind of piece that begins a map using this set.
# 	return: A kind of piece in this set.
func get_starting_piece() -> Piece.Kind:
	return Piece.Kind.PAWN
