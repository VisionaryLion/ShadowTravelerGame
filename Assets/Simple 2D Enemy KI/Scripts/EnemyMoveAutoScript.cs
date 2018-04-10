using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * See Scenes:
 * 
 * 1 EnemyAutoWalk
 * 2 EnemyAutoWalkAndPlayerFollowing
 * 3 Enemy Auto Shoot
 * 
 * You find the EnemyMoveAutoScript as an Enemy-Component
 */
namespace Simple2DEnemyKI {

	public class EnemyMoveAutoScript : MonoBehaviour {

		// Enabled PlayerFollowing if the enemy sees the player over raycasts
		[Tooltip("Enable this attribute for raycasting player and set PlayerFollowing = false on startup")]
		public bool PlayerFollowingAutoEnabler = false;
		// How long is the raycast
		public float raycastLength = 20;

		// Patrol is automatically enabled if PlayerFollowingAutoEnabler is true
		[HideInInspector]
		public bool Patrol = false;

		// If the PlayerFollwing is enabled the enemies takes other decisions on the EnemyStatePointController-Triggers
		// Player is above enemy: Jump. Play is below enemy: Move forward and reach the level below
		public bool PlayerFollowing = false;
		private Transform target;

		// Range for checking if player is above or below the enemy.
		private float range = 0.2f; 

		// Enemy to some auto jumps after time
		public bool EnemyAutoPeriodJumpUp = true;
		public float EnemyAutoJumpUpTimeDelayMin = 2.0f;
		public float EnemyAutoJumpUpTimeDelayMax = 5.0f;
		private bool IsInCoRoutineAutoJumpUp = false;

		// If you change the collider-size you must change the size of this variable too
		public float GroundedCheckRadius = 0.4f;

		public float MaxSpeed = 6.0f;
		public float JumpForceUp = 1300f;
		public float JumpForceUpForward = 750f;

		// Need for flip enemy
		private float horizontalPositionOld = 0.0f;

		private bool moveRight = true;


		[HideInInspector]
		public bool facingRight = true;

		private bool Jump = false;
		private bool JumpInProgress = false;
		private bool JumpFromBeginningToEnd = false;

		[HideInInspector]
		public EnemyStates EnemyState = EnemyStates.MOVE_FORWARD;

		// If enemy do jump then disable autowalk
		[HideInInspector]
		public bool StopWalking = false; 

		// Need for jumping through walls
		public bool JumpThroughWalls = false;
		private float highestJumpCoordinate = 0.0f;

		public Transform GroundCheck;
		[HideInInspector]
		public bool Grounded = false;
		private LayerMask whatIsGround;

		public CircleCollider2D EnemyTriggerCollider;
		public Collider2D EnemyJumpCollider;

		private Animator anim;

		private bool stayTriggerDirectionChangeCoolDown = false;

		void Awake(){

			anim = gameObject.GetComponent<Animator> ();

			whatIsGround = LayerMask.GetMask (EnemyAWConst.GROUND, EnemyAWConst.PLATFORMS);

			// Need for flip enemy
			horizontalPositionOld = transform.localPosition.x;

			// Enemy should not collide with other enemys
			Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer (EnemyAWConst.ENEMY), LayerMask.NameToLayer (EnemyAWConst.ENEMY), true);
		}

		void Start() {

			// Random direction at start
			int ran = Random.Range (0, 2);

			if (ran == 0) {
				moveRight = true;
			} else {
				moveRight = false;
			}

			// Set it on start 1 time
			if (PlayerFollowingAutoEnabler) {
				Patrol = true;
			}
		}

		void Update(){

			if (PlayerFollowing || PlayerFollowingAutoEnabler) {
				if(target == null) {
					InitPlayerTransform();
				}
			}

			switch (EnemyState) {
			case EnemyStates.JUMP_FORWARD:
				Jump = true;
				break;
			case EnemyStates.JUMP_UP:
				Jump = true;
				break;
			case EnemyStates.MOVE_FORWARD:
				// do nothing, move automatically
				break;
			}

			// Set the enemy-animations
			if (!Grounded) {
				anim.SetBool (EnemyAWConst.WALK, false);
				anim.SetBool (EnemyAWConst.GROUND, false);
				anim.SetBool (EnemyAWConst.JUMP, true);
			} else {
				anim.SetBool (EnemyAWConst.GROUND, true);
				anim.SetBool (EnemyAWConst.JUMP, false);

				if(StopWalking){
					anim.SetBool (EnemyAWConst.WALK, false);
				} else {
					anim.SetBool (EnemyAWConst.WALK, true);
				}
			}

			if (PlayerFollowingAutoEnabler) {

				if (target != null) {

					Vector2 pos = transform.position;

					// Raycast looks right or left
					if(facingRight) {
						pos.x += raycastLength;
					} else {
						pos.x -= raycastLength;
					}

					// Raycast sees the layers: player, ground and wall
					RaycastHit2D hit = Physics2D.Linecast(transform.position, pos, 1 << LayerMask.NameToLayer(EnemyAWConst.PLAYER) | 1 << LayerMask.NameToLayer(EnemyAWConst.GROUND)| 1 << LayerMask.NameToLayer(EnemyAWConst.WALL) | 1 << LayerMask.NameToLayer(EnemyAWConst.PLATFORMS));

					Debug.DrawLine(transform.position, pos, Color.green);

					if(hit) {

						if(hit.transform.tag.Equals(EnemyAWConst.PLAYER)) {
							PlayerFollowing = true;
							Patrol = false;
						} 
					}

				}
			}
		}

		void FixedUpdate(){

			Grounded = Physics2D.OverlapCircle (GroundCheck.position, GroundedCheckRadius, whatIsGround);

			if (Jump && !JumpFromBeginningToEnd && Grounded && !JumpInProgress) {

				JumpFromBeginningToEnd = true;
				Jump = false;
				JumpInProgress = true;
				StopWalking = true;

				// Get actual position
				highestJumpCoordinate = gameObject.transform.position.y; 
				// and fix it
				highestJumpCoordinate -= 0.1f;

				// Jump up and forward
				if(EnemyState.Equals(EnemyStates.JUMP_FORWARD)){

					GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
					GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForceUpForward);

					// Save as temp variable
					float JumpForwardSpeedTempValue = MaxSpeed;

					if(!facingRight){
						JumpForwardSpeedTempValue = JumpForwardSpeedTempValue * -1;	
					}

					GetComponent<Rigidbody2D>().velocity = new Vector2( JumpForwardSpeedTempValue, GetComponent<Rigidbody2D>().velocity.y );

					// Jump up
				} else if(EnemyState.Equals(EnemyStates.JUMP_UP)){

					GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
					GetComponent<Rigidbody2D>().AddForce(Vector2.up * JumpForceUp);

				}
			}

			if (JumpInProgress) {

				// This is the jumping process. If the enemy should jump through walls the
				// ground layer will be disable on up-jump and will be enabled when the enemy arrive the highest jump-point
				float actualY = gameObject.transform.localPosition.y;

				if(actualY > highestJumpCoordinate){
					highestJumpCoordinate = actualY;

					if(JumpThroughWalls){
						EnemyJumpCollider.enabled = false;
					}

				} else {

					if(JumpThroughWalls){
						EnemyJumpCollider.enabled = true;
					}

					JumpInProgress = false;

					// Wait a little bit to continue auto walking
					StartCoroutine(ResumeAutoWalkAfterTime(0.2f));
				}


			} else {

				// Auto walk when grounded
				if(!StopWalking && Grounded){

					// Save x value to for checking flip
					horizontalPositionOld = transform.position.x;

					if(moveRight) {
						transform.Translate(Vector3.right * Time.deltaTime * MaxSpeed, Space.World);
					} else {
						transform.Translate(-Vector3.right * Time.deltaTime * MaxSpeed, Space.World);
					}

					// Enemy schaut nach links bzw. nach rechts
					if(transform.position.x < horizontalPositionOld){
						// rechts
						if(facingRight){
							Flip ();
							facingRight = false;		
						}
					} else {
						if(!facingRight){
							Flip ();
							facingRight = true;
						}
					}

					// If EnemyAutoJumpUp activated the jump in certain intervals
					EnemyAutoJumpUpAfterTime();
				}
			}
		}

		private void EnemyAutoJumpUpAfterTime(){

			if (EnemyAutoPeriodJumpUp) {

				if (Grounded && !IsInCoRoutineAutoJumpUp && !Jump && !JumpInProgress) {

					IsInCoRoutineAutoJumpUp = true;
					StartCoroutine(AutoJumpUpAfterTime(Random.Range (EnemyAutoJumpUpTimeDelayMin, EnemyAutoJumpUpTimeDelayMax)));
				}
			}
		}

		IEnumerator ResumeAutoWalkAfterTime(float time){
			yield return (new WaitForSeconds (time));

			EnemyState = EnemyStates.MOVE_FORWARD;
			StopWalking = false;
			JumpFromBeginningToEnd = false;
		}

		public void ChangeDirection(){

			if(Grounded){
				if (facingRight) {
					moveRight = false;
				} else {
					moveRight = true;
				}
			}
		}

		void Flip(){

			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		private void OnTriggerEnter2D(Collider2D col){
			if (col.gameObject.tag.Equals (EnemyAWConst.FIELD_EDGE)) {
				ChangeDirection();
			}
		}

		private void OnTriggerStay2D(Collider2D col){
			
			if (col.gameObject.tag.Equals (EnemyAWConst.FIELD_EDGE)) {
				if (!stayTriggerDirectionChangeCoolDown) {
					stayTriggerDirectionChangeCoolDown = true;
					ChangeDirection ();
					StartCoroutine (ChangeDirectionAfterTime ());
				} 
			}
		}

		IEnumerator ChangeDirectionAfterTime(){
			yield return (new WaitForSeconds (0.3f));
			stayTriggerDirectionChangeCoolDown = false;
		}

		IEnumerator AutoJumpUpAfterTime(float time){
			yield return (new WaitForSeconds (time));

			if (Grounded && !Jump && !JumpInProgress) {

				GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				StopWalking = true;

				// Change direction after jump?
				if(Random.Range (0, 3) == 1){
					ChangeDirection();
				}

				// It looks better when enemy looks around before jump
				Flip ();
				yield return (new WaitForSeconds (0.3f));
				Flip ();
				yield return (new WaitForSeconds (0.3f));
				Flip ();
				yield return (new WaitForSeconds (0.3f));
				Jump = true;
				EnemyState = EnemyStates.JUMP_UP;
				yield return (new WaitForSeconds (0.3f));
				Flip ();
			}

			IsInCoRoutineAutoJumpUp = false;	
		}

		private void InitPlayerTransform(){

			if (target == null) {
				GameObject player = GameObject.FindWithTag(EnemyAWConst.PLAYER);

				if(player != null){
					target = player.transform; //target the player
				}
			}
		}


		public void DoEnemyStatePointAction(bool jump_up, bool jump_forward, bool move_forward, bool move_directionChange){

			if (Grounded && !Jump && !JumpInProgress && !JumpFromBeginningToEnd) {

				List<EnemyStates> EnemyStateList = new List<EnemyStates> ();

				if (jump_up) {
					EnemyStateList.Add(EnemyStates.JUMP_UP);
				}

				if (jump_forward) {

					if(PlayerFollowing){

						InitPlayerTransform();

						if(target != null){

							if (target.position.y > (transform.position.y - range)) {
								// Player is above
								EnemyStateList.Add(EnemyStates.JUMP_FORWARD);
							} else {
								// Player is down
								EnemyStateList.Add(EnemyStates.MOVE_FORWARD);
							}

						} else {
							EnemyStateList.Add(EnemyStates.MOVE_FORWARD);
						}

					} else {
						EnemyStateList.Add(EnemyStates.JUMP_FORWARD);
					}
				}

				if (move_forward) {

					if(PlayerFollowing){

						InitPlayerTransform();

						if(target != null){

							if (target.position.y > (transform.position.y + range)) {
								// Player is above
								EnemyStateList.Add(EnemyStates.JUMP_FORWARD);

							} else {
								// Player is down
								EnemyStateList.Add(EnemyStates.MOVE_FORWARD);
							}

						} else {
							EnemyStateList.Add(EnemyStates.MOVE_FORWARD);
						}

					} else {
						EnemyStateList.Add(EnemyStates.MOVE_FORWARD);
					}
				}

				if(move_directionChange){
					EnemyStateList.Add(EnemyStates.MOVE_CHANGEDIRECTION);
					ChangeDirection();
				}

				if (EnemyStateList.Count == 0) {
					EnemyStateList.Add(EnemyStates.JUMP_FORWARD);
					EnemyStateList.Add(EnemyStates.MOVE_FORWARD);
				}

				EnemyState = EnemyStateList[Random.Range (0, EnemyStateList.Count)];
			}
		}


		public enum EnemyStates {
			JUMP_UP,
			JUMP_FORWARD,
			MOVE_FORWARD,
			MOVE_CHANGEDIRECTION
		}
	}
}