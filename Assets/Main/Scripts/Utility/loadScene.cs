﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {

    public Object loadedScene;
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //load the end game scene
    void OnTriggerEnter2D(Collider2D other)
    {
        //only load the scene if the object is tagged as player
        if (other.tag == "Player")
        {
           SceneManager.LoadScene(loadedScene.name); //loads assigned scene
        }
    }
}
