using Persistence;
using ScriptableObjects.PrimitiveTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField] private SaveSystemSO saveSystem;
    [SerializeField] private IntVariable playerScore;
    [SerializeField] private StringVariable playerName;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started;
    private bool m_GameOver;
    
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        saveSystem.LoadSaveDataFromDisk();
        DisplayHighScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        playerScore.ApplyChange(point);
        ScoreText.text = $"Score : {playerScore.Value}";
    }

    public void GameOver()
    {
        SaveHighScore();
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    private void SaveHighScore()
    {
        if (string.IsNullOrEmpty(playerName.Value))
        {
            Debug.LogWarningFormat($"High Score not saved, since there is no PlayerName!");
            return;
        }
        
        if (playerScore.Value > saveSystem.saveData.HighScore)
        {
            saveSystem.SaveDataToDisk();
            DisplayHighScore();
        }
    }

    private void DisplayHighScore()
    {
        var highScore = saveSystem.saveData.HighScore;
        var highScorePlayerName = saveSystem.saveData.PlayerName;
        HighScoreText.text = $"Best Score: {highScorePlayerName} : {highScore}";
    }
}
