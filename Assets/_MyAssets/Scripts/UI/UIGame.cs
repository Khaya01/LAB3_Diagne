using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
            // Transfère les références du nouveau au ancien
            Instance._pausePanel = this._pausePanel;
            Instance._continueButton = this._continueButton;
            Instance._txtTime = this._txtTime;
            Instance._txtCollisions = this._txtCollisions;
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("UIGame Start!");
        Player.OnPlayerPause += Player_OnPlayerPause;
        Debug.Log("UIGame subscrit à OnPlayerPause");
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

        if (player != null && player.GetABouger())
        {
            float elapsedTime = Time.time - GestionJeu.Instance.StartTime;
            _txtTime.text = $"Temps : {elapsedTime:f2}";
        }
        else
        {
            _txtTime.text = "Temps : 0.00";
        }
    }

    private void CollisionManager_OnCollisionOccured(object sender, GestionCollision.OnCollisionOccuredEventArgs e)
    {
        ChangeCollisionUI();
    }

    private void ChangeCollisionUI()
    {
        _txtCollisions.text = $"Collisions : {GestionJeu.Instance.Pointage}";
    }

    public void FermerPausePanel()
    {
        _pausePanel.SetActive(false);
    }

    public void OnRecommencer()
    {
        GestionJeu.Instance.RecommencerNiveau();
    }

    public void OnContinue()
    {
        // Continuer
        Player.TriggerOnPlayerPause(this);
    }
}
