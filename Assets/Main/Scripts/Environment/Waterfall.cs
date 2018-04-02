using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour {


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //only trigger if the object is tagged as torch
        if (other.tag == "Torch")
        {
            Destroy(other.gameObject);
            Debug.Log("Torch destroyed");

        }
    }
}
