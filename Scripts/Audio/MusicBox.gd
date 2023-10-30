# A class for managing music.
class_name MusicBox extends Node


# The volume level to play music at.
var volume: int = 7

# The music player.
var _player: AudioStreamPlayer2D

# The decibel value to set music players to.
var _db: int = 0


# Plays the requested music.
# 	music: The music to play.
func play_music(music: Music.Kind):
	# Remove the current music player.
	if (_player != null):
		_player.stop()
		_player.queue_free()
		_player = null
	# Create the requested music player.
	if (music != Music.Kind.NONE):
		_player = Music.get_music_player(music)
		Main.node.add_child(_player)
		_player.volume_db = _db
		_player.play()


# Sets the volume level.
# 	level: The level to set the volume to [0:10].
func set_volume(level: int):
	level = clamp(level, 0, 10)
	_db = (level * 5) - 35
	if (level == 0): _db = -80
	_player.volume_db = _db
	volume = level
