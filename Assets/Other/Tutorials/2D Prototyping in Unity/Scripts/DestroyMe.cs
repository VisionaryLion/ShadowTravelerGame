using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour {

    public float aliveTime;

	// Use this for initialization
	void Awake () {
        Destroy(gameObject, aliveTime); //destroys whatever gameobject the script is attached to after the time set for aliveTime
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
