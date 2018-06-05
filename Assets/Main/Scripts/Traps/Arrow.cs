using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public GameObject thisBullet;
    public ParticleSystem BulletHit;
    public int bounceCount = 2;
    public float bulletSpeed = 1.1f;
    public int BulletDamage = 10;
    //private int HitCounter = 0;
    private bool stuck = false;

    //for destroying the arrow
    public GameObject particleFX;

    // Use this for initialization
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D clonerb = thisBullet.GetComponent<Rigidbody2D>();
        clonerb.AddForce(transform.right * bulletSpeed);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if ((coll.gameObject.CompareTag("Player")))
        {
            if (stuck == false)
            {
                //play particle effect
                //do damage
                //destroy bullet
                Destroy(gameObject);
            }
        }
        else
        { 
            //thisBullet.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            Rigidbody2D clonerb = thisBullet.GetComponent<Rigidbody2D>();
            //play particle effect
            clonerb.constraints = RigidbodyConstraints2D.FreezeAll;
            stuck = true;
            Instantiate(particleFX, transform.position, transform.rotation); //spawns the particle effects
            Destroy(gameObject);
        }
    }
}
