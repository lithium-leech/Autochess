# Makes the classic 8x8 board.
class_name ClassicBoardBuilder extends BoardBuilder


# Creates a new instance of a classic board builder.
#   game: The game to build a board in.
func _init(_game: Game):
	game = _game


# Builds a new game board.
#   return: A new Board.
func build() -> Board:
	# Create a new board.
	var board: Board = Board.new()
	board.game = game
	board.width = 8
	board.height = 8
	board.player_rows = 2
	board.enemy_rows = 2
	board.player_pieces = []
	board.enemy_pieces = []
	board.corner_tl = Vector2i(-128, -224)
	board.corner_br = board.corner_tl + Vector2i(board.width*32, board.height*32)
	# Create a square array of spaces.
	board.spaces = []
	for x in range(board.width):
		board.spaces.append([])
		for y in range(board.height):
			board.spaces[x].append(Space.new(board, x, y))
	# Create tiles for the configured spaces.
	create_tiles(board)
	# Return the finished board.
	return board
