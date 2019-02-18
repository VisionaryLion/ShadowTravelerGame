using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    public DoorTrigger[] doorTriggers; //array for one or more door triggers
    public bool sticky; //toggle to decide if the switch stays stuck down or not

    private bool down = false;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D target)
    {
        //guard against damage dealing items triggering the triggers
        if (target.tag == "Deadly")
            return;

        down = true;
        animator.SetInteger ("AnimState", 1);

        //turns on all door triggers in the array
        foreach (var trigger in doorTriggers)
        {
            if(trigger != null)
            {
                trigger.Toggle(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        //prevents the switch from being activated again if it's stuck down
        if (sticky && down)
            return;

        down = false;

        animator.SetInteger("AnimState", 2);

        //turns off all door triggers in the array
        foreach (var trigger in doorTriggers)
        {
            if (trigger != null)
            {
                trigger.Toggle(false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        //red = is sticky and green = not sticky
        Gizmos.color = sticky ? Color.red : Color.green;

        //draw a line from each switch to each door trigger
        foreach (var trigger in doorTriggers)
        {
            if (trigger != null)
            {
                Gizmos.DrawLine(transform.position, trigger.door.transform.position);
            }
        }
    }
}
