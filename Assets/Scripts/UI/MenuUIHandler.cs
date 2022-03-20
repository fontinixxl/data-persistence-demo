using ScriptableObjects.PrimitiveTypes;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
namespace UI
{
    [DefaultExecutionOrder(1000)]
    public class MenuUIHandler : MonoBehaviour
    {
        [SerializeField] private StringVariable playerName;
        [FormerlySerializedAs("PlayerInputField")] 
        [SerializeField] private InputField playerInputField;
        [Tooltip("Scene to Load when the Start Button is clicked")]
        [SerializeField] private AssetReference mainScene;

        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadLocation = default;
        [SerializeField] private VoidEventChannelSO deleteSavedData = default;
    
        private void Start()
        {
            playerInputField.text = playerName.Value;
        }

        public void StartNewGameButtonHandler()
        {
            // Save player name to a Session var (StringVariableSO) globally accessible
            playerName.Value = playerInputField.text;
            loadLocation.RaiseEvent(mainScene);
        }

        public void DeleteSavedDataButtonHandler()
        {
            deleteSavedData.RaiseEvent();
            playerInputField.text = string.Empty;
        }

        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
        }
    }
}
