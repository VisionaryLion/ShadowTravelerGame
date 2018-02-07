using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    //movement variables
    public float maxSpeed;

    //jumping variables
    bool grounded = false; //starts the player in the air
    float groundCheckRadius = 0.1f; //size of the ground check circle
    public LayerMask groundLayer; //layer to check for the circle
    public Transform groundCheck; //position of the circle
    public float jumpHeight; //force of the jump

    Rigidbody2D myRB; //reference to the rigidbody on the player
    Animator myAnim; //reference to the player's animator
    bool facingRight; //variable for if the player is facing to the right is true or not

    //for shooting
    public Transform gunTip; //reference to the location of where the missle starts from
    public GameObject bullet; //reference to the missile/projectile
    float fireRate = 0.5f; //how often the player can fire the missile or 1 rocket every X seconds
    float nextFire = 0f; //time when the player can fire again

    // Use this for initialization
	void Start () {
        myRB = GetComponent<Rigidbody2D>(); //GetComponent looks at the asset the script is attached to for a certain object
        myAnim = GetComponent<Animator>();

        facingRight = true; //starts the character facing to the right
		
	}

    // Update is called once per frame
    void Update () {

        //player jumping
        if (grounded && Input.GetAxis("Jump") > 0) { //checks if the player is on the ground and hit the jump button
            grounded = false;
            myAnim.SetBool("isGrounded", grounded); //assigns isGrounded (parameter from the animator) to the bool variable that checks if the player is grounded or not
            myRB.AddForce(new Vector2(0, jumpHeight)); //adds force to the player's Y
            Debug.Log(myRB.velocity);
        }

        //player shooting
        if (Input.GetAxisRaw("Fire1") > 0) fireRocket(); //checks if the player hit the Fire1 button and then calls the fireRocket function
    }

    // Update is called once per frame (no matter how long the frame took) VS FixedUpdate is called after a specific amount of time all the time (it's exact)
    void FixedUpdate () {

        //check if we are grounded - if no then we are falling
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); //OverlapCircle creates the circle out of the variables defined under //jumping variables
        myAnim.SetBool("isGrounded", grounded); //assigns isGrounded (parameter from the animator) to the bool variable that checks if the player is grounded or not

        myAnim.SetFloat("verticalSpeed", myRB.velocity.y); //sets the velocity of vertical speed on the rigid body only on the y axis i.e. up and down

        float move = Input.GetAxis("Horizontal"); //GetAxis is between -1 and 1, GetAxisRaw is -1, 0, or 1 //makes a float variable assigned to the Horizontal axis which are used by pressing the A and D keys or the left and righ arrow keys
        myAnim.SetFloat("speed", Mathf.Abs(move)); //sets the value of speed (parameter from the animator) as an absoulte value for move

        myRB.velocity = new Vector2(move * maxSpeed, myRB.velocity.y); //for the x value it multiplies the value for move by the maxSpeed set on the player & doesn't change the y value

        //if the player is pressing the D key and isn't facing right (facing left) else if the player is pressing the A key and facing right
        if (move > 0 && !facingRight) {
            flip(); //call the flip function
        } else if (move < 0 && facingRight) { 
            flip(); //call the flip function
        }
	}

    //flips the player's facing direction
    void flip() {
        facingRight = !facingRight; //reverses whichever way the player was facing
        Vector3 theScale = transform.localScale; //applies the transfrom values (x,y,z) from the player to the localScale and puts it to theScale
        theScale.x *= -1; //makes the x value of the scale negative or positive depending on its current value
        transform.localScale = theScale; //sets the new value back onto the transform value on the player
    }

    void fireRocket() {
        //checks if the time in the game is greater than the time the player can fire next
        if (Time.time > nextFire) {
            nextFire = Time.time + fireRate; //Makes the time the player can fire next equal to the time in the game + the rate at which the player can fire
            if (facingRight) {
                Instantiate(bullet, gunTip.position, Quaternion.Euler(new Vector3(0, 0, 0))); //instantiate the projectile at the gun tip's location with 0 rotation
            } else if(!facingRight) {
                Instantiate(bullet, gunTip.position, Quaternion.Euler(new Vector3(0, 0, 180f))); //instantiate the projectile at the gun tip's location with 180 degreee rotation on the z axis
            }
        }
    }
}
