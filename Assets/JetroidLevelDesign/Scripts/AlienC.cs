using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienC : MonoBehaviour {

    public float attackDelay = 3f;
    public GameObject projectile;

    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();

        if (attackDelay != 0)
        {
            StartCoroutine(OnAttack ());
        }
	}
	
    IEnumerator OnAttack()
    {
        //waits for the attack delay before firing
        yield return new WaitForSeconds(attackDelay);

        Fire();
        StartCoroutine(OnAttack()); //starts the coroutine over again
    }

    void Fire()
    {
        animator.SetInteger("AnimState", 1);
    }

	// Update is called once per frame
	void Update () {
        animator.SetInteger("AnimState", 0);
    }

    void OnFire() //fires during the alien c's attack animation
    {
        if (projectile != null)
        {
            var clone = Instantiate (projectile, transform.position, Quaternion.identity); //spawns the projectile at the position of the object that fired it and sets its orientation (Quaternion)
        }
    }


}
