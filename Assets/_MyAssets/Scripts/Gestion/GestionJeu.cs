using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionJeu : MonoBehaviour
{
    public static GestionJeu Instance;

    // ***** Attributs *****
    private int _pointage = 0;
    public int Pointage => _pointage;

    private List<int> _listeAccrochages = new List<int>();
    public List<int> ListeAccrochages => _listeAccrochages;

    private List<float> _listeTemps = new List<float>();
    public List<float> ListeTemps => _listeTemps;

    private float _startTimeNiveau;
    private int _pointageDebutNiveau;

    private float _startTime;
    public float StartTime => _startTime;

    private float _endTime;
    public float EndTime { get => _endTime; set => _endTime = value; }

    private bool _isPaused = false;
    private bool _tempsDemarre = false;

    private int _indexDerniereScene = -1;

    // ***** Méthodes privées *****
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1.0f;

        Player.OnPlayerPause -= Player_OnPlayerPause;
        Player.OnPlayerPause += Player_OnPlayerPause;

        GestionCollision.OnCollisionOccured -= CollisionManager_OnCollisionOccured;
        GestionCollision.OnCollisionOccured += CollisionManager_OnCollisionOccured;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1.0f;
        _isPaused = false;

        if (scene.buildIndex != _indexDerniereScene)
        {
            _startTimeNiveau = GetTempsActuel();
            _pointageDebutNiveau = _pointage;
            _indexDerniereScene = scene.buildIndex;
        }

        _tempsDemarre = false;

        Player.OnPlayerPause -= Player_OnPlayerPause;
        Player.OnPlayerPause += Player_OnPlayerPause;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        GestionCollision.OnCollisionOccured -= CollisionManager_OnCollisionOccured;
        Player.OnPlayerPause -= Player_OnPlayerPause;
    }

    // ***** Méthodes publiques *****

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
            _listeAccrochages.Add(_pointage - _listeAccrochages.Sum());
        }

        _listeTemps.Add(temps);
    }

    public void DemarrerTemps()
    {
        if (!_tempsDemarre)
        {
            _startTime = Time.time;
            _tempsDemarre = true;
        }
    }

    public bool GetTempsDemarre()
    {
        return _tempsDemarre;
    }

    public float GetTempsActuel()
    {
        if (_tempsDemarre)
        {
            return _startTimeNiveau + (Time.time - _startTime);
        }

        return _startTimeNiveau;
    }

    public float GetTempsDebutNiveau()
    {
        return _startTimeNiveau;
    }

    public void RecommencerNiveau()
    {
        _pointage = _pointageDebutNiveau;
        _isPaused = false;
        _tempsDemarre = false;
        Time.timeScale = 1.0f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Player_OnPlayerPause(object sender, System.EventArgs e)
    {
        Debug.Log("GestionJeu pause! isPaused: " + _isPaused + " timeScale: " + Time.timeScale);

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

    private void CollisionManager_OnCollisionOccured(object sender, GestionCollision.OnCollisionOccuredEventArgs e)
    {
        AugmenterPointage();
    }
}