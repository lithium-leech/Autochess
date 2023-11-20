# A set containing the pieces seen in a variant of chess
# played in China.
class_name XiangqiSet extends Set


# The pieces available in this set.
var pieces: Array = [
	[Piece.Kind.ZU],
	[Piece.Kind.JIANG, Piece.Kind.SHI],
	[Piece.Kind.MA, Piece.Kind.XIANG],
	[Piece.Kind.PAO],
	[Piece.Kind.JU]
]


# Gets the pieces available in this set.
func get_pieces() -> Array:
	return pieces


# Gets the kind of piece that begins a map using this set.
# 	return: A kind of piece in this set.
func get_starting_piece() -> Piece.Kind:
	return Piece.Kind.ZU
