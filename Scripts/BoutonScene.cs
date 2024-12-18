/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Ce script sert de moyen pour les boutons de communiquer avec le gamemanager pour changer la scène en cours. J'ai tenté de glisser le scipt du gamemanager à chaque bouton, mais leur auto-destruction empêchait la viabilité de cette méthode.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonScene : MonoBehaviour
{
    // fonction qui communique au gamemanager de changer la scène en cours. public
    public void AllerScene(string nomScene)
    {
        GameManager.instance.Aller(nomScene);
    }
}
