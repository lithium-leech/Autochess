# A class for managing music.
class_name MusicBox extends Node


# The volume level to play music at.
var volume: int = 7

# The music player.
var player: AudioStreamPlayer2D

# The decibel value to set music players to.
var db: int = 0


# Plays the requested music.
# 	music: The music to play.
func play_music(music: Music.Kind):
	# Remove the current music player.
	if (player != null):
		player.stop()
		player.queue_free()
		player = null
	# Create the requested music player.
	if (music != Music.Kind.NONE):
		player = Music.create_music_player(music)
		Main.node.add_child(player)
		player.volume_db = db
		player.play()


# Sets the volume level.
# 	level: The level to set the volume to [0:10].
func set_volume(level: int):
	level = clamp(level, 0, 10)
	db = (level * 5) - 35
	if (level == 0): db = -80
	if (player != null): player.volume_db = db
	volume = level
