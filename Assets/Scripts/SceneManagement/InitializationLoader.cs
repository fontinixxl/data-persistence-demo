using Fontinixxl.Shared.ScriptableObjects.EventChannels;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Fontinixxl.SceneManagement
{
    /// <summary>
    /// This class is responsible for starting the game by loading the persistent managers scene 
    /// and raising the event to load the Main Menu
    /// </summary>

    public class InitializationLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference managersScene = default;
        [SerializeField] private AssetReference menuToLoad = default;

        [Header("Broadcasting on")]
        [SerializeField] private AssetReference menuLoadChannel = default;

        private void Start()
        {
            //Load the persistent managers scene
            managersScene.LoadSceneAsync(LoadSceneMode.Additive).Completed += LoadEventChannel;
        }

        private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            menuLoadChannel.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
        }

        private void LoadMainMenu(AsyncOperationHandle<LoadEventChannelSO> obj)
        {
            obj.Result.RaiseEvent(menuToLoad);

            SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
        }
    }

}