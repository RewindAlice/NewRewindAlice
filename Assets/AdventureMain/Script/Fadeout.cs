using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fadeout : MonoBehaviour {

	public float fadeTime = 0.5f;

	private float currntRemainTime;
	private Image image;

	public GameObject controll;

	// Use this for initialization
	void Start () {
		currntRemainTime = fadeTime;
		image = gameObject.GetComponent<Image>();
        image.color = new Color(1.0f,1.0f,1.0f,0);
	
	}
	
	// Update is called once per frame
	void Update () {

		if (controll.GetComponent<TextController> ().stageNum == 53 &&controll.GetComponent<TextController> ().crickNum >= 25) {
			currntRemainTime -= Time.deltaTime;
			
			float alpha = currntRemainTime / fadeTime;
			var color = image.color;
			color.a = -alpha;
			image.color = color;
		}

	
	}
}
