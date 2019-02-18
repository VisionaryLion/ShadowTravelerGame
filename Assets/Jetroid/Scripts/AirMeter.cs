using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //to use the slider

public class AirMeter : MonoBehaviour {

    public float air = 10;
    public float maxAir = 10;
    public float airBurnRate = .5f;

    private Player player;
    private Slider slider;

	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<Player>();
        slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null) //if the player is dead leave the statement
            return;

        if (air > 0)
        {
            air -= Time.deltaTime * airBurnRate; //subtract air over time at a rate of airBurnRate
            slider.value = air / maxAir; //assigns the slider value to be a precentage of the player's max air
        } else
        {
            var script = player.GetComponent<Explode>(); //finds the explode script on the player and assigns it to a variable
            script.OnExplode(); //calls the OnExplode function from the Explode script 
        }
	}
}
