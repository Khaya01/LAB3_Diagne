using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class Player : MonoBehaviour
{
    // ***** Attributs *****
    
    [SerializeField] private float _vitesse = 800f;  //Vitesse de d�placement du joueur
    [SerializeField] private float _rotationSpeed = 700f;
    private Rigidbody _rb;  // Variable pour emmagasiner le rigidbody du joueur
    private bool _aBouger = false;
    private float _tempsDepart = -1f;
    private InputSystem_Actions _inputSystem_Actions;


    public static event EventHandler OnPlayerPause;

    public static void TriggerOnPlayerPause(object sender)
    {
        OnPlayerPause?.Invoke(sender, EventArgs.Empty);
    }

    //  ***** M�thodes priv�es *****

    private Action<UnityEngine.InputSystem.InputAction.CallbackContext> _pauseAction;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _aBouger = false;

        _inputSystem_Actions = new InputSystem_Actions();
        _inputSystem_Actions.Player.Enable();
            _pauseAction = (ctx) => {
        Debug.Log("Pause fired! Subscribers: " + OnPlayerPause?.GetInvocationList().Length);
        OnPlayerPause?.Invoke(this, EventArgs.Empty);
    };
        _inputSystem_Actions.Player.Pause.performed += _pauseAction;
    }

    private void OnDestroy()
    {
        _inputSystem_Actions.Player.Pause.performed -= _pauseAction;
        _inputSystem_Actions.Player.Disable();
    }


    private void Update()
    {
        if (_aBouger && _tempsDepart == -1)
        {
            _tempsDepart = Time.time;
        }
    }
    // Ici on utilise FixedUpdate car les mouvements du joueurs implique le d�placement d'un rigidbody
    private void FixedUpdate()
    {
        MouvementsJoueur();
    }

    /*
     * M�thode qui g�re les d�placements du joueur
     */
    private void MouvementsJoueur()
    {
        float positionX = Input.GetAxisRaw("Horizontal"); // R�cup�re la valeur de l'axe horizontal de l'input manager
        float positionZ = Input.GetAxisRaw("Vertical");  // R�cup�re la valeur de l'axe vertical de l'input manager
        Vector3 direction = new Vector3(positionX, 0f, positionZ);  // �tabli la direction du vecteur � appliquer sur le joueur
        direction.Normalize();
        _rb.linearVelocity = direction * Time.deltaTime * _vitesse;  // Applique la v�locit� sur le corps du joueur dans la direction du vecteur
        
        if (direction != Vector3.zero)
        {
            _aBouger = true;
            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
    }

    // ***** M�thodes publiques *****

    public float GetTempsDepart()
    {
        if ( _tempsDepart == -1)
        {
            return 0;
        }
        else
        {
            return _tempsDepart;
        }
    }

    public bool GetABouger()
    {
        return _aBouger;
    }

    public void EndGamePlayer()
    {
        Destroy(gameObject);
    }
}
