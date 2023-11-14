# The end of game scoreboard.
class_name GameOverMenu extends Menu


# The score text.
var score_text: Label

# The high score text.
var high_score_text: Label

# The retry button.
var retry_button: TextureButton


# Called when the node enters the scene tree for the first time.
func _ready():
	score_text = $Score
	high_score_text = $Highscore
	retry_button = $Retry


# Called when the menu is opened.
func on_open():
	get_label(score_text).text = tr("Text.Score").format({score = Main.game_state.level - 1})
	get_label(high_score_text).text = tr("Text.Highscore").format({score = Main.game_state.high_score})


# Called when the new game button is pressed.
func _on_new_pressed():
	# Delete the current contents of the game world.
	for child in Main.game_world.get_children():
		child.queue_free()
	# Add a new game scene.
	var game_scene: Control = load("res://Scenes/Game.tscn").instantiate()
	Main.game_world.add_child(game_scene)
	Main.menu_manager.initialize(game_scene.get_node("InGame/Menu"))
	Main.music_box.play_music(Music.Kind.PLANNING)


# Called when the end game button is pressed.
func _on_end_pressed():
	# Delete the current contents of the game world.
	for child in Main.game_world.get_children():
		child.queue_free()
	# Add a new title scene.
	var title_scene: Control = load("res://Scenes/Title.tscn").instantiate()
	Main.game_world.add_child(title_scene)
	Main.menu_manager.initialize(title_scene.get_node("Front/Menu"))
	Main.music_box.play_music(Music.Kind.MENU)
