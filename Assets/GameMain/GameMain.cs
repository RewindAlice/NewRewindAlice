using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMain : MonoBehaviour
{
    // ★カメラの回転関数ポインタ★
    public delegate void OnCameraMove();
    public event OnCameraMove cameraTurnLeftEvent;  // カメラの左回転
    public event OnCameraMove cameraTurnRightEvent; // カメラの右回転

    // ★プレイヤーの移動関数ポインタ★
    public delegate void OnPlayerMove();
    public event OnPlayerMove inputPlayerMoveEvent;         // キー入力によるプレイヤーの移動関数
    public event OnPlayerMove inputPlayerMoveReturnEvent;   // キー入力によるプレイヤーの巻き戻し関数
    public event OnPlayerMove inputPlayerMoveNextEvent;     // キー入力によるプレイヤーの早送り関数
    public event OnPlayerMove autoPlayerMoveEvent;

    // ★アリスの行動★
    public enum PlayerAction
    {
        NONE,       // 無し
        NEXT,       // 進む
        RETURN,     // 戻る
    }

    // ★ターン★
    public enum Turn
    {
        NONE,       // 無し
        PLAYER,     // プレイヤー
        GIMMICK,    // ギミック
    }

    public PlayerCamera camera; // カメラ
    public Player alice;        // プレイヤー
    public Stage stage;         // ステージ
    public int turnNum;         // ターン数

    public PlayerAction action;     // 行動が何か判断する
    public Turn turn;               // 誰のターンか判断する
    public int turnCountGimmick;    // ターンの時間稼ぎ


    public int stageNumber;
    //チュートリアルに必要な変数
    public bool tutorialFlag;      //チュートリアルか判断する
    public int tutorialTurn;       //チュートリアルのターン数
    public bool tutorialImageFlag;  //説明画像が出ているかどうか
    public int tutorialCount;      //チュートリアル中のカウント
    public GameObject ImageUI;

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        GameSetting();  // ゲームの設定
	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {
        MapCamera();    // マップカメラ
        CameraTurn();   // カメラの回転
        PlayerMove();   // プレイヤーの移動
        GameAction();   // 行動を行う

        if (tutorialImageFlag == true)
        {

            if (tutorialCount < 11)
                ImageUI.GetComponent<Image>().material.color = new Color(1.0f, 1.0f, 1.0f, tutorialCount / 10.0f);

            if (tutorialCount < 60)
            {
                tutorialCount++;
            }
        }
        else if (tutorialImageFlag == false)
        {
            ImageUI.GetComponent<Image>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }

        if (stageNumber == 1 && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7))
        {
            tutorialImageFlag = true;
        }
        else if (stageNumber == 2 && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6))
        {
            tutorialImageFlag = true;
        }
	}

    // ★ゲームの設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void GameSetting()
    {
        action = PlayerAction.NONE; // 行動に無しを設定
        turn = Turn.NONE;           // ターンに無しを設定
        turnCountGimmick = 0;       // カウントを０に

        stageNumber = 3;
        stage.setSelectStage(stageNumber);            // 選択されたステージを設定
        stage.CreateStage();                // ステージの生成
        turnNum = stage.getStageTurnNum();  // ターン数の取得

        alice.transform.position = stage.getStartPosition();    // アリスの座標を設定
        alice.arrayPosX = stage.getStartArrayPosition('x');     // アリスの配列上の座標Ｘを設定
        alice.arrayPosY = stage.getStartArrayPosition('y');     // アリスの配列上の座標Ｙを設定
        alice.arrayPosZ = stage.getStartArrayPosition('z');     // アリスの配列上の座標Ｚを設定
        alice.moveCount = turnNum;

        //チュートリアルに必要な変数
        if (stageNumber == 1 || stageNumber == 2)
        { }
        tutorialFlag = true;      //チュートリアルか判断する
        tutorialTurn = 0;       //チュートリアルのターン数
        tutorialImageFlag = false;  //説明画像が出ているかどうか
        tutorialCount = 0;      //チュートリアル中のカウント
        if (tutorialFlag == true)
        {
            ImageUI = GameObject.Find("EXImage");
            ImageUI.GetComponent<Image>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        }// アリスの移動数にステージのターン数を設定
    }

    // ★行動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void GameAction()
    {
        // 現在の行動が
        switch (action)
        {
            // 無しなら
            case PlayerAction.NONE:
                break;

            // 進むなら
            case PlayerAction.NEXT:
                // ターンがプレイヤーなら
                if (turn == Turn.PLAYER)
                {
                    // プレイヤーの移動
                    if(alice.moveFlag == false && alice.moveFinishFlag == false){ InputPlayerMove(); }

                    // プレイヤーの移動が完了したら
                    if (alice.moveFinishFlag == true)
                    {
                        stage.GimmickDecision(alice);   // ギミックとの判定
                        stage.FootDecision(alice);      // 足元との判定
                        alice.moveFinishFlag = false;   // 移動完了フラグを偽に

                        if(!alice.autoMoveFlag)
                        {
                            turn = Turn.GIMMICK;            // ターンをギミックに
                        }
                    }
                }
                // ターンがギミックなら
                else if (turn == Turn.GIMMICK)
                {
                    turnCountGimmick++; // カウントを増やす

                    // カウントが６０になったら
                    if (turnCountGimmick == 60)
                    {
                        action = PlayerAction.NONE;   // 行動を無しに
                        turn = Turn.NONE;       // ターンを無しに
                        turnNum--;

                        print("ターン終了");// デバッグ用コメント
                        if (tutorialFlag == true)
                        {
                            tutorialTurn++;
                        }
                    }
                }
                break;

            // 戻るなら
            case PlayerAction.RETURN:
                // ターンがギミックなら
                if (turn == Turn.GIMMICK)
                {
                    turnCountGimmick++; // カウントを増やす

                    // カウントが６０になったら
                    if (turnCountGimmick == 60)
                    {
                        turn = Turn.PLAYER; // ターンをプレイヤーに
                    }
                }
                // ターンがプレイヤーなら
                else if (turn == Turn.PLAYER)
                {
                    // プレイヤーの巻き戻し
                    if (alice.moveFlag == false && alice.moveFinishFlag == false) { InputPlayerMoveReturn(); }

                    // プレイヤーの巻き戻しが完了したら
                    if (alice.moveFinishFlag == true)
                    {
                        stage.GimmickDecision(alice);   // ギミックとの判定
                        stage.FootDecision(alice);      // 足元との判定
                        alice.moveFinishFlag = false;   // 移動完了フラグを偽に

                        if (!alice.autoMoveFlag)
                        {
                            action = PlayerAction.NONE;     // 行動を無しに
                            turn = Turn.NONE;               // ターンを無しに
                            turnNum++;

                            if (tutorialFlag == true)
                            {
                                tutorialTurn++;
                            }
                            print("ターン終了");// デバッグ用コメント
                        }
                    }
                }
                break;
        }
    }

    // ★マップカメラ★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void MapCamera()
    {
        // Ｃキーが押されたら
        if ((Input.GetKeyDown(KeyCode.C)))
        {
            camera.SwitchingMapCamera();    // マップカメラの切り替え
        }
    }

    // ★カメラの回転★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void CameraTurn()
    {
        if (turn == Turn.NONE)
        {
            if (tutorialFlag == false ||
                (tutorialFlag == true && stageNumber == 1 && (tutorialTurn == 0 || tutorialTurn == 1)))
            {
                // 矢印左を押したら(カメラ移動左回転)
                if ((Input.GetKeyDown(KeyCode.LeftArrow)))
                {
                    CameraTurnLeftMove();
                    print("カメラ左回転");// デバッグ用コメント
                }


            }

            if (tutorialFlag == false)
            {
                // 矢印右を押したら(カメラ移動右回転)
                if ((Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    CameraTurnRightMove();
                    print("カメラ右回転");// デバッグ用コメント
                }
            }
        }
    }

    // ★プレイヤーの移動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void PlayerMove()
    {
        // カメラが動いていなければ
        if (!camera.rotationFlag)
        {
            AliceMovePossibleDecision();    // アリスの移動可能範囲の判定

            // 自動移動フラグが真なら
            if (alice.autoMoveFlag)
            {
                // 自動移動の方向が
                switch(alice.moveDirection)
                {
                    case Player.MoveDirection.DOWN:
                        if(!alice.moveFlag)
                        {
                            if(!alice.moveReturnFlag)
                            {
                                action = PlayerAction.NEXT;     // 行動を進むに
                                turn = Turn.PLAYER;             // ターンをプレイヤーに
                                turnCountGimmick = 0;           // カウントを０に
                                AutoPlayerMove();
                            }
                            else
                            {
                                action = PlayerAction.RETURN;   // 行動を戻るに
                                turn = Turn.GIMMICK;            // ターンをギミックに
                                turnCountGimmick = 0;           // カウントを０に
                                AutoPlayerMove();
                            }
                        }
                        break;
                }
            }
            else
            {
                // ▼画面奥方向移動処理
                // Ｗキーが押された時、行動が無しなら////////////////////////////////////////////////////////////////////////////////////////
                if ((Input.GetKeyDown(KeyCode.W)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveFrontPossibleFlag)&&(tutorialFlag==false)||
                    (Input.GetKeyDown(KeyCode.W)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveFrontPossibleFlag) && (tutorialFlag == true) && 
                    (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6||tutorialTurn == 7)||
                    (Input.GetKeyDown(KeyCode.W)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveFrontPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 2 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 8 || tutorialTurn == 9 || tutorialTurn == 10))
                {
                    if (tutorialImageFlag == true)
                    {
                        if (tutorialCount == 60)
                        {
                            tutorialImageFlag = false;
                            tutorialTurn++;
                            tutorialCount = 0;
                        }

                    }
                    else
                    {
                        // 登りフラグが真なら
                        if (AliceClimb1Dicision(Player.MoveDirection.FRONT))
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.UP;
                            alice.inputKeyFlag = true;
                            print("上移動");// デバッグ用コメント
                        }
                        else if (AliceClimb2Dicision(Player.MoveDirection.FRONT))
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.DOWN;
                            alice.inputKeyFlag = true;
                            print("下移動");// デバッグ用コメント
                        }
                        else
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.FRONT;
                            alice.inputKeyFlag = true;
                            print("前移動");// デバッグ用コメント
                        }
                    }
                }

                // ▼画面手前方向移動処理
                // Ｓキーが押された時、行動が無しなら///////////////////////////////////////////////////////////////////////////////////////
                if ((Input.GetKeyDown(KeyCode.S)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveBackPossibleFlag) && (tutorialFlag == false)||
                    (Input.GetKeyDown(KeyCode.S)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveBackPossibleFlag) && (tutorialFlag == true) && 
                    (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 5||tutorialTurn == 6||tutorialTurn == 7) ||
                    (Input.GetKeyDown(KeyCode.S)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveBackPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6))
                {
                    if (tutorialImageFlag == true)
                    {
                        if (tutorialCount == 60)
                        {
                            tutorialImageFlag = false;
                            tutorialTurn++;
                            tutorialCount = 0;
                        }
                    }
                    else
                    {
                        // 登りフラグが真なら
                        if (AliceClimb1Dicision(Player.MoveDirection.BACK))
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.UP;
                            alice.inputKeyFlag = true;
                            print("上移動");// デバッグ用コメント
                        }
                        else if (AliceClimb2Dicision(Player.MoveDirection.BACK))
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.DOWN;
                            alice.inputKeyFlag = true;
                            print("下移動");// デバッグ用コメント
                        }
                        else
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.BACK;
                            alice.inputKeyFlag = true;
                            print("後移動");// デバッグ用コメント
                        }
                    }
                }

               // ▼画面左方向移動処理
                // Ａキーが押された時、行動が無しなら///////////////////////////////////////////////////////////////////////////////////////
                if ((Input.GetKeyDown(KeyCode.A)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveLeftPossibleFlag) && (tutorialFlag == false)||
                    (Input.GetKeyDown(KeyCode.A)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveLeftPossibleFlag) && (tutorialFlag == true) &&
                    (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7 || tutorialTurn == 8)||
                    (Input.GetKeyDown(KeyCode.A)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveLeftPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6))
                {
                    if (tutorialImageFlag == true)
                    {
                        if (tutorialCount == 60)
                        {
                            tutorialImageFlag = false;
                            tutorialTurn++;
                            tutorialCount = 0;
                        }
                    }
                    else
                    {
                        // 登りフラグが真なら
                        if (AliceClimb1Dicision(Player.MoveDirection.LEFT))
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.UP;
                            alice.inputKeyFlag = true;
                            print("上移動");// デバッグ用コメント
                        }
                        else if (AliceClimb2Dicision(Player.MoveDirection.LEFT))
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.DOWN;
                            alice.inputKeyFlag = true;
                            print("下移動");// デバッグ用コメント
                        }
                        else
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.LEFT;
                            alice.inputKeyFlag = true;
                            print("左移動");// デバッグ用コメント
                        }
                    }
                }

                 // ▼画面右方向移動処理
                // Ｄキーが押された時、行動が無しなら///////////////////////////////////////////////////////
                if ((Input.GetKeyDown(KeyCode.D)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveRightPossibleFlag) && (tutorialFlag == false)||
                     (Input.GetKeyDown(KeyCode.D)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveRightPossibleFlag) && (tutorialFlag == true) &&
                    (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7)||
                    (Input.GetKeyDown(KeyCode.D)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveRightPossibleFlag) && (tutorialFlag == true) &&
                    (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6))
                {
                    if (tutorialImageFlag == true)
                    {
                        if (tutorialCount == 60)
                        {
                            tutorialImageFlag = false;
                            tutorialTurn++;
                            tutorialCount = 0;
                        }
                    }
                    else
                    {
                        // 登りフラグが真なら
                        if (AliceClimb1Dicision(Player.MoveDirection.RIGHT))
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.UP;
                            alice.inputKeyFlag = true;
                            print("上移動");// デバッグ用コメント
                        }
                        else if (AliceClimb2Dicision(Player.MoveDirection.RIGHT))
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.DOWN;
                            alice.inputKeyFlag = true;
                            print("下移動");// デバッグ用コメント
                        }
                        else
                        {
                            action = PlayerAction.NEXT;     // 行動を進むに
                            turn = Turn.PLAYER;             // ターンをプレイヤーに
                            turnCountGimmick = 0;           // カウントを０に

                            alice.moveDirection = Player.MoveDirection.RIGHT;
                            alice.inputKeyFlag = true;
                            print("右移動");// デバッグ用コメント
                        }
                    }
                }

                // ▼待機処理
                // Ｘキーが押された時、行動が無しなら///////////////////////////////////////////////////////
                if ((Input.GetKeyDown(KeyCode.X)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (tutorialFlag == false) ||
                    (Input.GetKeyDown(KeyCode.X)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (tutorialFlag == true) && (stageNumber == 2) &&
                    (tutorialTurn == 0))
                {
                    action = PlayerAction.NEXT;     // 行動を進むに
                    turn = Turn.PLAYER;             // ターンをプレイヤーに
                    turnCountGimmick = 0;           // カウントを０に

                    alice.moveDirection = Player.MoveDirection.STOP;
                    alice.inputKeyFlag = true;
                    print("待機");// デバッグ用コメント
                }

                // ▼巻き戻し処理
                // Ｑキーが押された時、行動が無しなら///////////////////////////////////////////////////////
                if ((Input.GetKeyDown(KeyCode.Q)) && (action == PlayerAction.NONE) && (alice.saveCount > 0) && (tutorialFlag == false) ||
                    (Input.GetKeyDown(KeyCode.Q)) && (action == PlayerAction.NONE) && (alice.saveCount > 0) && (tutorialFlag == true) && (stageNumber == 2) &&
                    (tutorialTurn == 5 || tutorialTurn == 7))
                {
                    action = PlayerAction.RETURN;   // 行動を戻るに
                    turn = Turn.GIMMICK;            // ターンをギミックに
                    turnCountGimmick = 0;           // カウントを０に
                    alice.moveReturnFlag = true;    // 巻き戻しフラグを真に

                    print("巻き戻し");// デバッグ用コメント
                }

                // ▼早送り処理
                // Ｅキーが押された時、行動が無しなら//////////////////////////////
                if ((Input.GetKeyDown(KeyCode.E)) && (action == PlayerAction.NONE))
                {
                    if (alice.saveMoveDirection[alice.saveCount] != Player.MoveDirection.NONE)
                    {
                        action = PlayerAction.NEXT; // 行動を進むに
                        turn = Turn.PLAYER;         // ターンをプレイヤーに
                        turnCountGimmick = 0;
                        alice.moveNextFlag = true;
                        alice.inputKeyFlag = true;

                        print("早送り");// デバッグ用コメント
                    }
                }
            }
        }
    }

    // ★アリスの移動可能範囲の判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void AliceMovePossibleDecision()
    {
        alice.moveFrontPossibleFlag = stage.MovePossibleDecision(alice, Player.MoveDirection.FRONT, camera.cameraAngle);    // 前移動可能判定
        alice.moveBackPossibleFlag = stage.MovePossibleDecision(alice, Player.MoveDirection.BACK,camera.cameraAngle);       // 後移動可能判定
        alice.moveLeftPossibleFlag = stage.MovePossibleDecision(alice, Player.MoveDirection.LEFT, camera.cameraAngle);      // 左移動可能判定
        alice.moveRightPossibleFlag = stage.MovePossibleDecision(alice, Player.MoveDirection.RIGHT, camera.cameraAngle);    // 右移動可能判定
    }

    // ★アリスの登り判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    bool AliceClimb1Dicision(Player.MoveDirection direction)
    {
        bool flag = false;

        if(alice.climb1Flag)
        {
            switch(direction)
            {
                case Player.MoveDirection.FRONT:
                    if (((camera.cameraAngle == PlayerCamera.CameraAngle.FRONT) && (alice.playerAngle == Player.PlayerAngle.BACK)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.BACK) && (alice.playerAngle == Player.PlayerAngle.FRONT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.LEFT) && (alice.playerAngle == Player.PlayerAngle.RIGHT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.RIGHT) && (alice.playerAngle == Player.PlayerAngle.LEFT)))
                    {
                        flag = true;
                    }
                    break;
                case Player.MoveDirection.BACK:
                    if (((camera.cameraAngle == PlayerCamera.CameraAngle.FRONT) && (alice.playerAngle == Player.PlayerAngle.FRONT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.BACK) && (alice.playerAngle == Player.PlayerAngle.BACK)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.LEFT) && (alice.playerAngle == Player.PlayerAngle.LEFT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.RIGHT) && (alice.playerAngle == Player.PlayerAngle.RIGHT)))
                    {
                        flag = true;
                    }
                    break;
                case Player.MoveDirection.LEFT:
                    if(((camera.cameraAngle == PlayerCamera.CameraAngle.FRONT) && (alice.playerAngle == Player.PlayerAngle.RIGHT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.BACK) && (alice.playerAngle == Player.PlayerAngle.LEFT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.LEFT) && (alice.playerAngle == Player.PlayerAngle.FRONT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.RIGHT) && (alice.playerAngle == Player.PlayerAngle.BACK)))
                    {
                        flag = true;
                    }
                    break;
                case Player.MoveDirection.RIGHT:
                    if(((camera.cameraAngle == PlayerCamera.CameraAngle.FRONT) && (alice.playerAngle == Player.PlayerAngle.LEFT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.BACK) && (alice.playerAngle == Player.PlayerAngle.RIGHT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.LEFT) && (alice.playerAngle == Player.PlayerAngle.BACK)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.RIGHT) && (alice.playerAngle == Player.PlayerAngle.FRONT)))
                    {
                        flag = true;
                    }
                    break;
            }
        }

        return flag;
    }

    // ★アリスの登り判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    bool AliceClimb2Dicision(Player.MoveDirection direction)
    {
        bool flag = false;

        if (alice.climb2Flag)
        {
            switch (direction)
            {
                case Player.MoveDirection.FRONT:
                    if (((camera.cameraAngle == PlayerCamera.CameraAngle.FRONT) && (alice.playerAngle == Player.PlayerAngle.FRONT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.BACK) && (alice.playerAngle == Player.PlayerAngle.BACK)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.LEFT) && (alice.playerAngle == Player.PlayerAngle.LEFT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.RIGHT) && (alice.playerAngle == Player.PlayerAngle.RIGHT)))
                    {
                        flag = true;
                    }
                    break;
                case Player.MoveDirection.BACK:
                    if (((camera.cameraAngle == PlayerCamera.CameraAngle.FRONT) && (alice.playerAngle == Player.PlayerAngle.BACK)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.BACK) && (alice.playerAngle == Player.PlayerAngle.FRONT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.LEFT) && (alice.playerAngle == Player.PlayerAngle.RIGHT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.RIGHT) && (alice.playerAngle == Player.PlayerAngle.LEFT)))
                    {
                        flag = true;
                    }
                    break;
                case Player.MoveDirection.LEFT:
                    if (((camera.cameraAngle == PlayerCamera.CameraAngle.FRONT) && (alice.playerAngle == Player.PlayerAngle.LEFT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.BACK) && (alice.playerAngle == Player.PlayerAngle.RIGHT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.LEFT) && (alice.playerAngle == Player.PlayerAngle.BACK)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.RIGHT) && (alice.playerAngle == Player.PlayerAngle.FRONT)))
                    {
                        flag = true;
                    }
                    break;
                case Player.MoveDirection.RIGHT:
                    if (((camera.cameraAngle == PlayerCamera.CameraAngle.FRONT) && (alice.playerAngle == Player.PlayerAngle.RIGHT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.BACK) && (alice.playerAngle == Player.PlayerAngle.LEFT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.LEFT) && (alice.playerAngle == Player.PlayerAngle.FRONT)) ||
                        ((camera.cameraAngle == PlayerCamera.CameraAngle.RIGHT) && (alice.playerAngle == Player.PlayerAngle.BACK)))
                    {
                        flag = true;
                    }
                    break;
            }
        }

        return flag;
    }

    // ★カメラの左回転★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void CameraTurnLeftMove() { cameraTurnLeftEvent(); }

    // ★カメラの右回転★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void CameraTurnRightMove() { cameraTurnRightEvent(); }

    // ★キー入力によるプレイヤーの移動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void InputPlayerMove() { inputPlayerMoveEvent(); }

    // ★キー入力によるプレイヤーの巻き戻し★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void InputPlayerMoveReturn() { inputPlayerMoveReturnEvent(); }

    // ★キー入力によるプレイヤーの早送り★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void InputPlayerMoveNext() { inputPlayerMoveNextEvent(); }

    // ★自動移動によるプレイヤーの移動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void AutoPlayerMove() { autoPlayerMoveEvent(); }
}
