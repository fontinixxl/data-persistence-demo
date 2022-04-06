using System;
using Fontinixxl.Persistence;
using Fontinixxl.Shared.ScriptableObjects.EventChannels;
using Fontinixxl.Shared.ScriptableObjects.ScriptableTypes;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Fontinixxl.Gameplay
{
    public enum GameSate
    {
        WaitForSceneLoad,
        ReadyStart,
        Gameplay,
        GameOver
    }
    
    /// <summary>
    /// Central manager for the Gameplay, will trigger events for the UI to display
    /// score and handle logic for the game over
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public static event Action<int> OnScorePoint;
        public static event Action OnGameStart;
        
        [Header("Listening to")] [SerializeField]
        private VoidEventChannelSO onSceneReady;
        
        [Header("Broadcasting on")]
        [SerializeField] private AssetReference thisScene;
        [SerializeField] private BoolEventChannelSO gameOverEvent = default;
        
        [Header("Dependencies")]
        [SerializeField] private SaveSystemSO saveSystem;
        [SerializeField] private IntVariable playerScore;
        [SerializeField] private Brick brickPrefab;
        [SerializeField] private Ball ballPrefab;

        [Header("Level Generator")]
        [SerializeField] private int lineCount = 6;
        
        private GameObject _bricksAnchor;
        private GameSate _currentGameSate;
        private GameSate _previousGameSate;
        
        private void OnEnable()
        {
            _previousGameSate = _currentGameSate = GameSate.WaitForSceneLoad;
            onSceneReady.OnEventRaised += ResetGameplaySates;
        }
        
        private void OnDisable()
        {
            onSceneReady.OnEventRaised -= ResetGameplaySates;
        }

        private void ResetGameplaySates()
        {
            playerScore.SetValue(0);
            DestroyRemainingBricks();
            GenerateLevel();
            SpawnBall();
            
            ChangeGameState(GameSate.ReadyStart);
        }
        
        private void ChangeGameState(GameSate gameSate)
        {
            _previousGameSate = _currentGameSate;

            if (gameSate == GameSate.Gameplay)
            {
                if (_previousGameSate == GameSate.GameOver) ResetGameplaySates();
                OnGameStart?.Invoke();
            }
            
            _currentGameSate = gameSate;
        }

        private void Update()
        {
            if (_currentGameSate == GameSate.ReadyStart || _currentGameSate == GameSate.GameOver)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ChangeGameState(GameSate.Gameplay);
                }
            }
        }

        public void GameOver()
        {
            gameOverEvent.RaiseEvent(playerScore.Value > saveSystem.SaveData.HighScore);
            ChangeGameState(GameSate.GameOver);
        }
        
        private void GenerateLevel()
        {
            const float step = 0.6f;
            var perLine = Mathf.FloorToInt(4.0f / step);
            if (_bricksAnchor == null)
            {
                _bricksAnchor = new GameObject("Bricks");
            }
            int[] pointCountArray = {1,1,2,2,5,5};
            for (var i = 0; i < lineCount; ++i)
            {
                for (var x = 0; x < perLine; ++x)
                {
                    var position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                    var brick = Instantiate(brickPrefab, position, Quaternion.identity);
                    brick.transform.SetParent(_bricksAnchor.transform);
                
                    brick.PointValue = pointCountArray[i];
                    brick.onDestroyed.AddListener(AddPoint);
                }
            }
        }
        
        private void SpawnBall()
        {
            var paddleTransform = GetComponentInChildren<Paddle>().transform; 
            var paddleCollider = paddleTransform.GetComponent<BoxCollider>();
            var bounds = paddleCollider.bounds;
            var offset = bounds.extents.y + 0.1f;
            var spawnPoint = bounds.center + Vector3.up * offset;
            
            // Instantiate ball prefab as a child of paddle so before the game start the ball will
            // move following the paddle move
            Instantiate(ballPrefab, spawnPoint, ballPrefab.transform.rotation, paddleTransform);
        }

        private void AddPoint(int point)
        {
            playerScore.ApplyChange(point);
            OnScorePoint?.Invoke(playerScore.Value);
        }

        private void DestroyRemainingBricks()
        {
            // Only if the game has been played at least once
            if (_bricksAnchor == null) return;
            
            foreach (Transform child in _bricksAnchor.transform) {
                Destroy(child.gameObject);
            }
        }
    }
}
