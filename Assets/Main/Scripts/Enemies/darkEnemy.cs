using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkEnemy : MonoBehaviour {

    public Transform target;
    public float speed;
    private bool rangeEntered = false;
    public bool lightPresent = false;
    public GameObject darkEnemyObject;

    void Update()
    {
        if (rangeEntered == true && lightPresent == false)
        {
            float step = speed * Time.deltaTime;
            darkEnemyObject.transform.position = Vector3.MoveTowards(darkEnemyObject.transform.position, target.position, step);
        }

        else if (lightPresent == true)
        {
            float step = speed * 0;
            darkEnemyObject.transform.position = Vector3.MoveTowards(darkEnemyObject.transform.position, target.position, step);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        //only trigger if the object is tagged as player
        if (other.tag == "Player")
        {

            rangeEntered = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //only trigger if the object is tagged as player
        if (other.tag == "Player")
        {
            rangeEntered = false;

        }
    }
}
