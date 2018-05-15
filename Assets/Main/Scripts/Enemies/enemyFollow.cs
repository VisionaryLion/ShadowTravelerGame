using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFollow : MonoBehaviour {

    //for enemy follow
    private Transform player;
    public Transform enemy;
    public float followSpeed;
    private bool inRange;

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

        gametime += Time.deltaTime;

        if (startWalkingTime <= gametime)
        {
            doStartWalking = true;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform; //finds the position of the object tagged player

    }

    void FixedUpdate()
    {

        if (doStartWalking)
        {

            // Save actual position for flip-check
            horizontalPositionOld = transform.position.x;

            if (inRange == true)
            {
                enemy.position = Vector2.MoveTowards(enemy.position, player.position, followSpeed * Time.deltaTime); //the enemy moves closer to his target
            }

            else if (inRange == false)
            {
                WaypointWalk();
                Move();
            }

            // Flip-Check
            if (enemy.position.x < horizontalPositionOld)
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

            targetPositionDelta = targetPosition - enemy.transform.position;

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
        enemy.transform.Translate(moveDirection * Time.deltaTime, Space.World);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = enemy.transform.localScale;
        theScale.x *= -1;
        enemy.transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //only trigger if the object is tagged as player
        if (other.tag == "Player")
        {
            inRange = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //only trigger if the object is tagged as player
        if (other.tag == "Player")
        {
            inRange = false;

        }
    }
}
