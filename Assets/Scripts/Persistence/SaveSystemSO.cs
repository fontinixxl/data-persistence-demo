using ScriptableObjects.ScriptableTypes;
using UnityEngine;

namespace Persistence
{
    // Commented so no other instance of SaveSystem is created
    // [CreateAssetMenu(menuName = "SO/SaveSystem", fileName = "SaveSystem", order = 0)]
    public class SaveSystemSO : ScriptableObject
    {
        public string saveFilename = "save.json";
        [Header("Runtime Data to Save")]
        public IntVariable playerScore;
        public StringVariable playerName;
        public SaveData SaveData = new SaveData();

        private bool _dataLoadedInSession;

        private void OnDisable()
        {
            SaveData.Reset();
            _dataLoadedInSession = false;
        }

        public void SaveDataToDisk()
        {
            SaveData.HighScore = playerScore.Value;
            SaveData.PlayerName = playerName.Value;
            
            if (FileManager.WriteToFile(saveFilename, SaveData.ToJson()))
            {
                Debug.Log("Save successful");
            }
        }
    
        public bool LoadSaveDataFromDisk()
        {
            if (_dataLoadedInSession)
            {
                Debug.LogWarningFormat($"Data not loaded! it has been loaded already during this session!");
                return false;
            }
            
            if (FileManager.LoadFromFile(saveFilename, out var json))
            {
                SaveData.LoadFromJson(json);
                playerName.Value = SaveData.PlayerName;
                playerScore.Value = SaveData.HighScore;
                
                _dataLoadedInSession = true;
                return _dataLoadedInSession;
            }
            
            return false;
        }
        
        public void WriteEmptySaveFile()
        {
            SaveData.Reset();
            _dataLoadedInSession = false;
            FileManager.WriteToFile(saveFilename, "");
        }
    }
}
