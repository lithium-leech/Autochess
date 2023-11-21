# A single power up.
class_name Power extends GameObject


# An enumeration containing each kind of power.
enum Kind {
	FIRST,
	ROW,
	MINE,
	WALL,
	SHIELD
}

# A collection of power scripts.
static var scripts: Dictionary = {
	Kind.FIRST: "res://Scripts/Game/Powers/FirstPower.gd",
	Kind.ROW: "res://Scripts/Game/Powers/RowPower.gd",
	Kind.MINE: "res://Scripts/Game/Powers/MinePower.gd",
	Kind.WALL: "res://Scripts/Game/Powers/WallPower.gd",
	Kind.SHIELD: "res://Scripts/Game/Powers/ShieldPower.gd"
}

# A collection of power icons.
static var images: Dictionary = {
	Kind.FIRST: "res://Sprites/Powers/First.png",
	Kind.ROW: "res://Sprites/Powers/Row.png",
	Kind.MINE: "res://Sprites/Powers/Mine.png",
	Kind.WALL: "res://Sprites/Powers/Wall.png",
	Kind.SHIELD: "res://Sprites/Powers/Shield.png"
}


# Creates a requested power.
# 	kind: The kind of power to create.
# 	return: A power.
static func create_power(kind: Kind) -> Power:
	var sprite: Sprite2D = Sprite2D.new()
	sprite.texture = get_icon(kind)
	sprite.set_script(load(scripts[kind]))
	sprite.z_index = GameState.ZIndex.PIECE
	return sprite as Power


# Gets the icon for a requested power.
# 	kind: The kind of power to get an icon for.
# 	return: An icon.
static func get_icon(kind: Kind) -> CompressedTexture2D:
	return load(images[kind])


# Gets all of the powers that can be offered.
# 	game: The game powers are being offered in.
# 	player: True if these are the powers available to the player.
# 	current_powers: The powers already possessed.
# 	return: A collection of power kinds.
static func get_available_powers(game: Game, player: bool, current_powers: Array[Kind]) -> Array[Kind]:
	# Start the collection.
	var powers: Array[Kind] = []
	# Only offer "first" to the black player.
	var p_white: bool = Main.game_state.is_player_white
	if ((player and not p_white) or (not player and p_white)):
		powers.append(Kind.FIRST)
	# Only offer "row" to if a row is available.
	var p_rows: int = game.game_board.player_rows
	var e_rows: int = game.game_board.enemy_rows
	var rows: int = game.game_board.height
	if (p_rows + e_rows < rows - 2):
		powers.append(Kind.ROW)
	# Don't offer too many obstacles.
	var obstacles: int = 0
	for power in current_powers:
		match power:
			Kind.MINE:
				obstacles += 1
			Kind.WALL:
				obstacles += 2
	if (obstacles < 7):
		powers.append(Kind.WALL)
	if (obstacles < 8):
		powers.append(Kind.MINE)
	# Add remaining unconditional options.
	powers.append(Kind.SHIELD)
	# Return the final collection.
	return powers


# Must be implemented by inheriting classes.
# The kind of power this is.
func get_kind() -> Kind:
	return Kind.FIRST


# Applies this power to the current game.
func activate():
	pass


# Unapplies this power from the current game.
func deactivate():
	pass


# Removes this object from the entire game.
func destroy():
	deactivate()
	if (space != null):
		space.remove_object(self)
	queue_free()
