using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadDemoEnd : MonoBehaviour {

    //variable for checking if the trigger is entered
    bool TriggerEntered;

    // Use this for initialization
    void Start()
    {

    }

    //checks if the player enters the trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        //only sets the trigger variable to true if it's the player
        if (other.tag == "Player")
        {
            TriggerEntered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //only works if the variable is set to true
        if (TriggerEntered)
        {
            //loads the scene
            SceneManager.LoadScene("DemoEnd"); //loads assigned scene
        }
    }
}
