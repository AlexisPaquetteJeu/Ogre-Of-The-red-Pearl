/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Permet d'être la source audio de tous les gameobjects des scènes et de jouer plusieurs sons. Contient aussi un singleton
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _audio; //variable de la source audio
    private static SoundManager _instance; //variable SoundManager dans ce script

    public static SoundManager instance //variable SoundManager dans les autres scripts
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
            _audio = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Joue les clips audio qui lui sont envoyés. Public
    public void Jouer(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }
}
