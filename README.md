The Ogre of The Red Pearl
================================

This project was conceived in my third game design course of my multimedia Technique. It was my first experience as a solo developper working with Unity, giving me plenty of freedom to experiment with C# (before AI was a thing!)

As a lone miner in a dangerous lava cave, you must collect the precious red pearl kept by the monstrous ogre. With the help of your magnetic gems, you can pass over threatening obstacles to acheive your goal.
The lone miner will also have to complete a combination puzzle to get access to the lair of the ogre.

- Use ASWD to move,
- Use the right click of the mouse to throw a magnetic stone
- Use the left click of the mouse to throw a simple stone (useful to activate buttons)

# Snippets
## 1- Smooth camera following the character

Looking back on this project, this functionnality could have been completed by the use of a 2D cinemachine. Yet, I now understand the logic behind this technology to make a more polished experience.

I found that the Lerp function has a lot of uses and the variable _vitesse makes it so that the speed of the camera can be more easily maniplated.
```C#
    void Update() // Unity's standard function. Handles camera movement
    {
        _startPos = transform.position;
        _endPos = _cible.position;
        _endPos.z = transform.position.z; // Ensure the camera's Z position remains unchanged
        transform.position = Vector3.Lerp(_startPos, _endPos, _vitesse * Time.deltaTime);
    }
```

## 2- Making a code for the puzzle room

This game mechanic made me discover the practicality and importance of lists in C#. With lists, I was able to create new sequences of numbers to show the player. 

If the the player gets the puzzle wrong, the code simply gets reinitialised with this function.

```C#
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
```

## 3- Brining the character to life

In my game design course, understanding how to move a character was a good foundation, but I was glad to hear about the Normalize function. This way the character moves at the same speed even in diagonal directions.

```C#
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
```

Have fun playing!
