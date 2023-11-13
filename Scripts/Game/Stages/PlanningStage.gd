# The stage of the game where the player chooses where to put their pieces.
class_name PlanningStage extends Stage


# Creates a new instance of a planning stage.
#   game: The game to run the stage in.
func _init(_game: Game):
	game = _game


# The game this is being run in.
var game: Game

# The object currently held by the player.
var held: GameObject

# A node containing sprites highlighting the different placement zones.
var highlight_node: Node2D


# Runs once when the stage starts.
func start():
	# Start the planning music.
	Main.music_box.play_music(Music.Kind.PLANNING)
	# Set up the boards.
	game.game_board.clear()
	game.side_board.clear()
	set_up_player()
	PlacementAI.set_up_enemy(game)
	# Add highlights around the different the placement zones.
	highlight_node = Node2D.new()
	Main.game_world.add_child(highlight_node)
	Highlighter.new(game.game_board, highlight_node).add_highlights()
	Highlighter.new(game.side_board, highlight_node).add_highlights()
	# Activate buttons.
	game.in_game_menu.concede_button.pressed.connect(start_concede)
	game.in_game_menu.fight_button.pressed.connect(start_fight)
	# TEMPORARY: Add starting pieces.
	var player_space: Space = game.game_board.get_space(Vector2i(6, 6))
	game.game_board.add_piece(Piece.Kind.PAWN, true, player_space)
	var enemy_space: Space = game.game_board.get_space(Vector2i(1, 1))
	game.game_board.add_piece(Piece.Kind.PAWN, false, enemy_space)


# Runs repeatedly while the player is in this stage.
func during():
	# Get the current mouse position.
	var pointer = Main.game_port.get_mouse_position()
	pointer.x -= Main.game_world.position.x
	pointer.y -= Main.game_world.position.y
	# Check if the position is inside the game board or side board.
	var board: Board = null
	if (game.game_board.corner_tl.x <= pointer.x and \
		pointer.x <= game.game_board.corner_br.x and \
		game.game_board.corner_tl.y <= pointer.y and \
		pointer.y <= game.game_board.corner_br.y):
		board = game.game_board
	elif (game.side_board.corner_tl.x <= pointer.x and \
		pointer.x <= game.side_board.corner_br.x and \
		game.side_board.corner_tl.y <= pointer.y and \
		pointer.y <= game.side_board.corner_br.y):
		board = game.side_board
	# Get the current space.
	var space: Space = null
	if (board != null):
		space = board.get_space(board.to_coordinates(pointer))
	# Grab objects on click.
	if (Input.is_action_just_pressed("game_select") and \
		board != null and \
		space != null and \
		held == null):
		held = space.grab()
		if (held != null):
			held.warp_to(pointer)
	# Drag grabbed objects.
	elif (Input.is_action_pressed("game_select") and held != null):
		held.warp_to(pointer)
	# Drop objects on release.
	elif (Input.is_action_just_released("game_select") and held != null):
		if (board != null and \
			space != null and \
			held.is_placeable(space) and \
			space.is_enterable(held)):
			space.add_object(held)
		else:
			held.space.add_object(held)
		held = null
	# Disable the fight button when there are no pieces on the board
	if (game.game_board.player_pieces.size() > 0):
		game.in_game_menu.fight_button.disabled = false
	else:
		game.in_game_menu.fight_button.disabled = true


# Runs once when the stage ends.
func end():
	# Deactivate buttons.
	if (game.in_game_menu.concede_button.pressed.is_connected(start_concede)):
		game.in_game_menu.concede_button.pressed.disconnect(start_concede)
	if (game.in_game_menu.fight_button.pressed.is_connected(start_fight)):
		game.in_game_menu.fight_button.pressed.disconnect(start_fight)
	game.in_game_menu.concede_button.disabled = true
	game.in_game_menu.fight_button.disabled = true
	cancel_concede()
	# Remove highlights.
	highlight_node.queue_free()
	# Save the player's placement set up.
	pass


# Starts the fight sequence.
func start_fight():
	# Deactivate the fight button.
	game.in_game_menu.fight_button.pressed.disconnect(start_fight)
	# Queue the combat stage.
	game.next_stage = CombatStage.new(game)
	pass


# Displays the concede menu to the player.
func start_concede():
	pass


# The player concedes and the fight is lost.
func confirm_concede():
	pass


# Removes the concede menu and resets concede logic.
func cancel_concede():
	pass


# Sets up the player's pieces on the game board and side board.
func set_up_player():
	# Place the player's pieces on the game board.
	# Place the player's pieces on the side board.
	# Place the player's objects on the side board.
	pass
