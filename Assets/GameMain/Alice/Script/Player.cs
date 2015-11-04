using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // listを使用するため

public class Player : MonoBehaviour
{
    const int SAVE_NUM = 50;        // 移動保存数
    const float SPEED_W = 0.02f;    // 移動速度（横方向）
    const float SPEED_H = 0.02f;    // 移動速度（縦方向）

    const int ANGLE_FRONT = 0;   // プレイヤーの向き（前）
    const int ANGLE_BACK = 180;  // プレイヤーの向き（後）
    const int ANGLE_LEFT = 270;  // プレイヤーの向き（左）
    const int ANGLE_RIGHT = 90;  // プレイヤーの向き（右）

    // ★アリスの行動★
    public enum PlayerAction
    {
        NONE,       // 無し
        NEXT,       // 進む
        RETURN,     // 戻る
    }

    // ★プレイヤーの向き★
    public enum PlayerAngle
    {
        NONE,
        FRONT,  // 前
        BACK,   // 後
        LEFT,   // 左
        RIGHT,  // 右
    }

    // ★移動方向★
    public enum MoveDirection
    {
        NONE,
        FRONT,  // 前
        BACK,   // 後
        LEFT,   // 左
        RIGHT,  // 右
        UP,     // 上
        DOWN,   // 下
        STOP,   // 止
    }

    // ★配列操作★
    public enum ArrayMove
    {
        NONE,
        PLUS_X,     // Xを増やす
        PLUS_Y,     // Yを増やす
        PLUS_Z,     // Zを増やす
        MINUS_X,    // Xを減らす
        MINUS_Y,    // Yを減らす
        MINUS_Z,    // Zを減らす
    }

    public int arrayPosX = 0;   // 配列上の座標X
    public int arrayPosY = 0;   // 配列上の座標Y
    public int arrayPosZ = 0;   // 配列上の座標Z

    public bool moveFlag;                           // 移動フラグ
    public bool moveFinishFlag;                     // 移動完了フラグ
    public bool moveReturnFlag;                     // 巻き戻しフラグ
    public bool moveNextFlag;                       // 早送りフラグ
    public bool inputKeyFlag;                       // キー入力フラグ
    public Vector3 moveBeforePosition;              // ボタン入力時の座標
    public PlayerCamera camera;                     // カメラ
    public PlayerCamera.CameraAngle cameraAngle;    // カメラの向き

    public PlayerAngle playerAngle;     // アリスの向き
    public PlayerAction playerAction;   // アリスの行動
    public MoveDirection moveDirection; // 移動方向

    public int stopCount;               // 待機時のカウント

    // 移動情報の保存
    public PlayerAngle[] saveMovePlayerAngle = new PlayerAngle[SAVE_NUM];   // 保存用配列（アリスの向き）
    public MoveDirection[] saveMoveDirection = new MoveDirection[SAVE_NUM]; // 保存用配列（移動方向）
    public bool[] saveMoveInput = new bool[SAVE_NUM];                       // 保存用配列（キー入力）
    public int saveCount;                                                   // 現在の保存数
    public int moveCount;                                                   // 移動数
    public int turnCount;                                                   // ターン数

    // 移動可能フラグ
    public bool moveFrontPossibleFlag;  // 前移動可能フラグ
    public bool moveBackPossibleFlag;   // 後移動可能フラグ
    public bool moveLeftPossibleFlag;   // 左移動可能フラグ
    public bool moveRightPossibleFlag;  // 右移動可能フラグ

    public bool autoMoveFlag;           // 自動移動フラグ
    public MoveDirection autoMove;      // 自動移動の方向

    public bool climb1Flag; // 登り１フラグ
    public bool climb2Flag; // 登り２フラグ

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        moveFlag = false;
        moveFinishFlag = false;
        moveReturnFlag = false;
        moveNextFlag = false;
        inputKeyFlag = false;
        stopCount = 0;

        playerAngle = PlayerAngle.FRONT;    // アリスの初期の向きを前に

        autoMoveFlag = false;
        autoMove = MoveDirection.NONE;

        climb1Flag = false;
        climb2Flag = false;
	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {
        cameraAngle = camera.cameraAngle;   // カメラの向きを取得
        Move();                             // 移動
	}

    // ★カメラに対応した移動方向に変更★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void ChangeDirection(MoveDirection direction)
    {
        switch (direction)
        {
            // 前なら
            case MoveDirection.FRONT:
                switch (cameraAngle)
                {
                    case PlayerCamera.CameraAngle.FRONT: moveDirection = MoveDirection.BACK; break;
                    case PlayerCamera.CameraAngle.BACK: moveDirection = MoveDirection.FRONT; break;
                    case PlayerCamera.CameraAngle.LEFT: moveDirection = MoveDirection.RIGHT; break;
                    case PlayerCamera.CameraAngle.RIGHT: moveDirection = MoveDirection.LEFT; break;
                }
                break;
            // 後なら
            case MoveDirection.BACK:
                switch (cameraAngle)
                {
                    case PlayerCamera.CameraAngle.FRONT: moveDirection = MoveDirection.FRONT; break;
                    case PlayerCamera.CameraAngle.BACK: moveDirection = MoveDirection.BACK; break;
                    case PlayerCamera.CameraAngle.LEFT: moveDirection = MoveDirection.LEFT; break;
                    case PlayerCamera.CameraAngle.RIGHT: moveDirection = MoveDirection.RIGHT; break;
                }
                break;
            // 左なら
            case MoveDirection.LEFT:
                switch (cameraAngle)
                {
                    case PlayerCamera.CameraAngle.FRONT: moveDirection = MoveDirection.RIGHT; break;
                    case PlayerCamera.CameraAngle.BACK: moveDirection = MoveDirection.LEFT; break;
                    case PlayerCamera.CameraAngle.LEFT: moveDirection = MoveDirection.FRONT; break;
                    case PlayerCamera.CameraAngle.RIGHT: moveDirection = MoveDirection.BACK; break;
                }
                break;
            // 右なら
            case MoveDirection.RIGHT:
                switch (cameraAngle)
                {
                    case PlayerCamera.CameraAngle.FRONT: moveDirection = MoveDirection.LEFT; break;
                    case PlayerCamera.CameraAngle.BACK: moveDirection = MoveDirection.RIGHT; break;
                    case PlayerCamera.CameraAngle.LEFT: moveDirection = MoveDirection.BACK; break;
                    case PlayerCamera.CameraAngle.RIGHT: moveDirection = MoveDirection.FRONT; break;
                }
                break;
            // 上なら
            case MoveDirection.UP:
                moveDirection = MoveDirection.UP;
                break;
            // 下なら
            case MoveDirection.DOWN:
                moveDirection = MoveDirection.DOWN;
                break;
            // 待機なら
            case MoveDirection.STOP:
                moveDirection = MoveDirection.STOP;
                break;
        }
    }

    // ★向きを変更★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void ChangeAngle()
    {
        // 移動方向が
        switch (moveDirection)
        {
            // 前なら
            case MoveDirection.FRONT:
                playerAngle = PlayerAngle.FRONT;                                // アリスの向きを前に
                transform.localEulerAngles = new Vector3(0, ANGLE_FRONT, 0);    // 前方向の角度を指定
                break;
            // 後なら
            case MoveDirection.BACK:
                playerAngle = PlayerAngle.BACK;                                 // アリスの向きを後に
                transform.localEulerAngles = new Vector3(0, ANGLE_BACK, 0);     // 後方向の角度を指定
                break;
            // 左なら
            case MoveDirection.LEFT:
                playerAngle = PlayerAngle.LEFT;                                 // アリスの向きを左に
                transform.localEulerAngles = new Vector3(0, ANGLE_LEFT, 0);     // 左方向の角度を指定
                break;
            // 右なら
            case MoveDirection.RIGHT:
                playerAngle = PlayerAngle.RIGHT;                                // アリスの向きを右に
                transform.localEulerAngles = new Vector3(0, ANGLE_RIGHT, 0);    // 右方向の角度を指定
                break;
            // それ以外
            default:
                break;
        }
    }

    // ★移動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void Move()
    {
        // アリスの行動が
        switch(playerAction)
        {
            // 何も無しなら
            case PlayerAction.NONE:
                break;
            // 進めるなら
            case PlayerAction.NEXT:
                MoveNextPosition();                 // アリスを進める処理
                MoveFinishDecision(playerAction);   // 移動完了判定
                break;
            // 戻るなら
            case PlayerAction.RETURN:
                MoveReturnPosition();               // アリスを戻す処理
                MoveFinishDecision(playerAction);   // 移動完了判定
                break;
        }
    }

    // ★プレイヤーを進める★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveNextPosition()
    {
        // 移動方向が
        switch(moveDirection)
        {
            // 移動なら
            case MoveDirection.FRONT:
            case MoveDirection.BACK:
            case MoveDirection.LEFT:
            case MoveDirection.RIGHT:
                transform.Translate(Vector3.forward * SPEED_W);
                break;
            // 上移動なら
            case MoveDirection.UP:
                transform.Translate(Vector3.up * SPEED_H);
                break;
            // 下移動なら
            case MoveDirection.DOWN:
                transform.Translate(Vector3.down * SPEED_H);
                break;
            // 待機なら
            case MoveDirection.STOP:
                stopCount++;
                break;
        }
    }

    // ★プレイヤーを戻す★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveReturnPosition()
    {
        // 移動方向が
        switch (moveDirection)
        {
            // 移動なら
            case MoveDirection.FRONT:
            case MoveDirection.BACK:
            case MoveDirection.LEFT:
            case MoveDirection.RIGHT:
                transform.Translate(Vector3.back * SPEED_W);
                break;
            // 上移動なら
            case MoveDirection.UP:
                transform.Translate(Vector3.up * SPEED_H);
                break;
            // 下移動なら
            case MoveDirection.DOWN:
                transform.Translate(Vector3.down * SPEED_H);
                break;
            // 待機なら
            case MoveDirection.STOP:
                stopCount++;
                break;
        }
    }

    // ★移動開始処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveStart()
    {
        // 巻き戻しフラグが偽なら
        if(!moveReturnFlag)
        {
            // 早送りフラグが偽なら
            if(!moveNextFlag)
            {
                // 通常の移動開始処理//////////////////////////////////////////////////////////////
                moveFlag = true;                            // 移動フラグを真に
                moveBeforePosition = transform.position;    // 移動前の座標に現在の座標を入れる
                ChangeDirection(moveDirection);             // カメラの向きに対応した移動方向に変更
                ChangeAngle();                              // アリスの向きを変更
                playerAction = PlayerAction.NEXT;           // アリスの行動を進めるに
            }
            else
            {
                // 早送りの移動開始処理////////////////////////////////////////////////////////////////
                moveFlag = true;                                    // 移動フラグを真に
                moveBeforePosition = transform.position;            // 移動前の座標に現在の座標を入れる
                playerAngle = saveMovePlayerAngle[saveCount];       // 保存されている向きを設定
                moveDirection = saveMoveDirection[saveCount];       // 保存されている移動方向を設定
                ChangeAngle();                                      // アリスの向きを変更
                playerAction = PlayerAction.NEXT;                   // アリスの行動を進めるに
            }
        }
        else
        {
            // 保存数が０より大きい（保存されている移動があれば）
            if(saveCount > 0)
            {
                // 巻き戻しの移動開始処理//////////////////////////////////////////////////////////////
                moveFlag = true;                                    // 移動フラグを真に
                moveBeforePosition = transform.position;            // 移動前の座標に現在の座標を入れる
                playerAngle = saveMovePlayerAngle[saveCount - 1];   // １つ前の向きを設定
                moveDirection = saveMoveDirection[saveCount - 1];   // １つ前の移動方向を設定
                ChangeAngle();                                      // アリスの向きを変更
                playerAction = PlayerAction.RETURN;                 // アリスの行動を戻るに

                // 移動方向が上下の場合は反転する
                switch(moveDirection)
                {
                    case MoveDirection.UP: moveDirection = MoveDirection.DOWN; break;
                    case MoveDirection.DOWN: moveDirection = MoveDirection.UP; break;
                }

                saveCount--;
                print("保存場所を前に");
            }
        }
    }

    // ★移動完了判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveFinishDecision(PlayerAction action)
    {
        // 行動が
        switch(action)
        {
            // 何も無しなら
            case PlayerAction.NONE:
                break;
            // 進むなら
            case PlayerAction.NEXT:
                // 移動方向が
                switch(moveDirection)
                {
                    // 前なら
                    case MoveDirection.FRONT:
                        if(transform.localPosition.z >= moveBeforePosition.z + 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, moveBeforePosition.z + 1);
                            MoveFinish(position, ArrayMove.PLUS_Z);
                        }
                        break;
                    // 後なら
                    case MoveDirection.BACK:
                        if (transform.localPosition.z <= moveBeforePosition.z - 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, moveBeforePosition.z - 1);
                            MoveFinish(position, ArrayMove.MINUS_Z);
                        }
                        break;
                    // 左なら
                    case MoveDirection.LEFT:
                        if (transform.localPosition.x <= moveBeforePosition.x - 1)
                        {
                            Vector3 position = new Vector3(moveBeforePosition.x - 1, transform.localPosition.y, transform.localPosition.z);
                            MoveFinish(position, ArrayMove.MINUS_X);
                        }
                        break;
                    // 右なら
                    case MoveDirection.RIGHT:
                        if (transform.localPosition.x >= moveBeforePosition.x + 1)
                        {
                            Vector3 position = new Vector3(moveBeforePosition.x + 1, transform.localPosition.y, transform.localPosition.z);
                            MoveFinish(position, ArrayMove.PLUS_X);
                        }
                        break;
                    // 上なら
                    case MoveDirection.UP:
                        if (transform.localPosition.y >= moveBeforePosition.y + 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y + 1, transform.localPosition.z);
                            MoveFinish(position, ArrayMove.PLUS_Y);
                        }
                        break;
                    // 下なら
                    case MoveDirection.DOWN:
                        if (transform.localPosition.y <= moveBeforePosition.y - 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y - 1, transform.localPosition.z);
                            MoveFinish(position, ArrayMove.MINUS_Y);
                        }
                        break;
                    // 待機なら
                    case MoveDirection.STOP:
                        if(stopCount == 50)
                        {
                            MoveFinish(transform.localPosition, ArrayMove.NONE);
                        }
                        break;
                }
                break;
            // 戻るなら
            case PlayerAction.RETURN:
                // 移動方向が
                switch (moveDirection)
                {
                    // 前なら
                    case MoveDirection.FRONT:
                        if (transform.localPosition.z <= moveBeforePosition.z - 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, moveBeforePosition.z - 1);
                            MoveFinish(position, ArrayMove.MINUS_Z);
                            MoveAgain();
                        }
                        break;
                    // 後なら
                    case MoveDirection.BACK:
                        if (transform.localPosition.z >= moveBeforePosition.z + 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, moveBeforePosition.z + 1);
                            MoveFinish(position, ArrayMove.PLUS_Z);
                            MoveAgain();
                        }
                        break;
                    // 左なら
                    case MoveDirection.LEFT:
                        if (transform.localPosition.x >= moveBeforePosition.x + 1)
                        {
                            Vector3 position = new Vector3(moveBeforePosition.x + 1, transform.localPosition.y, transform.localPosition.z);
                            MoveFinish(position, ArrayMove.PLUS_X);
                            MoveAgain();
                        }
                        break;
                    // 右なら
                    case MoveDirection.RIGHT:
                        if (transform.localPosition.x <= moveBeforePosition.x - 1)
                        {
                            Vector3 position = new Vector3(moveBeforePosition.x - 1, transform.localPosition.y, transform.localPosition.z);
                            MoveFinish(position, ArrayMove.MINUS_X);
                            MoveAgain();
                        }
                        break;
                    // 上なら
                    case MoveDirection.UP:
                        if (transform.localPosition.y >= moveBeforePosition.y + 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y + 1, transform.localPosition.z);
                            MoveFinish(position, ArrayMove.PLUS_Y);
                            MoveAgain();
                        }
                        break;
                    // 下なら
                    case MoveDirection.DOWN:
                        if (transform.localPosition.y <= moveBeforePosition.y - 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y - 1, transform.localPosition.z);
                            MoveFinish(position, ArrayMove.MINUS_Y);
                            MoveAgain();
                        }
                        break;
                    // 待機なら
                    case MoveDirection.STOP:
                        if (stopCount == 50)
                        {
                            MoveFinish(transform.localPosition, ArrayMove.NONE);
                            MoveAgain();
                        }
                        break;
                }
                break;
        }
    }

    // ★移動完了処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveFinish(Vector3 position, ArrayMove arrayMove)
    {
        moveFlag = false;                   // 移動フラグを偽に
        moveFinishFlag = true;              // 移動完了フラグを偽に
        moveReturnFlag = false;             // 巻き戻しフラグを偽に
        moveNextFlag = false;               // 早送りフラグを偽に
        inputKeyFlag = false;               // キー入力フラグを偽に
        autoMoveFlag = false;               // 自動移動フラグを偽に
        transform.position = position;      // 座標を変更
        ChangeArrayPosition(arrayMove);     // 配列上の位置を変更

        // プレイヤーの行動が進めるなら
        if (playerAction == PlayerAction.NEXT)
        {
            MoveCountDown();
        }
        // プレイヤーの行動が戻るなら
        else if (playerAction == PlayerAction.RETURN)
        {
            MoveCountUp();
        }

        // 保存数が０なら初期の向きに直す
        if(saveCount == 0){ transform.localEulerAngles = new Vector3(0, 0, 0); }

        playerAction = PlayerAction.NONE;   // アリスの行動を無しに
        stopCount = 0;                      // 待機時のカウントを０に
    }

    // ★移動情報の保存★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void SaveMove()
    {
        if (!moveReturnFlag)
        {
            // 早送りの時はリセットしない
            if (!moveNextFlag)
            {
                for (int num = saveCount; num < SAVE_NUM; num++)
                {
                    saveMovePlayerAngle[num] = PlayerAngle.NONE;
                    saveMoveDirection[num] = MoveDirection.NONE;
                    saveMoveInput[num] = false;
                }
            }

            saveMovePlayerAngle[saveCount] = playerAngle;   // プレイヤーの向きを保存
            saveMoveDirection[saveCount] = moveDirection;   // 移動情報を保存

            if (inputKeyFlag) { saveMoveInput[saveCount] = true; }
            else { saveMoveInput[saveCount] = false; }

            saveCount++;                                    // 最後の保存場所を変更
            print("保存場所を次に");
        }
    }

    // ★配列上の座標を変更する★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void ChangeArrayPosition(ArrayMove arrayMove)
    {
        switch (arrayMove)
        {
            case ArrayMove.NONE: break;                     // 配列上の座標変化無し
            case ArrayMove.PLUS_X: arrayPosX++; break;      // 配列上の座標Ｘに１プラス
            case ArrayMove.MINUS_X: arrayPosX--; break;     // 配列上の座標Ｘに１マイナス
            case ArrayMove.PLUS_Y: arrayPosY++; break;      // 配列上の座標Ｙに１プラス
            case ArrayMove.MINUS_Y: arrayPosY--; break;     // 配列上の座標Ｙに１マイナス
            case ArrayMove.PLUS_Z: arrayPosZ++; break;      // 配列上の座標Ｚに１プラス
            case ArrayMove.MINUS_Z: arrayPosZ--; break;     // 配列上の座標Ｚに１マイナス
        }
    }

    // ★移動数のカウントを増やす★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveCountUp()
    {
        moveCount++;
    }

    // ★移動数のカウントを減らす★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveCountDown()
    {
        moveCount--;
    }

    // ★自動移動設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AutoMoveSetting(MoveDirection direction)
    {
        autoMoveFlag = true;        // 自動移動フラグを真に
        moveDirection = direction;  // 移動方向を設定
    }

    // ★もう一度移動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveAgain()
    {
        if(saveCount >= 0)
        {
            bool input = saveMoveInput[saveCount];
            MoveDirection direction = saveMoveDirection[saveCount];

            if(!input)
            {
                print("再移動");
                autoMoveFlag = true;
                moveDirection = direction;
                moveReturnFlag = true;
            }
        }
    }

    public int GetDirection()
    {
        int sentDirection = 0;
        switch (playerAngle)
        {
            case PlayerAngle.FRONT: sentDirection = 1; break;
            case PlayerAngle.LEFT: sentDirection = 4; break;
            case PlayerAngle.RIGHT: sentDirection = 2; break;
            case PlayerAngle.BACK: sentDirection = 3; break;
        }
        return sentDirection;
    }
}