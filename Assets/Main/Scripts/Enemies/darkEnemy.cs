using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class darkEnemy : MonoBehaviour {

    public float speed; //how fast the enemy moves
    public float stoppingDistance; //higher the number the further away the enemy will stop from the player 
    public float retreatDistance; //when the enemy should back away from his target

    public Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; //finds the position of the object tagged player
    }

    void Update() {
        //checks how far the player is from the enemy
        if(Vector2.Distance(transform.position, player.position) > stoppingDistance) //if the enemy is too far away
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime); //the enemy moves closer to his target

        } else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > retreatDistance) //if he is near enough to his target but not too near
        {
            transform.position = this.transform.position; //stops the enemy moving

        } else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime); //the enemy moves closer to his target
        }
    }

 
}
