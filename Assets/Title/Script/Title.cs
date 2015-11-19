using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    const float   BGMFadeTime = 60.0f;
    private float BGMTimer;
    private bool BGMDeleter; 
	// Use this for initialization
	void Start ()
    {
        BGMTimer = 0.0f;
        BGMDeleter = false;
        CameraFade.StartAlphaFade(Color.black, true, 1.0f, 0.5f);
        Singleton<SoundPlayer>.instance.playBGM("bgm001", 1.0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Singleton<SoundPlayer>.instance.update();
        if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.Joystick1Button3)) ||
            (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Joystick1Button0)) ||
            (Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.Joystick1Button2)) ||
            (Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.Joystick1Button1)) ||
            (Input.GetKeyDown(KeyCode.Joystick1Button7)))
        {
            Singleton<SoundPlayer>.instance.stopBGM(1.0f);
             BGMDeleter = true;
            
        }

        if (BGMTimer > BGMFadeTime)
        {
            BGMDeleter = false;
            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
            PlayerPrefs.SetInt("STAGE_SELECT_STAGE_NUM", 1);
            PlayerPrefs.SetInt("STAMP_NUM", 0);
            Application.LoadLevel("StageSelectScene");
        }
        if(BGMDeleter)
        {
            BGMTimer++;
        }
	}
}
