/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Script qui sert de petit <<GameManager>> pour l'entrée dans la salle du Boss. Ce code permet d'activer le redimensionnement de la caméra et de déplacer la porte de l'énigme à l'entrée de la salle du boss.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCombat : MonoBehaviour
{
    [SerializeField] private Perso _personnage; //Script du personnage
    [SerializeField] private CameraFollow _camera; //Scipt de la caméra
    [SerializeField] private Vector3[] _pos; //Tableau de Vector3 des positions de la porte
    [SerializeField] private GameObject _porte; // Gameobject de la porte
    private float _vitesse = 7f; //float de la vitesse de la porte
    private int _etape = -1; //int de l'étape de déplacement de la porte en cours
    private bool _estActif = false; //bool avec false, représente si le code a été activé ou non. Permet d'éviter une ré-utilisation du code

    // Fonction classique d'Unity, s'occupe des déplacements de la porte selon l'étape en cours
    void Update()
    {
        if(_etape == 0)
        {
            _porte.transform.position = Vector2.MoveTowards(_porte.transform.position, _pos[_etape], _vitesse * Time.deltaTime);
        }
        else if(_etape == 1)
        {
            _porte.transform.position = Vector2.MoveTowards(_porte.transform.position, _pos[_etape], _vitesse * Time.deltaTime);
        }

        if(Vector2.Distance(_porte.transform.position, _pos[0]) < 0.1f && _etape == 0)
        {
            _etape++; //Étape 0 à 1
        }
    }

    // Gère les interactions de collider entre la zone et le personnage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("perso") && _estActif == false)
        {
            _estActif = true;
            _camera.ChangerDimension();
            _personnage.RemplirVie();
            _etape++; //Étape -1 à 0
        }
    }
}
