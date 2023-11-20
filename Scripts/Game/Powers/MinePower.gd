# A power that awards a land mine.
extends Power


# The kind of power this is.
func get_kind() -> Kind:
	return Kind.MINE


# Applies this power to the current game.
func activate():
	if (self.is_player):
		var _space: Space = space.board.game.side_board.get_first_empty_space()
		var record: Placement = Placement.record_new_item(Item.Kind.MINE, space.coordinates, false)
		space.board.game.player_placements.append(record)
	else:
		space.board.game.enemy_items.append(Item.Kind.MINE)
