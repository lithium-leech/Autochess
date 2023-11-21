# A single game item.
class_name Item extends GameObject


# An enumeration containing each kind of item.
enum Kind {
	NONE,
	WALL,
	MINE,
	SHIELD
}

# A collection of item scripts.
static var scripts: Dictionary = {
	Kind.WALL: "res://Scripts/Game/Items/Terrain/Wall.gd",
	Kind.MINE: "res://Scripts/Game/Items/Terrain/Mine.gd",
	Kind.SHIELD: "res://Scripts/Game/Items/Equipment/Shield.gd"
}

# A collection of primary item icons.
static var images_1: Dictionary = {
	Kind.WALL: "res://Sprites/Items/WhiteWall.png",
	Kind.MINE: "res://Sprites/Items/WhiteMine.png",
	Kind.SHIELD: "res://Sprites/Items/Shield.png"
}

# A collection of secondary item icons.
static var images_2: Dictionary = {
	Kind.WALL: "res://Sprites/Items/BlackWall.png",
	Kind.MINE: "res://Sprites/Items/BlackMine.png",
	Kind.SHIELD: "res://Sprites/Items/ShieldEquipped.png"
}


# Creates a requested item.
# 	kind: The kind of terrain to create.
# 	white: True if the item is white.
# 	return: An item.
static func create_item(kind: Kind, white: bool) -> Item:
	var sprite = Sprite2D.new()
	sprite.set_script(load(scripts[kind]))
	match kind:
		Kind.WALL, Kind.MINE:
			sprite.texture = get_icon(kind, white)
			sprite.z_index = GameState.ZIndex.TERRAIN
		Kind.SHIELD:
			var equipment: Equipment = sprite
			equipment.item_sprite = get_icon(kind, true)
			equipment.equip_sprite = get_icon(kind, false)
			sprite.texture = equipment.item_sprite
			sprite.z_index = GameState.ZIndex.EQUIPMENT
	return sprite as Item


# Gets the icon for a requested item.
# 	kind: The kind of item to get an icon for.
# 	primary: True if the primary icon is wanted.
# 	return: An icon.
static func get_icon(kind: Kind, primary: bool) -> CompressedTexture2D:
	if (primary):
		return load(images_1[kind])
	else:
		return load(images_2[kind])


# Must be implemented by inheriting classes.
# The kind of item this is.
func get_kind() -> Kind:
	return Kind.NONE
