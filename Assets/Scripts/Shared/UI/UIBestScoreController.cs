using UnityEngine;
using UnityEngine.UI;

namespace Fontinixxl.Shared.UI
{
    public class UIBestScoreController : MonoBehaviour
    {
        [SerializeField] private Text bestScoreText;

        public void DisplayBestScoreText(string playerName, int highScore)
        {
            bestScoreText.text = $"Best Score: {playerName} : {highScore}";
        }
    }
}
