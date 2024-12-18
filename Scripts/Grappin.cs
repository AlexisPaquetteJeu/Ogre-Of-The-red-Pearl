/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Contient le code pour les réactions du grappin avec l'environnment.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappin : MonoBehaviour
{
    [SerializeField] private AudioClip _sonGrappin; //Clip de son du contact du grappin avec les murs
    private Perso _personnage; //Script du personnage
    private float _vitesse = 20f; //float de la vitesse du grappin
    private Vector3 _target; //Vector3 de la position du curseur
    private Vector3 _posFinale; //Vector3 de la position finale du grappin
    private bool estColle = false; //bool avec false, représente si le grappin est collé à un mur ou non

    //Fonction similaire à Start, assigne la variable _target à la position du curseur
    void Awake()
    {
       _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update() //Fonction de base d'Unity, gère le déplacement constant du grappin
    {
        if(estColle == false)
        {
            transform.Translate(Vector3.right * _vitesse * Time.deltaTime, Space.Self);
        }
    }

    //S'occupe de détruire le grappin. Public
    public void DetruireGrappin() 
    {
        Destroy(gameObject);
    }

    //Assigne le script du perso à la variable _personnage. Public
    public void SetPerso(Perso refPerso)
    {
        _personnage = refPerso;
    }

    //Gère l'interaction du grappin avec les murs. Privé
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("mur"))
        {
            SoundManager.instance.Jouer(_sonGrappin);
            estColle = true;
            _posFinale = transform.position;
            _personnage.AttirerPerso(_posFinale);
        }
    }
}
