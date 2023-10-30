# A game board that pieces can move on.
class_name Board


# An enumeration containing each kind of game board.
enum Kind {
	CLASSIC,
	HOURGLASS,
	ZIGZAG,
	CENTERCROSS,
	SMALL
}


# A collection of prefabricated game board icons.
static var _prefabs: Dictionary = {
	Kind.CLASSIC: "res://Scenes/Boards/Classic.tscn",
	Kind.HOURGLASS: "res://Scenes/Boards/Hourglass.tscn",
	Kind.ZIGZAG: "res://Scenes/Boards/ZigZag.tscn",
	Kind.CENTERCROSS: "res://Scenes/Boards/CenterCross.tscn",
	Kind.SMALL: "res://Scenes/Boards/Small.tscn"
}


# Gets the icon for a requested game board.
# 	board: The kind of game board to get an icon for.
# 	return: An icon.
static func get_board_icon(board: Kind) -> Sprite2D:
	var scene: PackedScene = load(_prefabs[board])
	return scene.instantiate() as Sprite2D
