using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    // ★カメラの角度★//////////////////
    const int CAMERA_FRONT = 150;   // 前
    const int CAMERA_BACK = 330;    // 後
    const int CAMERA_LEFT = 60;     // 左
    const int CAMERA_RIGHT = 240;   // 右

    // ★アリスに対してのアングル★
    public enum CameraAngle
    {
        FRONT,  // 前
        BACK,   // 後
        LEFT,   // 左
        RIGHT,  // 右
    }

    // ★回転方向★//////////
    public enum TurnDirection
    {
        NONE,   // 無し
        LEFT,   // 左方向
        RIGHT,  // 右方向
    }

    // ★マップカメラ★////////////////////////////////////////
    public Camera mapCamera;        // マップカメラ
    public bool mapCameraFlag;      // マップカメラの表示フラグ

    // ★プレイヤーカメラ★//////////////////////////////////////
    public Player player;           // 追従対象
    public CameraAngle cameraAngle; // 追従対象に対してのアングル
    public int currentRotationY;    // 現在の角度
    public int targetRotationY;     // 目的の角度
    public int inputKeyRotationY;   // キー入力時の角度
    public bool rotationFlag;       // 回転フラグ
    Vector3 offset;                 // カメラと対象の距離
    TurnDirection turnDirection;    // カメラの回転方向

    //リザルトの追加部分
    public bool clearFlag;//ゲームクリアしたら
    public bool onceInputFlag;//クリア時に一回だけ通すフラグ
    public bool endRolling;//回転終了フラグ
    public int  clearY;//リザルト開始時点の座標のY軸
    public int  clearTY;//クリアした後に実際にカメラを動かす軸
    public int  timer;  //時間制御  
    public float cameraXZ;
    public float cameraY;
    public float cameraRX;
    public GameObject camera;

    public int resultCounter;
	public GameObject pause; // ポーズ
 

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        // マップカメラの初期化////////////////////////////////////////
        mapCameraFlag = true;   // マップカメラの初期表示フラグをＯＮに

        // プレイヤーカメラの初期化////////////////////////////////////////////////////////////////////
        cameraAngle = CameraAngle.BACK;                                 // 初期アングルを後に
        currentRotationY = CAMERA_BACK;                                 // 初期カメラの角度を後に設定
        rotationFlag = false;                                           // カメラの回転フラグを偽に設定
        turnDirection = TurnDirection.NONE;                             // カメラの回転方向を無しに設定
        offset = this.transform.position - player.transform.position;   // カメラと対象の距離を設定
        targetRotationY = 0;                                            // 目的の角度を０に
        inputKeyRotationY = 0;                                          // キー入力時の角度を０に
        //リザルトでの追加
        camera = GameObject.Find("Main Camera");
        clearFlag = false;
        onceInputFlag = false;
        endRolling = false;
        cameraXZ = -4;
        cameraY = 2;
        timer = 0;
        cameraRX = 15;
        clearY = 0;
        clearTY = 0;
        resultCounter = 0;
		pause = GameObject.Find("Pause");
    }

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {
		if (pause.GetComponent<Pause>().pauseFlag == false)
		{
			// プレイヤーカメラの更新////////////////////////////////////////////////////////////
			this.transform.position = player.transform.position + offset;   // カメラを追従させる
			CameraRotation();                                               // カメラの回転
			//ゲームクリアしたら
			if (clearFlag)
			{
				ResultCameraMove();
                Singleton<SoundPlayer>.instance.stopBGM(1.0f);

                if(endRolling == true)
                {
                    resultCounter++;

                    if(resultCounter >150)
                    {
                        rotationFlag = false;
                        endRolling = false;
                        resultCounter = 0;

                        //クリアフラグの設定
                        switch (PlayerPrefs.GetInt("STAGE_NUM"))
                        {
                            case 1: PlayerPrefs.SetInt("Stage1_1Clear", 1); break;
                            case 2: PlayerPrefs.SetInt("Stage1_2Clear", 1); break;
                            case 3: PlayerPrefs.SetInt("Stage1_3Clear", 1); break;
                            case 4: PlayerPrefs.SetInt("Stage1_4Clear", 1); break;
                            case 5: PlayerPrefs.SetInt("Stage1_5Clear", 1); break;
                            case 6: PlayerPrefs.SetInt("Stage2_1Clear", 1); break;
                            case 7: PlayerPrefs.SetInt("Stage2_2Clear", 1); break;
                            case 8: PlayerPrefs.SetInt("Stage2_3Clear", 1); break;
                            case 9: PlayerPrefs.SetInt("Stage2_4Clear", 1); break;
                            case 10: PlayerPrefs.SetInt("Stage2_5Clear", 1); break;
                            case 11: PlayerPrefs.SetInt("Stage3_1Clear", 1); break;
                            case 12: PlayerPrefs.SetInt("Stage3_2Clear", 1); break;
                            case 13: PlayerPrefs.SetInt("Stage3_3Clear", 1); break;
                            case 14: PlayerPrefs.SetInt("Stage3_4Clear", 1); break;
                            case 15: PlayerPrefs.SetInt("Stage3_5Clear", 1); break;
                            case 16: PlayerPrefs.SetInt("Stage4_1Clear", 1); break;
                            case 17: PlayerPrefs.SetInt("Stage4_2Clear", 1); break;
                            case 18: PlayerPrefs.SetInt("Stage4_3Clear", 1); break;
                            case 19: PlayerPrefs.SetInt("Stage4_4Clear", 1); break;
                            case 20: PlayerPrefs.SetInt("Stage4_5Clear", 1); break;
                            case 21: PlayerPrefs.SetInt("Stage5_1Clear", 1); break;
                            case 22: PlayerPrefs.SetInt("Stage5_2Clear", 1); break;
                            case 23: PlayerPrefs.SetInt("Stage5_3Clear", 1); break;
                            case 24: PlayerPrefs.SetInt("Stage5_4Clear", 1); break;


                            case 25: PlayerPrefs.SetInt("EXStage1Clear", 1); break;
                            case 26: PlayerPrefs.SetInt("EXStage2Clear", 1); break;
                            case 27: PlayerPrefs.SetInt("EXStage3Clear", 1); break;
                            case 28: PlayerPrefs.SetInt("EXStage4Clear", 1); break;
                            case 29: PlayerPrefs.SetInt("EXStage5Clear", 1); break;
                            case 30: PlayerPrefs.SetInt("EXStage6Clear", 1); break;
                            case 31: PlayerPrefs.SetInt("EXStage7Clear", 1); break;
                            case 32: PlayerPrefs.SetInt("EXStage8Clear", 1); break;
                            case 33: PlayerPrefs.SetInt("EXStage9Clear", 1); break;
                            case 34: PlayerPrefs.SetInt("EXStage10Clear", 1); break;
                            case 35: PlayerPrefs.SetInt("EXStage11Clear", 1); break;
                            case 36: PlayerPrefs.SetInt("EXStage12Clear", 1); break;
                            case 37: PlayerPrefs.SetInt("EXStage13Clear", 1); break;
                            case 38: PlayerPrefs.SetInt("EXStage14Clear", 1); break;
                            case 39: PlayerPrefs.SetInt("EXStage15Clear", 1); break;
                            case 40: PlayerPrefs.SetInt("EXStage16Clear", 1); break;
                            case 41: PlayerPrefs.SetInt("EXStage17Clear", 1); break;
                            case 42: PlayerPrefs.SetInt("EXStage18Clear", 1); break;
                            case 43: PlayerPrefs.SetInt("EXStage19Clear", 1); break;
                            case 44: PlayerPrefs.SetInt("EXStage20Clear", 1); break;
                            case 45: PlayerPrefs.SetInt("EXStage21Clear", 1); break;
                                                                        
                                                                        
                        }                                               
                        Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                        CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });
                    }                                                   
                }                                                       
			}                                                           
		}                                                               
    }

    // ★カメラの回転★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void CameraRotation()
    {
        // 回転フラグが真なら
        if (rotationFlag == true)
        {
            // 回転方向が
            switch (turnDirection)
            {
                // 無しなら
                case TurnDirection.NONE:
                    break;

                // 左方向なら
                case TurnDirection.LEFT:
                    RotationLeft();     // カメラの回転

                    // ▽現在のカメラの向きが
                    switch (cameraAngle)
                    {
                        // ▼前なら////////////////////////////////////////////////////////////////////////////
                        case CameraAngle.FRONT: targetRotationY = CAMERA_FRONT; break;  // 目的の角度に前を設定
                        // ▼後なら////////////////////////////////////////////////////////////////////////////
                        case CameraAngle.BACK: targetRotationY = CAMERA_BACK; break;    // 目的の角度に後を設定
                        // ▼左なら////////////////////////////////////////////////////////////////////////////
                        case CameraAngle.LEFT: targetRotationY = CAMERA_LEFT; break;    // 目的の角度に左を設定
                        // ▼右なら////////////////////////////////////////////////////////////////////////////
                        case CameraAngle.RIGHT: targetRotationY = CAMERA_RIGHT; break;  // 目的の角度に右を設定
                    }

                    // 現在のカメラの角度が目的の角度と異なっていたら
                    if (currentRotationY != targetRotationY)
                    {
                        currentRotationY+=2;                     // 現在の角度を増やす
                        mapCamera.transform.Rotate(0, 0, -2);   // マップカメラを回転させる

                        // 現在の角度が３６０度になったら角度を０に変える
                        if (currentRotationY == 360){ currentRotationY = 0; }
                    }
                    break;

                // 右方向なら
                case TurnDirection.RIGHT:
                    RotationRight();    // カメラの回転

                    // ▽現在のカメラの向きが
                    switch (cameraAngle)
                    {
                        // ▼前なら////////////////////////////////////////////////////////////////////////////
                        case CameraAngle.FRONT: targetRotationY = CAMERA_FRONT; break;  // 目的の角度に前を設定
                        // ▼後なら////////////////////////////////////////////////////////////////////////////
                        case CameraAngle.BACK: targetRotationY = CAMERA_BACK; break;    // 目的の角度に後を設定
                        // ▼左なら////////////////////////////////////////////////////////////////////////////
                        case CameraAngle.LEFT: targetRotationY = CAMERA_LEFT; break;    // 目的の角度に左を設定
                        // ▼右なら////////////////////////////////////////////////////////////////////////////
                        case CameraAngle.RIGHT: targetRotationY = CAMERA_RIGHT; break;  // 目的の角度に右を設定
                    }

                    // 現在の角度が目的の角度と異なっていたら
                    if (currentRotationY != targetRotationY)
                    {
                        // 現在の角度が０度になったら角度を３６０に変える
                        if (currentRotationY == 0){ currentRotationY = 360; }

                        currentRotationY-=2;                     // 現在の角度を減らす
                        mapCamera.transform.Rotate(0, 0, 2);    // マップカメラを回転させる
                    }
                    break;
            }
        }

        transform.eulerAngles = new Vector3(0, currentRotationY, 0);    // カメラの角度に現在の角度を設定

        // 目標の角度に到達したら
        if (currentRotationY == targetRotationY)
        {
            GameObject.Find("GameMain").GetComponent<GameMain>().waitingTime = 0;
            GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn++;
            if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 2 &&GameObject.Find("GameMain").GetComponent<GameMain>().tutorialFlag == true)
            {
                GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(3);
            }
            targetRotationY = 0;
            rotationFlag = false;

        }
    }

    // ★左回転時の方向設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void RotationLeft()
    {
        // ▽キー入力時の角度が
        switch (inputKeyRotationY)
        {
            // ▼前なら////////////////////////////////////////////////////////////
            case CAMERA_FRONT: cameraAngle = CameraAngle.RIGHT; break;  // 右を設定
            // ▼後なら////////////////////////////////////////////////////////////
            case CAMERA_BACK: cameraAngle = CameraAngle.LEFT; break;    // 左を設定
            // ▼左なら////////////////////////////////////////////////////////////
            case CAMERA_LEFT: cameraAngle = CameraAngle.FRONT; break;   // 前を設定
            // ▼右なら////////////////////////////////////////////////////////////
            case CAMERA_RIGHT: cameraAngle = CameraAngle.BACK; break;   // 後を設定
        }
    }

    // ★右回転時の方向設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void RotationRight()
    {
        // ▽キー入力時の角度が
        switch (inputKeyRotationY)
        {
            // ▼前なら////////////////////////////////////////////////////////////
            case CAMERA_FRONT: cameraAngle = CameraAngle.LEFT; break;   // 左を設定
            // ▼後なら////////////////////////////////////////////////////////////
            case CAMERA_BACK: cameraAngle = CameraAngle.RIGHT; break;   // 右を設定
            // ▼左なら////////////////////////////////////////////////////////////
            case CAMERA_LEFT: cameraAngle = CameraAngle.BACK; break;    // 後を設定
            // ▼右なら////////////////////////////////////////////////////////////
            case CAMERA_RIGHT: cameraAngle = CameraAngle.FRONT; break;  // 前を設定
        }
    }

    // ★左回転★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void TurnLeft()
    {
        // 回転フラグが偽なら
        if(rotationFlag == false)
        {
            rotationFlag = true;                                // 回転フラグを真に
            turnDirection = TurnDirection.LEFT;                 // 回転方向を左に
            inputKeyRotationY = (int)transform.eulerAngles.y;   // キー入力時の角度に現在の角度を入れる
        }
    }

    // ★右回転★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void TurnRight()
    {
        // 回転フラグが偽なら
        if (rotationFlag == false)
        {
            rotationFlag = true;                                // 回転フラグを真に
            turnDirection = TurnDirection.RIGHT;                // 回転方向を右に
            inputKeyRotationY = (int)transform.eulerAngles.y;   // キー入力時の角度に現在の角度を入れる
        }
    }

    // ★マップカメラの切り替え★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void SwitchingMapCamera()
    {
        // マップカメラがＯＮなら
        if(mapCameraFlag)
        {
            mapCameraFlag = true;                              // マップカメラをＯＦＦに 
            mapCamera.GetComponent<Camera>().enabled = true;   // マップカメラを非表示
        }
        else
        {
            mapCameraFlag = false;                               // マップカメラをＯＮに
            mapCamera.GetComponent<Camera>().enabled = false;    // マップカメラを表示
        }
    }

    public void ResultCameraMove()
    {
            if (onceInputFlag == false)
            {
                clearY = (int)transform.eulerAngles.y;
                clearTY = clearY;
                onceInputFlag = true;
            }
            //アリスの向きを取得
            int direction = player.GetDirection();
            
           //アリスの向きによって回転させる
            switch (direction)
            {
                case 1:
                    if (clearY < 136 || clearY > 314)
                    {
                        if (clearTY != 135)
                        {
                            clearTY += 2;
                        }

                        if (clearTY == 360)
                        {
                            clearTY = 1;
                        }
                    }
                    else
                    {
                        if (clearTY != 135)
                        {
                            clearTY -= 2;
                        }

                        if (clearTY == 0)
                        {
                            clearTY = 359;
                        }
                    }
                    if (clearTY == 136) { clearTY = 135; }
                    if(clearTY == 135)
                    {
                        endRolling = true;
                    }


                    break;

                case 2:

                    if (clearY < 45 || clearY > 224)
                    {
                        if (clearTY != 225)
                        {
                            clearTY -= 2;
                        }

                        if (clearTY == 0)
                        {
                            clearTY = 359;
                        }
                    }
                    else
                    {
                        if (clearTY != 225)
                        {
                            clearTY += 2;
                        }

                        if (clearTY == 360)
                        {
                            clearTY = 1;
                        }
                    }
                    if (clearTY == 226) { clearTY = 225; }
                    if (clearTY == 225)
                    {
                        endRolling = true;
                    }
                    break;

                case 3:
                    if (clearY < 136 || clearY > 314)
                    {
                        if (clearTY != 315)
                        {
                            clearTY -= 2;
                        }

                        if (clearTY == 0)
                        {
                            clearTY = 359;
                        }
                    }
                    else
                    {
                        if (clearTY != 315)
                        {
                            clearTY += 2;
                        }

                        if (clearTY == 360)
                        {
                            clearTY = 1;
                        }
                    }
                    if (clearTY == 316) { clearTY = 315; }
                    if (clearTY == 315)
                    {
                        endRolling = true;
                    }
                    break;
                case 4:
                    if (clearY < 45 || clearY > 224)
                    {
                        if (clearTY != 45)
                        {
                            clearTY += 2;
                        }

                        if (clearTY == 360)
                        {
                            clearTY = 1;
                        }
                    }
                    else
                    {
                        if (clearTY != 45)
                        {
                            clearTY -= 2;
                        }

                        if (clearTY == 0)
                        {
                            clearTY = 359;
                        }
                    }
                    if (clearTY == 46) { clearTY = 45; }
                    if (clearTY == 45)
                    {
                       endRolling = true;
                    }
                    break;
            }
          
            //カメラを近づける
            if(timer < 60)
            {
                cameraXZ += 0.05f;

                cameraRX -= 0.25f;

                if(timer<50)
                {
                    cameraY -= 0.025f;
                }
                else
                {
                    cameraY -= 0.035f;
                }
                timer++;
                
            }
            
            transform.eulerAngles = new Vector3(0, clearTY, 0);    // カメラの角度に現在の角度を設定

            //ズーム
            camera.transform.localPosition = new Vector3(cameraXZ, cameraY, cameraXZ);     // 座標を変更
            camera.transform.localEulerAngles = new Vector3(cameraRX, 45.0f, 0.0f);
        
    }
    //リザルト用のカメラ移動が終わった時を送る
    public bool EndCameraMove()
    {
        if (endRolling)
        {
            return true;
        }
        return false;
    }

}