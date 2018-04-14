using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        //only trigger if the object is tagged as player
        if (other.tag == "Enemy")
        {
            //GameObject theEnemy = GameObject.Find("darkEnemy"); //find's the enemy game object
            //darkEnemy enemyScript = theEnemy.GetComponent<darkEnemy>(); //
            //enemyScript.lightPresent = true; //activates the bool for saying the enemy is near light
            //Debug.Log("Enemy is near light");

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //only trigger if the object is tagged as player
        if (other.tag == "Enemy")
        {
            //GameObject theEnemy = GameObject.Find("darkEnemy"); //find's the enemy game object
            //darkEnemy enemyScript = theEnemy.GetComponent<darkEnemy>(); //
            //enemyScript.lightPresent = false; //activates the bool for saying the enemy is near light
            //Debug.Log("Enemy is away from light");

        }
    }
}
