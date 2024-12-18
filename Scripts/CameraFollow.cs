/*
    Author: Alexis Paquette
    Date: 2023-12-10
    Description: This script contains the code for camera movements during the game scene. 
    The camera follows the player with a slight delay and zooms out when entering the boss area.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _cible; // Transform variable for the target to follow
    [SerializeField] private float _vitesse; // Camera movement speed
    private Vector3 _startPos; // Starting position of the camera
    private Vector3 _endPos; // Target position for the camera
    private float _vitesseZoom = 1.5f; // Zoom speed for the camera
    private Coroutine _augmenterDim; // Coroutine to handle camera zooming

    void Update() // Unity's standard function. Handles camera movement
    {
        _startPos = transform.position;
        _endPos = _cible.position;
        _endPos.z = transform.position.z; // Ensure the camera's Z position remains unchanged
        transform.position = Vector3.Lerp(_startPos, _endPos, _vitesse * Time.deltaTime);
    }

    // Activates the coroutine for changing the camera dimensions. Public
    public void ChangerDimension()
    {
        _augmenterDim = StartCoroutine(AugmenterDim());
    }

    // Gradually increases the camera's size at a constant speed. Private
    private IEnumerator AugmenterDim()
    {
        while (GetComponent<Camera>().orthographicSize < 7.5f)
        {
            float step = _vitesseZoom * Time.deltaTime;

            GetComponent<Camera>().orthographicSize += step;

            yield return null;
        }
    }
}
