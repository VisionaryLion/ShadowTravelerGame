using UnityEngine;
using System.Collections;

/*
 * See Scenes:
 * 
 * 1 EnemyAutoWalk
 * 2 EnemyAutoWalkAndPlayerFollowing
 * 3 Enemy Auto Shoot
 * 
 * What should the enemy do, if it trigger a collider with a EnemyStatePointController-Component
 */
namespace Simple2DEnemyKI {

	public class EnemyStatePointController : MonoBehaviour {

		[Tooltip("From which side comes the enemy")]
		public ConfigSide ComeFrom = ConfigSide.ALWAYS;

		public bool Patrol = false;
		public bool Jump_Up = false;
		public bool Jump_Forward = false;
		public bool Move_Forward = false;
		public bool Move_DirectionChange = false;

		void OnTriggerEnter2D(Collider2D col){

			if (col.gameObject.tag.Equals (EnemyAWConst.ENEMY)) {

				EnemyMoveAutoScript MoveAutoScript = col.gameObject.GetComponent<EnemyMoveAutoScript>();

				if(MoveAutoScript != null){
					if(Patrol) {
						if(MoveAutoScript.Patrol){
							if(ComeFrom.Equals(ConfigSide.LEFT) && MoveAutoScript.facingRight) {	
								MoveAutoScript.DoEnemyStatePointAction(Jump_Up, Jump_Forward, Move_Forward, Move_DirectionChange);

							} else if(ComeFrom.Equals(ConfigSide.RIGHT) && !MoveAutoScript.facingRight) {
								MoveAutoScript.DoEnemyStatePointAction(Jump_Up, Jump_Forward, Move_Forward, Move_DirectionChange);

							} else if(ComeFrom.Equals(ConfigSide.ALWAYS)) {
								MoveAutoScript.DoEnemyStatePointAction(Jump_Up, Jump_Forward, Move_Forward, Move_DirectionChange);
							}
						}

					} else {

						if(!MoveAutoScript.Patrol){
							if(ComeFrom.Equals(ConfigSide.LEFT) && MoveAutoScript.facingRight) {	
								MoveAutoScript.DoEnemyStatePointAction(Jump_Up, Jump_Forward, Move_Forward, Move_DirectionChange);

							} else if(ComeFrom.Equals(ConfigSide.RIGHT) && !MoveAutoScript.facingRight) {
								MoveAutoScript.DoEnemyStatePointAction(Jump_Up, Jump_Forward, Move_Forward, Move_DirectionChange);

							} else if(ComeFrom.Equals(ConfigSide.ALWAYS)) {
								MoveAutoScript.DoEnemyStatePointAction(Jump_Up, Jump_Forward, Move_Forward, Move_DirectionChange);
							}
						}
					}
				}
			}
		}

		public enum ConfigSide {
			LEFT,
			RIGHT,
			ALWAYS
		}
	}
}