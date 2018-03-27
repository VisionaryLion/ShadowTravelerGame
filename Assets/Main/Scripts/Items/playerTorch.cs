using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTorch : MonoBehaviour {

    public float delay;
    public float fadeTime ;
 
    private float fadeSpeed;
    private float intensity;
    private Color color;
 
    void Start() {
        fadeLight();
    }

    void Update() {
        if (delay > 0.0) {
            delay -= Time.deltaTime;
        }

        else if (intensity > 0.0) {
            intensity -= fadeSpeed * Time.deltaTime;
            GetComponent<SFLight>().intensity = intensity;
        }
    }

    void fadeLight() {
        if (GetComponent<SFLight>() == null)
        {
            Destroy(this);
            return;
        }

        intensity = GetComponent<SFLight>().intensity;


        fadeTime = Mathf.Abs(fadeTime);

        if (fadeTime > 0.0)
        {
            fadeSpeed = intensity / fadeTime;
        }

        else
        {
            fadeSpeed = intensity;
        }

        //alpha = 1.0;
    }
}
