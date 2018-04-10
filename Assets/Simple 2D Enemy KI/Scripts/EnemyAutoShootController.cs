using UnityEngine;
using System.Collections;

/*
 * See Scene:
 * 
 * 3 Enemy Auto Shoot
 * 
 * You find the EnemyAutoShootController as an Enemy-Component
 */
namespace Simple2DEnemyKI {	

	public class EnemyAutoShootController : MonoBehaviour {

		public GameObject FireObject;
		public float EnemyAutoShotRandomTimeMin = 0.1f;
		public float EnemyAutoShotRandomTimeMax = 0.5f;

		private Transform target;
		private float range = 0.2f;
		private bool CoRoutineIsRunning = false;

		private float horizontalPositionOld;
		private bool fireRight = false;

		private void InitPlayerTransform(){

			// Find players transform component
			if (target == null) {
				GameObject player = GameObject.FindWithTag(EnemyAWConst.PLAYER);

				if(player != null){
					target = player.transform;
				}
			}
		}

		void FixedUpdate(){
			// Get old x-position
			horizontalPositionOld = transform.position.x;
		}

		void Update () {

			if (!CoRoutineIsRunning) {
				if(CheckPlayerIsInVisionField()){	
					DoFire();
				}
			}
		}

		private bool CheckPlayerIsInVisionField(){

			InitPlayerTransform ();

			// Player ist links
			float y1 = transform.position.y + range;
			float y2 = transform.position.y - range;


			if (target != null) {

				// Check if player is in range on y axis
				if ((target.localPosition.y < y1) && (target.localPosition.y > y2)) {

					if (target.position.x < transform.position.x) {

						// Player is on the left
						if(transform.position.x < horizontalPositionOld){
							fireRight = false;
							return true;
						}

						// Player is on the right
					} else {
						if(transform.position.x > horizontalPositionOld){
							fireRight = true;
							return true;
						}
					}
				}
			}

			return false;
		}

		private void DoFire(){
			StartCoroutine (FireCoRoutine ());
			CoRoutineIsRunning = true;
		}

		IEnumerator FireCoRoutine(){
			yield return (new WaitForSeconds (Random.Range(EnemyAutoShotRandomTimeMin, EnemyAutoShotRandomTimeMax)));

			// Check vision field again afters waiting random seconds
			if (CheckPlayerIsInVisionField ()) {

				GameObject go = (GameObject) Instantiate(FireObject, transform.position, Quaternion.identity);

				//Fire left or right
				if(fireRight){
					go.GetComponent<ShootFlying> ().FlyRight = true;
				} else {
					go.GetComponent<ShootFlying> ().FlyLeft = true;
				}

			}

			StartCoroutine (CoolDown ());
		}

		IEnumerator CoolDown() {
			yield return (new WaitForSeconds (0.5f));

			// Revert CoRoutineValue ...dont shot permanently
			CoRoutineIsRunning = false;

		}
	}
}