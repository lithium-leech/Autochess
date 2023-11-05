# Generates a particular board configuration.
class_name BoardBuilder


# Gets the desired board builder.
static func get_board_builder(kind: Board.Kind, _game: Game):
    match (kind):
        Board.Kind.CLASSIC:
            return ClassicBoardBuilder.new(_game)
        Board.Kind.HOURGLASS:
            return HourglassBoardBuilder.new(_game)
        Board.Kind.ZIGZAG:
            return ZigZagBoardBuilder.new(_game)
        Board.Kind.CENTERCROSS:
            return CenterCrossBoardBuilder.new(_game)
        Board.Kind.SMALL:
            return SmallBoardBuilder.new(_game)


# Builds a new game board.
# Must be implemented by inheriting classes.
#   return: A new Board.
func build() -> Board:
    return null


# The game to make the board in.
var game: Game

# True if the top-left corner is a white space
var start_white: bool = true


# Creates tiles for the given board.
#   board: The board to create tiles for.
func create_tiles(board: Board):
    # Delete any existing tiles.
    if (board.tile_node != null):
        board.tile_node.queue_free()
        board.tile_node = null
    # Create a new tile node
    board.tile_node = Node2D.new()
    Main.game_world.add_child(board.tile_node)
    # Iterate the spaces plus their perimeter
    for x in range(-1, board.width+1):
        for y in range(-1, board.height+1):
            # Get the space and position of the current coordinates.
            var coordinates: Vector2i = Vector2i(x, y)
            var space: Space = board.get_space(coordinates)
            var position: Vector2i = board.to_position(coordinates)
            # Create a tile for the space
            if (space == null):
                create_border_tiles(board, coordinates, position)
            else:
                create_space_tiles(board, coordinates, position)


# Creates tiles for a single space on the board.
#   board: The board to create tiles for.
#   coordinates: The coordinates of the space to create tiles for.
#   position: The game world position of the space to create tiles for.
func create_space_tiles(board: Board, coordinates: Vector2i, position: Vector2i):
    # Determine if this space is black or white
    var start_color: bool = ((coordinates.x % 2) + (coordinates.y % 2)) % 2 == 0
    var white: bool = (start_color and start_white) or (!start_color and !start_white)
    var kind: Tile.Kind = Tile.Kind.SPACE_WHITE if white else Tile.Kind.SPACE_BLACK
    # Create 4 tiles to fill the board space
    var tile_bl: Sprite2D = Tile.create_tile(kind)
    var tile_br: Sprite2D = Tile.create_tile(kind)
    var tile_tl: Sprite2D = Tile.create_tile(kind)
    var tile_tr: Sprite2D = Tile.create_tile(kind)
    # Move the 4 tiles to each corner of the space
    tile_bl.position = position + Vector2i(-8, 8)
    tile_br.position = position + Vector2i(8, 8)
    tile_tl.position = position + Vector2i(-8, -8)
    tile_tr.position = position + Vector2i(8, -8)
    # Add the 4 tiles to the board's tile node
    board.tile_node.add_child(tile_bl)
    board.tile_node.add_child(tile_br)
    board.tile_node.add_child(tile_tl)
    board.tile_node.add_child(tile_tr)


# Creates tiles for a single space off the board.
#   board: The board to create tiles for.
#   coordinates: The coordinates of the space to create tiles for.
#   position: The game world position of the space to create tiles for.
func create_border_tiles(board: Board, coordinates: Vector2i, position: Vector2i):
    # Get the space above and below.
    var above: Space = board.get_space(coordinates + Vector2i(0, -1))
    var below: Space = board.get_space(coordinates + Vector2i(0, 1))
    # Check if they are promotion spaces.
    var top_promotion: bool = above != null and above.is_enemy_promotion
    var bottom_promotion: bool = below != null and below.is_player_promotion
    # Determine which neighboring spaces are present.
    var top: bool = above != null
    var bottom: bool = below != null
    var left: bool = board.get_space(coordinates + Vector2i(-1, 0)) != null
    var right: bool = board.get_space(coordinates + Vector2i(1, 0)) != null
    var top_left: bool = board.get_space(coordinates + Vector2i(-1, -1)) != null
    var top_right: bool = board.get_space(coordinates + Vector2i(1, -1)) != null
    var bottom_left: bool = board.get_space(coordinates + Vector2i(-1, 1)) != null
    var bottom_right: bool = board.get_space(coordinates + Vector2i(1, 1)) != null
    # Create the bottom-left tile of this space
    var key_bl: int = (1 if bottom_promotion else 0) << 0 | \
                     (1 if bottom else 0) << 1 | \
                     (1 if left else 0) << 2 | \
                     (1 if bottom_left else 0) << 3
    var tile_bl: Sprite2D = null
    match (key_bl):
        2, 10:
            tile_bl = Tile.create_tile(Tile.Kind.SIDE_TL)
        3, 11:
            tile_bl = Tile.create_tile(Tile.Kind.SIDE_TLP)
        4, 5, 12, 13:
            tile_bl = Tile.create_tile(Tile.Kind.SIDE_RB)
        6, 14:
            tile_bl = Tile.create_tile(Tile.Kind.CORNER_BL)
        7, 15:
            tile_bl = Tile.create_tile(Tile.Kind.CORNER_BLP)
        8, 9:
            tile_bl = Tile.create_tile(Tile.Kind.CORNER_TR)
    if (tile_bl != null):
        tile_bl.position = position + Vector2i(-8, 8)
        board.tile_node.add_child(tile_bl)
    # Create the bottom-right tile of this space
    var key_br: int = (1 if bottom_promotion else 0) << 0 | \
                     (1 if bottom else 0) << 1 | \
                     (1 if right else 0) << 2 | \
                     (1 if bottom_right else 0) << 3
    var tile_br: Sprite2D = null
    match (key_br):
        2, 10:
            tile_br = Tile.create_tile(Tile.Kind.SIDE_TR)
        3, 11:
            tile_br = Tile.create_tile(Tile.Kind.SIDE_TRP)
        4, 5, 12, 13:
            tile_br = Tile.create_tile(Tile.Kind.SIDE_LB)
        6, 14:
            tile_br = Tile.create_tile(Tile.Kind.CORNER_BR)
        7, 15:
            tile_br = Tile.create_tile(Tile.Kind.CORNER_BRP)
        8, 9:
            tile_br = Tile.create_tile(Tile.Kind.CORNER_TL)
    if (tile_br != null):
        tile_br.position = position + Vector2i(8, 8)
        board.tile_node.add_child(tile_br)
    # Create the top-left tile of this space
    var key_tl: int = (1 if top_promotion else 0) << 0 | \
                     (1 if top else 0) << 1 | \
                     (1 if left else 0) << 2 | \
                     (1 if top_left else 0) << 3
    var tile_tl: Sprite2D = null
    match (key_tl):
        2, 10:
            tile_tl = Tile.create_tile(Tile.Kind.SIDE_BL)
        3, 11:
            tile_tl = Tile.create_tile(Tile.Kind.SIDE_BLP)
        4, 5, 12, 13:
            tile_tl = Tile.create_tile(Tile.Kind.SIDE_RT)
        6, 14:
            tile_tl = Tile.create_tile(Tile.Kind.CORNER_TL)
        7, 15:
            tile_tl = Tile.create_tile(Tile.Kind.CORNER_TLP)
        8, 9:
            tile_tl = Tile.create_tile(Tile.Kind.CORNER_BR)
    if (tile_tl != null):
        tile_tl.position = position + Vector2i(-8, -8)
        board.tile_node.add_child(tile_tl)
    # Create the top-right tile of this space
    var key_tr: int = (1 if top_promotion else 0) << 0 | \
                     (1 if top else 0) << 1 | \
                     (1 if right else 0) << 2 | \
                     (1 if top_right else 0) << 3
    var tile_tr: Sprite2D = null
    match (key_tr):
        2, 10:
            tile_tr = Tile.create_tile(Tile.Kind.SIDE_BR)
        3, 11:
            tile_tr = Tile.create_tile(Tile.Kind.SIDE_BRP)
        4, 5, 12, 13:
            tile_tr = Tile.create_tile(Tile.Kind.SIDE_LT)
        6, 14:
            tile_tr = Tile.create_tile(Tile.Kind.CORNER_TR)
        7, 15:
            tile_tr = Tile.create_tile(Tile.Kind.CORNER_TRP)
        8, 9:
            tile_tr = Tile.create_tile(Tile.Kind.CORNER_BL)
    if (tile_tr != null):
        tile_tr.position = position + Vector2i(8, -8)
        board.tile_node.add_child(tile_tr)
