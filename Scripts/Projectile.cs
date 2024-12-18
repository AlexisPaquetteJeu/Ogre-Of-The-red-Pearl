/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Contient le code des interactions entre les projectiles et les murs et le boss
    Author: Alexis Paquette
    Date: 2023-12-10
    Description: Contains the code for interactions between projectiles, walls, and the boss
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Boss _boss; //variable du boss
    // variable for the boss
    public float _vitesse = 7f; //float de la vitesse du projectile
    // float for the projectile speed
    private Vector3 _target; //Vector3 de la position de la souris
    // Vector3 for the mouse position

    //Fonction de base d'Unity, assigne la variable _target la position de la caméra
    // Unity's base function, assigns the _target variable to the camera's position
    void Awake()
    {
       _target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //Assigne le script du boss à la variable _boss. Public
    // Assigns the boss script to the _boss variable. Public
    public void SetBoss(Boss refBoss)
    {
        _boss = refBoss;
    }

    // Fonction de base de Unity, s'occupe du déplacement du projectile
    // Unity's base function, handles the projectile movement
    void Update()
    {
        float angle = TrouverAngle(transform.position, _target);
        transform.Translate(Vector3.right * _vitesse * Time.deltaTime, Space.Self);
    }

    // Gère les interactions de collider entre les projectiles et les murs ainsi que le boss. Privé
    // Handles the collider interactions between projectiles, walls, and the boss. Private
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("mur"))
        {
            Destroy(gameObject);
        }
        else if(other.CompareTag("boss"))
        {
            Destroy(gameObject);
        }
    }

    //S'occupe de trouver un angle entre deux positions. Privé
    // Calculates the angle between two positions. Private
    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }
}
