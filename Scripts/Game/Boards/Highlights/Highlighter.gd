# Generates highlights around a board's placement zones.
class_name Highlighter


# Creates a new instance of a highlighter.
# 	board: The board to add highlights to.
func _init(_board: Board):
	board = _board


# The board to add highlights to.
var board: Board


# Adds highlights around the different placement zones.
func add_highlights():
	# Both placement zones have the same left and right boundaries.
	var left: int = 0
	var right: int = board.width - 1
	# Add highlights around the player placement zone.
	var player_top: int = board.height - board.player_rows
	var player_bottom: int = board.height - 1
	var player_zone: Vector4i = Vector4i(player_top, player_bottom, left, right)
	add_zone_highlights(player_zone, true)
	# Add highlights around the enemy placement zone.
	var enemy_top: int = 0
	var enemy_bottom: int = board.enemy_rows - 1
	var enemy_zone: Vector4i = Vector4i(enemy_top, enemy_bottom, left, right)
	add_zone_highlights(enemy_zone, false)


# Adds highlights around a given placement zone.
# 	zone: The zone to add highlights around.
# 	green: True if the highlights are green, False if red.
func add_zone_highlights(zone: Vector4i, green: bool):
	# Iterate in and around the given zone.
	for x in range(zone.z - 1, zone.w + 2):
		for y in range(zone.x - 1, zone.y + 2):
			var coordinates: Vector2i = Vector2i(x, y)
			# Look for a space inside the zone.
			var space: Space = get_zone_space(zone, coordinates)
			# Add highlights when a space is not found
			if (space == null):
				add_space_highlights(zone, coordinates, green)


# Adds highlights at a given space.
# 	zone: The zone to add highlights around.
# 	coordinates: The coordinates of the space to add highlights for.
# 	green: True if the highlights are green, False if red.
func add_space_highlights(zone: Vector4i, coordinates: Vector2i, green: bool):
	# Determine which spaces are present
	var top: bool = get_zone_space(zone, coordinates + Vector2i(0, -1)) != null
	var bottom: bool = get_zone_space(zone, coordinates + Vector2i(0, 1)) != null
	var left: bool = get_zone_space(zone, coordinates + Vector2i(-1, 0)) != null
	var right: bool = get_zone_space(zone, coordinates + Vector2i(1, 0)) != null
	var top_left: bool = get_zone_space(zone, coordinates + Vector2i(-1, -1)) != null
	var top_right: bool = get_zone_space(zone, coordinates + Vector2i(1, -1)) != null
	var bottom_left: bool = get_zone_space(zone, coordinates + Vector2i(-1, 1)) != null
	var bottom_right: bool = get_zone_space(zone, coordinates + Vector2i(1, 1)) != null
	var position: Vector2i = board.to_position(coordinates)
	# Add the top-left highlight for this space.
	var key_tl: int = (1 if top else 0) << 0 | \
					  (1 if left else 0) << 1 | \
					  (1 if top_left else 0) << 2
	var tile_tl: Sprite2D = null
	match (key_tl):
		1, 5:
			tile_tl = Highlight.create_highlight(Highlight.Kind.SIDE_B, green)
		2, 6:
			tile_tl = Highlight.create_highlight(Highlight.Kind.SIDE_R, green)
		3, 7:
			tile_tl = Highlight.create_highlight(Highlight.Kind.INSIDE_CORNER_BR, green)
		4:
			tile_tl = Highlight.create_highlight(Highlight.Kind.CORNER_BR, green)
	if (tile_tl != null):
		tile_tl.position = position + Vector2i(-8, -8)
		board.tile_node.add_child(tile_tl)
	# Add the top-right highlight for this space.
	var key_tr: int = (1 if top else 0) << 0 | \
					  (1 if right else 0) << 1 | \
					  (1 if top_right else 0) << 2
	var tile_tr: Sprite2D = null
	match (key_tr):
		1, 5:
			tile_tr = Highlight.create_highlight(Highlight.Kind.SIDE_B, green)
		2, 6:
			tile_tr = Highlight.create_highlight(Highlight.Kind.SIDE_L, green)
		3, 7:
			tile_tr = Highlight.create_highlight(Highlight.Kind.INSIDE_CORNER_BL, green)
		4:
			tile_tr = Highlight.create_highlight(Highlight.Kind.CORNER_BL, green)
	if (tile_tr != null):
		tile_tr.position = position + Vector2i(8, -8)
		board.tile_node.add_child(tile_tr)
	# Add the bottom-left highlight for this space.
	var key_bl: int = (1 if bottom else 0) << 0 | \
					  (1 if left else 0) << 1 | \
					  (1 if bottom_left else 0) << 2
	var tile_bl: Sprite2D = null
	match (key_bl):
		1, 5:
			tile_bl = Highlight.create_highlight(Highlight.Kind.SIDE_T, green)
		2, 6:
			tile_bl = Highlight.create_highlight(Highlight.Kind.SIDE_R, green)
		3, 7:
			tile_bl = Highlight.create_highlight(Highlight.Kind.INSIDE_CORNER_TR, green)
		4:
			tile_bl = Highlight.create_highlight(Highlight.Kind.CORNER_TR, green)
	if (tile_bl != null):
		tile_bl.position = position + Vector2i(-8, 8)
		board.tile_node.add_child(tile_bl)
	# Add the bottom-right highlight for this space.
	var key_br: int = (1 if bottom else 0) << 0 | \
					  (1 if right else 0) << 1 | \
					  (1 if bottom_right else 0) << 2
	var tile_br: Sprite2D = null
	match (key_br):
		1, 5:
			tile_br = Highlight.create_highlight(Highlight.Kind.SIDE_T, green)
		2, 6:
			tile_br = Highlight.create_highlight(Highlight.Kind.SIDE_L, green)
		3, 7:
			tile_br = Highlight.create_highlight(Highlight.Kind.INSIDE_CORNER_TL, green)
		4:
			tile_br = Highlight.create_highlight(Highlight.Kind.CORNER_TL, green)
	if (tile_br != null):
		tile_br.position = position + Vector2i(8, 8)
		board.tile_node.add_child(tile_br)


# Gets a space inside of a given zone.
# 	zone: The zone to get a space within.
# 	coordinates: The coordinates to get a space at.
# 	return: A space within the zone, or null.
func get_zone_space(zone: Vector4i, coordinates: Vector2i) -> Space:
	if (coordinates.x < zone.z or \
		coordinates.x > zone.w or \
		coordinates.y < zone.x or \
		coordinates.y > zone.y):
		return null
	else:
		return board.get_space(coordinates)
