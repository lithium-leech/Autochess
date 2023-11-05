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

# A collection of prefabricated set icons.
static var _prefabs: Dictionary = {
	Kind.WESTERN: "res://Scenes/Sets/Western.tscn",
	Kind.XIANGQI: "res://Scenes/Sets/Xiangqi.tscn",
	Kind.SHOGI: "res://Scenes/Sets/Shogi.tscn",
	Kind.TAMERLANE: "res://Scenes/Sets/Tamerlane.tscn",
	Kind.MILITARY: "res://Scenes/Sets/Military.tscn"
}

# Creates a new icon for a requested set of pieces.
# 	kind: The kind of piece set to create an icon for.
# 	return: An icon.
static func create_set_icon(kind: Kind) -> Sprite2D:
	var scene: PackedScene = load(_prefabs[kind])
	return scene.instantiate() as Sprite2D
