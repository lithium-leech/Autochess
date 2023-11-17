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

# A collection of board tile images.
static var images: Dictionary = {
	Kind.CORNER_BL: "res://Sprites/Tiles/CornerBL.png",
	Kind.CORNER_BLP: "res://Sprites/Tiles/CornerBLP.png",
	Kind.CORNER_BR: "res://Sprites/Tiles/CornerBR.png",
	Kind.CORNER_BRP: "res://Sprites/Tiles/CornerBRP.png",
	Kind.CORNER_TL: "res://Sprites/Tiles/CornerTL.png",
	Kind.CORNER_TLP: "res://Sprites/Tiles/CornerTLP.png",
	Kind.CORNER_TR: "res://Sprites/Tiles/CornerTR.png",
	Kind.CORNER_TRP: "res://Sprites/Tiles/CornerTRP.png",
	Kind.SIDE_BL: "res://Sprites/Tiles/SideBL.png",
	Kind.SIDE_BLP: "res://Sprites/Tiles/SideBLP.png",
	Kind.SIDE_BR: "res://Sprites/Tiles/SideBR.png",
	Kind.SIDE_BRP: "res://Sprites/Tiles/SideBRP.png",
	Kind.SIDE_LB: "res://Sprites/Tiles/SideLB.png",
	Kind.SIDE_LT: "res://Sprites/Tiles/SideLT.png",
	Kind.SIDE_RB: "res://Sprites/Tiles/SideRB.png",
	Kind.SIDE_RT: "res://Sprites/Tiles/SideRT.png",
	Kind.SIDE_TL: "res://Sprites/Tiles/SideTL.png",
	Kind.SIDE_TLP: "res://Sprites/Tiles/SideTLP.png",
	Kind.SIDE_TR: "res://Sprites/Tiles/SideTR.png",
	Kind.SIDE_TRP: "res://Sprites/Tiles/SideTRP.png",
	Kind.SPACE_BLACK: "res://Sprites/Tiles/SpaceBlack.png",
	Kind.SPACE_WHITE: "res://Sprites/Tiles/SpaceWhite.png"
}

# Creates a requested board tile.
# 	kind: The kind of board tile to create.
# 	return: A tile.
static func create_tile(kind: Kind) -> Sprite2D:
	var sprite: Sprite2D = Sprite2D.new()
	sprite.texture = load(images[kind])
	return sprite
