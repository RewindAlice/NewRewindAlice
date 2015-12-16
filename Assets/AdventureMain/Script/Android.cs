using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Android : MonoBehaviour {

    public bool androidMenuFlag;
	// Use this for initialization
	void Start () {
        if (androidMenuFlag)
        {
            GameObject.Find("StageNumberButton").GetComponent<Image>().enabled = true;
            GameObject.Find("Left").GetComponent<Image>().enabled = true;
            GameObject.Find("Right").GetComponent<Image>().enabled = true;

        }
        else
        {
            GameObject.Find("StageNumberButton").GetComponent<Image>().enabled = false;
            GameObject.Find("Left").GetComponent<Image>().enabled = false;
            GameObject.Find("Right").GetComponent<Image>().enabled = false;

        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
