using System;
using System.IO;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    private static string FILE_NAME = "savefile.json";
    
    public static DataPersistence Instance;

    // Current session player name
    public string CurrentPlayerName { get; set; }
    
    // Save/Load data
    public string HighScorePlayerName { get; private set; }
    public int HighScore { get; private set; }
    
    private string _savePath;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        _savePath = Path.Combine(Application.persistentDataPath, FILE_NAME);
        Load();
    }

    private void ResetLocalData()
    {
        HighScorePlayerName = string.Empty;
        HighScore = 0;
    }
    
    [Serializable]
    public struct SaveData
    {
        public string PlayerName;
        public int HighScore;
    }
    
    public void Save(int points)
    {
        HighScorePlayerName = CurrentPlayerName;
        HighScore = points;
        
        var data = new SaveData
        {
            PlayerName = CurrentPlayerName,
            HighScore =  points
        };

        var json = JsonUtility.ToJson(data);
  
        File.WriteAllText(_savePath, json);
        
        Debug.Log("Data saved!");
    }

    private void Load()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScorePlayerName = data.PlayerName;
            HighScore = data.HighScore;
            
            Debug.Log("Data Loaded!");
        }
    }

    public void DeleteSaveData()
    {
        ResetLocalData();
        
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Saved data deleted!");
        }
    }
}
