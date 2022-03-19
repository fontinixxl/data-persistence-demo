#if UNITY_EDITOR
using UnityEditor;
#endif
using Persistence;
using ScriptableObjects.PrimitiveTypes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private SaveSystemSO saveSystem;
    [SerializeField] private StringVariable playerName;
    [FormerlySerializedAs("PlayerInputField")] 
    [SerializeField] private InputField playerInputField;
    
    private void Start()
    {
        if (!saveSystem.LoadSaveDataFromDisk()) return;
        
        playerName.Value = saveSystem.saveData.PlayerName;
        playerInputField.text = playerName.Value;
    }

    public void StartNewGameButtonHandler()
    {
        StoreInputFieldPlayerNameIntoVariable();
        SceneManager.LoadScene(1);
    }
    
    private void StoreInputFieldPlayerNameIntoVariable()
    {
        playerName.Value = playerInputField.text;
    }
    
    public void DeleteSavedDataButtonHandler()
    {
        saveSystem.WriteEmptySaveFile();
        playerInputField.text = string.Empty;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
        saveSystem.SaveDataToDisk();
    }
}
