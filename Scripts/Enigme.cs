/*
    Author: Alexis Paquette
    Date: 2023-12-10
    Description: This script serves as a "GameManager" to handle interactions in the puzzle area. 
    It primarily manages the creation of a 4-digit password to reproduce, resets it upon an error, and controls the door's movement.
*/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enigme : MonoBehaviour
{
    [SerializeField] private AudioClip _sonEchec; // Audio clip for the sound played on a failed password attempt
    [SerializeField] private Vector3 _pos; // Final position of the door upon success
    [SerializeField] private GameObject _porte; // GameObject representing the door
    [SerializeField] private Bouton[] _btn; // Array of the four buttons
    [SerializeField] private TextMeshPro _txtCode; // Text field displaying the password
    private List<int> _code = new List<int>(){1}; // List of the password to display
    private List<int> _codeCompose; // List of the password composed by the player
    private bool _aReussit = false; // Boolean indicating whether the player succeeded or not
    
    void Start() // Unity's standard function. Resets the password to complete
    {
        _code = ReinitialiserCode();
        _codeCompose = new List<int>();
    }

    void Update() // Unity's standard function. Handles the door's movement
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

    // Returns a reset version of the password to complete. Private
    private List<int> ReinitialiserCode()
    {
        _code.Clear();
        List<int> nouveauCode = new List<int>();
        List<int> listeTemp = new List<int>(){1,2,3,4};
        int nbItems = listeTemp.Count;
        for (int i = 0; i < nbItems; i++)
        {
            int n = Random.Range(0, listeTemp.Count);
            nouveauCode.Add(listeTemp[n]);
            listeTemp.RemoveAt(n);
        }
        _txtCode.text = string.Join("-", nouveauCode); // Displays the password
        Debug.Log(string.Join("-", nouveauCode) + "");
        return nouveauCode;
    }

    // Adds the number from the last activated button to the composed password. Public
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

    // Compares the password to reproduce with the composed password. Private
    private bool ComparerCode(List<int> a, List<int> b)
    {
        if(a.Count != b.Count) return false;

        for (int i = 0; i < a.Count; i++)
        {
            if(a[i] != b[i]) return false;
        }
        return true;
    }

    // Notifies the player that the password was not reproduced correctly. Private
    private void MontrerErreur()
    {
        _txtCode.text = "X-X-X-X";
        SoundManager.instance.Jouer(_sonEchec);
        for (int i = 0; i < _btn.Length; i++)
        {
            _btn[i].MontrerEchec();
        }
    }

    // Displays the reset password and clears the composed password. Public
    public void AfficherCode()
    {
        _code = ReinitialiserCode();
        _codeCompose.Clear();
    }
}
