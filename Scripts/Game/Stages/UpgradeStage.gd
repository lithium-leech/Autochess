class_name UpgradeStage extends Stage


# Creates a new instance of an upgrade stage.
#   game: The game to run the stage in.
func _init(_game: Game):
	game = _game


# The game this is being run in.
var game: Game

# The upgrade being offered in this stage.
var upgrade: Upgrade

# The selected choice.
var selected: int


# Runs once when the stage starts.
func start():
	# Start the planning music.
	Main.music_box.play_music(Music.Kind.PLANNING)
	# Determine the upgrade to give.
	if (Main.game_state.level % GameState.MAP_ROUNDS == 0):
		upgrade = MapUpgrade.new(game)
	elif (Main.game_state.level % GameState.POWER_ROUNDS == 0):
		upgrade = PieceUpgrade.new(game)
	elif (Main.game_state.level % GameState.PIECE_ROUNDS == 0):
		upgrade = PieceUpgrade.new(game)
	else:
		game.next_stage = PlanningStage.new(game)
		upgrade = Upgrade.new()
		upgrade._base_init(game)
	# Display the choice menu.
	selected = -1
	for i in range(Upgrade.N_CHOICES):
		var button: TextureButton = game.choice_menu.choice_buttons[i]
		button.button_pressed = false
	upgrade.remove_info()
	upgrade.show_choices()
	game.in_game_menu.get_label_group().visible_text = false
	Main.menu_manager.add_active_menu(game.choice_menu)
	# Add signal connections.
	for i in range(Upgrade.N_CHOICES):
		var button: TextureButton = game.choice_menu.choice_buttons[i]
		button.pressed.connect(func(): select(i))
	game.choice_menu.confirm_button.pressed.connect(_on_confirm_pressed)


# Runs once when the stage ends.
func end():
	# Remove signal connections.
	for i in range(Upgrade.N_CHOICES):
		var button: TextureButton = game.choice_menu.choice_buttons[i]
		for connection in button.pressed.get_connections():
			connection["signal"].disconnect(connection["callable"])
	game.choice_menu.confirm_button.pressed.disconnect(_on_confirm_pressed)
	# Default to the first choice if one was not selected.
	if (selected < 0):
		selected = 0
	# Apply the selected choice.
	upgrade.apply_choice(selected)
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
		if (choice == i):
			button.button_pressed = true
			selected = i
		else:
			button.button_pressed = false
	game.choice_menu.confirm_button.disabled = false
	upgrade.show_info(choice)


# Called when the confirm button is pressed.
func _on_confirm_pressed():
	if (selected > -1):
		game.next_stage = PlanningStage.new(game)
