class_name UpgradeStage extends Stage


# Creates a new instance of an upgrade stage.
#   game: The game to run the stage in.
func _init(_game: Game):
	game = _game


# The game this is being run in.
var game: Game

# The upgrade being offered in this stage.
var upgrade: Upgrade


# Runs once when the stage starts.
func start():
	# Start the planning music.
	Main.music_box.play_music(Music.Kind.PLANNING)
	# Determine the upgrade to give.
	upgrade = PieceUpgrade.new(game)
	# Display the choice menu.
	upgrade.remove_info()
	upgrade.show_choices()
	game.in_game_menu.get_label_group().visible_text = false
	Main.menu_manager.add_active_menu(game.choice_menu)
	# Add button signals.
	for i in range(Upgrade.N_CHOICES):
		var button: TextureButton = game.choice_menu.choice_buttons[i]
		button.pressed.connect(func(): select(i))


# Runs repeatedly while the player is in this stage.
# 	delta: The elapsed time since the previous frame.
func during(_delta: float):
	pass


# Runs once when the stage ends.
func end():
	# Go to the next level.
	Main.game_state.level += 1
	if (Main.game_state.level > Main.game_state.high_score):
		Main.game_state.high_score = Main.game_state.level
	game.in_game_menu.set_level(Main.game_state.level)
	# Remove the choice menu.
	Main.menu_manager.remove_active_menu(game.choice_menu)
	game.in_game_menu.get_label_group().visible_text = true


# Selects the given choice.
# 	choice: The index of the choice to select.
func select(choice: int):
	for i in range(Upgrade.N_CHOICES):
		var button: TextureButton = game.choice_menu.choice_buttons[i]
		button.button_pressed = choice == i
	game.choice_menu.confirm_button.disabled = false
	upgrade.show_info(choice)
