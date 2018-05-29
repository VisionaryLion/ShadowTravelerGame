using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour {

    //determine type of enemy
    public bool afraidofDark; //makes it a light enemy
    public bool afraidofLight; //makes it a dark enemy

    //light checking
    public float lightCheckRadius; //size of the light check circle
    public LayerMask lightLayer; //layer the enemy looks for light
    public bool lightInRange; //if the enemy is touching light or not

    //finding the player
    public float playerCheckRadius; //size of the player check circle
    public LayerMask playerLayer; //layer the enemy looks for the player
    public bool playerInRange = false; //if the player is in range or not

    //for enemy follow
    private Transform player;
    public float followSpeed;

    //for enemy stop and retreat from lighting
    public float stoppingDistance; //higher the number the further away the enemy will stop from the light
    public float retreatDistance; //when the enemy should back away from the light


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //detect when light is in range
        lightInRange = Physics2D.OverlapCircle(transform.position, lightCheckRadius, lightLayer);

        //detec when the player is in range
        playerInRange = Physics2D.OverlapCircle(transform.position, playerCheckRadius, playerLayer);

        //find the player object
        player = GameObject.FindGameObjectWithTag("Player").transform; //finds the position of the object tagged player

        //dark enemy behavior
        if (afraidofDark)
        {
            if (playerInRange && !lightInRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime); //the enemy moves closer to his target           
            }

            else if (playerInRange && lightInRange && Vector2.Distance(transform.position, player.position) > retreatDistance)
            {
                transform.position = this.transform.position; //stops the enemy moving
            }

            else if (playerInRange && lightInRange && Vector2.Distance(transform.position, player.position) < retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, -followSpeed * Time.deltaTime); //the enemy moves away from his target
            }
        }

        //light enemy behavior
        if (afraidofLight)
        {
            if (playerInRange && lightInRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime); //the enemy moves closer to his target           
            }

            else if (playerInRange && !lightInRange && Vector2.Distance(transform.position, player.position) > retreatDistance)
            {
                transform.position = this.transform.position; //stops the enemy moving
            }

            else if (playerInRange && !lightInRange && Vector2.Distance(transform.position, player.position) < retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, -followSpeed * Time.deltaTime); //the enemy moves away from his target
            }
        }

    }

    void OnDrawGizmosSelected()
    {

        //draws sphere around enemy showing his range to find the player
        Gizmos.DrawSphere(transform.position, playerCheckRadius);

        //draws sphere around enemy showing his range to find light
        Gizmos.DrawSphere(transform.position, lightCheckRadius);
    }
}
