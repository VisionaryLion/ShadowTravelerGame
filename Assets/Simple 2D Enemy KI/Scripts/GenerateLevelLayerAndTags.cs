using UnityEngine;
using System.Collections;

/*
 * Needed to add layers and tags programmatically to the gameobjects after importing the 2D Enemy KI Asset 
 * 
 * You don't need to implement it on your project
*/
namespace Simple2DEnemyKI {

	public class GenerateLevelLayerAndTags : MonoBehaviour {

		public bool TagEnemy = false;
		public bool TagPlayer = false;
		public bool TagFieldEdge = false;
		public bool TagFieldTop = false;
		public bool TagShot = false;
		public bool TagPlayerBullet = false;
		public bool LayerGround = false;
		public bool LayerEnemy = false;
		public bool LayerPlayer = false;
		public bool LayerWall = false;

		void Awake(){

			if (TagEnemy) {
				gameObject.tag = EnemyAWConst.ENEMY;
			} else if (TagPlayer) {
				gameObject.tag = EnemyAWConst.PLAYER;
			} else if (TagFieldEdge) {
				gameObject.tag = EnemyAWConst.FIELD_EDGE;
			} else if (TagFieldTop) {
				gameObject.tag = EnemyAWConst.FIELD_TOP;
			} else if (TagShot) {
				gameObject.tag = EnemyAWConst.SHOT;
			} else if (TagPlayerBullet) {
				gameObject.tag = EnemyAWConst.PLAYER_BULLET;
			}

			if (LayerGround) {
				gameObject.layer = LayerMask.NameToLayer (EnemyAWConst.GROUND);
			} else if (LayerEnemy) {
				gameObject.layer = LayerMask.NameToLayer (EnemyAWConst.ENEMY);
			} else if (LayerPlayer) {
				gameObject.layer = LayerMask.NameToLayer (EnemyAWConst.PLAYER);
			} else if (LayerWall) {
				gameObject.layer = LayerMask.NameToLayer (EnemyAWConst.WALL);
			}
		}
	}
}