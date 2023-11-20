# A set containing the pieces seen in a variant of chess
# played in Central Asia.
class_name TamerlaneSet extends Set


# The pieces available in this set.
var pieces: Array = [
	[Piece.Kind.PAWN],
	[Piece.Kind.KING, Piece.Kind.FERZ, Piece.Kind.WAZIR, Piece.Kind.ELEPHANT, Piece.Kind.DABBABA],
	[Piece.Kind.KNIGHT, Piece.Kind.CAMEL, Piece.Kind.SWORDSMAN],
	[Piece.Kind.TALIAH, Piece.Kind.RYUMA],
	[Piece.Kind.ROOK, Piece.Kind.GIRAFFE]
]


# The kind of set this is.
func get_kind() -> Kind:
	return Kind.TAMERLANE


# Gets the pieces available in this set.
func get_pieces() -> Array:
	return pieces


# Gets the kind of piece that begins a map using this set.
# 	return: A kind of piece in this set.
func get_starting_piece() -> Piece.Kind:
	return Piece.Kind.PAWN
