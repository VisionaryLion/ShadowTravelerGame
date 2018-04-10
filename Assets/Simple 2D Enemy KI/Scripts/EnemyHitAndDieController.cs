using UnityEngine;
using System.Collections;

/*
 * See Scene:
 * 
 * 7 EnemyHitAndDie
 * 
 * You find the EnemyHitAndDieController as an Enemy-Component
 */
namespace Simple2DEnemyKI {

	public class EnemyHitAndDieController : MonoBehaviour {

		public GameObject RespawnPoint;
		public int EnemyLives = 3;

		private Animator anim;
		[HideInInspector]
		public bool isBlinking = false;
		private float damageEffectPause = 0.2f;
		private EnemyMoveAutoScript enemyMoveAutoScript;

		void Awake() {
			enemyMoveAutoScript = gameObject.GetComponent<EnemyMoveAutoScript> ();
			anim = gameObject.GetComponent<Animator> ();
		}

		void OnTriggerEnter2D(Collider2D col){

			bool doHit = false;

			if (LayerMask.LayerToName (col.gameObject.layer).Equals (EnemyAWConst.PLAYER_PROJECTILES)) {
				doHit = true;
			}

			if (col.gameObject.tag.Equals (EnemyAWConst.PLAYER_BULLET)) {
				doHit = true;
			}

			// Is the enemy hit by the player bullet
			if (doHit){
				
				// Player can only hit if enemy is not blinking
				if(!isBlinking){

					EnemyLives--;

					// Enemy die and respawn
					if(EnemyLives <= 0) {
						if (enemyMoveAutoScript != null) {
							enemyMoveAutoScript.StopWalking = true;
						}

						anim.SetBool(EnemyAWConst.DIE, true);

						StartCoroutine(DoRespawnAfterTime());

						// Enemy hit and blink
					} else {

						StartCoroutine (BlinkAfterHit ());
					}

					// Destroy bullet
					Destroy(col.gameObject);
				}
			}
		}

		IEnumerator DoRespawnAfterTime() {
			yield return (new WaitForSeconds (0.25f));

			// Do respawn on position
			if (RespawnPoint != null) {
				transform.position = RespawnPoint.transform.position;

				anim.SetBool (EnemyAWConst.DIE, false);
				EnemyLives = 3;

				if (enemyMoveAutoScript != null) {
					enemyMoveAutoScript.StopWalking = false;
				}

			} else {
				Destroy (gameObject);
			}
		}

		// If Enemy is blinking player cannot hit again
		IEnumerator BlinkAfterHit(){

			// If you want that the enemy does not collide with the player while the enemy is blinking add the a new layer
			// to your Project Settings like "Player". Add this als layer to your player and comment the following two Physics2D-Lines in
			// Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer (Const.ENEMY), LayerMask.NameToLayer (Const.PLAYER), true);

			isBlinking = true;

			GetComponent<Renderer>().enabled = false;
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = true;		
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = false;
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = true;
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = false;
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = true;
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = false;
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = true;
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = false;
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = true;
			yield return new WaitForSeconds(damageEffectPause);
			GetComponent<Renderer>().enabled = true;

			isBlinking = false;

			// Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer (Const.ENEMY), LayerMask.NameToLayer (Const.PLAYER), false);

		}
	}
}