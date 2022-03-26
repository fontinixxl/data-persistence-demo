using ScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Load Events")]
        [SerializeField] private LoadEventChannelSO loadLocation = default;
        [Header("BroadCast On")] 
        [SerializeField] private VoidEventChannelSO onSceneReady;
    
        public AssetReference mainMenuScene;

        private AsyncOperationHandle<SceneInstance> _handle;
        private AssetReference _currentlyLoadedScene;
        private AssetReference _sceneToLoad;
        private bool _isLoading;
    
        private void OnEnable()
        {
            loadLocation.OnLoadingRequested += LoadLocation;
        }
    
        private void Start()
        {
            LoadLocation(mainMenuScene);
        }

        /// <summary>
        /// This function loads the location scenes passed as parameter
        /// </summary>
        private void LoadLocation(AssetReference locationToLoad)
        {
            if (_isLoading) return;

            _sceneToLoad = locationToLoad;
            _isLoading = true;
        
            UnloadPreviousScene();
        }

        private void UnloadPreviousScene()
        {
            if (_currentlyLoadedScene != null && _handle.IsValid())
            {
                _currentlyLoadedScene.UnLoadScene();
            }
        
            LoadNewScene();
        }

        private void LoadNewScene()
        {
            _sceneToLoad.LoadSceneAsync(LoadSceneMode.Additive).Completed += SceneLoadComplete;
        }

        private void SceneLoadComplete(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _currentlyLoadedScene = _sceneToLoad;
                _handle = obj;
                SceneManager.SetActiveScene(obj.Result.Scene);
                _isLoading = false;
                onSceneReady.RaiseEvent();
            }
            else
            {
                Debug.LogWarningFormat($"Failure loading {mainMenuScene.Asset.name} Scene");
            }
        }

        private void OnDisable()
        {
            loadLocation.OnLoadingRequested -= LoadLocation;
        }
    }
}
