using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkEnemy : MonoBehaviour {

    public float speed; //how fast the enemy moves
    public float stoppingDistance; //higher the number the further away the enemy will stop from the player 
    public float retreatDistance; //when the enemy should back away from his target
    private Transform target; //what the enemy should target
    public Transform firstpatrolPoint; //the enemy's point to return to after loosing the player

    public bool playerInRange; //toggle if the player is in range or not
    public bool playerWasInRange; //toggle to check if the player was in range before

    public Transform player;
    public Transform lightObject;

    void Start()
    {
        
    }

    void Update() {
        if (playerInRange == true)
        {
            target = player;
            checkLights();
        }

        else if (playerInRange != true && playerWasInRange == true)
        {
            target = firstpatrolPoint;
            checkLights();
        }

        else if (playerInRange != true && playerWasInRange != true)
        {
            patrol();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //only trigger if the object is tagged as player
        if (other.tag == "Player")
        {
            playerInRange = true;
            playerWasInRange = false;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //only trigger if the object is tagged as player
        if (other.tag == "Player")
        {
            playerInRange = false;
            playerWasInRange = true;
        }
    }

    void checkLights()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; //finds the position of the object tagged player
        lightObject = GameObject.FindGameObjectWithTag("Light").transform; //finds the position of the object tagged light

        if(Vector2.Distance(transform.position, lightObject.position) < retreatDistance) //if a light is too close
        {

        }

        //checks how far the target is from the enemy
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance) //if the enemy is too far away
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime); //the enemy moves closer to his target

        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) //if he is near enough to his target but not too near
        {
            transform.position = this.transform.position; //stops the enemy moving

        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime); //the enemy moves away from his target
        }
    }

    void patrol()
    {

    }

    void idle()
    {

    }


}
