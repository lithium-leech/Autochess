# Manages labels in the game world by moving their text to the text viewport.
# 	To work properly the node this script is attached
# 	to must be in the center of the root node.
class_name LabelGroup extends Control


# True if the text in this group is presently visible.
@export var visible_text: bool = true:
	set(value):
		for label in labels.values():
			label.visible = value

# The labels belonging to this group.
# Maps game world labels to text world labels.
var labels: Dictionary

# The root node generated in the text world space.
var text_tree: Control


# Called when the node enters the scene tree for the first time.
func _ready():
	# Create the text tree.
	text_tree = Control.new()
	Main.text_world.add_child(text_tree)
	# Grow the text tree.
	var is_rtl: bool = Atlas.is_rtl(Main.atlas.current_locale)
	for child in get_children():
		grow_text_tree(child, text_tree, is_rtl)
	# Connect to the locale change event
	Main.atlas.on_locale_change.connect(_on_locale_change)


# Called when the node is about to leave the scene tree.
func _exit_tree():
	text_tree.queue_free()


# Called when the locale is changed.
func _on_locale_change():
	for label in labels.values():
			label.label_settings.font = Main.atlas.fonts[Main.atlas.current_locale]


# Grows a node from the text world space into a tree.
# 	game_node: The node from the game world space to process.
# 	text_parent: The node from the text tree to create new nodes in.
# 	is_rtl: True if the current locale is an rtl locale.
func grow_text_tree(game_node: Node, text_parent: Control, is_rtl: bool):
	# Check the type of the given game node.
	var text_node: Node
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
		text_node.label_settings.font = Main.atlas.fonts[Main.atlas.current_locale]
		text_node.label_settings.font_size *= 4
		text_node.text = game_node.text
		text_node.size = game_node.size * Vector2(4, 4)
		text_node.position = game_node.position * Vector2(4, 4)
		if (is_rtl): text_node.position.x -= text_parent.size.x
		# Erase the text in the game world space.
		game_node.text = ""
	elif (game_node is Control):
		# Create a new control with the same layout.
		text_node = Control.new()
		text_node.layout_direction = game_node.layout_direction
		text_node.layout_mode = game_node.layout_mode
		text_node.size = game_node.size * Vector2(4, 4)
		text_node.position = game_node.position * Vector2(4, 4)
		if (is_rtl): text_node.position.x -= text_parent.size.x
	else:
		# Create a new node with no layout.
		text_node = Node.new()
	# Add the new node to the text tree.
	text_parent.add_child(text_node)
	# Grow the remaining branches.
	for game_child in game_node.get_children():
		grow_text_tree(game_child, text_node, is_rtl)
