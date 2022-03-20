using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using ScriptableObjects.PrimitiveTypes;

public class GameController : MonoBehaviour
{
    [Header("Listening to")] [SerializeField]
    private VoidEventChannelSO onSceneReady;
    [Header("Broadcasting on")]
    [SerializeField] private AssetReference thisScene;
    [SerializeField] private LoadEventChannelSO loadLocation = default;
    [SerializeField] private VoidEventChannelSO gameOverEvent = default;
    [Space]
    [SerializeField] private IntVariable playerScore;
    
    public static event Action<int> OnScorePoint;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    private bool _started;
    private bool _gameOver;

    private void OnEnable()
    {
        onSceneReady.OnEventRaised += StartGame;
    }

    private void StartGame()
    {
        // Reset previous score
        playerScore.SetValue(0);
        
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
                StartCoroutine(ReloadSceneRoutine());
            }
        }
    }

    private void AddPoint(int point)
    {
        playerScore.ApplyChange(point);
        OnScorePoint?.Invoke(playerScore.Value);
    }

    public void GameOver()
    {
        gameOverEvent.RaiseEvent();
        _gameOver = true;
    }

    private IEnumerator ReloadSceneRoutine()
    {
        yield return new WaitForEndOfFrame();
        loadLocation.RaiseEvent(thisScene);
    }

    private void OnDisable()
    {
        onSceneReady.OnEventRaised -= StartGame;
    }
}
