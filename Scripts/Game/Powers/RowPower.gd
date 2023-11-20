# A power that adds another row for placing pieces.
extends Power


# The kind of power this is.
func get_kind() -> Kind:
	return Kind.ROW


# Applies this power to the current game.
func activate():
	# Add a row.
	if (self.is_player):
		space.board.game.game_board.player_rows += 1
	else:
		space.board.game.game_board.enemy_rows += 1


# Unapplies this power from the current game.
func deactivate():
	# Remove a row.
	if (self.is_player):
		space.board.game.game_board.player_rows -= 1
	else:
		space.board.game.game_board.enemy_rows -= 1
