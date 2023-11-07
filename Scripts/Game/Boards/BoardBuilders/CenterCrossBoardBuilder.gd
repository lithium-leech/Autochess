# Makes the classic 8x8 board.
class_name CenterCrossBoardBuilder extends BoardBuilder


# Creates a new instance of a classic board builder.
#   game: The game to build a board in.
func _init(_game: Game):
	game = _game
	start_white = false


# Builds a new game board.
#   return: A new Board.
func build() -> Board:
	# Create a new board.
	var board: Board = Board.new()
	board.game = game
	board.width = 7
	board.height = 7
	board.player_rows = 2
	board.enemy_rows = 2
	board.player_pieces = []
	board.enemy_pieces = []
	board.corner_tl = Vector2i(-112, -208)
	board.corner_br = board.corner_tl + Vector2i(board.width*32, board.height*32)
	# Create a square array of spaces.
	board.spaces = []
	for x in range(board.width):
		board.spaces.append([])
		for y in range(board.height):
			board.spaces[x].append(Space.new(board, x, y))
	# Remove spaces in the desired locations.
	board.spaces[3][3] = null
	board.spaces[2][3] = null
	board.spaces[4][3] = null
	board.spaces[3][2] = null
	board.spaces[3][4] = null
	# Change the top and bottom rows to promotion spaces.
	for x in range(board.width):
		board.spaces[x][0].is_player_promotion = true
		board.spaces[x][board.height-1].is_enemy_promotion = true
	# Create tiles for the configured spaces.
	create_tiles(board)
	# Return the finished board.
	return board
