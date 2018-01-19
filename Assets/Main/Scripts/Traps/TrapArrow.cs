using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour {

    //public GameObject Turret;
    //public GameObject TurretLaserbeam;
    //Animator animator;

    public GameObject Bullet;
    public float fireRate = 0.3f;
    //public float bulletSpeed = 1.8f;

    public GameObject BaseArrow;
    [Tooltip("Always add an extra 1. If you want 3 arrows, put 4")]
    public int TotalArrows = 4;
    //public static float damage = 10;

    public Vector2 Guntip;
    public GameObject Gunshot;
    private float lastShot = 0.0f;
    private int shotArrows = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Player")))
        { 
            if(shotArrows < TotalArrows && Time.time > fireRate + lastShot)
            {
                //shoot arrow function
                //StartCoroutine(TurretShooting());
                StartCoroutine(HideBaseArrow());
                TurretShoot();
                shotArrows++;
                
            }
            else
            {
                BaseArrow.gameObject.SetActive(false);
            }   
        }
    }

    public void TurretShoot()
    {
        Guntip = new Vector2(Gunshot.transform.position.x, Gunshot.transform.position.y);
        //animator.speed = 0;

        if (Time.time > fireRate + lastShot)
        {
            //Instantiate a bullet
            GameObject arrowClone = Instantiate(Bullet, Guntip, Quaternion.identity) as GameObject;
            Rigidbody2D clonerb = arrowClone.GetComponent<Rigidbody2D>();
            clonerb.transform.rotation = Gunshot.transform.rotation;
            lastShot = Time.time;
        }
    }

    IEnumerator HideBaseArrow()
    {
        BaseArrow.gameObject.SetActive(false);
        yield return new WaitForSeconds(fireRate);
        if (shotArrows < TotalArrows)
        {
            BaseArrow.gameObject.SetActive(true);
        }
    }
}
