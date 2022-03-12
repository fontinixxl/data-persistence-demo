#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Sets the script to be executed later than all default scripts
// This is helpful for UI, since other things may need to be initialized before setting the UI
[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private InputField PlayerInputField;

    private void Start()
    {
        PlayerInputField.text = DataPersistence.Instance.HighScorePlayerName;
    }

    public void StartNew()
    {
        SavePlayerName();
        SceneManager.LoadScene(1);
    }
    
    private void SavePlayerName()
    {
        if (string.IsNullOrEmpty(PlayerInputField.text)) return;
        
        DataPersistence.Instance.CurrentPlayerName = PlayerInputField.text;
    }
    
    public void DeleteSavedDataButtonHandler()
    {
        DataPersistence.Instance.DeleteSaveData();
        PlayerInputField.text = string.Empty;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
        // MainManager.Instance.SaveColor();
    }
}
