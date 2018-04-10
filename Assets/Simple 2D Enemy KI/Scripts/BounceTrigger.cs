using UnityEngine;
using System.Collections;

/*
 * See Scene:
 * 
 * 4 EnemyBouncing
 * 
 * You find the BounceTrigger on the BouncingEnemy-Children GameObjects
 */
namespace Simple2DEnemyKI {

	public class BounceTrigger : MonoBehaviour {

		public bool Top = false;
		public bool Bottom = false;
		public bool Front = false;

		public EnemyBounceWalkScript bounceScript;

		void OnTriggerEnter2D(Collider2D col){	
			DoBounce (col);
		}

		void OnTriggerStay2D(Collider2D col){	
			DoBounce (col);
		}

		private void DoBounce(Collider2D col){
			bool collision = false;

			// With with elements should the bounce trigger interact.
			if (col.gameObject.tag.Equals (EnemyAWConst.FIELD_EDGE)) {
				collision = true;
			}

			if (LayerMask.LayerToName (col.gameObject.layer).Equals (EnemyAWConst.GROUND) ||
				LayerMask.LayerToName (col.gameObject.layer).Equals (EnemyAWConst.PLATFORMS)) {
				collision = true;
			}

			if (collision) {
				// Do bouncing
				bounceScript.Bounce(Top, Bottom, Front);
			}
		}
	}
}
