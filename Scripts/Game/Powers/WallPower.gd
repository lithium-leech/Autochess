# A power that awards walls.
extends Power


# The kind of power this is.
func get_kind() -> Kind:
	return Kind.WALL


# Applies this power to the current game.
func activate():
	for i in range(2):
		if (self.is_player):
			var _space: Space = Placement.get_first_empty_side_board_space(space.board.game)
			if (_space != null):
				var record: Placement = Placement.record_new_item(Item.Kind.WALL, _space.coordinates, false)
				space.board.game.player_placements.append(record)
		else:
			space.board.game.enemy_items.append(Item.Kind.WALL)
