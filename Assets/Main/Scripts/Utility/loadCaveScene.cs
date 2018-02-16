using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadCaveScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //load the cave scene
    void OnTriggerEnter2D(Collider2D other)
    {
        //only load the scene if the object is tagged as player
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("CaveTilemap"); //loads assigned scene
        }
    }
}
