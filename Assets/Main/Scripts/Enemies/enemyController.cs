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

    //for waypoint walking
    public List<GameObject> WaypointPositions;
    public float speed = 1;

    private int currentWaypoint = 0;

    private Vector3 targetPositionDelta;
    private Vector3 moveDirection = Vector3.zero;

    private float horizontalPositionOld;
    private bool facingRight = true;

    [Tooltip("Default=0 - Set a value in seconds, how long the enemy should wait to move")]
    public float StartWalkingAfterSeconds = 0;

    private float startWalkingTime;
    private bool doStartWalking = false;
    private float gametime;

    //start walking on awake
    void Awake()
    {

        // Get the current time
        startWalkingTime = Time.deltaTime + StartWalkingAfterSeconds;

        // If default = 0 do walking
        if (StartWalkingAfterSeconds == 0)
        {
            doStartWalking = true;
        }
    }

    // Update is called once per frame
    void Update () {

        //detect when light is in range
        lightInRange = Physics2D.OverlapCircle(transform.position, lightCheckRadius, lightLayer);

        //detec when the player is in range
        playerInRange = Physics2D.OverlapCircle(transform.position, playerCheckRadius, playerLayer);

        //find the player object
        player = GameObject.FindGameObjectWithTag("Player").transform; //finds the position of the object tagged player

        //for walking
        gametime += Time.deltaTime;

        if (startWalkingTime <= gametime)
        {
            doStartWalking = true;
        }

    }

    void FixedUpdate()
    {

        if (doStartWalking)
        {

            // Save actual position for flip-check
            horizontalPositionOld = transform.position.x;

            //dark enemy behavior
            if (afraidofLight)
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

                else if (!playerInRange)
                {
                    WaypointWalk();
                    Move();
                }
            }

            // Flip-Check
            if (transform.position.x < horizontalPositionOld)
            {

                if (facingRight)
                {
                    Flip();
                    facingRight = false;
                }
            }
            else
            {
                if (!facingRight)
                {
                    Flip();
                    facingRight = true;
                }
            }
        }
    }

    void WaypointWalk()
    {

        if (WaypointPositions.Count > 0)
        {

            GameObject wp = (GameObject)WaypointPositions[currentWaypoint];
            Vector3 targetPosition = wp.transform.position;

            targetPositionDelta = targetPosition - transform.position;

            // if i´m near the next waypoint count one high
            if (targetPositionDelta.sqrMagnitude <= 0.01f)
            {

                currentWaypoint++;

                // If the last waypoint reached, start again
                if (currentWaypoint >= WaypointPositions.Count)
                {
                    currentWaypoint = 0;
                }
            }
        }
    }

    protected virtual void Move()
    {
        moveDirection = targetPositionDelta.normalized * speed;
        transform.Translate(moveDirection * Time.deltaTime, Space.World);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    //make the light and player detection radius' visible in the editor
    void OnDrawGizmosSelected()
    {

        //draws sphere around enemy showing his range to find the player
        Gizmos.DrawSphere(transform.position, playerCheckRadius);

        //draws sphere around enemy showing his range to find light
        Gizmos.DrawSphere(transform.position, lightCheckRadius);
    }
}
