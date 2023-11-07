# A single sprite for highlighting board zones.
class_name Highlight


# An enumeration containing each kind of highlight.
enum Kind {
	CORNER_BL,
	CORNER_BR,
	CORNER_TL,
	CORNER_TR,
	INSIDE_CORNER_BL,
	INSIDE_CORNER_BR,
	INSIDE_CORNER_TL,
	INSIDE_CORNER_TR,
	SIDE_B,
	SIDE_L,
	SIDE_R,
	SIDE_T
}

# A collection of prefabricated green highlights.
static var green_prefabs: Dictionary = {
	Kind.CORNER_BL: "res://Scenes/Highlights/GreenCornerBL.tscn",
	Kind.CORNER_BR: "res://Scenes/Highlights/GreenCornerBR.tscn",
	Kind.CORNER_TL: "res://Scenes/Highlights/GreenCornerTL.tscn",
	Kind.CORNER_TR: "res://Scenes/Highlights/GreenCornerTR.tscn",
	Kind.INSIDE_CORNER_BL: "res://Scenes/Highlights/GreenInsideCornerBL.tscn",
	Kind.INSIDE_CORNER_BR: "res://Scenes/Highlights/GreenInsideCornerBR.tscn",
	Kind.INSIDE_CORNER_TL: "res://Scenes/Highlights/GreenInsideCornerTL.tscn",
	Kind.INSIDE_CORNER_TR: "res://Scenes/Highlights/GreenInsideCornerTR.tscn",
	Kind.SIDE_B: "res://Scenes/Highlights/GreenSideB.tscn",
	Kind.SIDE_L: "res://Scenes/Highlights/GreenSideL.tscn",
	Kind.SIDE_R: "res://Scenes/Highlights/GreenSideR.tscn",
	Kind.SIDE_T: "res://Scenes/Highlights/GreenSideT.tscn"
}

# A collection of prefabricated red highlights.
static var red_prefabs: Dictionary = {
	Kind.CORNER_BL: "res://Scenes/Highlights/RedCornerBL.tscn",
	Kind.CORNER_BR: "res://Scenes/Highlights/RedCornerBR.tscn",
	Kind.CORNER_TL: "res://Scenes/Highlights/RedCornerTL.tscn",
	Kind.CORNER_TR: "res://Scenes/Highlights/RedCornerTR.tscn",
	Kind.INSIDE_CORNER_BL: "res://Scenes/Highlights/RedInsideCornerBL.tscn",
	Kind.INSIDE_CORNER_BR: "res://Scenes/Highlights/RedInsideCornerBR.tscn",
	Kind.INSIDE_CORNER_TL: "res://Scenes/Highlights/RedInsideCornerTL.tscn",
	Kind.INSIDE_CORNER_TR: "res://Scenes/Highlights/RedInsideCornerTR.tscn",
	Kind.SIDE_B: "res://Scenes/Highlights/RedSideB.tscn",
	Kind.SIDE_L: "res://Scenes/Highlights/RedSideL.tscn",
	Kind.SIDE_R: "res://Scenes/Highlights/RedSideR.tscn",
	Kind.SIDE_T: "res://Scenes/Highlights/RedSideT.tscn"
}

# Creates a requested highlight.
# 	kind: The kind of highlight to create.
# 	player: True if this is a highlight for the player placement zone.
# 	return: A highlight.
static func create_highlight(kind: Kind, player: bool) -> Sprite2D:
	var scene: PackedScene
	if (player):
		scene = load(green_prefabs[kind])
	else:
		scene = load(red_prefabs[kind])
	return scene.instantiate() as Sprite2D
