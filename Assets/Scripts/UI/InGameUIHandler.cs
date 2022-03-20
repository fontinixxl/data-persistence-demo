using Persistence;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InGameUIHandler : MonoBehaviour
    {
        [SerializeField] private SaveSystemSO saveSystem;
        [Header("Listen to")]
        [SerializeField] private VoidEventChannelSO onGameOver = default;
        [Header("UI Elements")]
        [SerializeField] private Text scoreText;
        [SerializeField] private Text highScoreText;
        [SerializeField] private GameObject gameOverText;
        
        private void OnEnable()
        {
            GameController.OnScorePoint += DisplayPoints;
            onGameOver.OnEventRaised += DisplayGameOverText;
        }

        private void Start()
        {
            DisplayHighScore();
        }

        private void DisplayPoints(int points)
        {
            scoreText.text = $"Score : {points}";
        }
        
        private void DisplayGameOverText()
        {
            gameOverText.SetActive(true);
            DisplayHighScore();
        }

        private void DisplayHighScore()
        {
            var playerName = saveSystem.saveData.PlayerName;
            var highScore = saveSystem.saveData.HighScore;
            highScoreText.text = $"Best Score: {playerName} : {highScore}";
        }
        
        private void OnDisable()
        {
            GameController.OnScorePoint -= DisplayPoints;
            onGameOver.OnEventRaised -= DisplayGameOverText;
        }
    }
}
