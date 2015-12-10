using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    const float   BGMFadeTime = 60.0f;
    private float BGMTimer;
    private bool BGMDeleter;

    //タッチ判定のためのオブジェクト
    public GameObject TouchObject;
    public TouchController touchController;

	void Start ()
    {
        BGMTimer = 0.0f;
        BGMDeleter = false;
        CameraFade.StartAlphaFade(Color.black, true, 1.0f, 0.5f);
        Singleton<SoundPlayer>.instance.playBGM("bgm001", 1.0f);

        //タッチコントローラーから、スクリプトを取得
        //touchController = TouchObject.GetComponent<TouchController>();
    }
	
	void Update ()
    {
        Singleton<SoundPlayer>.instance.update();

        //何かキーが押されたら　or 画面がタッチされたら
        if ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.Joystick1Button3)) ||
            (Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Joystick1Button0)) ||
            (Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.Joystick1Button2)) ||
            (Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.Joystick1Button1)) ||
            (Input.GetKeyDown(KeyCode.Joystick1Button7))||
            ((touchController.touchPosX > 0) && (touchController.touchPosX < 1280)) &&
            ((touchController.touchPosY > 0) && (touchController.touchPosY < 720)) &&
            ((touchController.detachPosX > 0) && (touchController.detachPosX < 1280)) &&
            ((touchController.detachPosY > 0) && (touchController.detachPosY < 720)))

        {

            //BGMを一秒で
            Singleton<SoundPlayer>.instance.stopBGM(1.0f);
            BGMDeleter = true;

            //タッチ座標の初期化
            touchController.TouchPostionInitialize();
        }

        //もし、手を離したときに、なにも無ければ初期化
        if (touchController.detachPosX != 0 && touchController.detachPosY != 0)
        {
            touchController.TouchPostionInitialize();
        }

        //BGMが止まっていたら
        if (BGMTimer > BGMFadeTime)
        {
            //シーン移動
            BGMDeleter = false;
            BGMTimer = 0;
            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
            PlayerPrefs.SetInt("STAGE_SELECT_STAGE_NUM", 1);
            PlayerPrefs.SetInt("STAMP_NUM", 0);
            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });
            
        }

        if(BGMDeleter)
        {
            BGMTimer++;
        }
	}
}
