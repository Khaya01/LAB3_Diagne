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
            SceneManager.sceneLoaded += OnSceneLoaded; 
        }
        else
        {
            Instance._pausePanel = this._pausePanel;
            Instance._continueButton = this._continueButton;
            Instance._txtTime = this._txtTime;
            Instance._txtCollisions = this._txtCollisions;

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Instance != this) return;
        Player.OnPlayerPause -= Player_OnPlayerPause;
        Player.OnPlayerPause += Player_OnPlayerPause;
        GestionCollision.OnCollisionOccured -= CollisionManager_OnCollisionOccured;
        GestionCollision.OnCollisionOccured += CollisionManager_OnCollisionOccured;
        ChangeCollisionUI();
    }

    private void Player_OnPlayerPause(object sender, System.EventArgs e)
    {
        Debug.Log("UIGame reçu! pausePanel: " + _pausePanel);
        _pausePanel.SetActive(!_pausePanel.activeSelf);
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(_continueButton.gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Player.OnPlayerPause -= Player_OnPlayerPause;
        GestionCollision.OnCollisionOccured -= CollisionManager_OnCollisionOccured;
    }



    private void Update()
    {
        TimeDisplayUI();
    }

    private void TimeDisplayUI()
    {
        Player player = FindFirstObjectByType<Player>();
        if (player == null)
        {
            Debug.Log("Player introuvable!");
            return;
        }

        Debug.Log($"aBouger: {player.GetABouger()} | temps: {GestionJeu.Instance.GetTempsActuel()}");

        if (player.GetABouger())
        {
            _txtTime.text = $"Temps : {GestionJeu.Instance.GetTempsActuel():f2}";
        }
        else
        {
            _txtTime.text = "Temps : 0.00";
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("UIGame rebind après reload");
        StartCoroutine(RebindAfterLoad());
    }

    private IEnumerator RebindAfterLoad()
    {
        yield return null; // attend que tous les Awake/Start de la scène soient faits

        Player.OnPlayerPause -= Player_OnPlayerPause;
        Player.OnPlayerPause += Player_OnPlayerPause;

        GestionCollision.OnCollisionOccured -= CollisionManager_OnCollisionOccured;
        GestionCollision.OnCollisionOccured += CollisionManager_OnCollisionOccured;

        ChangeCollisionUI();
    }

    private void CollisionManager_OnCollisionOccured(object sender, GestionCollision.OnCollisionOccuredEventArgs e)
    {
        ChangeCollisionUI();
    }

    private void ChangeCollisionUI()
    {
        Debug.Log($"_txtCollisions null? {_txtCollisions == null} | pointage: {GestionJeu.Instance.Pointage}");
        _txtCollisions.text = $"Collisions : {GestionJeu.Instance.Pointage}";
    }

    public void FermerPausePanel()
    {
        _pausePanel.SetActive(false);
    }

    public void OnRecommencer()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        GestionJeu.Instance.RecommencerNiveau();
    }


    public void OnContinue()
    {
        // Continuer
        Player.TriggerOnPlayerPause(this);
    }
}
