using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public float closeDelay = .5f; //time it takes the door to close

    Animator animator;
    new BoxCollider2D collider2D;
    
    // Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void EnableCollider2D()
    {
        collider2D.enabled = true;
    }

    void DisableCollider2D()
    {
        collider2D.enabled = false;
    }

    public void Open()
    {
        animator.SetInteger("AnimState", 1);
    }

    public void Close()
    {
        StartCoroutine(CloseNow());
    }

    IEnumerator CloseNow()
    {
        yield return new WaitForSeconds(closeDelay); //set it so the door closes after a delay
        animator.SetInteger("AnimState", 2);
    }
}
