using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    void OnDestroy()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        OnDestroy();
    }

}
