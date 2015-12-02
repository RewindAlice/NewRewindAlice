using UnityEngine;
using System.Collections;

//タッチ座標を保持するクラス
public class TouchController : MonoBehaviour {


    public int touchPosX;   //タッチ時のX座標
    public int touchPosY;   //タッチ時のY座標
    public int detachPosX;  //タッチを離したときのX座標
    public int detachPosY;  //タッチを離したときのY座標
    int width  = 1280;      //画面比(横)
    int height = 720;       //画面比(縦)

	// Use this for initialization
	void Start () {
        Screen.SetResolution(width, height, false);
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // タッチ開始
                touchPosX = (int)touch.position.x;
                touchPosY = (int)touch.position.y;

                Debug.Log("touchPosX" + touchPosX);
                Debug.Log("touchPosY" + touchPosY);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // タッチ移動
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // タッチ終了
                detachPosX = (int)touch.position.x;
                detachPosY = (int)touch.position.y;
                Debug.Log("detachPosX" + detachPosX);
                Debug.Log("detachPosY" + detachPosY);
            }
        }
	}


    //タッチ座標の初期化処理
    public void TouchPostionInitialize()
    {
        touchPosX = 0;
        touchPosY = 0;
        detachPosX = 0;
        detachPosY = 0;
    }
}
