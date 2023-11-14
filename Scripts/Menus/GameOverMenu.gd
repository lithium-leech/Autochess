# The end of game scoreboard.
class_name GameOverMenu extends Menu


# The score text.
var score_text: Label

# The high score text.
var high_score_text: Label


# Called when the node enters the scene tree for the first time.
func _ready():
	score_text = $Score
	high_score_text = $Highscore


# Called when the menu is opened.
func on_open():
	get_label(score_text).text = tr("Text.Score").format({score = Main.game_state.level})
	get_label(high_score_text).text = tr("Text.Highscore").format({score = Main.game_state.high_score})


# Called when the retry button is pressed.
func _on_retry_pressed():
	pass


# Called when the new game button is pressed.
func _on_new_pressed():
	pass


# Called when the end game button is pressed.
func _on_end_pressed():
	pass
