using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Static methods which can be used to load and save data
/// </summary>
public static class SaveSystem
{
    /// <summary>The file path to write and read data from</summary>
    private static string FilePath { get { return Path.Combine(Application.persistentDataPath, "mem.bin"); } }

    /// <summary>Creates a save file</summary>
    /// <param name="data">The save data to store</param>
    public static void Save(SaveData data)
    {
        Debug.Log("Saving data...");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(FilePath, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Finished saving data.");
    }

    /// <summary>Loads saved data</summary>
    /// <returns>Saved data</returns>
    public static SaveData Load()
    {
        // If there is no save data, return a default state
        if (!File.Exists(FilePath)) return new SaveData();

        // Load the saved data
        Debug.Log("Loading saved data...");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(FilePath, FileMode.Open);
        SaveData data = formatter.Deserialize(stream) as SaveData;
        stream.Close();
        Debug.Log("Finished loading saved data.");
        return data;
    }
}
