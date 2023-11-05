# A music track.
class_name Music


# An enumeration containing each kind of music track.
enum Kind {
	NONE,
	MENU,
	PLANNING,
	BATTLE
}

# A collection of prefabricated music players.
static var _prefabs: Dictionary = {
	Kind.MENU: "res://Scenes/Music/Menu.tscn",
	Kind.PLANNING: "res://Scenes/Music/Planning.tscn",
	Kind.BATTLE: "res://Scenes/Music/Battle.tscn"
}

# Creates a new music player with the requested music track.
# 	kind: The kind of music track to create a player for.
# 	return: A music player.
static func create_music_player(kind: Kind) -> AudioStreamPlayer2D:
	if (kind == Kind.NONE): return null
	var scene: PackedScene = load(_prefabs[kind])
	return scene.instantiate() as AudioStreamPlayer2D
