using UnityEngine;
using System.Collections;

/*
* See Scenes:
* 
* 3 Enemy Auto Shoot
* 6 EnemyShotHorizontalAndVertical
* 
* Killzone to kill the shot-gameobject. You find the Killzone on the field edges
*/
namespace Simple2DEnemyKI {
	
	public class Killzone : MonoBehaviour {

		void OnTriggerEnter2D(Collider2D col){

			// Destroy shot
			if(col.gameObject.tag.Equals (EnemyAWConst.SHOT)){
				Destroy (col.gameObject);
			}
		}
	}
}