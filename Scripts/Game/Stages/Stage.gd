# Represents one stage of gameplay.
class_name Stage


# Must be implemented by inheriting classes.
# Runs once when the stage starts.
func start():
	pass


# Must be implemented by inheriting classes.
# Runs repeatedly while the player is in this stage.
func during():
	pass


# Must be implemented by inheriting classes.
# Runs once when the stage ends.
func end():
	pass
