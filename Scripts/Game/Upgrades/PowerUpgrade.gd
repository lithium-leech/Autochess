# Contains a set of powers to choose from.
class_name PowerUpgrade extends Upgrade


# Create a new instance of a power upgrade.
func _init(_game: Game):
	_base_init(_game)
	get_random_powers()


# The available player powers to choose from.
var player_powers: Array[Power.Kind] = []

# The available enemy powers to choose from.
var enemy_powers: Array[Power.Kind] = []


# Gets the title text to be displayed with this upgrade.
# 	return: A string.
func get_title_text():
	return "Text.ChoosePower"


# Displays the offered upgrade choices.
func show_choices():
	# Set the sprites for each choice.
	for i in range(N_CHOICES):
		game.choice_menu.player_choices[i].texture = Power.get_icon(player_powers[i])
		game.choice_menu.enemy_choices[i].texture = Power.get_icon(enemy_powers[i])


# Displays info about the selected upgrade choice.
# 	choice: The choice to display information about.
func show_info(choice: int):
	# Erase previously displayed info.
	remove_info()
	# Set the info pieces.
	game.choice_menu.player_info.texture = Power.get_icon(player_powers[choice])
	game.choice_menu.enemy_info.texture = Power.get_icon(enemy_powers[choice])
	# Change the text.
	var p_key: String = Power.Kind.keys()[player_powers[choice]].to_pascal_case()
	var e_key: String = Power.Kind.keys()[enemy_powers[choice]].to_pascal_case()
	var p_name: String = "PowerName." + p_key
	var p_info: String = "PowerInfo." + p_key
	var e_name: String = "PowerName." + e_key
	var e_info: String = "PowerInfo." + e_key
	game.choice_menu.set_info_text(p_name, p_info, e_name, e_info)


# Applies the given choice to the game.
# 	choice: The index of the selected choice.
func apply_choice(choice: int):
	# Create and activate the enemy power.
	var player_power: Power = Power.create_power(player_powers[choice])
	player_power.is_player = true
	player_power.is_white = false
	player_power.is_grabable = false
	Main.game_world.add_child(player_power)
	var player_space: Space = game.player_power_board.get_first_empty_space()
	game.player_power_board.add_object(player_power, player_space)
	player_power.activate()
	# Create and activate the player power.
	var enemy_power: Power = Power.create_power(enemy_powers[choice])
	enemy_power.is_player = false
	enemy_power.is_white = false
	enemy_power.is_grabable = false
	Main.game_world.add_child(enemy_power)
	var enemy_space: Space = game.enemy_power_board.get_first_empty_space()
	game.enemy_power_board.add_object(enemy_power, enemy_space)
	enemy_power.activate()


# Gets the random choices of powers.
func get_random_powers():
	# Get the currently possessed player powers.
	var current_player: Array[Power.Kind] = []
	for x in range(game.player_power_board.width):
		for y in range(game.player_power_board.height):
			var space: Space = game.player_power_board.get_space(Vector2i(x, y))
			if (space != null and space.object != null and space.object is Power):
				current_player.append(space.object.get_kind())
	# Get the currently possessed enemy powers.
	var current_enemy: Array[Power.Kind] = []
	for x in range(game.enemy_power_board.width):
		for y in range(game.enemy_power_board.height):
			var space: Space = game.enemy_power_board.get_space(Vector2i(x, y))
			if (space != null and space.object != null and space.object is Power):
				current_enemy.append(space.object.get_kind())
	# Get the available powers.
	var available_player: Array[Power.Kind] = Power.get_available_powers(game, true, player_powers)
	var available_enemy: Array[Power.Kind] = Power.get_available_powers(game, false, enemy_powers)
	# Set the power choices.
	for i in range(N_CHOICES):
		player_powers.append(available_player[randi_range(0, available_player.size() - 1)])
		enemy_powers.append(available_enemy[randi_range(0, available_enemy.size() - 1)])
