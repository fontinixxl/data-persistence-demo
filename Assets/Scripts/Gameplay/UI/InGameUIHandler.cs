using Fontinixxl.Persistence;
using Fontinixxl.Shared.ScriptableObjects.EventChannels;
using Fontinixxl.Shared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Fontinixxl.Gameplay.UI
{
    /// <summary>
    /// Handle in game UI, displaying the current and high score
    /// </summary>
    public class InGameUIHandler : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private SaveSystemSO saveSystem;

        [Header("Listen to")]
        [SerializeField] private BoolEventChannelSO onGameOver = default;
        [SerializeField] private VoidEventChannelSO onSceneReady = default;
        
        [Header("UI Elements")]
        [SerializeField] private UIBestScoreController UIBestScorePanel;
        [SerializeField] private Text scoreText;
        [SerializeField] private GameObject gameOverText;
        
        private void OnEnable()
        {
            GameController.OnGameStart += HideGameOverText;
            GameController.OnScorePoint += DisplayPoints;
            onSceneReady.OnEventRaised += OnGameStartEventHandler;
            onGameOver.OnEventRaised += OnGameOverEventHandler;
        }

        private void OnDisable()
        {
            GameController.OnGameStart -= HideGameOverText;
            GameController.OnScorePoint -= DisplayPoints;
            onSceneReady.OnEventRaised -= OnGameStartEventHandler;
            onGameOver.OnEventRaised -= OnGameOverEventHandler;
        }

        private void OnGameStartEventHandler()
        {
            gameOverText.SetActive(false);
            // Best Score is displayed after the OnSceneReady event so we know the save data is loaded
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

        private void HideGameOverText()
        {
            gameOverText.SetActive(false);
        }

        private void DisplayBestScore(string pName, int bestScore)
        {
            UIBestScorePanel.DisplayBestScoreText(pName, bestScore);
        }
    }
}
