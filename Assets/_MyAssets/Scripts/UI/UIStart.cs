using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIStart : UI
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _instructionPanel;

    [SerializeField] private Button _startButton;
    [SerializeField] private Button _closeButton;

    [SerializeField] private ScrollRect _scrollRect;

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
        StartCoroutine(SelectionnerBoutonStart());
    }

    private IEnumerator SelectionnerBoutonStart()
    {
        yield return null;

        if (EventSystem.current != null && _startButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_startButton.gameObject);
        }
    }

    private IEnumerator SelectionnerBoutonClose()
    {
        yield return null;

        if (EventSystem.current != null && _closeButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_closeButton.gameObject);
        }
    }

    public void OnStartClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnInstructionClick()
    {
        _startPanel.SetActive(false);
        _instructionPanel.SetActive(true);

        StartCoroutine(SelectionnerBoutonClose());
    }

    public void OnCloseClick()
    {
        _startPanel.SetActive(true);
        _instructionPanel.SetActive(false);

        StartCoroutine(SelectionnerBoutonStart());
    }
    private void Update()
    {
        if (_instructionPanel.activeSelf && _scrollRect != null)
        {
            float input = Input.GetAxis("Vertical");

            if (Mathf.Abs(input) > 0.1f)
            {
                _scrollRect.verticalNormalizedPosition += input * Time.unscaledDeltaTime * 2f;
            }
        }
    }

}