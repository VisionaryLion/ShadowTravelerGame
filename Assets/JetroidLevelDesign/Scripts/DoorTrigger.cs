using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    public Door door;
    public bool ignoreTrigger; //toggle to ignore the door's trigger and require a switch to open it

    void OnTriggerEnter2D(Collider2D target)
    {
        //returns out of the statement if the ignoreTrigger is activated
        if (ignoreTrigger)
            return;

        if (target.gameObject.tag == "Player")
        {
            door.Open();
        }
    }

    void OnTriggerExit2D(Collider2D target)
    {
        //returns out of the statement if the ignoreTrigger is activated
        if (ignoreTrigger)
            return;

        if (target.gameObject.tag == "Player")
        {
            door.Close();
        }
    }

    public void Toggle(bool value)
    {
        if (value)
        {
            door.Open();
        }
        else
        {
            door.Close();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = ignoreTrigger ? Color.gray : Color.green; //makes the gizmo gray if the door trigger is inactive and green if it's active

        var bc2D = GetComponent<BoxCollider2D>(); 
        var pos = bc2D.transform.position; //sets the drawn box collider to the position of the box collider

        var newPos = new Vector2(pos.x + bc2D.offset.x, pos.y + bc2D.offset.y); //in case you move the trigger area away from the door
        Gizmos.DrawWireCube(newPos, new Vector2(bc2D.size.x, bc2D.size.y)); //draws gizmo for the door trigger based on it's new position and the actual size of the trigger
    }
}
