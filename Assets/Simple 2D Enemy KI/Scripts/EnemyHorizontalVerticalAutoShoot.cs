using UnityEngine;
using System.Collections;

/*
 * See Scene:
 * 
 * 6 EnemyShootHorizontalAndVertical
 * 
 * You find the EnemyHorizontalVerticalAutoShoot as an Enemy-Component
 */
namespace Simple2DEnemyKI {

	public class EnemyHorizontalVerticalAutoShoot : MonoBehaviour {

		public GameObject Shot;

		public float ShotAfterTimeMin = 1.0f;
		public float ShotAfterTimeMax = 3.0f;

		public float MaxSpeed = 10.0f;

		public bool ShotLeft = false;
		public bool ShotRight = false;
		public bool ShotUp = false;
		public bool ShotDown = false;

		private bool CoRoutineIsRunningLeft = false;
		private bool CoRoutineIsRunningRight = false;
		private bool CoRoutineIsRunningUp = false;
		private bool CoRoutineIsRunningDown = false;

		void Update(){

			// Do shot after time
			if (ShotLeft) {
				if(!CoRoutineIsRunningLeft){
					CoRoutineIsRunningLeft = true;
					StartCoroutine(FireCoRoutine(ShotState.LEFT));
				}
			}

			if (ShotRight) {
				if(!CoRoutineIsRunningRight){
					CoRoutineIsRunningRight = true;
					StartCoroutine(FireCoRoutine(ShotState.RIGHT));
				}
			}

			if (ShotUp) {
				if(!CoRoutineIsRunningUp){
					CoRoutineIsRunningUp = true;
					StartCoroutine(FireCoRoutine(ShotState.UP));
				}
			}

			if (ShotDown) {
				if(!CoRoutineIsRunningDown){
					CoRoutineIsRunningDown = true;
					StartCoroutine(FireCoRoutine(ShotState.DOWN));
				}
			}
		}

		IEnumerator FireCoRoutine(ShotState state){
			yield return (new WaitForSeconds (Random.Range (ShotAfterTimeMin, ShotAfterTimeMax)));

			DoShot (state);

		}
		private void DoShot(ShotState state){

			GameObject go = (GameObject) Instantiate(Shot, transform.position, Quaternion.identity);
			ShootFlying shotFlying = go.GetComponent<ShootFlying> ();

			switch (state) {

			case ShotState.DOWN:
				// Set flying direction and revert CoRoutineRunningValue
				shotFlying.FlyDown = true;
				CoRoutineIsRunningDown = false;
				break;

			case ShotState.LEFT:
				shotFlying.FlyLeft = true;
				CoRoutineIsRunningLeft = false;
				break;

			case ShotState.RIGHT:
				shotFlying.FlyRight = true;
				CoRoutineIsRunningRight = false;
				break;

			case ShotState.UP:
				shotFlying.FlyUp = true;
				CoRoutineIsRunningUp = false;
				break;
			}
		}

		public enum ShotState {
			LEFT,
			RIGHT,
			UP, 
			DOWN
		}
	}
}