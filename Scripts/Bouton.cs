/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Ce script contient le code pour l'interaction des boutons avec les projectiles du joueurs et leur changement de couleur lors d'une erreur.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouton : MonoBehaviour
{
    [SerializeField] private AudioClip _sonActivation; //clips de son d'une activation
    [SerializeField] private int _nombre; //int du nombre du bouton. Permet de différencier les quatres boutons en leur assignant un chiffre
    [SerializeField] private Enigme _enigmeScript; //script de la zone d'énigme
    private Coroutine _modifCouleur; //coroutine pour modifier la culeur des boutons lors d'une erreur
    private float _tempsAffichage = 3f; //float du temps d'affichage de l'erreur
    private bool _estActif = false; //bool avec false, représente si le bouton a été activé ou non

    // Interaction du bouton avec le projectile. Privé
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("tir") && _estActif == false)
        {
            _estActif = true;
            SoundManager.instance.Jouer(_sonActivation);
            GetComponent<SpriteRenderer>().color=Color.yellow;
            _enigmeScript.AjouterChiffre(_nombre);
        }
    }

    // Donne un lapse de temps d'affichage d'erreur à l'utilisateur. Public
    public void MontrerEchec()
    {
        _modifCouleur = StartCoroutine(ModifCouleur());
        Invoke("Reinitialiser", _tempsAffichage);
    }

    // coroutine de la modification de couleur en alternance des boutons (blanc à rouge). Privé
    private IEnumerator ModifCouleur()
    {
        while(true)
        {
            if(GetComponent<SpriteRenderer>().color==Color.yellow || GetComponent<SpriteRenderer>().color==Color.white)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if(GetComponent<SpriteRenderer>().color==Color.red)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    // Réinitialisation des boutons à leur état normal. Privé
    private void Reinitialiser()
    {
        _estActif = false;
        StopCoroutine(_modifCouleur);
        _enigmeScript.AfficherCode();
        GetComponent<SpriteRenderer>().color=Color.white;
    }
}
