using Persistence;
using ScriptableObjects.PrimitiveTypes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    [Header("Listening to")] [SerializeField]
    private VoidEventChannelSO onSceneReady;
    [Header("Broadcasting on")]
    [SerializeField] private AssetReference thisScene;
    [SerializeField] private LoadEventChannelSO loadLocation = default;
    [Space]
    [SerializeField] private SaveSystemSO saveSystem;
    [Space]
    [SerializeField] private IntVariable playerScore;
    [SerializeField] private StringVariable playerName;
    
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    [Header("UI")]
    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool _started;
    private bool _gameOver;

    private void OnEnable()
    {
        onSceneReady.OnEventRaised += StartGame;
    }

    private void StartGame()
    {
        const float step = 0.6f;
        var perLine = Mathf.FloorToInt(4.0f / step);
        var bricksAnchor = new GameObject("Bricks");
        int[] pointCountArray = {1,1,2,2,5,5};
        for (var i = 0; i < LineCount; ++i)
        {
            for (var x = 0; x < perLine; ++x)
            {
                var position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.transform.SetParent(bricksAnchor.transform);
                
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        saveSystem.LoadSaveDataFromDisk();
        DisplayHighScore();
    }

    private void Update()
    {
        if (!_started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (_gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                loadLocation.RaiseEvent(thisScene);
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
        _gameOver = true;
        playerScore.SetValue(0);
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

    private void OnDisable()
    {
        onSceneReady.OnEventRaised -= StartGame;
    }
}
