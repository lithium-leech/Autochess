# A single tile sprite for constructing boards.
class_name Tile


# An enumeration containing each kind of board tile.
enum Kind {
	CORNER_BL,
	CORNER_BLP,
	CORNER_BR,
	CORNER_BRP,
	CORNER_TL,
	CORNER_TLP,
	CORNER_TR,
	CORNER_TRP,
	SIDE_BL,
	SIDE_BLP,
	SIDE_BR,
	SIDE_BRP,
	SIDE_LB,
	SIDE_LT,
	SIDE_RB,
	SIDE_RT,
	SIDE_TL,
	SIDE_TLP,
	SIDE_TR,
	SIDE_TRP,
	SPACE_BLACK,
	SPACE_WHITE
}

# A collection of prefabricated board tiles.
static var prefabs: Dictionary = {
	Kind.CORNER_BL: "res://Scenes/Tiles/CornerBL.tscn",
	Kind.CORNER_BLP: "res://Scenes/Tiles/CornerBLP.tscn",
	Kind.CORNER_BR: "res://Scenes/Tiles/CornerBR.tscn",
	Kind.CORNER_BRP: "res://Scenes/Tiles/CornerBRP.tscn",
	Kind.CORNER_TL: "res://Scenes/Tiles/CornerTL.tscn",
	Kind.CORNER_TLP: "res://Scenes/Tiles/CornerTLP.tscn",
	Kind.CORNER_TR: "res://Scenes/Tiles/CornerTR.tscn",
	Kind.CORNER_TRP: "res://Scenes/Tiles/CornerTRP.tscn",
	Kind.SIDE_BL: "res://Scenes/Tiles/SideBL.tscn",
	Kind.SIDE_BLP: "res://Scenes/Tiles/SideBLP.tscn",
	Kind.SIDE_BR: "res://Scenes/Tiles/SideBR.tscn",
	Kind.SIDE_BRP: "res://Scenes/Tiles/SideBRP.tscn",
	Kind.SIDE_LB: "res://Scenes/Tiles/SideLB.tscn",
	Kind.SIDE_LT: "res://Scenes/Tiles/SideLT.tscn",
	Kind.SIDE_RB: "res://Scenes/Tiles/SideRB.tscn",
	Kind.SIDE_RT: "res://Scenes/Tiles/SideRT.tscn",
	Kind.SIDE_TL: "res://Scenes/Tiles/SideTL.tscn",
	Kind.SIDE_TLP: "res://Scenes/Tiles/SideTLP.tscn",
	Kind.SIDE_TR: "res://Scenes/Tiles/SideTR.tscn",
	Kind.SIDE_TRP: "res://Scenes/Tiles/SideTRP.tscn",
	Kind.SPACE_BLACK: "res://Scenes/Tiles/SpaceBlack.tscn",
	Kind.SPACE_WHITE: "res://Scenes/Tiles/SpaceWhite.tscn"
}

# Creates a requested board tile.
# 	kind: The kind of board tile to create.
# 	return: A tile.
static func create_tile(kind: Kind) -> Sprite2D:
	var scene: PackedScene = load(prefabs[kind])
	return scene.instantiate() as Sprite2D
