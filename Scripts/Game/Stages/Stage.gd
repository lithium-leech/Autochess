# Represents one stage of gameplay.
class_name Stage


# Must be implemented by inheriting classes.
# Runs once when the stage starts.
func start():
	pass


# Must be implemented by inheriting classes.
# Runs repeatedly while the player is in this stage.
# 	delta: The elapsed time since the previous frame.
func during(_delta: float):
	pass


# Must be implemented by inheriting classes.
# Runs once when the stage ends.
func end():
	pass
