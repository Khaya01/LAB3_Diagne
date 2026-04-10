using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionJeu : MonoBehaviour
{
    public static GestionJeu Instance;
    
    // ***** Attributs *****
    private int _pointage = 0;  // Attribut qui conserve le nombre d'accrochages
    public int Pointage => _pointage; // Accesseur de l'attribut

    private List<int> _listeAccrochages = new List<int>();
    public List<int> ListeAccrochages => _listeAccrochages;

    private List<float> _listeTemps = new List<float>();
    public List<float> ListeTemps => _listeTemps;




    // ***** Méthodes privées *****
    private void Awake()
    {
        // Singleton        
        // Vérifie si un gameObject GestionJeu est déjà présent sur la scène si oui
        // on détruit celui qui vient d'être ajouté. Sinon on le conserve pour le 
        // scène suivante et associe Instance.
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

    // 
    private float _startTimeNiveau;
    private int _pointageDebutNiveau;

    private float _startTime;
    public float StartTime => _startTime;

    private float _endTime;
    public float EndTime { get => _endTime; set => _endTime = value; }

    private bool _isPaused = false;

    // ***** Méthodes publiques ******

    /*
     * Méthode publique qui permet d'augmenter le pointage de 1
     */
    public void AugmenterPointage()
    {
        _pointage++;
    }

    // Méthode qui reçoit les valeurs pour le niveau et l'ajoute dans les listes respectives
    public void SetNiveau(float temps)
    {
        //Si premier niveau on ajoute directement le nombre de collision
        //Sinon on ajoute les collisions - les collisions des niveaux précédents
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

    private void Start()
    {
        Time.timeScale = 1.0f;
        if (_startTime == 0) 
        {
            _startTime = Time.time;
        }
        _startTimeNiveau = Time.time;
        _pointageDebutNiveau = _pointage;
        Player.OnPlayerPause += Player_OnPlayerPause;
        GestionCollision.OnCollisionOccured += CollisionManager_OnCollisionOccured;
    }

    public void RecommencerNiveau()
    {
        _pointage = _pointageDebutNiveau; 
        _startTime = Time.time - (_startTimeNiveau - _startTime); 
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        GestionCollision.OnCollisionOccured -= CollisionManager_OnCollisionOccured;
        Player.OnPlayerPause -= Player_OnPlayerPause;
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


