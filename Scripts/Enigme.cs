/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Ce script sert de <<GameManager>> pour gérer les réactions de la zone d'énigme. Ce code sert principalement pour la création d'un mot de passe de 4 chiffres à reproduire, sa re-création lors d'une erreur et le mouvement de la porte.
*/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enigme : MonoBehaviour
{
    [SerializeField] private AudioClip _sonEchec; //clip de son d'un échec à reproduire le mot de passe
    [SerializeField] private Vector3 _pos; //Vector3 de la position finale de la porte lors d'une réussite
    [SerializeField] private GameObject _porte; //Gameobject de la porte
    [SerializeField] private Bouton[] _btn; //Tableau des quatres boutons
    [SerializeField] private TextMeshPro _txtCode; //champ de texte de l'affichage du code
    private List<int> _code = new List<int>(){1}; //liste du mot de passe à afficher
    private List<int> _codeCompose; //liste du mot de passe composé par le joueur
    private bool _aReussit = false; //bool avec false, représente si le joueur a réussit ou non
    
    void Start() //Fonction classique d'Unity. Permet de réinitialiser le mot de passe à compléter
    {
        _code = ReinitialiserCode();
        _codeCompose = new List<int>();
    }

    void Update() //Fonction classique d'Unity. Gère les déplacements de la porte
    {
        if(_aReussit)
        {
            _porte.transform.position = Vector2.MoveTowards(_porte.transform.position, _pos, 3f * Time.deltaTime);
        }

        if(Vector2.Distance(_porte.transform.position, _pos) < 0.1f)
        {
            _aReussit = false;
        }
    }

    // Retourne une liste réinitialisée du mot de passe à compléter. Privé
    private List<int> ReinitialiserCode()
    {
        _code.Clear();
        List<int> nouveauCode = new List<int>();
        List<int> listeTemp = new List<int>(){1,2,3,4};
        int nbItems = listeTemp.Count;
        for (int i = 0; i < nbItems; i++)
        {
            int n = Random.Range(0,listeTemp.Count);
            nouveauCode.Add(listeTemp[n]);
            listeTemp.RemoveAt(n);
        }
        _txtCode.text = string.Join("-", nouveauCode); //Affichage du mot de passe
        Debug.Log(string.Join("-", nouveauCode) + "");
        return nouveauCode;
    }

    //S'occupe d'ajouter le chiffre du dernier bouton activé au code composé. Public
    public void AjouterChiffre(int chiffre)
    {
        _codeCompose.Add(chiffre);
        Debug.Log(string.Join("-", _codeCompose) + "");
        if(_codeCompose.Count == 4)
        {
            if(ComparerCode(_code, _codeCompose) == false)
            {
                MontrerErreur();
            }
            else
            {
                _aReussit = true;
            }
        }
    }

    //S'occupe de comparer le mdp(mot de passe) à reproduire et le mdp composé. Private
    private bool ComparerCode(List<int> a, List<int> b)
    {
        if(a.Count != b.Count) return false;

        for (int i = 0; i < a.Count; i++)
        {
            if(a[i]!=b[i]) return false;
        }
        return true;
    }

    // Communique au joueur que le mdp n'a pas été correctement reproduit. Private
    private void MontrerErreur()
    {
        _txtCode.text = "X-X-X-X";
        SoundManager.instance.Jouer(_sonEchec);
        for (int i = 0; i < _btn.Length; i++)
        {
            _btn[i].MontrerEchec();
        }
    }

    //Affichage du mdp à reproduire réinitialisé et efface le mdp composé. Public
    public void AfficherCode()
    {
        _code = ReinitialiserCode();
        _codeCompose.Clear();
    }
}
