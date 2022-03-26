using ScriptableObjects.EventChannels;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class InGameUIHandler : MonoBehaviour
    {
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
        
        private void OnDisable()
        {
            GameController.OnScorePoint -= DisplayPoints;
            onGameOver.OnEventRaised -= DisplayGameOverText;
        }
        
        private void DisplayPoints(int points)
        {
            scoreText.text = $"Score : {points}";
        }
        
        private void DisplayGameOverText()
        {
            gameOverText.SetActive(true);
        }

        public void DisplayBestScore(string playerName, int bestScore)
        {
            UIBestScorePanel.DisplayBestScoreText(playerName, bestScore);
        }
    }
}
