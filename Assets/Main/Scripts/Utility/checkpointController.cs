using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpointController : MonoBehaviour {

    public Sprite leftHandle;
    public Sprite rightHandle;
    private SpriteRenderer checkpointSpriteRenderer;
    public bool checkpointReached;
    
    // Use this for initialization
	void Start () {
        checkpointSpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            checkpointSpriteRenderer.sprite = rightHandle;
            checkpointReached = true;
        }
    }
}
