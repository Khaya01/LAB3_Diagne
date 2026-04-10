using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinPartie : MonoBehaviour
{
    // ***** Attributs *****
    private bool _finPartie = false;  // booléen qui détermine si la partie est terminée
    private Player _player;  // attribut qui contient un objet de type Player

    // ***** Méthode privées  *****
    
    private void Start()
    {
        _player = FindAnyObjectByType<Player>();  // récupère sur la scène le gameObject de type Player
    }

    /*
     * Méthode qui se produit quand il y a collision avec le gameObject de fin
     */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !_finPartie)  // Si la collision est produite avec le joueur et la partie n'est pas terminée
        {
            _finPartie = true; // met le booléen à vrai pour indiquer la fin de la partie
            int noScene = SceneManager.GetActiveScene().buildIndex; // Récupère l'index de la scène en cours
            GestionJeu.Instance.SetNiveau(Time.time - _player.GetTempsDepart());
            Debug.Log($"ListeTemps après SetNiveau: {string.Join(", ", GestionJeu.Instance.ListeTemps)}");
            if (noScene != SceneManager.sceneCountInBuildSettings - 2)
            {
                SceneManager.LoadScene(noScene + 1);
            }
            else 
            {
                GestionJeu.Instance.EndTime = GestionJeu.Instance.ListeTemps.Sum();

                Destroy(_player.gameObject);
                for(int i=0; i < GestionJeu.Instance.ListeTemps.Count; i++)
                {
                    Debug.Log("Temps niveau" + (i+1) + " : " + GestionJeu.Instance.ListeTemps[i].ToString("f2") + " secs.");
                    Debug.Log("Collisions niveau" + (i+1) + " : " + GestionJeu.Instance.ListeAccrochages[i].ToString());
                    float total = GestionJeu.Instance.ListeTemps[i] + GestionJeu.Instance.ListeAccrochages[i];
                    Debug.Log("Temps total niveau" + (i+1) + " : " + total.ToString("f2") + " secs.");
                    Debug.Log("****************************************");
                }
                float totalFinal = GestionJeu.Instance.ListeTemps.Sum() + GestionJeu.Instance.ListeAccrochages.Sum();
                Debug.Log("Temps final : " + totalFinal.ToString("f2") + " secondes");
                SceneManager.LoadScene(noScene + 1);
            }
        }   
    }
}
