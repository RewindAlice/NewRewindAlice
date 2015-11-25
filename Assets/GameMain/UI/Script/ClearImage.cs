using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClearImage : MonoBehaviour {

    GameObject playercamera;
    PlayerCamera camera;
	// Use this for initialization
	void Start () {
        GetComponent<Image>().enabled = false;
        playercamera = GameObject.Find("Camera");
        camera = playercamera.GetComponent<PlayerCamera>();
	}
    
	
	// Update is called once per frame
	void Update () {
        if (camera.clearFlag)
        {
            GetComponent<Image>().enabled = true;
        }
	}
}
