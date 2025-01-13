using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string saveDirectory = Application.persistentDataPath + "/Saves/";
    private static string defaultFileName = "SaveFile.json";

    public static void SaveGame(GameData gameData, string fileName = null)
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
        }

        string path = saveDirectory + (fileName ?? defaultFileName);
        string json = JsonUtility.ToJson(gameData, true);

        File.WriteAllText(path, json);
        Debug.Log($"Game saved to {path}");
    }

    public static GameData LoadGame(string fileName = null)
    {
        string path = saveDirectory + (fileName ?? defaultFileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            Debug.Log($"Game loaded from {path}");
            return gameData;
        }
        else
        {
            Debug.LogWarning($"Save file not found at {path}");
            return null;
        }
    }

    public static string[] GetSaveFiles()
    {
        if (Directory.Exists(saveDirectory))
        {
            return Directory.GetFiles(saveDirectory, "*.json");
        }
        return new string[0];
    }
}
