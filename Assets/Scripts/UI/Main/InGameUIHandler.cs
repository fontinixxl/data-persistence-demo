using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Main
{
    public class InGameUIHandler : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO saveSystem;
        [Header("Listen to")]
        [SerializeField] private VoidEventChannelSO onGameOver = default;
        [Header("UI Elements")]
        [SerializeField] private UIBestScoreController UIBestScorePanel;
        [SerializeField] private Text scoreText;
        [SerializeField] private GameObject gameOverText;
        
        private void OnEnable()
        {
            GameController.OnScorePoint += DisplayPoints;
            onGameOver.OnEventRaised += DisplayGameOverText;
        }

        private void Start()
        {
            DisplayBestScore();
        }

        private void DisplayPoints(int points)
        {
            scoreText.text = $"Score : {points}";
        }
        
        private void DisplayGameOverText()
        {
            gameOverText.SetActive(true);
            DisplayBestScore();
        }

        private void DisplayBestScore()
        {
            var playerName = saveSystem.SaveData.PlayerName;
            var bestScore = saveSystem.SaveData.HighScore;
            
            UIBestScorePanel.DisplayBestScoreText(playerName, bestScore);
        }

        private void OnDisable()
        {
            GameController.OnScorePoint -= DisplayPoints;
            onGameOver.OnEventRaised -= DisplayGameOverText;
        }
    }
}
