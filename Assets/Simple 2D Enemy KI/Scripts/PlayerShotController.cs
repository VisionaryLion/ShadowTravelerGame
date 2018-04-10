using UnityEngine;
using System.Collections;

namespace Simple2DEnemyKI {

	public class PlayerShotController : MonoBehaviour {

		public GameObject Bullet;
		private MyPlayerController myPlayerController;

		void Awake () {
			myPlayerController = gameObject.GetComponent<MyPlayerController>();
		}
			
		void Update () {

			if( Input.GetButtonDown(EnemyAWConst.FIRE)) {

				GameObject go = (GameObject) Instantiate(Bullet, transform.position, Quaternion.identity);
				ShootFlying shootFlying = go.GetComponent<ShootFlying>();

				if(myPlayerController.facingRight){
					shootFlying.FlyRight = true;
				} else {
					shootFlying.FlyLeft = true;
				}
			}
		}
	}
}
