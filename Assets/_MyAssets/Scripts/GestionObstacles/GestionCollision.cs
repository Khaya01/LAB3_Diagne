using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GestionCollision : MonoBehaviour
{
    // ***** Attributs *****
    [SerializeField] private Material _materielRouge;
    [SerializeField] private float _delaiReactivation = 4f;
    private Material _materielInitial;
    private bool _touche;  // Booléen qui permet de détecter si l'objet a été touché
    private float _tempsTouche = 0f;

    public static event EventHandler<OnCollisionOccuredEventArgs> OnCollisionOccured;

    public class OnCollisionOccuredEventArgs : EventArgs
    {
        public int CollisionValue;   // Arg de l'evenement
    }

    // ***** Méthodes privées *****
    private void Start()
    {
        _materielInitial = GetComponent<MeshRenderer>().material;
        _touche= false;  // initialise le booléen à faux au début de la scène
    }

    private void Update()
    {
        if (Time.time > (_tempsTouche + _delaiReactivation) && _touche)
        {
            gameObject.GetComponent<MeshRenderer>().material = _materielInitial;
            _touche = false;
        }
    }

    /* 
     * Rôle : Méthode qui se déclenche quand une collision se produit avec l'objet
     * Entrée : un objet de type Collision qui représente l'objet avec qui la collision s'est produite
     */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !_touche)  // Si l'objet avec la collision s'est produite est le joueur et qu'il n'a pas déjà et touché
        {
            _touche = true;  // change le booléen à vrai pour indiqué que l'objet a été touché
            gameObject.GetComponent<MeshRenderer>().material = _materielRouge;  //change la couleur du matériel à rouge
            GestionJeu.Instance.AugmenterPointage();  // Appelle la méthode publique dans GestionJeu pour augmenter le pointage
            _tempsTouche = Time.time;
            OnCollisionOccured?.Invoke(this, new OnCollisionOccuredEventArgs { CollisionValue = 1 });
        }
    }
}
