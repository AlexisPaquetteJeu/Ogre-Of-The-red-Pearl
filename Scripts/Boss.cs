/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Ce script contient le code pour les déplacements du boss, son interaction avec les projectiles et sa réaction lorsque celui-ci perd tout ses points de vie. 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private AudioClip[] _sonOuch; //Tableau des clips de son d'une perte de vie
    [SerializeField] private Sprite[] _spritesBoss; //Tableau des sprites du boss
    [SerializeField] private Vector3[] _pos; //Tableau des position à donner pour le déplacement du boss
    [SerializeField] private Projectile _projectileScript; //Script du projectile pour communiquer avec celui-ci
    private const int _NB_TOTAL_VIES = 45; //Le nombre total de vie (Ne devrait pas changer)
    private int _nbVies; //variable int du nombre de vies restantes
    private Coroutine _modifTranparence; //Coroutine qui modifie l'apparence du boss lors de sa défaite
    private float _vitesseTransparence = 0.1f; //float de la vitesse du changement de transparence
    private float _stepTrans; //float de la _vitesseTransparence avec un deltatime
    private float _transFinale = 1f; //float qui sera employée dans le changement du <Color> du boss
    private float _vitesse = 5.5f; //float de la vitesse du boss
    private float _viePourcentage; //float de la vie en pourcentage du boss
    private int _choixPos; //int de la position à poursuivre pour le boss
    private int _proportionSon = 4; //int qui gère la proportion de l'utilisation de sons ouch
    private bool _peutBouger = true;
    //bool avec false, représente si le boss est autorisé à bouger ou non.

    void Start() //Fonction classique d'Unity. Associe les éléments de base.
    {
        _projectileScript.GetComponent<Projectile>().SetBoss(this);
        _nbVies = _NB_TOTAL_VIES;
    }

    void Update() //Fonction classique d'Unity. Gère les déplacements
    {
        if(_peutBouger)
        {
            // Si la position du boss équivaut à la position qu'il faut poursuivre...
            if(transform.position == _pos[_choixPos])
            {
                _choixPos = Random.Range(0, _pos.Length);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, _pos[_choixPos], _vitesse * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) //Gère l'interaction des projectiles avec le boss. privé
    {
        if (other.CompareTag("tir"))
        {
            _nbVies --;
            ChangerSprite();
            int nombre = Random.Range(0,_sonOuch.Length * _proportionSon);
            if(nombre < 3)
            {
                SoundManager.instance.Jouer(_sonOuch[nombre]);
            }
        }
    }

    // S'occupe de changer le sprite du boss lorsque sa vie diminue à certains points. privé
    private void ChangerSprite()
    {
        _viePourcentage = (_nbVies * 100) / _NB_TOTAL_VIES; //Calcul de fraction croisée pour obtenir le pourcentage de vie restante
        if(_viePourcentage > 80f && _viePourcentage != 100f) GetComponent<SpriteRenderer>().sprite = _spritesBoss[1];
        if(_viePourcentage > 60f && _viePourcentage < 80f) GetComponent<SpriteRenderer>().sprite = _spritesBoss[2];
        if(_viePourcentage > 40f && _viePourcentage < 60f) 
        {
            GetComponent<SpriteRenderer>().sprite = _spritesBoss[3];
            _vitesse = 8f;
        }
        if(_viePourcentage > 20f && _viePourcentage < 40f) GetComponent<SpriteRenderer>().sprite = _spritesBoss[4];
        if(_viePourcentage <= 0f) FinirPartie();
    }

    // Relatif à la conclusion d'une partie lorsque les points de vie du boss atteignent 0. privé
    private void FinirPartie()
    {
        _peutBouger = false;
        SoundManager.instance.Jouer(_sonOuch[2]);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = _spritesBoss[5];
        _modifTranparence = StartCoroutine(ModifTranparence());
        Invoke("AllerEcranReussite", 3f);
    }

    // coroutine qui modifie à vitesse constante la transparence du boss jusqu'à l'invisible
    private IEnumerator ModifTranparence()
    {
        _stepTrans = _vitesseTransparence * Time.deltaTime;
        while(GetComponent<SpriteRenderer>().color.a != 0f)
        {
            _transFinale -= _stepTrans;

            GetComponent<SpriteRenderer>().color = new Color (1,1,1,_transFinale);
            
            yield return null;
        }
    }

    // Aller à la scène de réussite
    private void AllerEcranReussite()
    {
        GameManager.instance.Aller("Fin");
    }
}
