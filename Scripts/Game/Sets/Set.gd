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


# Gets the icon for a requested set of pieces.
# 	set_kind: The kind of piece set to get an icon for.
# 	return: An icon.
static func get_set_icon(set_kind: Kind) -> Sprite2D:
	var scene: PackedScene = load(_prefabs[set_kind])
	return scene.instantiate() as Sprite2D
