using UnityEngine;
using System.Collections;

/*
 * Example player for PlayerFollowing on Scene:
 * 
 * 2 EnemyAutoWalkAndPlayerFollowing
 */
namespace Simple2DEnemyKI {

	public class MyPlayerController : MonoBehaviour {

		public GameObject PlayerPrefab;
		public GameObject RespawnPointPrefab;

		public float MoveForce = 700f;
		public float MaxSpeed = 0.05f;
		public float JumpForce = 27f;

		public Transform GroundCheck;
		private bool grounded = false;
		private LayerMask whatIsGround;

		private Animator anim = null;

		private float highestJumpCoordinate = 0.0f;
		private BoxCollider2D PlayerBoxCollider;
		[HideInInspector]
		public PolygonCollider2D PlayerPolygonCollider;

		private bool PlayerDie = false;

		[HideInInspector]
		public bool PlayerBlinkt = false;

		private bool jump = false;
		[HideInInspector]
		public bool facingRight = true;
		private bool jumpInProgress = false;
		private float horizontal;


		void Awake(){

			anim = GetComponent<Animator> ();

			whatIsGround = LayerMask.GetMask (EnemyAWConst.GROUND, EnemyAWConst.PLATFORMS);

			InitRigidbodyUndCollider();
		}

		public void InitRigidbodyUndCollider(){

			transform.parent = null;

			PlayerPolygonCollider = GetComponent<PolygonCollider2D> ();
			PlayerPolygonCollider.enabled = true;

			PlayerBoxCollider = GetComponent<BoxCollider2D>();
			PlayerBoxCollider.enabled = true;
			PlayerBoxCollider.isTrigger = false;

			if (GetComponent<Rigidbody2D>() == null) {
				gameObject.AddComponent<Rigidbody2D> ();

				GetComponent<Rigidbody2D>().mass = 2;
				GetComponent<Rigidbody2D>().gravityScale = 3;
				//GetComponent<Rigidbody2D>().fixedAngle = true; // Needed for Unity 4.6.x
				GetComponent<Rigidbody2D>().freezeRotation = true;
				GetComponent<Rigidbody2D>().interpolation =  RigidbodyInterpolation2D.Interpolate;
				GetComponent<Rigidbody2D>().angularDrag = 0.05f;
				GetComponent<Rigidbody2D>().drag = 1;
			}
		}

		private bool InitJumpBeforeFixedUpdate(){

			bool doJump = true;

			// Dont jump if cursor down
			if(Input.GetButtonDown(EnemyAWConst.VERTICAL)){
				if((Input.GetAxis(EnemyAWConst.VERTICAL) < 0)) {
					doJump = false;
				}
			}

			if(doJump){
				jump = true;
			}			


			if(jump){

				if(GetComponent<Rigidbody2D>() != null){
					highestJumpCoordinate = gameObject.transform.localPosition.y;
				}
			}

			return jump;
		}


		void Update(){

			grounded = Physics2D.OverlapCircle (GroundCheck.position, 0.15f, whatIsGround);

			if (PlayerDie) {

				// If you want that the enemy does not collide with the player while is dying add the a new layer
				// to your Project Settings like "Player". Add this als layer to your player and comment the following Physics2D-Line in
				// Do the same in the RespawnAfterTime-Method
				// Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer (Const.ENEMY), LayerMask.NameToLayer (Const.PLAYER), true);

				int rotateValue = 16;

				if(facingRight){
					rotateValue *= -1;
				}

				transform.Rotate(0, 0, rotateValue);

				if(GetComponent<Rigidbody2D>() != null){
					Destroy(GetComponent<Rigidbody2D>());
				}

				PlayerPolygonCollider.enabled = false;

				StartCoroutine(RespawnAfterTime());

			} else {
				if (grounded) {
					anim.SetBool(EnemyAWConst.GROUND, true);
					anim.SetBool (EnemyAWConst.JUMP, false);

					anim.SetFloat( EnemyAWConst.SPEED, Mathf.Abs( horizontal ) );
				} else {
					anim.SetBool(EnemyAWConst.GROUND, false);
					anim.SetBool (EnemyAWConst.JUMP, true);

					anim.SetFloat( EnemyAWConst.SPEED, 0.0f);
				}

				if( grounded && Input.GetButtonDown(EnemyAWConst.VERTICAL)) {
					InitJumpBeforeFixedUpdate();
				}

				if (jumpInProgress) {

					// Player jump through the walls
					float actualY = gameObject.transform.localPosition.y;

					if(actualY > highestJumpCoordinate){
						highestJumpCoordinate = actualY;
					} else {
						PlayerBoxCollider.isTrigger = false;
						jumpInProgress = false;

					}
				}
			}
		}


		IEnumerator RespawnAfterTime() {
			yield return (new WaitForSeconds (1.0f));

			Instantiate (PlayerPrefab, RespawnPointPrefab.transform.position, Quaternion.identity);

			// Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer (Const.ENEMY), LayerMask.NameToLayer (Const.PLAYER), false);

			Destroy (gameObject);
		}

		void FixedUpdate () {

			if (!PlayerDie) {
				horizontal = Input.GetAxis(EnemyAWConst.HORIZONTAL);
				horizontal = Mathf.Clamp( horizontal, -1f, 1f );

				if(GetComponent<Rigidbody2D>() != null){
					if( horizontal * GetComponent<Rigidbody2D>().velocity.x < MaxSpeed )
						GetComponent<Rigidbody2D>().AddForce( Vector2.right * horizontal * MoveForce );

					if( Mathf.Abs( GetComponent<Rigidbody2D>().velocity.x ) > MaxSpeed )
						GetComponent<Rigidbody2D>().velocity = new Vector2( Mathf.Sign( GetComponent<Rigidbody2D>().velocity.x ) * MaxSpeed, GetComponent<Rigidbody2D>().velocity.y );
				}

				if( horizontal > 0f && !facingRight ) {
					Flip();

				} else if( horizontal < 0f && facingRight ) {
					Flip();
				}

				if (jump) {
					jump = false;

					if (grounded) {

						PlayerBoxCollider.isTrigger = true;

						GetComponent<Rigidbody2D>().velocity = new Vector2 (0, 0);
						GetComponent<Rigidbody2D>().AddForce (Vector2.up * JumpForce, ForceMode2D.Impulse);

						jumpInProgress = true;

					}
				}
			}
		}

		void Flip(){

			facingRight = !facingRight;
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}

		void OnTriggerEnter2D(Collider2D col){
			if(col.gameObject.tag.Equals(EnemyAWConst.ENEMY)){

				EnemyHitAndDieController hitAndDieController = col.gameObject.GetComponent<EnemyHitAndDieController>();

				// Only hit the Player if the enemy is not blinking.
				if(hitAndDieController != null) {
					if(hitAndDieController.isBlinking) {
						return;
					}
				}

				PlayerDie = true;
			}

			if (col.gameObject.tag.Equals (EnemyAWConst.SHOT)) {
				PlayerDie = true;
				Destroy(col.gameObject);
			}
		}
	}
}