using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class KeyImage : MonoBehaviour {
    public Image image;

    public bool Android;
	// Use this for initialization
	void Start () {
        if (Android)
        {
            gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
       
	}
}
