using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        CameraFade.StartAlphaFade(Color.black, true, 1.0f, 0.5f);
	
    }
	
	// Update is called once per frame
	void Update ()
    {
        if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.Joystick1Button3)) ||
            (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Joystick1Button0)) ||
            (Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.Joystick1Button2)) ||
            (Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.Joystick1Button1)) ||
            (Input.GetKeyDown(KeyCode.Joystick1Button7)))
        {
            PlayerPrefs.SetInt("STAGE_SELECT_STAGE_NUM", 1);
            PlayerPrefs.SetInt("STAMP_NUM", 0);
            Application.LoadLevel("StageSelectScene");
        }
	}
}
