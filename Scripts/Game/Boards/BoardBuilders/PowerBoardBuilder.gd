# Makes a small 4x1 board for displaying powers above or below the game board.
class_name PowerBoardBuilder extends BoardBuilder


# Creates a new instance of a power board builder.
#   game: The game to build a board in.
#   player: True if this is the player's power board.
func _init(_game: Game, player: bool):
	game = _game
	start_white = player


# Builds a new game board.
#   return: A new Board.
func build() -> Board:
	# Create a new board.
	var board: Board = Board.new()
	board.game = game
	board.width = 4
	board.height = 1
	board.player_rows = 1
	board.enemy_rows = 0
	board.player_pieces = []
	board.enemy_pieces = []
	board.corner_tl = Vector2i(-64, 48) if start_white else Vector2i(-64, -272)
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
