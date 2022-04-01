using Fontinixxl.Persistence;
using Fontinixxl.Shared.ScriptableObjects;
using Fontinixxl.Shared.ScriptableObjects.EventChannels;
using Fontinixxl.Shared.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
namespace Fontinixxl.MainMenu
{
    [DefaultExecutionOrder(1000)]
    public class MainMenuController : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private SaveSystemSO saveSystem;
        [FormerlySerializedAs("PlayerInputField")] 
        [SerializeField] private InputField playerInputField;
        [SerializeField] private UIBestScoreController UIBestScorePanel;
        
        [Header("Start Load")]
        [Tooltip("Scene to Load when the Start Button is clicked")]
        [SerializeField] private AssetReference mainScene;

        [Header("Broadcasting on")]
        [SerializeField] private LoadEventChannelSO loadLocation = default;
        [SerializeField] private VoidEventChannelSO deleteSavedData = default;
    
        private void Start()
        {
            playerInputField.text = saveSystem.playerName.Value;
            UIBestScorePanel.DisplayBestScoreText(saveSystem.playerName.Value, saveSystem.playerScore.Value);
        }

        public void StartNewGameButtonHandler()
        {
            // Save player name to a Session var (StringVariableSO) globally accessible
            saveSystem.playerName.Value = playerInputField.text;
            loadLocation.RaiseEvent(mainScene);
        }

        public void DeleteSavedDataButtonHandler()
        {
            deleteSavedData.RaiseEvent();
            playerInputField.text = string.Empty;
            UIBestScorePanel.DisplayBestScoreText("", 0);
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
