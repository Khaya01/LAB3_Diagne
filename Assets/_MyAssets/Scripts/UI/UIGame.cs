using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIGame : UI
{
    public static UIGame Instance;

    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Button _continueButton;
    [SerializeField] private TextMeshProUGUI _txtTime;
    [SerializeField] private TextMeshProUGUI _txtCollisions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void Start()
    {
        Debug.Log("UIGame Start!");

        Player.OnPlayerPause -= Player_OnPlayerPause;
        Player.OnPlayerPause += Player_OnPlayerPause;

        GestionCollision.OnCollisionOccured -= CollisionManager_OnCollisionOccured;
        GestionCollision.OnCollisionOccured += CollisionManager_OnCollisionOccured;

        ChangeCollisionUI();
    }

    private void OnDestroy()
    {
        Player.OnPlayerPause -= Player_OnPlayerPause;
        GestionCollision.OnCollisionOccured -= CollisionManager_OnCollisionOccured;
    }

    private void Update()
    {
        TimeDisplayUI();
    }

    private void Player_OnPlayerPause(object sender, System.EventArgs e)
    {
        Debug.Log("UIGame reçu! pausePanel: " + _pausePanel);

        if (_pausePanel != null)
        {
            _pausePanel.SetActive(!_pausePanel.activeSelf);
        }

        if (_pausePanel != null && _pausePanel.activeSelf)
        {
            StartCoroutine(SelectionnerBoutonContinue());
        }
    }

    private IEnumerator SelectionnerBoutonContinue()
    {
        yield return null;

        if (EventSystem.current != null && _continueButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_continueButton.gameObject);
        }
    }

    private void TimeDisplayUI()
    {
        Player player = FindFirstObjectByType<Player>();

        if (player != null && player.GetABouger())
        {
            GestionJeu.Instance.DemarrerTemps();
            float elapsedTime = GestionJeu.Instance.GetTempsActuel();
            _txtTime.text = $"Temps : {elapsedTime:f2}";
        }
        else
        {
            _txtTime.text = $"Temps : {GestionJeu.Instance.GetTempsDebutNiveau():f2}";
        }
    }

    private void CollisionManager_OnCollisionOccured(object sender, GestionCollision.OnCollisionOccuredEventArgs e)
    {
        ChangeCollisionUI();
    }

    private void ChangeCollisionUI()
    {
        if (_txtCollisions != null)
        {
            _txtCollisions.text = $"Collisions : {GestionJeu.Instance.Pointage}";
        }
    }

    public void FermerPausePanel()
    {
        if (_pausePanel != null)
        {
            _pausePanel.SetActive(false);
        }
    }

    public void OnRecommencer()
    {
        if (_pausePanel != null)
        {
            _pausePanel.SetActive(false);
        }

        Time.timeScale = 1.0f;
        GestionJeu.Instance.RecommencerNiveau();
    }

    public void OnContinue()
    {
        Player.TriggerOnPlayerPause(this);
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject pauseObj = GameObject.Find("PausePanel");
        if (pauseObj != null)
        {
            _pausePanel = pauseObj;
        }

        GameObject continueObj = GameObject.Find("BtnContinue");
        if (continueObj != null)
        {
            _continueButton = continueObj.GetComponent<Button>();
        }

        GameObject timeObj = GameObject.Find("TxtTime");
        if (timeObj != null)
        {
            _txtTime = timeObj.GetComponent<TextMeshProUGUI>();
        }

        GameObject collisionsObj = GameObject.Find("TxtCollisions");
        if (collisionsObj != null)
        {
            _txtCollisions = collisionsObj.GetComponent<TextMeshProUGUI>();
        }

        ChangeCollisionUI();
    }
}