using UnityEngine;
using System.Collections;

/*
* See Scenes:
* 
* 3 Enemy Auto Shoot
* 6 EnemyShotHorizontalAndVertical
* 
* You find the EnemyShootFlying Script as a EnemyShot-Component (Prefab)
*/
namespace Simple2DEnemyKI {
	
	public class EnemyShootFlying : MonoBehaviour {

		[HideInInspector]
		public bool FlyRight = false;
		[HideInInspector]
		public bool FlyLeft = false;
		[HideInInspector]
		public bool FlyUp = false;
		[HideInInspector]
		public bool FlyDown = false;

		public float maxSpeed = 10.0f;

		void FixedUpdate () {

			if (FlyRight) {
				transform.Translate(Vector3.right * Time.deltaTime * maxSpeed);
			} else if(FlyLeft) {
				transform.Translate(Vector3.left * Time.deltaTime * maxSpeed);
			} else if(FlyUp){
				transform.Translate(Vector3.up * Time.deltaTime * maxSpeed);
			} else if(FlyDown){
				transform.Translate(Vector3.down * Time.deltaTime * maxSpeed);
			}
		}
	}
}