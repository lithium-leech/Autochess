# A set of pieces that can be offered to the player.
class_name Set


# An enumeration containing each kind of piece set.
enum Kind {
	WESTERN,
	XIANGQI,
	SHOGI,
	TAMERLANE,
	MILITARY
}

# A collection of set icons.
static var images: Dictionary = {
	Kind.WESTERN: "res://Sprites/Sets/Western.png",
	Kind.XIANGQI: "res://Sprites/Sets/Xiangqi.png",
	Kind.SHOGI: "res://Sprites/Sets/Shogi.png",
	Kind.TAMERLANE: "res://Sprites/Sets/Tamerlane.png",
	Kind.MILITARY: "res://Sprites/Sets/Military.png"
}

# Gets the icon for a requested set of pieces.
# 	kind: The kind of piece set get an icon for.
# 	return: An icon.
static func get_icon(kind: Kind) -> CompressedTexture2D:
	return load(images[kind])


# Gets a set containing kinds of pieces to choose from.
# 	kind: The kind of set to get.
# 	return: A set containing kinds of pieces.
static func get_set(kind: Kind) -> Set:
	match kind:
		Kind.WESTERN:
			return WesternSet.new()
		Kind.XIANGQI:
			return XiangqiSet.new()
		Kind.SHOGI:
			return ShogiSet.new()
		Kind.TAMERLANE:
			return TamerlaneSet.new()
		Kind.MILITARY:
			return MilitarySet.new()
	return null


# Must be implemented by inheriting classes.
# The kind of set this is.
func get_kind() -> Kind:
	return Kind.WESTERN


# Must be implemented by inheriting classes.
# Gets the pieces available in this set.
func get_pieces() -> Array:
	return []


# Must be implemented by inheriting classes.
# Gets the kind of piece that begins a map using this set.
# 	return: A kind of piece in this set.
func get_starting_piece() -> Piece.Kind:
	return Piece.Kind.NONE


# Gets a single kind of piece in this set.
# 	strength: The desired strength of the piece (on a scale of 0 to 1).
# 	return: A kind of piece in this set.
func get_piece(strength: float) -> Piece.Kind:
	# Choose a piece value.
	var pieces: Array = get_pieces()
	var min_v: int = 0
	var max_v: int = pieces.size() - 1
	var expected: int = round(strength * max_v)
	var roll: int = randi_range(min_v, max_v) + randi_range(min_v, max_v)
	var result: int = clamp(expected + (roll - max_v), min_v, max_v)
	# Pick a piece with the chosen value.
	return pieces[result][randi_range(0, pieces[result].size() - 1)]


# Gets the value of a given kind of piece in this set.
# 	kind: The kind of piece to evaluate.
# 	return: An integer value.
func get_piece_value(kind: Piece.Kind) -> int:
	var pieces: Array = get_pieces()
	for i in range(pieces.size()):
		for piece in pieces[i]:
			if (piece == kind):
				return i
	# Return an extremely high value for pieces not in the set.
	return 1000
