# A music track.
class_name Music


# An enumeration containing each kind of music track.
enum Kind {
	NONE,
	MENU,
	PLANNING,
	BATTLE
}

# A collection of music recordings.
static var recordings: Dictionary = {
	Kind.MENU: preload("res://Audio/Music/MenuMusic.ogg"),
	Kind.PLANNING: preload("res://Audio/Music/PlanningMusic.ogg"),
	Kind.BATTLE: preload("res://Audio/Music/BattleMusic.ogg")
}

# Creates a new music player with the requested music track.
# 	kind: The kind of music track to create a player for.
# 	return: A music player.
static func create_music_player(kind: Kind) -> AudioStreamPlayer2D:
	if (kind == Kind.NONE): return null
	var player: AudioStreamPlayer2D = AudioStreamPlayer2D.new()
	player.stream = recordings[kind]
	return player
