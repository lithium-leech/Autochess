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

# A collection of prefabricated white pieces.
static var white_prefabs: Dictionary = {
Kind.PAWN: "res://Scenes/Pieces/WhitePawn.tscn",
	Kind.ROOK: "res://Scenes/Pieces/WhiteRook.tscn",
	Kind.KNIGHT: "res://Scenes/Pieces/WhiteKnight.tscn",
	Kind.BISHOP: "res://Scenes/Pieces/WhiteBishop.tscn",
	Kind.QUEEN: "res://Scenes/Pieces/WhiteQueen.tscn",
	Kind.KING: "res://Scenes/Pieces/WhiteKing.tscn",
	Kind.PRIVATE: "res://Scenes/Pieces/WhitePrivate.tscn",
	Kind.CAPTAIN: "res://Scenes/Pieces/WhiteCaptain.tscn",
	Kind.SERGEANT: "res://Scenes/Pieces/WhiteSergeant.tscn",
	Kind.LIEUTENANT: "res://Scenes/Pieces/WhiteLieutenant.tscn",
	Kind.COLONEL: "res://Scenes/Pieces/WhiteColonel.tscn",
	Kind.GENERAL: "res://Scenes/Pieces/WhiteGeneral.tscn",
	Kind.FERZ: "res://Scenes/Pieces/WhiteFerz.tscn",
	Kind.WAZIR: "res://Scenes/Pieces/WhiteWazir.tscn",
	Kind.DABBABA: "res://Scenes/Pieces/WhiteDabbaba.tscn",
	Kind.TALIAH: "res://Scenes/Pieces/WhiteTaliah.tscn",
	Kind.CAMEL: "res://Scenes/Pieces/WhiteCamel.tscn",
	Kind.ELEPHANT: "res://Scenes/Pieces/WhiteElephant.tscn",
	Kind.GIRAFFE: "res://Scenes/Pieces/WhiteGiraffe.tscn",
	Kind.SWORDSMAN: "res://Scenes/Pieces/WhiteSwordsman.tscn",
	Kind.FUHYO: "res://Scenes/Pieces/WhiteFuhyo.tscn",
	Kind.TOKIN: "res://Scenes/Pieces/WhiteTokin.tscn",
	Kind.HISHA: "res://Scenes/Pieces/WhiteHisha.tscn",
	Kind.RYUO: "res://Scenes/Pieces/WhiteRyuo.tscn",
	Kind.KAKUGYO: "res://Scenes/Pieces/WhiteKakugyo.tscn",
	Kind.RYUMA: "res://Scenes/Pieces/WhiteRyuma.tscn",
	Kind.KEIMA: "res://Scenes/Pieces/WhiteKeima.tscn",
	Kind.NARIKEI: "res://Scenes/Pieces/WhiteNarikei.tscn",
	Kind.OSHO: "res://Scenes/Pieces/WhiteOsho.tscn",
	Kind.KYOSHA: "res://Scenes/Pieces/WhiteKyosha.tscn",
	Kind.NARIKYO: "res://Scenes/Pieces/WhiteNarikyo.tscn",
	Kind.GINSHO: "res://Scenes/Pieces/WhiteGinsho.tscn",
	Kind.NARIGIN: "res://Scenes/Pieces/WhiteNarigin.tscn",
	Kind.KINSHO: "res://Scenes/Pieces/WhiteKinsho.tscn",
	Kind.ZU: "res://Scenes/Pieces/WhiteZu.tscn",
	Kind.JU: "res://Scenes/Pieces/WhiteJu.tscn",
	Kind.MA: "res://Scenes/Pieces/WhiteMa.tscn",
	Kind.XIANG: "res://Scenes/Pieces/WhiteXiang.tscn",
	Kind.JIANG: "res://Scenes/Pieces/WhiteJiang.tscn",
	Kind.SHI: "res://Scenes/Pieces/WhiteShi.tscn",
	Kind.PAO: "res://Scenes/Pieces/WhitePao.tscn"
}

# A collection of prefabricated black pieces.
static var black_prefabs: Dictionary = {
	Kind.PAWN: "res://Scenes/Pieces/BlackPawn.tscn",
	Kind.ROOK: "res://Scenes/Pieces/BlackRook.tscn",
	Kind.KNIGHT: "res://Scenes/Pieces/BlackKnight.tscn",
	Kind.BISHOP: "res://Scenes/Pieces/BlackBishop.tscn",
	Kind.QUEEN: "res://Scenes/Pieces/BlackQueen.tscn",
	Kind.KING: "res://Scenes/Pieces/BlackKing.tscn",
	Kind.PRIVATE: "res://Scenes/Pieces/BlackPrivate.tscn",
	Kind.CAPTAIN: "res://Scenes/Pieces/BlackCaptain.tscn",
	Kind.SERGEANT: "res://Scenes/Pieces/BlackSergeant.tscn",
	Kind.LIEUTENANT: "res://Scenes/Pieces/BlackLieutenant.tscn",
	Kind.COLONEL: "res://Scenes/Pieces/BlackColonel.tscn",
	Kind.GENERAL: "res://Scenes/Pieces/BlackGeneral.tscn",
	Kind.FERZ: "res://Scenes/Pieces/BlackFerz.tscn",
	Kind.WAZIR: "res://Scenes/Pieces/BlackWazir.tscn",
	Kind.DABBABA: "res://Scenes/Pieces/BlackDabbaba.tscn",
	Kind.TALIAH: "res://Scenes/Pieces/BlackTaliah.tscn",
	Kind.CAMEL: "res://Scenes/Pieces/BlackCamel.tscn",
	Kind.ELEPHANT: "res://Scenes/Pieces/BlackElephant.tscn",
	Kind.GIRAFFE: "res://Scenes/Pieces/BlackGiraffe.tscn",
	Kind.SWORDSMAN: "res://Scenes/Pieces/BlackSwordsman.tscn",
	Kind.FUHYO: "res://Scenes/Pieces/BlackFuhyo.tscn",
	Kind.TOKIN: "res://Scenes/Pieces/BlackTokin.tscn",
	Kind.HISHA: "res://Scenes/Pieces/BlackHisha.tscn",
	Kind.RYUO: "res://Scenes/Pieces/BlackRyuo.tscn",
	Kind.KAKUGYO: "res://Scenes/Pieces/BlackKakugyo.tscn",
	Kind.RYUMA: "res://Scenes/Pieces/BlackRyuma.tscn",
	Kind.KEIMA: "res://Scenes/Pieces/BlackKeima.tscn",
	Kind.NARIKEI: "res://Scenes/Pieces/BlackNarikei.tscn",
	Kind.OSHO: "res://Scenes/Pieces/BlackOsho.tscn",
	Kind.KYOSHA: "res://Scenes/Pieces/BlackKyosha.tscn",
	Kind.NARIKYO: "res://Scenes/Pieces/BlackNarikyo.tscn",
	Kind.GINSHO: "res://Scenes/Pieces/BlackGinsho.tscn",
	Kind.NARIGIN: "res://Scenes/Pieces/BlackNarigin.tscn",
	Kind.KINSHO: "res://Scenes/Pieces/BlackKinsho.tscn",
	Kind.ZU: "res://Scenes/Pieces/BlackZu.tscn",
	Kind.JU: "res://Scenes/Pieces/BlackJu.tscn",
	Kind.MA: "res://Scenes/Pieces/BlackMa.tscn",
	Kind.XIANG: "res://Scenes/Pieces/BlackXiang.tscn",
	Kind.JIANG: "res://Scenes/Pieces/BlackJiang.tscn",
	Kind.SHI: "res://Scenes/Pieces/BlackShi.tscn",
	Kind.PAO: "res://Scenes/Pieces/BlackPao.tscn"
}

# Creates a requested piece.
# 	kind: The kind of piece to create.
# 	white: True if the piece is white, false if black.
# 	return: A piece.
static func create_piece(_kind: Kind, white: bool) -> Piece:
	var scene: PackedScene
	if (white):
		scene = load(white_prefabs[_kind])
	else:
		scene = load(black_prefabs[_kind])
	return scene.instantiate() as Piece


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
func _process(_delta: float):
	# Move towards the target when moving is activated.
	if (is_moving):
		lerp_i = clamp(lerp_i + (_delta * 2.0 / Main.game_state.turn_pause), 0.0, 1.0)
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
	path = _path
	var destination: Space = space.board.get_space(path[path.size() - 1])
	if (destination != null and destination != space):
		is_moving = true
		lerp_i = 0.0
		space.exit(self)
		destination.enter(self)
	space.board.game.on_move_finish.connect(finish_moving)


# Resolves final actions once the piece is done moving.
func finish_moving():
	# Remove the listener for this move
	space.board.game.on_move_finish.disconnect(finish_moving)
	# Destroy the captured piece.
	if (captured != null):
		captured.destroy()
		captured = null
	# Perform promotion.
	if (promotion != Kind.NONE):
		var _space = space
		_space.exit(self)
		_space.board.add_piece(promotion, is_player, _space)
		queue_free()
