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
	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {
        // プレイヤーカメラの更新////////////////////////////////////////////////////////////
        this.transform.position = player.transform.position + offset;   // カメラを追従させる
        CameraRotation();                                               // カメラの回転
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
                        currentRotationY++;                     // 現在の角度を増やす
                        mapCamera.transform.Rotate(0, 0, -1);   // マップカメラを回転させる

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

                        currentRotationY--;                     // 現在の角度を減らす
                        mapCamera.transform.Rotate(0, 0, 1);    // マップカメラを回転させる
                    }
                    break;
            }
        }

        transform.eulerAngles = new Vector3(0, currentRotationY, 0);    // カメラの角度に現在の角度を設定

        // 目標の角度に到達したら
        if (currentRotationY == targetRotationY)
        {
            GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn++;
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
            mapCameraFlag = false;                              // マップカメラをＯＦＦに 
            mapCamera.GetComponent<Camera>().enabled = false;   // マップカメラを非表示
        }
        else
        {
            mapCameraFlag = true;                               // マップカメラをＯＮに
            mapCamera.GetComponent<Camera>().enabled = true;    // マップカメラを表示
        }
    }
}