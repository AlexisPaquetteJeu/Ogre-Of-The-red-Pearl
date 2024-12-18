/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Script universel entre les scènes. Contient un singleton et le code pour le changement de scène.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //IMPORTANT pour le changement de scène

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioClip _son; //clip de son de l'activation de bouton dans le UI.
    private static GameManager _instance; //variable GameManager dans ce script

    public static GameManager instance //variable GameManager dans les autres script
    {
        get { return _instance; }
    }

    //Fonction similaire à Start. Contient le code d'un singleton
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Fonction de base de Unity. Observe si certaines touches on été utilisées. Privé
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) Aller("Jeu");
        if(Input.GetKeyDown("0")) Aller("Intro");
    }

    //Contient le code pour le chargement d'une nouvelle scène. Public
    public void Aller(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
        SoundManager.instance.Jouer(_son);
    }

}