using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // listを使用するため

public class Player : MonoBehaviour
{
    // ★移動情報保存///////////////////////////////////
    const int SAVE_NUM = 50;        // 移動保存数

    // ★プレイヤー移動/////////////////////////////////
    const float SPEED_W = 0.02f;    // 移動速度（横方向）
    const float SPEED_H = 0.02f;    // 移動速度（縦方向）

    // ★プレイヤーの向きに対応した角度//////////////////
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

    // ★アリスの状態★
    public enum PlayerMode
    {
        NOMAL,  // 通常
        SMALL,  // 小さい
        BIG,    // 大きい
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

    // ★アリスの動き★
    public enum Motion
    {
        WALK_NEXT,          // 歩く（進む）
        WALK_RETURN,        // 歩く（戻る）
        DROP_NEXT,          // 落下（進む）
        DROP_RETURN,        // 落下（戻る）
        STOP_NEXT,          // 待機（進む）
        STOP_RETURN,        // 待機（戻る）
        PUSH_NEXT,          // 押す（進む）
        PUSH_RETURN,        // 押す（戻る）
        CLIMB_START_NEXT,   // 登り開始（進む）
        CLIMB_START_RETURN, // 登り開始（戻る）
        CLIMB_NEXT,         // 登り（進む）
        CLIMB_RETURN,       // 登り（戻る）
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

	// ★入手した鍵の色★
	public enum GetKeyColor
	{
		NONE,
		RED,		// 赤
		BLUE,		// 青
		YELLOW,	// 黄
		GREEN,	// 緑
	}

	const int InvisibleLimit = 3; // 透明化のターン数

    // ★配列上の座標///////////////////////////
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

    public PlayerAction playerAction;   // アリスの行動
    public PlayerMode playerMode;       // アリスの状態
    public PlayerAngle playerAngle;     // アリスの向き
    public MoveDirection moveDirection; // 移動方向

    public int stopCount;               // 待機時のカウント
	public GameObject pause; // ポーズ

    // 移動情報の保存
    public PlayerMode[] saveMovePlayerMode = new PlayerMode[SAVE_NUM];      // 保存用配列（アリスの状態）
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

    public bool climb1Flag; // 登り１フラグ
    public bool climb2Flag; // 登り２フラグ

    public int countBig;    // 大きくなっているターン数
    public int countSmall;  // 小さくなっているターン数
    //------------------------
    //松村脩平追加部分
    //------------------------
    public bool invisibleFlag;

	//------------------------
	// 西尾竜太郎追加部分
	//------------------------
	// 所持している鍵の色
	public bool getKeyColor_Red = false;		// 赤
	public bool getKeyColor_Blue = false;	// 青
	public bool getKeyColor_Yellow = false;	// 黄
	public bool getKeyColor_Green = false;	// 緑

	// 鍵を入手してからのターン数
	public int gettingKeyTurn_Red = 0;	// 赤
	public int gettingKeyTurn_Blue = 0;	// 青
	public int gettingKeyTurn_Yellow = 0;	// 黄
	public int gettingKeyTurn_Green = 0;	// 緑

	//public bool invisibleFlag = false; // 透明化フラグ
	public bool invisibleMemory = false; // 透明化になったことがあるか
	public int invisibleTurn = 0; // 透明化のターン

	private Renderer renderer;

    //ためし用(後々消す)
    public bool gameOverFlag;
    //------------------------

    // アニメーション用フラグ
    public bool animationFlagWalkNext;          // 歩きアニメーション（進む）
    public bool animationFlagWalkReturn;        // 歩きアニメーション（戻る）
    public bool animationFlagDropNext;          // 落下アニメーション（進む）
    public bool animationFlagDropReturn;        // 落下アニメーション（戻る）
    public bool animationFlagStopNext;          // 待機アニメーション（進む）
    public bool animationFlagStopReturn;        // 待機アニメーション（戻る）
    public bool animationFlagPushNext;          // 押すアニメーション（進む）
    public bool animationFlagPushReturn;        // 押すアニメーション（戻る）
    public bool animationFlagClimbStartNext;    // 登り開始アニメーション（進む）
    public bool animationFlagClimbStartReturn;  // 登り開始アニメーション（戻る）
    public bool animationFlagClimbNext;         // 登りアニメーション（進む）
    public bool animationFlagClimbReturn;       // 登りアニメーション（戻る）

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        moveFlag = false;
        moveFinishFlag = false;
        moveReturnFlag = false;
        moveNextFlag = false;
        inputKeyFlag = false;
        stopCount = 0;

        playerMode = PlayerMode.NOMAL;      // アリスの初期の状態を通常に
        playerAngle = PlayerAngle.FRONT;    // アリスの初期の向きを前に
		pause = GameObject.Find("Pause");
        autoMoveFlag = false;

        climb1Flag = false;
        climb2Flag = false;

        countBig = 0;       // 大きくなっているターン数を０に
        countSmall = 0;     // 小さくなっているターン数を０に

        ModeChange();

        //------------------------
        //松村脩平追加部分
        //------------------------
        invisibleFlag = false;
        //ためし用(後々消す)
        gameOverFlag = false;
        //------------------------

		//------------------------
		//西尾竜太郎追加部分
		//------------------------
		renderer = GetComponentInChildren<Renderer>();

        // アニメーション用フラグ
        animationFlagWalkNext = false;
        animationFlagWalkReturn = false;
        animationFlagDropNext = false;
        animationFlagDropReturn = false;
        animationFlagStopNext = false;
        animationFlagStopReturn = false;
        animationFlagPushNext = false;
        animationFlagPushReturn = false;
        animationFlagClimbStartNext = false;
        animationFlagClimbStartReturn = false;
        animationFlagClimbNext = false;
        animationFlagClimbReturn = false;
	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {
		if (pause.GetComponent<Pause>().pauseFlag == false)
		{
			cameraAngle = camera.cameraAngle;   // カメラの向きを取得
			Move();                             // 移動
		}
	}

    // ★カメラに対応した移動方向に変更★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void ChangeDirection(MoveDirection direction)
    {
        // ▽移動方向が
        switch (direction)
        {
            // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////////////
            case MoveDirection.FRONT:
                // ▽カメラの向きが
                switch (cameraAngle)
                {
                    // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.FRONT: moveDirection = MoveDirection.BACK; break;     // 移動方向を後に変更
                    // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.BACK: moveDirection = MoveDirection.FRONT; break;     // 移動方向を前に変更
                    // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.LEFT: moveDirection = MoveDirection.RIGHT; break;     // 移動方向を右に変更
                    // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.RIGHT: moveDirection = MoveDirection.LEFT; break;     // 移動方向を左に変更
                }
                break;
            // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////////////
            case MoveDirection.BACK:
                // ▽カメラの向きが
                switch (cameraAngle)
                {
                    // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.FRONT: moveDirection = MoveDirection.FRONT; break;    // 移動方向を前に変更
                    // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.BACK: moveDirection = MoveDirection.BACK; break;      // 移動方向を後に変更
                    // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.LEFT: moveDirection = MoveDirection.LEFT; break;      // 移動方向を左に変更
                    // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.RIGHT: moveDirection = MoveDirection.RIGHT; break;    // 移動方向を右に変更
                }
                break;
            // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////////////
            case MoveDirection.LEFT:
                // ▽カメラの向きが
                switch (cameraAngle)
                {
                    // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.FRONT: moveDirection = MoveDirection.RIGHT; break;    // 移動方向を右に変更
                    // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.BACK: moveDirection = MoveDirection.LEFT; break;      // 移動方向を左に変更
                    // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.LEFT: moveDirection = MoveDirection.FRONT; break;     // 移動方向を前に変更
                    // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.RIGHT: moveDirection = MoveDirection.BACK; break;     // 移動方向を後に変更
                }
                break;
            // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////////////
            case MoveDirection.RIGHT:
                // ▽カメラの向きが
                switch (cameraAngle)
                {
                    // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.FRONT: moveDirection = MoveDirection.LEFT; break;     // 移動方向を左に変更
                    // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.BACK: moveDirection = MoveDirection.RIGHT; break;     // 移動方向を右に変更
                    // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.LEFT: moveDirection = MoveDirection.BACK; break;      // 移動方向を後に変更
                    // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////
                    case PlayerCamera.CameraAngle.RIGHT: moveDirection = MoveDirection.FRONT; break;    // 移動方向を前に変更
                }
                break;
            // ▼上なら//////////////////////////////////////////////
            case MoveDirection.UP:
                moveDirection = MoveDirection.UP;   // 移動方向を上に
                break;
            // ▼下なら//////////////////////////////////////////////
            case MoveDirection.DOWN:
                moveDirection = MoveDirection.DOWN; // 移動方向を下に
                break;
            // ▼待機なら////////////////////////////////////
            case MoveDirection.STOP:
                moveDirection = MoveDirection.STOP; // 待機に
                break;
        }
    }

    // ★向きを変更★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void ChangeAngle()
    {
        // ▽移動方向が
        switch (moveDirection)
        {
            // ▼前なら//////////////////////////////////////////////////////////////////////////////
            case MoveDirection.FRONT:
                playerAngle = PlayerAngle.FRONT;                                // アリスの向きを前に
                transform.localEulerAngles = new Vector3(0, ANGLE_FRONT, 0);    // 前方向の角度を指定
                break;
            // ▼後なら//////////////////////////////////////////////////////////////////////////////
            case MoveDirection.BACK:
                playerAngle = PlayerAngle.BACK;                                 // アリスの向きを後に
                transform.localEulerAngles = new Vector3(0, ANGLE_BACK, 0);     // 後方向の角度を指定
                break;
            // ▼左なら//////////////////////////////////////////////////////////////////////////////
            case MoveDirection.LEFT:
                playerAngle = PlayerAngle.LEFT;                                 // アリスの向きを左に
                transform.localEulerAngles = new Vector3(0, ANGLE_LEFT, 0);     // 左方向の角度を指定
                break;
            // ▼右なら//////////////////////////////////////////////////////////////////////////////
            case MoveDirection.RIGHT:
                playerAngle = PlayerAngle.RIGHT;                                // アリスの向きを右に
                transform.localEulerAngles = new Vector3(0, ANGLE_RIGHT, 0);    // 右方向の角度を指定
                break;
            // ▼それ以外
            default:
                break;
        }
    }

    // ★移動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void Move()
    {
        // ▽アリスの行動が
        switch (playerAction)
        {
            // ▼何も無しなら
            case PlayerAction.NONE:
                break;
            // ▼進めるなら//////////////////////////////////////////////
            case PlayerAction.NEXT:
                if (gameOverFlag == false)
                {
                    MoveNextPosition();                 // アリスを進める処理
                    MoveFinishDecision(playerAction);   // 移動完了判定
                }
                break;
            // ▼戻るなら//////////////////////////////////////////////
            case PlayerAction.RETURN:
                MoveReturnPosition();               // アリスを戻す処理
                MoveFinishDecision(playerAction);   // 移動完了判定
                break;
        }
    }

    // ★プレイヤーを進める★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveNextPosition()
    {
        // ▽移動方向が
        switch (moveDirection)
        {
            // ▼前なら////////////////////////////////////////////////////////////
            case MoveDirection.FRONT:
            // ▼後なら////////////////////////////////////////////////////////////
            case MoveDirection.BACK:
            // ▼左なら////////////////////////////////////////////////////////////
            case MoveDirection.LEFT:
            // ▼右なら////////////////////////////////////////////////////////////
            case MoveDirection.RIGHT:
                transform.Translate(Vector3.forward * SPEED_W); // プレイヤーを移動
                break;
            // ▼上なら////////////////////////////////////////////////////////////
            case MoveDirection.UP:
                transform.Translate(Vector3.up * SPEED_H);      // プレイヤーを移動
                break;
            // ▼下なら////////////////////////////////////////////////////////////
            case MoveDirection.DOWN:
                transform.Translate(Vector3.down * SPEED_H);    // プレイヤーを移動
                break;
            // ▼待機なら//////////////////////////////////////
            case MoveDirection.STOP:
                stopCount++;            // 待機カウントを増やす
                break;
        }
    }

    // ★プレイヤーを戻す★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveReturnPosition()
    {
        // ▽移動方向が
        switch (moveDirection)
        {
            // ▼前なら////////////////////////////////////////////////////////////
            case MoveDirection.FRONT:
            // ▼後なら////////////////////////////////////////////////////////////
            case MoveDirection.BACK:
            // ▼左なら////////////////////////////////////////////////////////////
            case MoveDirection.LEFT:
            // ▼右なら////////////////////////////////////////////////////////////
            case MoveDirection.RIGHT:
                transform.Translate(Vector3.back * SPEED_W);    // プレイヤーを移動
                break;
            // ▼上なら////////////////////////////////////////////////////////////
            case MoveDirection.UP:
                transform.Translate(Vector3.up * SPEED_H);      // プレイヤーを移動
                break;
            // ▼下なら////////////////////////////////////////////////////////////
            case MoveDirection.DOWN:
                transform.Translate(Vector3.down * SPEED_H);    // プレイヤーを移動
                break;
            // ▼待機なら//////////////////////////////////////
            case MoveDirection.STOP:
                stopCount++;            // 待機カウントを増やす
                break;
        }
    }

    // ★移動開始処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveStart()
    {
        // 巻き戻しフラグが偽なら
        if (!moveReturnFlag)
        {
            // 早送りフラグが偽なら
            if (!moveNextFlag)
            {
                // ◆通常移動開始処理//////////////////////////////////////////////////////////////
                moveFlag = true;                            // 移動フラグを真に
                moveBeforePosition = transform.position;    // 移動前の座標に現在の座標を入れる
                ChangeDirection(moveDirection);             // カメラの向きに対応した移動方向に変更
                ChangeAngle();                              // アリスの向きを変更
                playerAction = PlayerAction.NEXT;           // アリスの行動を進めるに
            }
            else
            {
                // ◆早送り移動開始処理////////////////////////////////////////////////////////////////
                moveFlag = true;                                    // 移動フラグを真に
                moveBeforePosition = transform.position;            // 移動前の座標に現在の座標を入れる
                playerMode = saveMovePlayerMode[saveCount];         // 保存されている状態を設定
                playerAngle = saveMovePlayerAngle[saveCount];       // 保存されている向きを設定
                moveDirection = saveMoveDirection[saveCount];       // 保存されている移動方向を設定
                inputKeyFlag = saveMoveInput[saveCount];            // １つ前の入力を設定
                ChangeAngle();                                      // アリスの向きを変更
                playerAction = PlayerAction.NEXT;                   // アリスの行動を進めるに
            }
        }
        // ◆巻き戻し移動開始処理///////////////////////////////////////////////////////////////////////
        else
        {
            // 保存数が０より大きい（保存されている移動があれば）
            if (saveCount > 0)
            {
                // 巻き戻しの移動開始処理//////////////////////////////////////////////////////////////
                moveFlag = true;                                    // 移動フラグを真に
                moveBeforePosition = transform.position;            // 移動前の座標に現在の座標を入れる
                playerMode = saveMovePlayerMode[saveCount - 1];     // １つ前の状態を設定
                playerAngle = saveMovePlayerAngle[saveCount - 1];   // １つ前の向きを設定
                moveDirection = saveMoveDirection[saveCount - 1];   // １つ前の移動方向を設定
                inputKeyFlag = saveMoveInput[saveCount - 1];        // １つ前の入力を設定
                ChangeAngle();                                      // アリスの向きを変更
                ModeChange();                                       // 状態の切り替え
                playerAction = PlayerAction.RETURN;                 // アリスの行動を戻るに

                // プレイヤーの状態が通常なら////////////////////////////
                if (playerMode == PlayerMode.NOMAL)
                {
                    countBig = 0;       // 大きくなっているカウントを０に
                    countSmall = 0;     // 小さくなっているカウントを０に
                }

                // キー入力による移動の場合
                if(inputKeyFlag)
                {
                    switch(moveDirection)
                    {
                        case MoveDirection.FRONT:
                        case MoveDirection.BACK:
                        case MoveDirection.LEFT:
                        case MoveDirection.RIGHT:
                            SetAnimation(Motion.WALK_RETURN, true);
                            break;
                        case MoveDirection.STOP:
                            SetAnimation(Motion.STOP_RETURN, true);
                            break;
                    }
                }
                else
                {
                    switch(moveDirection)
                    {
                        case MoveDirection.DOWN:
                            SetAnimation(Motion.DROP_RETURN, true);
                            break;
                    }
                }

                // ▽移動方向が
                switch (moveDirection)
                {

                    // ▼上なら//////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.UP: moveDirection = MoveDirection.DOWN; break;   // 移動方向を下に変更
                    // ▼下なら//////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.DOWN: moveDirection = MoveDirection.UP; break;   // 移動方向を上に変更
                }

                saveCount--;    // 保存場所を１つ前に
            }
        }
    }

    // ★移動完了判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveFinishDecision(PlayerAction action)
    {
        // ▽行動が
        switch (action)
        {
            // ▼何も無しなら//////
            case PlayerAction.NONE:
                break;
            // ▼進むなら////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case PlayerAction.NEXT:
                // ▽移動方向が
                switch (moveDirection)
                {
                    // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.FRONT:
                        // アリスの座標Ｚが移動前から１増えているなら
                        if (transform.localPosition.z >= moveBeforePosition.z + 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, moveBeforePosition.z + 1); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.PLUS_Z);                                                                         // 移動完了処理
                        }
                        break;
                    // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.BACK:
                        // アリスの座標Ｚが移動前から１減っているなら
                        if (transform.localPosition.z <= moveBeforePosition.z - 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, moveBeforePosition.z - 1); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.MINUS_Z);                                                                        // 移動完了処理
                        }
                        break;
                    // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.LEFT:
                        // アリスの座標Ｘが移動前から１減っているなら
                        if (transform.localPosition.x <= moveBeforePosition.x - 1)
                        {
                            Vector3 position = new Vector3(moveBeforePosition.x - 1, transform.localPosition.y, transform.localPosition.z); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.MINUS_X);                                                                        // 移動完了処理
                        }
                        break;
                    // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.RIGHT:
                        // アリスの座標Ｘが移動前から１増えているなら
                        if (transform.localPosition.x >= moveBeforePosition.x + 1)
                        {
                            Vector3 position = new Vector3(moveBeforePosition.x + 1, transform.localPosition.y, transform.localPosition.z); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.PLUS_X);                                                                         // 移動完了処理
                        }
                        break;
                    // ▼上なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.UP:
                        if ((transform.localPosition.y >= moveBeforePosition.y + 0.5f) && animationFlagClimbNext)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y + 0.5f, transform.localPosition.z);  // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.PLUS_Y);                                                                             // 移動完了処理
                        }
                        // アリスの座標Ｙが移動前から１増えているなら
                        else if (transform.localPosition.y >= moveBeforePosition.y + 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y + 1, transform.localPosition.z); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.PLUS_Y);                                                                         // 移動完了処理
                        }
                        break;
                    // ▼下なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.DOWN:
                        if ((transform.localPosition.y <= moveBeforePosition.y - 0.5f) && animationFlagClimbNext)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y - 0.5f, transform.localPosition.z);  // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.MINUS_Y);                                                                            // 移動完了処理
                        }
                        // アリスの座標Ｙが移動前から１減っているなら
                        else if (transform.localPosition.y <= moveBeforePosition.y - 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y - 1, transform.localPosition.z); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.MINUS_Y);                                                                        // 移動完了処理
                        }
                        break;
                    // ▼待機なら//////////////////////////////////////////////////////////////////
                    case MoveDirection.STOP:
                        // 待機カウントが５０になったら
                        if (stopCount == 50)
                        {
                            MoveFinish(transform.localPosition, ArrayMove.NONE);    // 移動完了処理
                        }
                        break;
                }
                break;
            // ▼戻るなら////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case PlayerAction.RETURN:
                // ▽移動方向が
                switch (moveDirection)
                {
                    // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.FRONT:
                        // アリスの座標Ｚが移動前から１減っているなら
                        if (transform.localPosition.z <= moveBeforePosition.z - 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, moveBeforePosition.z - 1); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.MINUS_Z);                                                                        // 移動完了処理
                            MoveAgain();                                                                                                    // 再移動
                        }
                        break;
                    // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.BACK:
                        // アリスの座標Ｚが移動前から１増えているなら
                        if (transform.localPosition.z >= moveBeforePosition.z + 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, moveBeforePosition.z + 1); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.PLUS_Z);                                                                         // 移動完了処理
                            MoveAgain();                                                                                                    // 再移動
                        }
                        break;
                    // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.LEFT:
                        // アリスの座標Ｘが移動前から１増えているなら
                        if (transform.localPosition.x >= moveBeforePosition.x + 1)
                        {
                            Vector3 position = new Vector3(moveBeforePosition.x + 1, transform.localPosition.y, transform.localPosition.z); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.PLUS_X);                                                                         // 移動完了処理
                            MoveAgain();                                                                                                    // 再移動
                        }
                        break;
                    // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.RIGHT:
                        // アリスの座標Ｘが移動前から１減っているなら
                        if (transform.localPosition.x <= moveBeforePosition.x - 1)
                        {
                            Vector3 position = new Vector3(moveBeforePosition.x - 1, transform.localPosition.y, transform.localPosition.z); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.MINUS_X);                                                                        // 移動完了処理
                            MoveAgain();                                                                                                    // 再移動
                        }
                        break;
                    // ▼上なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.UP:
                        if ((transform.localPosition.y >= moveBeforePosition.y + 0.5f) && animationFlagClimbNext)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y + 0.5f, transform.localPosition.z);  // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.PLUS_Y);                                                                             // 移動完了処理
                        }
                        // アリスの座標Ｙが移動前から１増えているなら
                        else if (transform.localPosition.y >= moveBeforePosition.y + 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y + 1, transform.localPosition.z); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.PLUS_Y);                                                                         // 移動完了処理
                            MoveAgain();                                                                                                    // 再移動
                        }
                        break;
                    // ▼下なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case MoveDirection.DOWN:
                        if ((transform.localPosition.y <= moveBeforePosition.y - 0.5f) && animationFlagClimbNext)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y - 0.5f, transform.localPosition.z);  // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.MINUS_Y);                                                                            // 移動完了処理
                        }
                        // アリスの座標Ｙが移動前から１減っているなら
                        else if (transform.localPosition.y <= moveBeforePosition.y - 1)
                        {
                            Vector3 position = new Vector3(transform.localPosition.x, moveBeforePosition.y - 1, transform.localPosition.z); // 移動後の座標を設定
                            MoveFinish(position, ArrayMove.MINUS_Y);                                                                        // 移動完了処理
                            MoveAgain();                                                                                                    // 再移動
                        }
                        break;
                    // ▼待機なら//////////////////////////////////////////////////////////////////
                    case MoveDirection.STOP:
                        // 待機カウントが５０になったら
                        if (stopCount == 50)
                        {
                            MoveFinish(transform.localPosition, ArrayMove.NONE);    // 移動完了処理
                            MoveAgain();                                            // 再移動
                        }
                        break;
                }
                break;
        }
    }

    // ★移動完了処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MoveFinish(Vector3 position, ArrayMove arrayMove)
    {
        SaveMove();     // 移動の保存

        // プレイヤーの行動が進めるなら
        if (playerAction == PlayerAction.NEXT && inputKeyFlag == true)
        {
            MoveCountDown();    // 移動数を減らす
            turnCount++;

            // アリスが大きくなっていたら
            if (playerMode == PlayerMode.BIG)
            {
                countBig--; // 大きくなっているカウントを減らす

                // 大きくなっている
                if (countBig == 0)
                {
                    playerMode = PlayerMode.NOMAL;
                }
            }

            if (playerMode == PlayerMode.SMALL)
            {
                countSmall--;

                if (countSmall == 0)
                {
                    playerMode = PlayerMode.NOMAL;
                }
            }
        }
        // プレイヤーの行動が戻るなら////////////////////////////////////////
        else if (playerAction == PlayerAction.RETURN && inputKeyFlag == true)
        {
            MoveCountUp();
            turnCount--;

            if (playerMode == PlayerMode.BIG)
            {
                if (countBig == 3)
                {
                    playerMode = PlayerMode.NOMAL;
                }

                countBig++;
            }

            if (playerMode == PlayerMode.SMALL)
            {
                if (countSmall == 3)
                {
                    playerMode = PlayerMode.NOMAL;
                }

                countSmall++;
            }
        }

        ModeChange();   // 状態の切り替え

        moveFlag = false;                   // 移動フラグを偽に
        moveFinishFlag = true;              // 移動完了フラグを偽に
        moveReturnFlag = false;             // 巻き戻しフラグを偽に
        moveNextFlag = false;               // 早送りフラグを偽に
        inputKeyFlag = false;               // キー入力フラグを偽に
        autoMoveFlag = false;               // 自動移動フラグを偽に
        transform.position = position;      // 座標を変更
        ChangeArrayPosition(arrayMove);     // 配列上の位置を変更
		KeyTurnChange(); // 鍵所持ターンカウントの変動
		InvisibleTurnChange(); // 透明化ターンカウントの変動

        // 保存数が０なら初期の向きに直す
        if (saveCount == 0) { transform.localEulerAngles = new Vector3(0, 0, 0); }


        if (playerAction == PlayerAction.NEXT)
        {
            //上るフラグのリセット
            climb1Flag = false;
            climb2Flag = false;
        }
        if (playerAction == PlayerAction.RETURN && gameOverFlag)
        {
            gameOverFlag = false;
        }

        if((playerAction == PlayerAction.RETURN)&&( climb2Flag == true))
        {
            climb2Flag = false;
            climb1Flag = true;
        }
        if((playerAction == PlayerAction.RETURN)&&(climb1Flag == true))
        {
            climb1Flag = false;
        }

        //GameObject so = GameObject.Find("Stage");
        
        playerAction = PlayerAction.NONE;   // アリスの行動を無しに
        stopCount = 0;                      // 待機時のカウントを０に

        // アニメーションのリセット
        SetAnimation(Motion.WALK_NEXT, false);
        SetAnimation(Motion.WALK_RETURN, false);
        SetAnimation(Motion.DROP_NEXT, false);
        SetAnimation(Motion.DROP_RETURN, false);
        SetAnimation(Motion.STOP_NEXT, false);
        SetAnimation(Motion.STOP_RETURN, false);
        SetAnimation(Motion.PUSH_NEXT, false);
        SetAnimation(Motion.PUSH_RETURN, false);
        SetAnimation(Motion.CLIMB_START_NEXT, false);
        SetAnimation(Motion.CLIMB_START_RETURN, false);
        SetAnimation(Motion.CLIMB_NEXT, false);
        SetAnimation(Motion.CLIMB_RETURN, false);
    }

    // ★移動情報の保存★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void SaveMove()
    {
        // 巻き戻し以外の場合
        if (!moveReturnFlag)
        {
            // 早送り、自動移動以外の場合は移動情報をリセット
            if (!moveNextFlag && !autoMoveFlag)
            {
                for (int num = saveCount; num < SAVE_NUM; num++)
                {
                    // 保存されている情報をリセット//////////////////////////////////////////////////
                    saveMovePlayerMode[num] = PlayerMode.NOMAL;     // プレイヤーの状態をリセット
                    saveMovePlayerAngle[num] = PlayerAngle.NONE;    // プレイヤーの向きをリセット
                    saveMoveDirection[num] = MoveDirection.NONE;    // プレイヤーの移動方向をリセット
                    saveMoveInput[num] = false;
                }
            }

            // 情報の保存////////////////////////////////////////////////////////////////
            saveMovePlayerMode[saveCount] = playerMode;     // プレイヤーの状態を保存
            saveMovePlayerAngle[saveCount] = playerAngle;   // プレイヤーの向きを保存
            saveMoveDirection[saveCount] = moveDirection;   // プレイヤーの移動方向を保存

            if (inputKeyFlag) { saveMoveInput[saveCount] = true; }
            else { saveMoveInput[saveCount] = false; }

            saveCount++;                                    // 最後の保存場所を変更
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

    // ★状態の切り替え★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void ModeChange()
    {
        switch (playerMode)
        {
            case PlayerMode.NOMAL: transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); break;
            case PlayerMode.BIG: transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); break;
            case PlayerMode.SMALL: transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); break;
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

    //----------------------------
    //松村脩平追加部分
    //----------------------------
    //アリスの配列座標の取得
    public Vector3 GetArray()
    {
        return new Vector3(arrayPosX, arrayPosY, arrayPosZ);
    }
    
    //アリスが透明であるか取得
    public bool GetInvisible()
    {
        return invisibleFlag;
    }
    //------------------------------------------------------

	// ★鍵の取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void GetKey(GetKeyColor color)
	{
		int keyTurnAdjuster;
		switch (playerAction)
		{
			case PlayerAction.NEXT:
				keyTurnAdjuster = -1;
				break;
			case PlayerAction.RETURN:
				keyTurnAdjuster = 1;
				break;
			default:
				keyTurnAdjuster = 0;
				break;
		}

		if (color == GetKeyColor.RED)
		{
			getKeyColor_Red = true;
			gettingKeyTurn_Red = keyTurnAdjuster;
		}
		else if (color == GetKeyColor.BLUE)
		{
			getKeyColor_Blue = true;
			gettingKeyTurn_Blue = keyTurnAdjuster;
		}
		else if (color == GetKeyColor.YELLOW)
		{
			getKeyColor_Yellow = true;
			gettingKeyTurn_Yellow = keyTurnAdjuster;
		}
		else if (color == GetKeyColor.GREEN)
		{
			getKeyColor_Green = true;
			gettingKeyTurn_Green = keyTurnAdjuster;
		}
	}

	// ★鍵所持ターンカウントの変動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void KeyTurnChange()
	{
		switch (playerAction)
		{
			case PlayerAction.NONE:
			case PlayerAction.NEXT:
				if (getKeyColor_Red) { gettingKeyTurn_Red++; }
				if (getKeyColor_Blue) { gettingKeyTurn_Blue++; }
				if (getKeyColor_Yellow) { gettingKeyTurn_Yellow++; }
				if (getKeyColor_Green) { gettingKeyTurn_Green++; }
				break;
			case PlayerAction.RETURN:
				if (getKeyColor_Red) { gettingKeyTurn_Red--; }
				if (getKeyColor_Blue) { gettingKeyTurn_Blue--; }
				if (getKeyColor_Yellow) { gettingKeyTurn_Yellow--; }
				if (getKeyColor_Green) { gettingKeyTurn_Green--; }
				break;
			default:
				break;
		}

		// 所持ターンがマイナスになったら、鍵所持状態を解除する
		if (gettingKeyTurn_Red < 0) { getKeyColor_Red = false; }
		if (gettingKeyTurn_Blue < 0) { getKeyColor_Blue = false; }
		if (gettingKeyTurn_Yellow < 0) { getKeyColor_Yellow = false; }
		if (gettingKeyTurn_Green < 0) { getKeyColor_Green = false; }
	}

	// ★チェシャに触れて透明化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void TouchCheshire()
	{
		int keyTurnAdjuster;
		switch (playerAction)
		{
			case PlayerAction.NEXT:
				keyTurnAdjuster = -1;
				break;
			case PlayerAction.RETURN:
				keyTurnAdjuster = 1;
				break;
			default:
				keyTurnAdjuster = 0;
				break;
		}
		invisibleFlag = true;
		invisibleMemory = true;
		invisibleTurn = keyTurnAdjuster;
		renderer.enabled = false;    // 描画しない
	}

	// ★透明化ターンカウントの変動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void InvisibleTurnChange()
	{
		if (invisibleMemory)
		{
			switch (playerAction)
			{
				case PlayerAction.NONE:
				case PlayerAction.NEXT:
					invisibleTurn++;
					break;
				case PlayerAction.RETURN:
					invisibleTurn--;
					break;
				default:
					break;
			}
		}

		// 透明化ターンがマイナスになったら、透明化状態を完全に解除する
		if (invisibleTurn < 0)
		{
			invisibleFlag = false;
			invisibleMemory = false;
		}

		// 透明化ターンが規定ターンを超えたら透明化状態を解除する
		else if (invisibleTurn > InvisibleLimit) 
			invisibleFlag = false;

		// 透明化ターンが0以上規定ターン以内になったら再透明化する
		else if (((invisibleTurn >= 0) || (invisibleTurn < InvisibleLimit)) && (invisibleMemory))
			invisibleFlag = true;

		// 透明の場合、描画をしない
		if(invisibleFlag)
			renderer.enabled = false;    // 描画しない
		else
			renderer.enabled = true;    // 描画する
	}

    // ★アニメーションのセット★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void SetAnimation(Motion motion, bool flag)
    {
        switch(motion)
        {
            case Motion.WALK_NEXT: AnimationWalkNext(flag); break;
            case Motion.WALK_RETURN: AnimationWalkReturn(flag); break;
            case Motion.DROP_NEXT: AnimationDropNext(flag); break;
            case Motion.DROP_RETURN: AnimationDropReturn(flag); break;
            case Motion.STOP_NEXT: AnimationStopNext(flag); break;
            case Motion.STOP_RETURN: AnimationStopReturn(flag); break;
            case Motion.PUSH_NEXT: AnimationPushNext(flag); break;
            case Motion.PUSH_RETURN: AnimationPushReturn(flag); break;
            case Motion.CLIMB_START_NEXT: AnimationClimbStartNext(flag); break;
            case Motion.CLIMB_START_RETURN: AnimationClimbStartReturn(flag); break;
            case Motion.CLIMB_NEXT: AnimationClimbNext(flag); break;
            case Motion.CLIMB_RETURN: AnimationClimbReturn(flag); break;
        }
    }

    // ★歩きアニメーション（進む）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationWalkNext(bool flag)
    {
        animationFlagWalkNext = flag;
        GetComponent<Animator>().SetBool("WalkMotionFlag_Next", animationFlagWalkNext);
    }

    // ★歩きアニメーション（戻る）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationWalkReturn(bool flag)
    {
        animationFlagWalkReturn = flag;
        GetComponent<Animator>().SetBool("WalkMotionFlag_Return", animationFlagWalkReturn);
    }

    // ★落下アニメーション（進む）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationDropNext(bool flag)
    {
        animationFlagDropNext = flag;
        GetComponent<Animator>().SetBool("DropMotionFlag_Next", animationFlagDropNext);
    }

    // ★落下アニメーション（戻る）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationDropReturn(bool flag)
    {
        animationFlagDropReturn = flag;
        GetComponent<Animator>().SetBool("DropMotionFlag_Return", animationFlagDropReturn);
    }

    // ★待機アニメーション（進む）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationStopNext(bool flag)
    {
        animationFlagStopNext = flag;
        GetComponent<Animator>().SetBool("StopMotionFlag_Next", animationFlagStopNext);
    }

    // ★待機アニメーション（戻る）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationStopReturn(bool flag)
    {
        animationFlagStopReturn = flag;
        GetComponent<Animator>().SetBool("StopMotionFlag_Return", animationFlagStopReturn);
    }

    // ★押すアニメーション（進む）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationPushNext(bool flag)
    {
        animationFlagPushNext = flag;
        GetComponent<Animator>().SetBool("PushMotionFlag_Next", animationFlagPushNext);
    }

    // ★押すアニメーション（戻る）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationPushReturn(bool flag)
    {
        animationFlagPushReturn = flag;
        GetComponent<Animator>().SetBool("PushMotionFlag_Return", animationFlagPushReturn);
    }

    // ★登り開始アニメーション（進む）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationClimbStartNext(bool flag)
    {
        animationFlagClimbStartNext = flag;
        GetComponent<Animator>().SetBool("ClimbStartMotionFlag_Next", animationFlagClimbStartNext);
    }

    // ★登り開始ニメーション（戻る）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationClimbStartReturn(bool flag)
    {
        animationFlagClimbStartReturn = flag;
        GetComponent<Animator>().SetBool("ClimbStartMotionFlag_Return", animationFlagClimbStartReturn);
    }

    // ★登りアニメーション（進む）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationClimbNext(bool flag)
    {
        animationFlagClimbNext = flag;
        GetComponent<Animator>().SetBool("ClimbMotionFlag_Next", animationFlagClimbNext);
    }

    // ★登りニメーション（戻る）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AnimationClimbReturn(bool flag)
    {
        animationFlagClimbReturn = flag;
        GetComponent<Animator>().SetBool("ClimbMotionFlag_Return", animationFlagClimbReturn);
    }

	// ★アリスが巨大化中であるか取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public bool GetBig()
	{
		if (playerMode == PlayerMode.BIG)
			return true;

		return false;
	}

	// ★アリスの移動方向を取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public int GetMoveDirection()
	{
		int direction = 0;

		if (playerAction == PlayerAction.NEXT)
		{
			switch (playerAngle)
			{
				case PlayerAngle.FRONT:
					direction = 1;
					break;
				case PlayerAngle.BACK:
					direction = 2;
					break;
				case PlayerAngle.LEFT:
					direction = 3;
					break;
				case PlayerAngle.RIGHT:
					direction = 4;
					break;
			}
		}
		if (moveDirection == MoveDirection.STOP)
			direction = 0;

		if (playerAction == PlayerAction.RETURN)
			direction = 5;

		return direction;
	}

	// ★自動移動フラグを返す★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public bool GetAutoMove()
	{
		return autoMoveFlag;
	}
}