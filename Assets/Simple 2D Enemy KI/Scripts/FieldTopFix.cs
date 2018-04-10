using UnityEngine;
using System.Collections;

/*
* See Scenes:
* 
* All Scenes
* 
* You find the FieldTopFix on the upper fringe.
* If the player or the walking enemy jumps over the upper fringe, disable the isTrigger-Function
* On TriggerExit enabled the isTrigger-Function.
* 
* So the player or the enemy cannot jump through the upper fringe
*/
namespace Simple2DEnemyKI {

	public class FieldTopFix : MonoBehaviour {

		void OnTriggerEnter2D(Collider2D col){
			FixGameObjectPosition (col, false);
		}

		void OnTriggerStay2D(Collider2D col){
			FixGameObjectPosition (col, false);
		}

		void OnTriggerExit2D(Collider2D col){
			FixGameObjectPosition (col, true);
		}

		private void FixGameObjectPosition(Collider2D col, bool triggerEnabled){

			// Der Player darf nicht durch die Seitenwände springen
			if (col.gameObject.tag.Equals (EnemyAWConst.PLAYER)) {
				MyPlayerController cont = col.gameObject.GetComponent<MyPlayerController>();
				cont.PlayerPolygonCollider.isTrigger = triggerEnabled;
			}

			if (LayerMask.LayerToName (col.gameObject.layer).Equals (EnemyAWConst.ENEMY)) {
				if (!col.gameObject.tag.Equals (EnemyAWConst.ENEMY_BOUNCING)) {
					EnemyMoveAutoScript cont = col.gameObject.GetComponent<EnemyMoveAutoScript>();
					cont.EnemyTriggerCollider.isTrigger = triggerEnabled;
				}
			}
		}
	}
}