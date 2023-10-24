# Finds all child labels, and displays them in the text viewport.
# 	To work properly the node this script is attached
# 	to must be in the center of the root node.
extends Control


# True if the text in this group is presently visible.
@export var visible_text: bool = true:
	set(value):
		for label in labels.values():
			label.visible = value

# The labels belonging to this group.
# Maps game world labels to text world labels
var labels: Dictionary

# The root node generated in the text world space.
var text_tree: Control


# Called when the node enters the scene tree for the first time.
func _ready():
	# Create the text tree.
	text_tree = Control.new()
	GameState.text_world.add_child(text_tree)
	# Grow the text tree.
	for child in get_children():
		grow_text_tree(child, text_tree)


# Called when the node is about to leave the scene tree.
func _exit_tree():
	text_tree.queue_free()


# Grows a node from the text world space into a tree.
# 	game_node: The node from the game world space to process.
# 	text_parent: The node from the text tree to create new nodes in.
func grow_text_tree(game_node: Node, text_parent: Control):
	# Check if the given game node is a label.
	var text_node: Control
	if (game_node is Label):
		# Create a new label and add it to the label collection.
		text_node = Label.new()
		labels[game_node] = text_node
		# Copy the properties of the original label.
		text_node.visible = game_node.is_visible_in_tree()
		text_node.layout_direction = game_node.layout_direction
		text_node.layout_mode = game_node.layout_mode
		text_node.localize_numeral_system = game_node.localize_numeral_system
		text_node.horizontal_alignment = game_node.horizontal_alignment
		text_node.vertical_alignment = game_node.vertical_alignment
		text_node.clip_text = true
		text_node.label_settings = game_node.label_settings
		text_node.label_settings.font_size *= 4
		text_node.text = game_node.text
		# Erase the text in the game world space.
		game_node.text = ""
	else:
		# Create a new control with the same layout.
		text_node = Control.new()
		text_node.layout_direction = game_node.layout_direction
		text_node.layout_mode = game_node.layout_mode
	# Add the new node to the text tree.
	text_parent.add_child(text_node)
	text_node.size = game_node.size * Vector2(4, 4)
	text_node.position = game_node.position * Vector2(4, 4)
	# Grow the remaining branches.
	for game_child in game_node.get_children():
		grow_text_tree(game_child, text_node)
