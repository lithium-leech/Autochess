# A set containing the pieces seen in a variant of chess
# played in Japan.
class_name ShogiSet extends Set


# The pieces available in this set.
var pieces: Array = [
	[Piece.Kind.FUHYO],
	[Piece.Kind.GINSHO, Piece.Kind.KEIMA],
	[Piece.Kind.OSHO, Piece.Kind.KINSHO, Piece.Kind.NARIGIN, Piece.Kind.NARIKEI, Piece.Kind.NARIKYO, Piece.Kind.TOKIN],
	[Piece.Kind.KAKUGYO, Piece.Kind.RYUMA],
	[Piece.Kind.KYOSHA, Piece.Kind.HISHA, Piece.Kind.RYUO]
]


# Gets the pieces available in this set.
func get_pieces() -> Array:
	return pieces


# Gets the kind of piece that begins a map using this set.
# 	return: A kind of piece in this set.
func get_starting_piece() -> Piece.Kind:
	return Piece.Kind.FUHYO
