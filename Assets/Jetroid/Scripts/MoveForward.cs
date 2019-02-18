using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {

	public float speed = 10f;

	private Rigidbody2D body2D;

	// Use this for initialization
	void Start () {
		body2D = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		body2D.velocity = new Vector2 (transform.localScale.x, 0) * speed;
	}
}
