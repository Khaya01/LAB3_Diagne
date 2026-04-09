using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIStart : MonoBehaviour
{

    [SerializeField] GameObject _startPanel;
    [SerializeField] GameObject _instructionPanel;

    [SerializeField] Button _startButton;
    [SerializeField] Button _closeButton;

    public void OnStratClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnInstructionClick()
    {
        // Faire apparaitre l'instruction
        _startPanel.SetActive(false);
        _instructionPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_closeButton.gameObject);
    }

    public void OnCloseClick()
    {
        // Faire apparaitre l'instruction
        _startPanel.SetActive(true);
        _instructionPanel.SetActive(false);
    }

    public void OnQuitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Quitter l'executable en cours
#endif
    }
}
