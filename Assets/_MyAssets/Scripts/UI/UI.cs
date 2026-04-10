using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{


    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Quitter l'executable en cours
#endif
    }

    public void OnRestartClick()
    {
        SceneManager.LoadScene(0);
    }


}
