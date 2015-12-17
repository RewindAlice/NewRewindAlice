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


    public bool touchFlag;

    public GameObject Effect01;


	// Use this for initialization
	void Start () {
        
        //デザイン解像度の変更(1280*720)
        Screen.SetResolution(width, height, false);
        touchFlag = false;
	}
	
	// Update is called once per frame
	void Update () {

        //タッチされている数が0以上ならば
        if (Input.touchCount > 0)
        {
            //最初のタッチの取得
            Touch touch = Input.GetTouch(0);

            //タッチが始まったら
            if (touch.phase == TouchPhase.Began)
            {
                //タッチの座標を保存
                touchPosX = (int)touch.position.x;
                touchPosY = (int)touch.position.y;

                // タッチした画面座標からワールド座標へ変換
                Vector3 pos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 5.0f));
                // 指定したエフェクトを作成
                GameObject go = (GameObject)Instantiate (Effect01, pos, Quaternion.identity);

                // エフェクトを消す
                Destroy(go, 0.4f);
                
            }
            //タッチ中に移動したら
            else if (touch.phase == TouchPhase.Moved)
            {
            }
            //画面から指が離れたら
            else if (touch.phase == TouchPhase.Ended)
            {
                detachPosX = (int)touch.position.x;
                detachPosY = (int)touch.position.y;
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
