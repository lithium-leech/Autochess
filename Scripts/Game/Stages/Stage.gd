# Represents one stage of gameplay.
class_name Stage

# Runs once when the stage starts.
# Must be implemented by inheriting classes.
func start():
	pass

# Runs repeatedly while the player is in this stage.
func during():
	pass

# Runs once when the stage ends.
func end():
	pass
