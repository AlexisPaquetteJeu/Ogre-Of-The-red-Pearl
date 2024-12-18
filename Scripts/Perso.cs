/*
    Auteur : Alexis Paquette
    Date : 2023-12-10
    Description : Code du personnage contrôlé par le joueur qui contient ses interactions avec les dangers, ses animations, ses contrôles de mouvement, l'utilisation de sons et l'utilisation des prefabs de projectiles et le grappin.
    Author: Alexis Paquette
    Date: 2023-12-10
    Description: Code for the player-controlled character, including interactions with dangers, animations, movement controls, sound usage, and prefabs for projectiles and the grappling hook.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Perso : MonoBehaviour
{
    [SerializeField] private GameObject _projectile; //Gameobject du projectile
    // GameObject for the projectile
    [SerializeField] private GameObject _grappinObj; //Gameobject du grappin
    // GameObject for the grappling hook
    [SerializeField] private GameObject[] _partiesCoeur; //Tableau de gameobject des parties de coeur
    // Array of GameObjects for heart parts
    [SerializeField] private AudioClip _sonOuch; //Clip de son de dégât
    // Audio clip for damage sound
    [SerializeField] private AudioClip _sonMort; //Clip de son de la mort 
    // Audio clip for death sound
    [SerializeField] private AudioClip[] _sonLancer; //Tableau de clips de son du lancer de projectiles
    // Array of audio clips for projectile throwing
    [SerializeField] private Collider2D _eau; //Collider des cases d'eau et de trous
    // Collider for water and hole tiles
    private GameObject _grappinScript; //Script du grappin
    // Script for the grappling hook
    private const int _NB_TOTAL_VIES = 3; //La nombre total de vie (Ne devrait pas changer)
    // Total number of lives (should not change)
    private int _nbVies; //variable int du nombre de vies restantes
    // Integer variable for remaining lives
    private Animator _anim; //variable de type Animator. Gère les animations
    // Animator variable. Manages animations
    private Rigidbody2D _rb; //Rigidbody du personnage
    // Rigidbody for the character
    private Vector3 _axis; //Vector3 de la direction du personnage lors de son mouvement
    // Vector3 for the character's direction during movement
    private Vector3 _targetTir; //Vector3 de la position du curseur
    // Vector3 for the cursor position
    private Vector3 _targetGrappin; //Vector3 de la position finale du grappin
    // Vector3 for the final position of the grappling hook
    private Coroutine _modifTranparence; //Coroutine de la modification de l'alpha lors d'un dégât
    // Coroutine for alpha transparency modification during damage
    private float _vitesse = 5.5f; //float de la vitesse du personnage
    // Float for the character's speed
    private float _vitesseGrappin = 20f; //float de la vitesse pendant que le personnage est attiré par le grappin
    // Float for speed while the character is pulled by the grappling hook
    private float _vitesseTransparence = 0.5f; //float de la vitesse du changement de l'alpha
    // Float for the alpha transparency change speed
    private float _stepTrans; //float de la _vitesseTransparence avec un deltatime
    // Float for _vitesseTransparence with deltaTime
    private float _transFinale = 1f; //float qui sera employée dans le changement du <Color> du boss
    // Float used for <Color> changes
    private float _tempsInvincible = 3f; //float du temps d'invicibilité
    // Float for invincibility time
    private bool _estAttire = false; //bool avec false, représente si le personnage est attiré par le grappin ou non
    // Boolean, false represents whether the character is pulled by the grappling hook or not
    private bool _peutBouger = true; //bool avec true, représente si le personnage est autorisé à bouger ou non
    // Boolean, true represents whether the character is allowed to move
    private bool _estVulnerable = true; //bool avec true, représente si le personnage peut recevoir des dégâts ou non
    // Boolean, true represents whether the character can take damage

    void Start() //Fonction de base d'Unity, s'occupe de l'assignation de certaines variables
    // Unity's base function, handles the assignment of certain variables
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _nbVies = _NB_TOTAL_VIES;
    }

    // Fonction de base d'Unity, gère principalement les touches utilisées et les fonctions à appeler en conséquence
    // Unity's base function, primarily handles key inputs and calls the appropriate functions accordingly
    void Update()
    {
        _axis.x = Input.GetAxisRaw("Horizontal");
        _axis.y = Input.GetAxisRaw("Vertical");
        _axis.Normalize();

        if(_axis.x < 0 && _peutBouger)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else if(_axis.x > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }

        _anim.SetFloat("magnitude", _axis.magnitude);
        _anim.SetFloat("Horizontal", _axis.x);
        _anim.SetFloat("Vertical", _axis.y);

        if (Input.GetMouseButtonDown(0)) Tirer("projectile");
        if (Input.GetMouseButtonDown(1)) Tirer("grappin");

        if(Input.GetKeyDown("1")) Teleporter(0, 0);
        if(Input.GetKeyDown("2")) Teleporter(33f, 10f);
        if(Input.GetKeyDown("3")) Teleporter(33f, 90.5f);
        if(Input.GetKeyDown("4")) Teleporter(9f, 101f);
    }

    //S'occupe d'instancier le prefab assigné par le code du Update(). Privé
    // Handles instantiating the prefab assigned in the Update() code. Private
    private void Tirer(string objet)
    {
        _targetTir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = TrouverAngle(transform.position, _targetTir);
        if(objet == "projectile" && _nbVies > 0) //Projectile
        {
            int nombre = Random.Range(0,_sonLancer.Length);
            SoundManager.instance.Jouer(_sonLancer[nombre]);
            Instantiate(_projectile, transform.position, Quaternion.Euler(0,0,angle * Mathf.Rad2Deg));  
        }
        else if(_peutBouger && _nbVies > 0) //Grappin
        {
            _grappinScript = Instantiate(_grappinObj, transform.position, Quaternion.Euler(0,0,angle * Mathf.Rad2Deg));
            _grappinScript.GetComponent<Grappin>().SetPerso(this);
            _peutBouger = false;
        }
    }

        // Active l'état d'attirance du personnage. Public
    // Activates the character's pull state. Public
    public void AttirerPerso(Vector3 position)
    {
        _targetGrappin = position;
        _estAttire = true;
    }

    // Fonction de base d'Unity, gère les 3 états du personnage mouvement, immobile et attiré
    // Unity's base function, manages the character's three states: moving, stationary, and being pulled
    void FixedUpdate()
    {
        if(_estAttire == false && _peutBouger == true) //État de mouvement
        // Movement state
        {
            _rb.MovePosition(transform.position + _vitesse * Time.fixedDeltaTime * _axis);
        }
        else if(_estAttire == true) //État attiré
        // Pulled state
        {
            _estVulnerable = false;
            _eau.enabled = false;
            float step = _vitesseGrappin * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.magenta;
            transform.position = Vector2.MoveTowards(transform.position, _targetGrappin, step);
            _anim.SetBool("attire", true);

            if(Vector2.Distance(transform.position, _targetGrappin) < 0.8f)
            {
                _estAttire = false;
                _peutBouger = true;
                _estVulnerable = true;
                _anim.SetBool("attire", false);
                GetComponent<SpriteRenderer>().color = Color.white;
                _grappinScript.GetComponent<Grappin>().DetruireGrappin();
                _eau.enabled = true;
            }
        }
    }

    // Téléporte le personnage à l'endroit assigné. Privé
    // Teleports the character to the assigned location. Private
    private void Teleporter(float x, float y)
    {
        transform.position = new Vector3(x, y, 0);
    }

    // S'occupe de trouver un angle entre deux positions. Privé
    // Finds the angle between two positions. Private
    private float TrouverAngle(Vector2 v1, Vector2 v2)
    {
        Vector2 v = v2 - v1;
        return Mathf.Atan2(v.y, v.x);
    }

    // S'occupe des interactions de colliders entre le personnage et les dangers. Gère aussi la diminution des points de vie. Privé
    // Handles collider interactions between the character and dangers. Also manages health reduction. Private
    private void OnTriggerStay2D(Collider2D other)
    {
        if ((other.CompareTag("danger") && _estVulnerable) || (other.CompareTag("boss") && _estVulnerable))
        {
            _estVulnerable = false;
            _nbVies--;
            _partiesCoeur[_nbVies].SetActive(false);
            Invoke("RemettreVulnerable", _tempsInvincible);
            _anim.SetBool("degat", true);

            if(_nbVies <= 0)
            {
                _peutBouger = false;
                SoundManager.instance.Jouer(_sonMort);
                _anim.SetBool("mort", true);
                Invoke("AllerEcranDefaite", _tempsInvincible);
            }
            else
            {
                _modifTranparence = StartCoroutine(ModifTranparence());
                SoundManager.instance.Jouer(_sonOuch);
            }
        }
    }

    // Coroutine du changement à intervalles de la transparence du personnage. Privé
    // Coroutine to periodically change the character's transparency. Private
    private IEnumerator ModifTranparence()
    {
        _stepTrans = _vitesseTransparence * Time.deltaTime;

        while(true)
        {
            for (float i = 1; i > 0.2f; i -= _stepTrans)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
                yield return null;
            }

            for (float i = 0.2f; i < 1; i += _stepTrans)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, i);
                yield return null;
            }

            yield return null;
        }
    }

    // S'occupe de remettre le personnage dans un état de vulnérabilité et arrête la coroutine. Privé
    // Restores the character's vulnerability state and stops the coroutine. Private
    private void RemettreVulnerable()
    {
        _estVulnerable = true;
        StopCoroutine(_modifTranparence);
        _transFinale = 1f;
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, _transFinale);
    }

    // Arrête l'animation de dégât du personnage. Public
    // Stops the character's damage animation. Public
    public void FinirAnimDegat()
    {
        _anim.SetBool("degat", false);
    }

    // Charger la scène de défaite. Privé
    // Loads the defeat scene. Private
    private void AllerEcranDefaite()
    {
        GameManager.instance.Aller("Defaite");
    }

    // Remplir la gauge de vie. Public
    // Refills the health gauge. Public
    public void RemplirVie()
    {
        _nbVies = _NB_TOTAL_VIES;
        for (int i = 0; i < _partiesCoeur.Length; i++)
        {
            _partiesCoeur[i].SetActive(true);
        }
    }
}

