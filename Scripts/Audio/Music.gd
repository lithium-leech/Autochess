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


# Gets a music player with the requested music track.
# 	music: The kind of music track to get a player for.
# 	return: A music player.
static func get_music_player(music: Kind) -> AudioStreamPlayer2D:
	if (music == Kind.NONE): return null
	var scene: PackedScene = load(_prefabs[music])
	return scene.instantiate() as AudioStreamPlayer2D
