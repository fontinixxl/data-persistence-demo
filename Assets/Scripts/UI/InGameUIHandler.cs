using Fontinixxl.Gameplay;
using Fontinixxl.Persistence;
using Fontinixxl.ScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.UI;

namespace Fontinixxl.UI
{
    public class InGameUIHandler : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private SaveSystemSO saveSystem;

        [Header("Listen to")]
        [SerializeField] private BoolEventChannelSO onGameOver = default;
        
        [Header("UI Elements")]
        [SerializeField] private UIBestScoreController UIBestScorePanel;
        [SerializeField] private Text scoreText;
        [SerializeField] private GameObject gameOverText;
        
        private void OnEnable()
        {
            GameController.OnScorePoint += DisplayPoints;
            onGameOver.OnEventRaised += OnGameOverEventHandler;
        }
        
        private void OnDisable()
        {
            GameController.OnScorePoint -= DisplayPoints;
            onGameOver.OnEventRaised -= OnGameOverEventHandler;
        }

        private void Start()
        {
            // Update Best Score UI with data loaded from disk
            DisplayBestScore(saveSystem.SaveData.PlayerName, saveSystem.SaveData.HighScore);
        }

        private void OnGameOverEventHandler(bool isHighScore)
        {
            if (isHighScore)
            {
                DisplayBestScore(saveSystem.playerName.Value, saveSystem.playerScore.Value);
            }
            
            DisplayGameOverText();
        }

        private void DisplayPoints(int points)
        {
            scoreText.text = $"Score : {points}";
        }
        
        private void DisplayGameOverText()
        {
            gameOverText.SetActive(true);
        }

        private void DisplayBestScore(string pName, int bestScore)
        {
            UIBestScorePanel.DisplayBestScoreText(pName, bestScore);
        }
    }
}
