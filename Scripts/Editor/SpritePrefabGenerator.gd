# A tool for generating prefabs from sprite images.
@tool extends EditorScript


# Change this property to target different sprites.
# The folder to find sprite images in.
var sprites_path: String = "res://Sprites/Highlights/"

# The folder to save scenes in.
var scenes_path: String = "res://Scenes/"


# Called when the editor script is executed.
func _run():
	# Get the sprites.
	var sprites: Array[Sprite2D] = []
	var dir: DirAccess = DirAccess.open(sprites_path)
	for path in dir.get_files():
		if (path.ends_with(".png")):
			var image: CompressedTexture2D = load(sprites_path + path)
			var sprite: Sprite2D = Sprite2D.new()
			sprite.name = path.replace(".png", "")
			sprite.texture = image
			sprites.append(sprite)
	# Create a new scene folder.
	var components = sprites_path.split("/")
	var group_name: String = components[components.size() - 2]
	DirAccess.make_dir_absolute(scenes_path + group_name)
	# Create a scene for each sprite.
	for sprite in sprites:
		var scene_path: String = scenes_path + group_name + "/" + sprite.name + ".tscn"
		var scene: PackedScene = PackedScene.new()
		scene.resource_path = scene_path
		scene.pack(sprite)
		var error: Error = ResourceSaver.save(scene)
		if error != Error.OK:
			print("Error saving packed scene:", error)
		else:
			print("Packed scene saved successfully:", scene.resource_path)
