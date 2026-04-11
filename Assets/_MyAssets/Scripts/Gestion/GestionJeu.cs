using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionJeu : MonoBehaviour
{
    public static GestionJeu Instance;

    private int _pointage = 0;
    public int Pointage => _pointage;

    private List<int> _listeAccrochages = new List<int>();
    public List<int> ListeAccrochages => _listeAccrochages;

    private List<float> _listeTemps = new List<float>();
    public List<float> ListeTemps => _listeTemps;

    private float _startTime;
    private bool _aCommence = false; 

    private float _startTimeNiveau;
    private int _pointageDebutNiveau;

    private bool _isPaused = false;

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
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        GestionCollision.OnCollisionOccured += CollisionManager_OnCollisionOccured;
    }

    private void Start()
    {
        _startTime = Time.time;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1.0f;

        _startTimeNiveau = Time.time;
        _pointageDebutNiveau = _pointage;

        _aCommence = false;

        Player.OnPlayerPause -= Player_OnPlayerPause;
        Player.OnPlayerPause += Player_OnPlayerPause;
    }

    public void StartTimer()
    {
        if (!_aCommence)
        {
            _startTime = Time.time;
            _aCommence = true;
        }
    }

    public float GetTempsActuel()
    {
        if (!_aCommence) return 0f;
        return Time.time - _startTime;
    }

    public void RecommencerNiveau()
    {
        Time.timeScale = 1.0f;

        // Détruire UIGame pour qu'il se recrée proprement
        if (UIGame.Instance != null)
        {
            Destroy(UIGame.Instance.gameObject);
        }

        // Se détruire soi-même aussi
        Destroy(gameObject);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void RecommencerNiveau()
    //{
    //    _pointage = 0;        
    //    _aCommence = false;
    //    Time.timeScale = 1.0f;
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    public void AugmenterPointage()
    {
        _pointage++;
    }

    public void SetNiveau(float temps)
    {
        if (_listeAccrochages.Count == 0)
        {
            _listeAccrochages.Add(_pointage);
        }
        else
        {
            ListeAccrochages.Add(_pointage - _listeAccrochages.Sum());
        }

        _listeTemps.Add(temps);
    }

    private void CollisionManager_OnCollisionOccured(object sender, GestionCollision.OnCollisionOccuredEventArgs e)
    {
        AugmenterPointage();
    }

    public void Player_OnPlayerPause(object sender, System.EventArgs e)
    {
        if (_isPaused)
        {
            Time.timeScale = 1.0f;
            _isPaused = false;
        }
        else
        {
            Time.timeScale = 0.0f;
            _isPaused = true;
        }
    }

    private float _endTime;
public float EndTime
{
    get => _endTime;
    set => _endTime = value;
}

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GestionCollision.OnCollisionOccured -= CollisionManager_OnCollisionOccured;
        Player.OnPlayerPause -= Player_OnPlayerPause;
    }
}