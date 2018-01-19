using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    public GameObject Spike;

    public float SpikeHeight = 0.55f;
    public float animationSpeed = 0.09f;
    public float animationDownSpeed = 0.1f;
    public float SpikesUpTime = 1f;
    public float WaitToLaunch = .55f;

    private float animationWaitTime = .05f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Player")))
        {
            //play animation
            StartCoroutine(ActivateSpikes());
            //this script only plays the animation
            //Add a script to the spike gameObject that kills the player
        }
    }

    IEnumerator ActivateSpikes()
    {

        yield return new WaitForSeconds(WaitToLaunch);

        //define the local scale variable
        Vector3 SpikeScale = transform.localScale;
            
           //Spike is actived
            Spike.GetComponent<Collider2D>().enabled = true;
            //this.GetComponent<Collider2D>().enabled = true;
            while (SpikeScale.y < SpikeHeight)
            {
                SpikeScale.y = SpikeScale.y + animationSpeed;
                transform.localScale = SpikeScale;
                yield return new WaitForSeconds(animationWaitTime);
            }

            yield return new WaitForSeconds(SpikesUpTime);

        //spike is retracted
        //this.GetComponent<Collider2D>().enabled = false;
        while (SpikeScale.y > .05f)
        {
            SpikeScale.y = SpikeScale.y - animationDownSpeed;
            transform.localScale = SpikeScale;
            yield return new WaitForSeconds(animationWaitTime);
        }
        Spike.GetComponent<Collider2D>().enabled = false;
        //yield return new WaitForSeconds(laserDownTime);
    }
}
