using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Save
{
    public static void SaveGame(Player player, int level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameProgress.spicy";
        FileStream stream = new FileStream(path, FileMode.Create);
        GameProgress data = new GameProgress(player, level);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameProgress LoadGame()
    {
        string path = Application.persistentDataPath + "/gameProgress.spicy";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameProgress data = formatter.Deserialize(stream) as GameProgress;
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }
}
