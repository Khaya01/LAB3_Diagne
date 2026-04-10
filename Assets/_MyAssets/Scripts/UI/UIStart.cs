using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIStart : UI
{

    [SerializeField] GameObject _startPanel;
    [SerializeField] GameObject _instructionPanel;

    [SerializeField] Button _startButton;
    [SerializeField] Button _closeButton;

    private void Awake()
    {
        GestionJeu gameManager = FindAnyObjectByType<GestionJeu>();
        if (gameManager != null)
        {
            Destroy(gameManager.gameObject);
        }

        UIGame uigame = FindAnyObjectByType<UIGame>();
        if (uigame != null)
        {
            Destroy(uigame.gameObject);
        }
    }


    private void Start()
    {
        //Selectionner le bouton demarrer au lancement de la scene
        EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
    }

    public void OnStartClick()
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



}
