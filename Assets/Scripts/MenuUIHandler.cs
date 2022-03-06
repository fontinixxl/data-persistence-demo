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

    public void StartNew()
    {
        SavePlayerName();
        // TODO: Refactor using Scene names with addressables or something different than indexes
        SceneManager.LoadScene(1);
    }
    
    private void SavePlayerName()
    {
        if (string.IsNullOrEmpty(PlayerInputField.text)) return;
        
        DataPersistence.Instance.PlayerName = PlayerInputField.text;
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
