# The concede confirmation pop-up.
class_name ConcedeMenu extends Menu


# The confirmation button.
var confirm_button: TextureButton

# The cancel button.
var cancel_button: TextureButton


# Called when the node enters the scene tree for the first time.
func _ready():
	confirm_button = $Confirm
	cancel_button = $Cancel
