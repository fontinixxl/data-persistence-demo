using ScriptableObjects.PrimitiveTypes;
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
        public SaveData saveData = new SaveData();

        private bool _dataLoadedInSession;

        private void OnDisable()
        {
            saveData.Reset();
            _dataLoadedInSession = false;
        }

        public void SaveDataToDisk()
        {
            saveData.HighScore = playerScore.Value;
            saveData.PlayerName = playerName.Value;
            
            if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
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
                saveData.LoadFromJson(json);
                playerScore.Value = saveData.HighScore;
                
                _dataLoadedInSession = true;
                return _dataLoadedInSession;
            }
            
            return false;
        }
        
        public void WriteEmptySaveFile()
        {
            saveData.Reset();
            _dataLoadedInSession = false;
            FileManager.WriteToFile(saveFilename, "");
        }
    }
}
