using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Fontinixxl.Shared.ScriptableObjects.EventChannels;

/// <summary>
/// Allows a "cold start" in the editor, when pressing Play and not passing from the PersistenceManager scene.
/// </summary> 
public class EditorColdStartup : MonoBehaviour
{
    [SerializeField] private AssetReference persistenceManagerScene = default;
    [SerializeField] private VoidEventChannelSO onSceneReadyChannel = default;
    
    private bool _isColdStart;

    private void Awake()
    {
        if (!SceneManager.GetSceneByName(persistenceManagerScene.editorAsset.name).isLoaded)
        {
            _isColdStart = true;
        }
    }

    private void Start()
    {
        if (_isColdStart)
        {
            persistenceManagerScene.LoadSceneAsync(LoadSceneMode.Additive).Completed += LoadEventChannel;
        }
    }

    private void LoadEventChannel(AsyncOperationHandle<SceneInstance> notUsedHandler)
    {
        //Raise a fake scene ready event, so the gameplay will start
        onSceneReadyChannel.RaiseEvent();
    }
}
