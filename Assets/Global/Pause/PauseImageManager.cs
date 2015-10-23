using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseImageManager : MonoBehaviour {


    public int i;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
