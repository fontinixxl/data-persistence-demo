using Core;
using UnityEngine;

namespace Persistence
{
    public class PersistenceManager : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO saveSystem;

        [Header("Listen to")]
        [SerializeField] private VoidEventChannelSO gameOverEvent = default;
        [SerializeField] private VoidEventChannelSO deleteSaveDataEvent = default;

        private void Awake()
        {
            saveSystem.LoadSaveDataFromDisk();
        }

        private void OnEnable()
        {
            gameOverEvent.OnEventRaised += SaveHighScore;
            deleteSaveDataEvent.OnEventRaised += DeleteSavedData;
        }

        private void DeleteSavedData()
        {
            saveSystem.WriteEmptySaveFile();
        }

        private void SaveHighScore()
        {
            if (saveSystem.playerScore.Value > saveSystem.SaveData.HighScore)
            {
                saveSystem.SaveDataToDisk();
            }
        }
    }
}
