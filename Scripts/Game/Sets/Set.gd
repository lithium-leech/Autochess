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
