# This script is attached to the audio manager node.
extends Node


# The available music players.
@export var music_players: Array[AudioStreamPlayer2D]

# The music currently being played.
var current_music: Music = Music.NONE

# The volume level to play the music at.
var volume: int = 7


# Plays the requested music.
# 	music: The music to play.
func play_music(music: Music):
	if (current_music != Music.NONE): music_players[current_music].stop()
	current_music = music
	if (current_music != Music.NONE): music_players[current_music].play()


# Sets the volume level.
# 	level: The level to set the volume to [0:10].
func set_volume(level: int):
	level = clamp(level, 0, 10)
	var db: int = (level * 5) - 35
	if (level == 0): db = -80
	for player in music_players:
		player.volume_db = db
	volume = level


# An enumeration of the available music tracks.
enum Music {
	NONE = -1,
	MENU = 0,
	PLANNING = 1,
	BATTLE = 2
}
