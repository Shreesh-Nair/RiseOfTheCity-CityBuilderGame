using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    // Call this from the UI Button's OnClick event
    public void QuitGame()
    {
#if UNITY_EDITOR
        // This will stop Play Mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // This will quit the built application
        Application.Quit();
#endif
        Debug.Log("QuitGame called");
    }
}
