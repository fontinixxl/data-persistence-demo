using Fontinixxl.Shared.ScriptableObjects.EventChannels;
using UnityEngine;

namespace Fontinixxl.Persistence
{
    public class PersistenceManager : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO saveSystem;

        [Header("Listen to")]
        [SerializeField] private BoolEventChannelSO gameOverEvent = default;
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

        private void OnDisable()
        {
            gameOverEvent.OnEventRaised -= SaveHighScore;
            deleteSaveDataEvent.OnEventRaised -= DeleteSavedData;
        }

        private void DeleteSavedData()
        {
            saveSystem.WriteEmptySaveFile();
        }

        private void SaveHighScore(bool isHighScore)
        {
            if (isHighScore) saveSystem.SaveDataToDisk();
        }
    }
}
