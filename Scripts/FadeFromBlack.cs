/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Ce script s'occupe d'un effet de <<fade>> en modifiant rapidement l'opacité d'un carré noir après qu'une scène soit chargée.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFromBlack : MonoBehaviour
{
    private Image _img; //variable du carré noir
    private float _alpha = 1f; //float de l'alpha du carré noir

    [SerializeField]
    [Range(0.1f, 5f)]
    private float _fadeSpeed; //float de la vitesse du fade

    void Start() //Fonction classique d'Unity. Initialise l'image
    {
        _img = GetComponent<Image>();
        ChangeAlpha(_alpha);
    }

    void Update() //Fonction classique d'Unity. Modfie à vitesse constante l'opacité du carré
    {
        _alpha -= Time.deltaTime * _fadeSpeed;

        if (_alpha <= 0f)
        {
            ChangeAlpha(0f);
        }
        else
        {
            ChangeAlpha(_alpha);
        }

    }

    //Code qui modifie directement l'opacité du carré. Privé
    private void ChangeAlpha(float value)
    {
        _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, value);
    }
}
