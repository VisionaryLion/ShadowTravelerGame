using UnityEngine;
using System.Collections;

public class FloatEffect : MonoBehaviour {

	private float startY = 0f;
	private float duration = 1f;
	private RectTransform rectTransform;

	// Use this for initialization
	void Start () {
		rectTransform = GetComponent<RectTransform> ();
		startY = rectTransform.anchoredPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
		var newY = startY + (startY + Mathf.Cos (Time.time / duration * 2)) / .1f;
		rectTransform.anchoredPosition = new Vector2 (rectTransform.anchoredPosition.x, newY);
	}
}
