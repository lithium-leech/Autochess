# A power that steals the first turn.
extends Power


# The kind of power this is.
func get_kind() -> Kind:
	return Kind.FIRST


# Applies this power to the current game.
func activate():
	# Switch player colors.
	Main.game_state.is_player_white = not Main.game_state.is_player_white


# Unapplies this power from the current game.
func deactivate():
	# Switch player colors.
	Main.game_state.is_player_white = not Main.game_state.is_player_white
