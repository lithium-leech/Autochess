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

# A collection of green highlight images.
static var green_images: Dictionary = {
	Kind.CORNER_BL: "res://Sprites/Highlights/GreenCornerBL.png",
	Kind.CORNER_BR: "res://Sprites/Highlights/GreenCornerBR.png",
	Kind.CORNER_TL: "res://Sprites/Highlights/GreenCornerTL.png",
	Kind.CORNER_TR: "res://Sprites/Highlights/GreenCornerTR.png",
	Kind.INSIDE_CORNER_BL: "res://Sprites/Highlights/GreenInsideCornerBL.png",
	Kind.INSIDE_CORNER_BR: "res://Sprites/Highlights/GreenInsideCornerBR.png",
	Kind.INSIDE_CORNER_TL: "res://Sprites/Highlights/GreenInsideCornerTL.png",
	Kind.INSIDE_CORNER_TR: "res://Sprites/Highlights/GreenInsideCornerTR.png",
	Kind.SIDE_B: "res://Sprites/Highlights/GreenSideB.png",
	Kind.SIDE_L: "res://Sprites/Highlights/GreenSideL.png",
	Kind.SIDE_R: "res://Sprites/Highlights/GreenSideR.png",
	Kind.SIDE_T: "res://Sprites/Highlights/GreenSideT.png"
}

# A collection of red highlight images.
static var red_images: Dictionary = {
	Kind.CORNER_BL: "res://Sprites/Highlights/RedCornerBL.png",
	Kind.CORNER_BR: "res://Sprites/Highlights/RedCornerBR.png",
	Kind.CORNER_TL: "res://Sprites/Highlights/RedCornerTL.png",
	Kind.CORNER_TR: "res://Sprites/Highlights/RedCornerTR.png",
	Kind.INSIDE_CORNER_BL: "res://Sprites/Highlights/RedInsideCornerBL.png",
	Kind.INSIDE_CORNER_BR: "res://Sprites/Highlights/RedInsideCornerBR.png",
	Kind.INSIDE_CORNER_TL: "res://Sprites/Highlights/RedInsideCornerTL.png",
	Kind.INSIDE_CORNER_TR: "res://Sprites/Highlights/RedInsideCornerTR.png",
	Kind.SIDE_B: "res://Sprites/Highlights/RedSideB.png",
	Kind.SIDE_L: "res://Sprites/Highlights/RedSideL.png",
	Kind.SIDE_R: "res://Sprites/Highlights/RedSideR.png",
	Kind.SIDE_T: "res://Sprites/Highlights/RedSideT.png"
}

# Creates a requested highlight.
# 	kind: The kind of highlight to create.
# 	player: True if the highlight is green, false if red.
# 	return: A highlight.
static func create_highlight(kind: Kind, green: bool) -> Sprite2D:
	var sprite: Sprite2D = Sprite2D.new()
	if (green):
		sprite.texture = load(green_images[kind])
	else:
		sprite.texture = load(red_images[kind])
	sprite.z_index = GameState.ZIndex.HIGHLIGHT
	return sprite
