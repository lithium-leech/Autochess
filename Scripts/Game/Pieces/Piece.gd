# A single game piece.
class_name Piece extends GameObject


# An enumeration containing each kind of piece.
enum Kind {
	NONE,
	PAWN,
	ROOK,
	KNIGHT,
	BISHOP,
	QUEEN,
	KING,
	PRIVATE,
	CAPTAIN,
	SERGEANT,
	LIEUTENANT,
	COLONEL,
	GENERAL,
	FERZ,
	WAZIR,
	DABBABA,
	TALIAH,
	CAMEL,
	ELEPHANT,
	GIRAFFE,
	SWORDSMAN,
	FUHYO,
	TOKIN,
	HISHA,
	RYUO,
	KAKUGYO,
	RYUMA,
	KEIMA,
	NARIKEI,
	OSHO,
	KYOSHA,
	NARIKYO,
	GINSHO,
	NARIGIN,
	KINSHO,
	ZU,
	JU,
	MA,
	XIANG,
	JIANG,
	SHI,
	PAO
}

# A collection of piece scripts.
static var scripts: Dictionary = {
	Kind.PAWN: "res://Scripts/Game/Pieces/Pawn.gd",
	Kind.ROOK: "res://Scripts/Game/Pieces/Rook.gd",
	Kind.KNIGHT: "res://Scripts/Game/Pieces/Knight.gd",
	Kind.BISHOP: "res://Scripts/Game/Pieces/Bishop.gd",
	Kind.QUEEN: "res://Scripts/Game/Pieces/Queen.gd",
	Kind.KING: "res://Scripts/Game/Pieces/King.gd",
	Kind.PRIVATE: "res://Scripts/Game/Pieces/Private.gd",
	Kind.CAPTAIN: "res://Scripts/Game/Pieces/Captain.gd",
	Kind.SERGEANT: "res://Scripts/Game/Pieces/Sergeant.gd",
	Kind.LIEUTENANT: "res://Scripts/Game/Pieces/Lieutenant.gd",
	Kind.COLONEL: "res://Scripts/Game/Pieces/Colonel.gd",
	Kind.GENERAL: "res://Scripts/Game/Pieces/General.gd",
	Kind.FERZ: "res://Scripts/Game/Pieces/Ferz.gd",
	Kind.WAZIR: "res://Scripts/Game/Pieces/Wazir.gd",
	Kind.DABBABA: "res://Scripts/Game/Pieces/Dabbaba.gd",
	Kind.TALIAH: "res://Scripts/Game/Pieces/Taliah.gd",
	Kind.CAMEL: "res://Scripts/Game/Pieces/Camel.gd",
	Kind.ELEPHANT: "res://Scripts/Game/Pieces/Elephant.gd",
	Kind.GIRAFFE: "res://Scripts/Game/Pieces/Giraffe.gd",
	Kind.SWORDSMAN: "res://Scripts/Game/Pieces/Swordsman.gd",
	Kind.FUHYO: "res://Scripts/Game/Pieces/Fuhyo.gd",
	Kind.TOKIN: "res://Scripts/Game/Pieces/Tokin.gd",
	Kind.HISHA: "res://Scripts/Game/Pieces/Hisha.gd",
	Kind.RYUO: "res://Scripts/Game/Pieces/Ryuo.gd",
	Kind.KAKUGYO: "res://Scripts/Game/Pieces/Kakugyo.gd",
	Kind.RYUMA: "res://Scripts/Game/Pieces/Ryuma.gd",
	Kind.KEIMA: "res://Scripts/Game/Pieces/Keima.gd",
	Kind.NARIKEI: "res://Scripts/Game/Pieces/Narikei.gd",
	Kind.OSHO: "res://Scripts/Game/Pieces/Osho.gd",
	Kind.KYOSHA: "res://Scripts/Game/Pieces/Kyosha.gd",
	Kind.NARIKYO: "res://Scripts/Game/Pieces/Narikyo.gd",
	Kind.GINSHO: "res://Scripts/Game/Pieces/Ginsho.gd",
	Kind.NARIGIN: "res://Scripts/Game/Pieces/Narigin.gd",
	Kind.KINSHO: "res://Scripts/Game/Pieces/Kinsho.gd",
	Kind.ZU: "res://Scripts/Game/Pieces/Zu.gd",
	Kind.JU: "res://Scripts/Game/Pieces/Ju.gd",
	Kind.MA: "res://Scripts/Game/Pieces/Ma.gd",
	Kind.XIANG: "res://Scripts/Game/Pieces/Xiang.gd",
	Kind.JIANG: "res://Scripts/Game/Pieces/Jiang.gd",
	Kind.SHI: "res://Scripts/Game/Pieces/Shi.gd",
	Kind.PAO: "res://Scripts/Game/Pieces/Pao.gd"
}

# A collection of white piece sprites.
static var white_images: Dictionary = {
	Kind.PAWN: "res://Sprites/Pieces/WhitePawn.png",
	Kind.ROOK: "res://Sprites/Pieces/WhiteRook.png",
	Kind.KNIGHT: "res://Sprites/Pieces/WhiteKnight.png",
	Kind.BISHOP: "res://Sprites/Pieces/WhiteBishop.png",
	Kind.QUEEN: "res://Sprites/Pieces/WhiteQueen.png",
	Kind.KING: "res://Sprites/Pieces/WhiteKing.png",
	Kind.PRIVATE: "res://Sprites/Pieces/WhitePrivate.png",
	Kind.CAPTAIN: "res://Sprites/Pieces/WhiteCaptain.png",
	Kind.SERGEANT: "res://Sprites/Pieces/WhiteSergeant.png",
	Kind.LIEUTENANT: "res://Sprites/Pieces/WhiteLieutenant.png",
	Kind.COLONEL: "res://Sprites/Pieces/WhiteColonel.png",
	Kind.GENERAL: "res://Sprites/Pieces/WhiteGeneral.png",
	Kind.FERZ: "res://Sprites/Pieces/WhiteFerz.png",
	Kind.WAZIR: "res://Sprites/Pieces/WhiteWazir.png",
	Kind.DABBABA: "res://Sprites/Pieces/WhiteDabbaba.png",
	Kind.TALIAH: "res://Sprites/Pieces/WhiteTaliah.png",
	Kind.CAMEL: "res://Sprites/Pieces/WhiteCamel.png",
	Kind.ELEPHANT: "res://Sprites/Pieces/WhiteElephant.png",
	Kind.GIRAFFE: "res://Sprites/Pieces/WhiteGiraffe.png",
	Kind.SWORDSMAN: "res://Sprites/Pieces/WhiteSwordsman.png",
	Kind.FUHYO: "res://Sprites/Pieces/WhiteFuhyo.png",
	Kind.TOKIN: "res://Sprites/Pieces/WhiteTokin.png",
	Kind.HISHA: "res://Sprites/Pieces/WhiteHisha.png",
	Kind.RYUO: "res://Sprites/Pieces/WhiteRyuo.png",
	Kind.KAKUGYO: "res://Sprites/Pieces/WhiteKakugyo.png",
	Kind.RYUMA: "res://Sprites/Pieces/WhiteRyuma.png",
	Kind.KEIMA: "res://Sprites/Pieces/WhiteKeima.png",
	Kind.NARIKEI: "res://Sprites/Pieces/WhiteNarikei.png",
	Kind.OSHO: "res://Sprites/Pieces/WhiteOsho.png",
	Kind.KYOSHA: "res://Sprites/Pieces/WhiteKyosha.png",
	Kind.NARIKYO: "res://Sprites/Pieces/WhiteNarikyo.png",
	Kind.GINSHO: "res://Sprites/Pieces/WhiteGinsho.png",
	Kind.NARIGIN: "res://Sprites/Pieces/WhiteNarigin.png",
	Kind.KINSHO: "res://Sprites/Pieces/WhiteKinsho.png",
	Kind.ZU: "res://Sprites/Pieces/WhiteZu.png",
	Kind.JU: "res://Sprites/Pieces/WhiteJu.png",
	Kind.MA: "res://Sprites/Pieces/WhiteMa.png",
	Kind.XIANG: "res://Sprites/Pieces/WhiteXiang.png",
	Kind.JIANG: "res://Sprites/Pieces/WhiteJiang.png",
	Kind.SHI: "res://Sprites/Pieces/WhiteShi.png",
	Kind.PAO: "res://Sprites/Pieces/WhitePao.png"
}

# A collection of black piece sprites.
static var black_images: Dictionary = {
	Kind.PAWN: "res://Sprites/Pieces/BlackPawn.png",
	Kind.ROOK: "res://Sprites/Pieces/BlackRook.png",
	Kind.KNIGHT: "res://Sprites/Pieces/BlackKnight.png",
	Kind.BISHOP: "res://Sprites/Pieces/BlackBishop.png",
	Kind.QUEEN: "res://Sprites/Pieces/BlackQueen.png",
	Kind.KING: "res://Sprites/Pieces/BlackKing.png",
	Kind.PRIVATE: "res://Sprites/Pieces/BlackPrivate.png",
	Kind.CAPTAIN: "res://Sprites/Pieces/BlackCaptain.png",
	Kind.SERGEANT: "res://Sprites/Pieces/BlackSergeant.png",
	Kind.LIEUTENANT: "res://Sprites/Pieces/BlackLieutenant.png",
	Kind.COLONEL: "res://Sprites/Pieces/BlackColonel.png",
	Kind.GENERAL: "res://Sprites/Pieces/BlackGeneral.png",
	Kind.FERZ: "res://Sprites/Pieces/BlackFerz.png",
	Kind.WAZIR: "res://Sprites/Pieces/BlackWazir.png",
	Kind.DABBABA: "res://Sprites/Pieces/BlackDabbaba.png",
	Kind.TALIAH: "res://Sprites/Pieces/BlackTaliah.png",
	Kind.CAMEL: "res://Sprites/Pieces/BlackCamel.png",
	Kind.ELEPHANT: "res://Sprites/Pieces/BlackElephant.png",
	Kind.GIRAFFE: "res://Sprites/Pieces/BlackGiraffe.png",
	Kind.SWORDSMAN: "res://Sprites/Pieces/BlackSwordsman.png",
	Kind.FUHYO: "res://Sprites/Pieces/BlackFuhyo.png",
	Kind.TOKIN: "res://Sprites/Pieces/BlackTokin.png",
	Kind.HISHA: "res://Sprites/Pieces/BlackHisha.png",
	Kind.RYUO: "res://Sprites/Pieces/BlackRyuo.png",
	Kind.KAKUGYO: "res://Sprites/Pieces/BlackKakugyo.png",
	Kind.RYUMA: "res://Sprites/Pieces/BlackRyuma.png",
	Kind.KEIMA: "res://Sprites/Pieces/BlackKeima.png",
	Kind.NARIKEI: "res://Sprites/Pieces/BlackNarikei.png",
	Kind.OSHO: "res://Sprites/Pieces/BlackOsho.png",
	Kind.KYOSHA: "res://Sprites/Pieces/BlackKyosha.png",
	Kind.NARIKYO: "res://Sprites/Pieces/BlackNarikyo.png",
	Kind.GINSHO: "res://Sprites/Pieces/BlackGinsho.png",
	Kind.NARIGIN: "res://Sprites/Pieces/BlackNarigin.png",
	Kind.KINSHO: "res://Sprites/Pieces/BlackKinsho.png",
	Kind.ZU: "res://Sprites/Pieces/BlackZu.png",
	Kind.JU: "res://Sprites/Pieces/BlackJu.png",
	Kind.MA: "res://Sprites/Pieces/BlackMa.png",
	Kind.XIANG: "res://Sprites/Pieces/BlackXiang.png",
	Kind.JIANG: "res://Sprites/Pieces/BlackJiang.png",
	Kind.SHI: "res://Sprites/Pieces/BlackShi.png",
	Kind.PAO: "res://Sprites/Pieces/BlackPao.png"
}


# Creates a requested piece.
# 	kind: The kind of piece to create.
# 	white: True if the piece is white, false if black.
# 	return: A piece.
static func create_piece(kind: Kind, white: bool) -> Piece:
	var sprite: Sprite2D = Sprite2D.new()
	sprite.texture = get_icon(kind, white)
	sprite.set_script(load(scripts[kind]))
	sprite.z_index = GameState.ZIndex.PIECE
	return sprite as Piece


# Gets an icon for the requested piece.
# 	kind: The kind of piece to get an icon for.
# 	white: True if the piece is white, false if black.
# 	return: An icon.
static func get_icon(kind: Piece.Kind, white: bool):
	if (white):
		return load(white_images[kind])
	else:
		return load(black_images[kind])


# Must be implemented by inheriting classes.
# The kind of piece this is.
func get_kind() -> Kind:
	return Kind.NONE

# A kind of piece to be promoted to.
var promotion: Kind = Kind.NONE

# An item this piece is equipped with.
var equipment: Equipment = null

# Another piece captured by this one.
var captured: Piece = null

# True if the piece is moving towards a target position.
var is_moving: bool = false

# A sequence of coordinates to move through.
var path: Array[Vector2i] = []

# An incrementor used for lerp calculations.
var lerp_i: float = 0.0


# Must be implemented by inheriting classes.
# Takes the piece's turn.
func take_turn():
	pass


# Called every frame.
# 	delta: The elapsed time since the previous frame.
func _process(delta: float):
	# Move towards the target when moving is activated.
	if (is_moving):
		lerp_i = clamp(lerp_i + (delta * 2.0 / Main.game_state.turn_pause), 0.0, 1.0)
		var target: Vector2 = space.board.to_position(path[path.size() - 1])
		if (lerp_i == 1.0):
			# Set the position to the target if the lerp is over.
			position = target
		else:
			# Determine the position within the current path segment.
			var segment: int = floor(lerp_i * (path.size() - 1))
			var segment_i: float = (lerp_i * (path.size() - 1) - segment)
			var segment_start: Vector2 = space.board.to_position(path[segment])
			var segment_end: Vector2 = space.board.to_position(path[segment + 1])
			position = lerp(segment_start, segment_end, segment_i)
		if (position - target == Vector2(0, 0)):
			# Stop moving once the target has been reached.
			is_moving = false


# Destroys this piece.
func destroy():
	if (space != null):
		space.remove_object(self)
	if (equipment != null):
		equipment.destroy()
	queue_free()


# Checks if this piece can be captured by a given piece.
# 	piece: The piece attempting to capture this piece.
# 	return: True if this piece can be captured.
func is_capturable(piece: Piece) -> bool:
	# Check that the given piece is an opponent.
	if (piece.is_player == is_player):
		return false
	# Check if this piece is protected by its equipment.
	if (equipment != null and equipment.protects_from(piece)):
			return false
	# Check that the given piece can enter this terrain.
	if (space.terrain != null and !space.terrain.is_enterable(piece)):
		return false
	return true


# Checks if the given path is a valid path this piece can traverse.
# 	path: The path to check.
# 	jump: True if obstacles can be jumped over.
# 	return: True if the path is traversable.
func is_valid_path(_path: Array[Vector2i], jump: bool) -> bool:
	# Check that the path starts from the current location.
	if (_path.size() < 1):
		return false
	if (_path[0] != space.coordinates):
		return false
	# Check that the path is traversable
	for i in range(1, path.size()):
		if (!space.board.on_board(path[i])):
			return false
		var _space: Space = space.board.get_space(path[i])
		if (!jump and _space == null):
			return false
		elif (jump and _space == null):
			continue
		elif (!jump and !_space.is_enterable(self)):
			return false
		# This check is not necessary unless further checks are added
		# elif (jump and !_space.is_enterable(self)):
		# 	continue
	# The path is valid if nothing proved it invalid.
	return true


# Gets possible move and capture paths in the specified direction.
# 	start: The initial path to branch out from.
# 	dx: x increment.
# 	dy: y increment.
# 	steps: The number of times to increment.
# 	jump: True if obstacles can be jumped over.
# 	moves: A collection of possible move paths to add to.
# 	captures: A collection of possible capture paths to add to.
func add_linear_paths(start: Array[Vector2i], dx: int, dy: int, steps: int, jump: bool, moves: Array, captures: Array):
	var _path: Array[Vector2i] = start.duplicate()
	var pointer: Vector2i = _path[_path.size() - 1]
	for i in steps:
		# Take one step using the given increment.
		pointer.x += dx
		pointer.y += dy
		if (!space.board.on_board(pointer)):
			break
		_path.append(pointer)
		# Determine the possible paths at this step
		var _space = space.board.get_space(pointer)
		if (!jump and _space == null):
			break
		elif (jump and _space == null):
			continue
		elif (_space.has_capturable(self)):
			captures.append(_path.duplicate())
			if (!jump):
				break
			else:
				continue
		elif (!jump and !_space.is_enterable(self)):
			break
		elif (jump and !_space.is_enterable(self)):
			continue
		moves.append(_path.duplicate())


# Gets possible horizontal paths.
# 	start: The initial path to branch out from.
# 	i: The size of each step.
# 	steps: The number of times to increment.
# 	jump: True if obstacles can be jumped over.
# 	moves: A collection of possible move paths to add to.
# 	captures: A collection of possible capture paths to add to.
func add_horizontal_paths(start: Array[Vector2i], i: int, steps: int, jump: bool, moves: Array, captures: Array):
	add_linear_paths(start, i, 0, steps, jump, moves, captures)
	add_linear_paths(start, -i, 0, steps, jump, moves, captures)


# Gets possible vertical paths.
# 	start: The initial path to branch out from.
# 	i: The size of each step.
# 	steps: The number of times to increment.
# 	jump: True if obstacles can be jumped over.
# 	moves: A collection of possible move paths to add to.
# 	captures: A collection of possible capture paths to add to.
func add_vertical_paths(start: Array[Vector2i], i: int, steps: int, jump: bool, moves: Array, captures: Array):
	add_linear_paths(start, 0, i, steps, jump, moves, captures)
	add_linear_paths(start, 0, -i, steps, jump, moves, captures)


# Gets possible vertical and horizontal paths.
# 	start: The initial path to branch out from.
# 	i: The size of each step.
# 	steps: The number of times to increment.
# 	jump: True if obstacles can be jumped over.
# 	moves: A collection of possible move paths to add to.
# 	captures: A collection of possible capture paths to add to.
func add_orthogonal_paths(start: Array[Vector2i], i: int, steps: int, jump: bool, moves: Array, captures: Array):
	add_horizontal_paths(start, i, steps, jump, moves, captures)
	add_vertical_paths(start, i, steps, jump, moves, captures)


# Gets possible diagonal paths.
# 	start: The initial path to branch out from.
# 	i: The size of each step.
# 	steps: The number of times to increment.
# 	jump: True if obstacles can be jumped over.
# 	moves: A collection of possible move paths to add to.
# 	captures: A collection of possible capture paths to add to.
func add_diagonal_paths(start: Array[Vector2i], i: int, steps: int, jump: bool, moves: Array, captures: Array):
	add_linear_paths(start, i, i, steps, jump, moves, captures)
	add_linear_paths(start, -i, i, steps, jump, moves, captures)
	add_linear_paths(start, -i, -i, steps, jump, moves, captures)
	add_linear_paths(start, i, -i, steps, jump, moves, captures)


# This piece starts traversing the given path.
# 	path: The path to traverse.
func start_turn(_path: Array[Vector2i]):
	# Check that the piece is doing something.
	path = _path
	var destination: Space = space.board.get_space(path[path.size() - 1])
	if (destination != null and destination != space):
		# Start the turn
		Main.game_state.is_active_round = true
		z_index = GameState.ZIndex.DYNAMIC
		is_moving = true
		lerp_i = 0.0
		space.exit(self)
		destination.enter(self)
		space.board.game.on_move_finish.connect(finish_moving, CONNECT_ONE_SHOT)


# Resolves final actions once the piece is done moving.
func finish_moving():
		# Destroy the captured piece.
		if (captured != null):
			captured.destroy()
			captured = null
		# Perform promotion.
		if (promotion != Kind.NONE):
			var _space = space
			_space.exit(self)
			var new_piece: Piece = Piece.create_piece(promotion, is_white)
			new_piece.is_player = is_player
			new_piece.is_white = is_white
			new_piece.is_grabable = is_grabable
			Main.game_world.add_child(new_piece)
			_space.add_object(new_piece)
			queue_free()
		# Return to the stationary z index
		z_index = GameState.ZIndex.PIECE
