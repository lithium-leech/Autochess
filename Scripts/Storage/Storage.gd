# Contains functions for storing and retrieving data.
class_name Storage


# The file path to store app data at.
const SAVE_PATH: String = "user://data.bin"


# Stores the current state of the app.
static func save_app_data():
	var file: FileAccess = FileAccess.open(SAVE_PATH, FileAccess.WRITE)
	file.store_var(Main.game_state.start_board)
	file.store_var(Main.game_state.start_set)
	file.store_var(Main.game_state.high_score)
	file.store_var(Main.music_box.volume)
	file.store_var(Main.atlas.current_locale)


# Restores the last stored state of the app.
static func load_app_data():
	if (FileAccess.file_exists(SAVE_PATH)):
		var file: FileAccess = FileAccess.open(SAVE_PATH, FileAccess.READ)
		Main.game_state.start_board = file.get_var()
		Main.game_state.start_set = file.get_var()
		Main.game_state.high_score = file.get_var()
		Main.music_box.set_volume(file.get_var())
		Main.atlas.current_locale = file.get_var()
		TranslationServer.set_locale(Main.atlas.current_locale)
