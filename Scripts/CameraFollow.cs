/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Ce script contient le code pour les mouvements de la caméra pendant la scène de jeu. Avec ce code, la caméra suit le joueur avec un léger ralentissement et s'agrandie lors de l'entrée dans la zone du boss.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _cible; //variable Transform de la cible à atteindre
    [SerializeField] private float _vitesse; //float de la vitesse de la caméra
    private Vector3 _startPos; //Vector3 de la position de départ
    private Vector3 _endPos; //Vector3 de la position finale
    private float _vitesseZoom = 1.5f; //float de la vitesse s'agrandissement de la caméra
    private Coroutine _augmenterDim; //coroutine de l'augmentation des dimensions de la caméra

    void Update() //Fonction classique d'Unity. Gère les déplacements
    {
        _startPos = transform.position;
        _endPos = _cible.position;
        _endPos.z = transform.position.z; 
         transform.position = Vector3.Lerp(_startPos, _endPos, _vitesse * Time.deltaTime);  
    }

    // S'occupe d'activer la coroutine du changement des dimensions de la caméra. public
    public void ChangerDimension()
    {
        _augmenterDim = StartCoroutine( AugmenterDim() );
    }

    // S'occupe d'agrandir à vitesse constante les dimensions de la caméra. privé
    private IEnumerator AugmenterDim()
    {
        while(GetComponent<Camera>().orthographicSize < 7.5f)
        {
            float step = _vitesseZoom * Time.deltaTime;

            GetComponent<Camera>().orthographicSize += step;

            yield return null;
        }
    }
}