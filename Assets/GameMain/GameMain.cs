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

    public AliceActionNotifer aliceActionNotifer;   // ギミックホルダー

    public PlayerCamera camera; // カメラ
    public Player alice;        // プレイヤー
    public Stage stage;         // ステージ
    public int turnNum;         // ターン数

    public PlayerAction action;     // 行動が何か判断する
    public Turn turn;               // 誰のターンか判断する
    public int turnCountGimmick;    // ターンの時間稼ぎ

    public int beforeStageNumber; //デバッグ用変数
    public int stageNumber;
	public GameObject pause; // ポーズ
    //チュートリアルに必要な変数
    public bool tutorialFlag;      //チュートリアルか判断する
    public int tutorialTurn;       //チュートリアルのターン数
    public bool tutorialImageFlag;  //説明画像が出ているかどうか
    public int tutorialCount;      //チュートリアル中のカウント
    public float getVol;           //BGMの音量を送る
    public GameObject ImageUI;

    public WatchHandAnimation watchHand;
    public int limitTurn;
    public int waitingTime;
    public GameObject CharacterTaklText;

    public TouchController touchController;      //Androidのタッチ用クラス

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        getVol = PlayerPrefs.GetFloat("VALUE");
		pause = GameObject.Find("Pause");
        GameSetting();  // ゲームの設定
        Singleton<SoundPlayer>.instance.playBGM("Gbgm01", 2.0f, false, getVol);

	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update()
	{
        //音楽フェード
        Singleton<SoundPlayer>.instance.update();
        //-----------------------------------------------------
        //松村脩平変更点
        //デバッグ機能(ステージ番号を変更後、ステージ変更される)
        if(beforeStageNumber != stageNumber)
        {
            ChangeTextName();
            PlayerPrefs.SetInt("Debug",1);
            PlayerPrefs.SetInt("StageNumber", stageNumber);

            beforeStageNumber = stageNumber;
            if(stageNumber == 1||stageNumber == 2)
            {
                CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("TutorialMainScene"); });
            }
            else
            {
                CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("GameMainScene"); });
            }
           
        }
        //-----------------------------------------------------


		if (pause.GetComponent<Pause>().pauseFlag == false)
		{

            //pause起動
            if (((touchController.touchPosX > 0) && (touchController.touchPosX < 320)) &&
               ((touchController.touchPosY > 500) && (touchController.touchPosY < 720)) &&
               ((touchController.detachPosX > 0) && (touchController.detachPosX < 320)) &&
               ((touchController.detachPosY > 500) && (touchController.detachPosY < 720)) && tutorialFlag == false)
            {
                touchController.TouchPostionInitialize();
                //pause.GetComponent<Pause>().pauseFlag = true;
                pause.GetComponent<Pause>().pauseImageManager1.GetComponent<Image>().enabled = true;
                pause.GetComponent<Pause>().pauseImageManager2.GetComponent<Image>().enabled = true;
                pause.GetComponent<Pause>().pauseImageManager3.GetComponent<Image>().enabled = true;
                pause.GetComponent<Pause>().pauseImageManager4.GetComponent<Image>().enabled = true;
                pause.GetComponent<Pause>().pauseImageManager5.GetComponent<Image>().enabled = true;
                pause.GetComponent<Pause>().pauseImageManager6.GetComponent<Image>().enabled = true;
                pause.GetComponent<Pause>().Initialize();
            }

			MapCamera();    // マップカメラ
			
			PlayerMove();   // プレイヤーの移動
            CameraTurn();   // カメラの回転
            GameAction();   // 行動を行う

			if ((stageNumber == 1) ||
				(stageNumber == 2))
			{
				waitingTime++;
				if (tutorialImageFlag == true)
				{
					if (stageNumber == 1 && ((tutorialTurn == 2 && waitingTime > 140) || tutorialTurn == 4 || (tutorialTurn == 6 && waitingTime > 120) || tutorialTurn == 7) ||
						stageNumber == 2 & (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6))
					{
						if (tutorialCount > 1 && tutorialCount < 11)
							ImageUI.GetComponent<Image>().material.color = new Color(1.0f, 1.0f, 1.0f, tutorialCount / 10.0f);

						if (tutorialCount < 60)
						{
							tutorialCount++;
						}
					}

				}
				else if (tutorialImageFlag == false)
				{
					ImageUI.GetComponent<Image>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

					camera.GetComponent<PlayerCamera>().mapCameraFlag = true;
					camera.GetComponent<PlayerCamera>().SwitchingMapCamera();
				}

				if (stageNumber == 1 && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7))
				{
					tutorialImageFlag = true;
					camera.GetComponent<PlayerCamera>().mapCameraFlag = false;
					camera.GetComponent<PlayerCamera>().SwitchingMapCamera();
				}
				else if (stageNumber == 2 && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6))
				{
					tutorialImageFlag = true;
					camera.GetComponent<PlayerCamera>().mapCameraFlag = false;
					camera.GetComponent<PlayerCamera>().SwitchingMapCamera();
				}
			}

            if (touchController.detachPosX != 0)
            {
                touchController.TouchPostionInitialize();
            }

		}
        else if (pause.GetComponent<Pause>().pauseFlag == true && tutorialFlag == false)
        {

            if(touchController.detachPosX != 0)
            {
                //ゲームへもどる
                if (((touchController.touchPosX > 275) && (touchController.touchPosX < 650)) &&
                    ((touchController.touchPosY > 470) && (touchController.touchPosY < 545)) &&
                    ((touchController.detachPosX > 275) && (touchController.detachPosX < 650)) &&
                    ((touchController.detachPosY > 470) && (touchController.detachPosY < 545)))
                {
                    pause.GetComponent<Pause>().pauseImageManager1.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager2.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager3.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager4.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager5.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager6.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().EscapePause();
                    pause.GetComponent<Pause>().pauseImageManager6.transform.localPosition = new Vector3(80, 140, 0);
               
                    touchController.TouchPostionInitialize();
                }
                //はじめから
                else if (((touchController.touchPosX > 275) && (touchController.touchPosX < 650)) &&
                    ((touchController.touchPosY > 315) && (touchController.touchPosY < 390)) &&
                    ((touchController.detachPosX > 275) && (touchController.detachPosX < 650)) &&
                    ((touchController.detachPosY > 315) && (touchController.detachPosY < 390)))
                {
                    pause.GetComponent<Pause>().EscapePause();
                    Application.LoadLevel("GameMainScene");
                    CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("GameMainScene"); });
                    touchController.TouchPostionInitialize();
                    pause.GetComponent<Pause>().pauseImageManager6.transform.localPosition = new Vector3(80, -10, 0);
               
                }
                //ステージセレクト
                else if (((touchController.touchPosX > 275) && (touchController.touchPosX < 650)) &&
                    ((touchController.touchPosY > 170) && (touchController.touchPosY < 240)) &&
                    ((touchController.detachPosX > 275) && (touchController.detachPosX < 650)) &&
                    ((touchController.detachPosY > 170) && (touchController.detachPosY < 240)))
                {
                    pause.GetComponent<Pause>().EscapePause();
                    Application.LoadLevel("StageSelectScene");
                    CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });
                    touchController.TouchPostionInitialize();
                    pause.GetComponent<Pause>().pauseImageManager6.transform.localPosition = new Vector3(80, -160, 0);
                
                }
                else
                {
                    pause.GetComponent<Pause>().pauseImageManager1.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager2.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager3.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager4.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager5.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().pauseImageManager6.GetComponent<Image>().enabled = false;
                    pause.GetComponent<Pause>().EscapePause();
                    touchController.TouchPostionInitialize();
                }
            }
            
        }
	}

    // ★ゲームの設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void GameSetting()
    {
		CameraFade.StartAlphaFade(Color.black, true, 1.0f, 0.5f);

        action = PlayerAction.NONE; // 行動に無しを設定
        turn = Turn.NONE;           // ターンに無しを設定
        turnCountGimmick = 0;       // カウントを０に

        //--------------------------------------
        //松村脩平変更点
        //デバッグ用ステージ変更
        if(PlayerPrefs.GetInt("Debug") == 1)
        {
            stageNumber = PlayerPrefs.GetInt("StageNumber");
            PlayerPrefs.SetInt("Debug", 0);
        }
        else
        {
            stageNumber = PlayerPrefs.GetInt("STAGE_NUM");
        }

        ChangeTextName();
        beforeStageNumber = stageNumber;
        //---------------------------------------
        stage.setSelectStage(stageNumber);  // 選択されたステージを設定
        stage.CreateStage();                // ステージの生成
        turnNum = stage.getStageTurnNum();  // ターン数の取得
        limitTurn = stage.getStageTurnNum();
		pause = GameObject.Find("Pause");
        aliceActionNotifer = new AliceActionNotifer();
        aliceActionNotifer.GimmickArray = GameObject.FindGameObjectsWithTag("Gimmick");

        alice.transform.position = stage.getStartPosition();    // アリスの座標を設定
        alice.arrayPosX = stage.getStartArrayPosition('x');     // アリスの配列上の座標Ｘを設定
        alice.arrayPosY = stage.getStartArrayPosition('y');     // アリスの配列上の座標Ｙを設定
        alice.arrayPosZ = stage.getStartArrayPosition('z');     // アリスの配列上の座標Ｚを設定
        alice.moveCount = turnNum;// アリスの移動数にステージのターン数を設定

        //チュートリアルに必要な変数
        if (stageNumber == 1 || stageNumber == 2)
        {
            waitingTime = 0;
            tutorialFlag = true;      //チュートリアルか判断する
            tutorialTurn = 0;       //チュートリアルのターン数
            tutorialImageFlag = false;  //説明画像が出ているかどうか
            tutorialCount = 0;      //チュートリアル中のカウント
        }
        if (tutorialFlag == true)
        {
            ImageUI = GameObject.Find("EXImage");
            CharacterTaklText = GameObject.Find("CharacterTaklText");
            ImageUI.GetComponent<Image>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        //画面から指が離れたら
        if(touchController.detachPosX != 0)
        {
            //初期化
            touchController.TouchPostionInitialize();
        }
    }

    // ★行動★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void GameAction()
    {
        // ▽現在の行動が
        switch (action)
        {
            // ▼無しなら
            case PlayerAction.NONE:
                if(GameObject.Find("Camera").GetComponent<PlayerCamera>().clearFlag)
                {
                    alice.arrowDrawFlag = false;
                    alice.SetAnimation(Player.Motion.GAMECLEAR, true);
                }
                else if (alice.gameOverFlag)
                {
                    alice.arrowDrawFlag = false;
                    alice.SetAnimation(Player.Motion.GAMEOVER, true);
                }
                else
                {
                    alice.arrowDrawFlag = true;
                }
                break;

            // ▼進むなら
            case PlayerAction.NEXT:
                // ターンがプレイヤーなら
                if (turn == Turn.PLAYER)
                {
                    if (alice.gameOverFlag == false)
                    {

                        // プレイヤーの移動
                        if (alice.moveFlag == false && alice.moveFinishFlag == false) { InputPlayerMove(); }

                        // プレイヤーの移動が完了したら
                        if (alice.moveFinishFlag == true)
                        {
                            stage.ChangeArrayGimmickNext();
                            stage.GimmickDecision(alice, Player.PlayerAction.NEXT);   // ギミックとの判定
                            stage.FootDecision(alice, Player.PlayerAction.NEXT);      // 足元との判定
                            alice.moveFinishFlag = false;   // 移動完了フラグを偽に

                            if (!alice.autoMoveFlag)
                            {
                                turn = Turn.GIMMICK;                                // ターンをギミックに
                                aliceActionNotifer.NotiferNext(alice.turnCount);    // ギミックを次の段階へ

                                if(alice.autoMoveFlag)
                                {
                                    turn = Turn.PLAYER;
                                    AutoPlayerMove();
                                }
                            }
                        }
                    }
                }
                // ターンがギミックなら
                else if (turn == Turn.GIMMICK)
                {
                    turnCountGimmick++; // カウントを増やす
                    stage.ChangeArrayGimmickNext();//条件が一致した場合のみ処理を呼ぶ関数


                    // カウントが６０になったら
                    if (turnCountGimmick == 60)
                    {
                        //stage.FlowerDecision(alice);      // 足元との判定
                        stage.SearchRockFallAgain();
                        action = PlayerAction.NONE;   // 行動を無しに
                        turn = Turn.NONE;       // ターンを無しに
                        turnNum--;

                        // ターン数が０になるかアリスが地面に着いていたら
                        if(turnNum == 0 || alice.arrayPosY == 0)
                        {
                            alice.gameOverFlag = true;
                        }

                        watchHand.NextTurn();
                        print("ターン終了");// デバッグ用コメント
                        if (tutorialFlag == true)
                        {
                            waitingTime = 0;
                            tutorialTurn++;
                        }
                        stage.FlowerFootDecision(alice);
                    }
                }
                break;

            // ▼戻るなら
            case PlayerAction.RETURN:
                // ターンがギミックなら
                if (turn == Turn.GIMMICK)
                {
                    if (turnCountGimmick == 0 && !alice.autoMoveFlag)
                    {
                        
                        stage.ChangeArrayGimmickReturn();     // 一部ギミックの配列上の位置を変更
                        aliceActionNotifer.NotiferReturn(alice.turnCount);
                    }

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
                        //巻き戻し移動後、アリスの下が穴であったとき、
                        if (stage.GetFootHole(alice) == true && PlayerAction.RETURN == action)
                        {
                            //処理しないでフラグを戻す
                           
                            alice.moveFinishFlag = false;
                           // alice.arrowDrawFlag = false;
                        }
                        else
                        {
                            alice.moveFinishFlag = false;   // 移動完了フラグを偽に

                        }
                       
                        if (!alice.autoMoveFlag)
                        {
                            action = PlayerAction.NONE;     // 行動を無しに
                            turn = Turn.NONE;               // ターンを無しに
                            turnNum++;
                            watchHand.BackTurn();
                            if (tutorialFlag == true)
                            {
                                waitingTime = 0;
                                tutorialTurn++;

                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 8)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(3);
                                }
                            }
                            stage.GimmickDecision(alice,Player.PlayerAction.RETURN);   // ギミックとの判定
                            stage.FootDecision(alice, Player.PlayerAction.RETURN);   // ギミックとの判定
                            
                                                        
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

        float HorizontalKeyInput = Input.GetAxis("HorizontalKey");

        if (turn == Turn.NONE)
        {
            if (tutorialFlag == false ||
                (tutorialFlag == true && stageNumber == 1 && (tutorialTurn == 0 || tutorialTurn == 1)))
            {
                // 矢印左を押したら(カメラ移動左回転)
                if ((Input.GetKeyDown(KeyCode.LeftArrow)) || (HorizontalKeyInput < -0.9f))
                {
                    CameraTurnLeftMove();
                    print("カメラ左回転");// デバッグ用コメント

                    alice.arrowDrawFlag = false;
                }

                if (touchController.detachPosX != 0 && touchController.detachPosY != 0)
                {
                    
                    //右めくり
                    if (touchController.touchPosX - touchController.detachPosX < -180)
                    {
                        CameraTurnLeftMove();
                        touchController.TouchPostionInitialize();

                    }
                }

            }

            if (tutorialFlag == false)
            {
                // 矢印右を押したら(カメラ移動右回転)
                if ((Input.GetKeyDown(KeyCode.RightArrow)) || (HorizontalKeyInput > 0.9f))
                {
                    CameraTurnRightMove();
                    print("カメラ右回転");// デバッグ用コメント

                    alice.arrowDrawFlag = false;
                }
                if (touchController.detachPosX != 0 && touchController.detachPosY != 0)
                {
                    //ページ移動判定
                    //左めくり
                    //右めくり
                    if (touchController.touchPosX - touchController.detachPosX > +180)
                    {
                   
                        CameraTurnRightMove();
                        touchController.TouchPostionInitialize();
                    }
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
                alice.arrowDrawFlag = false;

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
            else if (camera.clearFlag == false)
            {
                float TrigerInput = 0.0f;
                TrigerInput = Input.GetAxis("Triger");

                // ▼画面奥方向移動処理
                // Ｗキーが押された時、行動が無しなら////////////////////////////////////////////////////////////////////////////////////////
                if (((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.Joystick1Button3))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveFrontPossibleFlag) && (tutorialFlag == false) ||
                    ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.Joystick1Button3))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveFrontPossibleFlag) && (tutorialFlag == true) &&
                    (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7) ||
                    ((Input.GetKeyDown(KeyCode.W)) || (Input.GetKeyDown(KeyCode.Joystick1Button3))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveFrontPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 2 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 8 || tutorialTurn == 9 || tutorialTurn == 10) ||
                    ((touchController.touchPosX > 520) && (touchController.touchPosX < 705)) &&
                    ((touchController.touchPosY > 290) && (touchController.touchPosY < 530)) &&
                    ((touchController.detachPosX > 520) && (touchController.detachPosX < 705)) &&
                    ((touchController.detachPosY > 290) && (touchController.detachPosY < 530)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveFrontPossibleFlag) && (tutorialFlag == false) ||
                     ((touchController.touchPosX > 520) && (touchController.touchPosX < 705)) &&
                    ((touchController.touchPosY > 290) && (touchController.touchPosY < 530)) &&
                    ((touchController.detachPosX > 520) && (touchController.detachPosX < 705)) &&
                    ((touchController.detachPosY > 290) && (touchController.detachPosY < 530)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveFrontPossibleFlag) && (tutorialFlag == true) &&
                    (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7) ||
                     ((touchController.touchPosX > 520) && (touchController.touchPosX < 705)) &&
                    ((touchController.touchPosY > 290) && (touchController.touchPosY < 530)) &&
                    ((touchController.detachPosX > 520) && (touchController.detachPosX < 705)) &&
                    ((touchController.detachPosY > 290) && (touchController.detachPosY < 530)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveFrontPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 2 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 8 || tutorialTurn == 9 || tutorialTurn == 10))
                {
                    alice.arrowDrawFlag = false;
                    touchController.TouchPostionInitialize();

                    if (tutorialImageFlag == true)
                    {
                        if (tutorialCount == 60)
                        {
                            tutorialImageFlag = false;
                            waitingTime = 0;
                            tutorialTurn++;
                            tutorialCount = 0;
                            if (stageNumber == 1)
                            {
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 5)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(5);
                                }
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 8)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(7);
                                }
                            }
                            else if (stageNumber == 2)
                            {
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 2)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(1);
                                }
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 4)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(2);
                                }
                            }

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
                            alice.SetAnimation(Player.Motion.CLIMB, true);
                            alice.SetAnimation(Player.Motion.CLIMB_START, false);
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
                            waitingTime = 0;
                            alice.moveDirection = Player.MoveDirection.FRONT;
                            alice.inputKeyFlag = true;
                            alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 0.5f, alice.GetComponent<Player>().arrayPosZ);
                            alice.SetAnimation(Player.Motion.CLIMB_START, false);
                            alice.SetAnimation(Player.Motion.CLIMB, false);
                            alice.SetAnimation(Player.Motion.WALK_NEXT, true);
                            print("前移動");// デバッグ用コメント
                        }
                    }
                }

                // ▼画面手前方向移動処理
                // Ｓキーが押された時、行動が無しなら///////////////////////////////////////////////////////////////////////////////////////
                if (((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Joystick1Button0))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveBackPossibleFlag) && (tutorialFlag == false) ||
                     ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Joystick1Button0))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveBackPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 5 || tutorialTurn == 6 || tutorialTurn == 7) ||
                     ((Input.GetKeyDown(KeyCode.S)) || (Input.GetKeyDown(KeyCode.Joystick1Button0))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveBackPossibleFlag) && (tutorialFlag == true) &&
                      (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6) ||
                     ((touchController.touchPosX > 570) && (touchController.touchPosX < 805)) &&
                     ((touchController.touchPosY > 60) && (touchController.touchPosY < 220)) &&
                     ((touchController.detachPosX > 570) && (touchController.detachPosX < 805)) &&
                     ((touchController.detachPosY > 60) && (touchController.detachPosY < 220)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveBackPossibleFlag) && (tutorialFlag == false) ||
                      ((touchController.touchPosX > 570) && (touchController.touchPosX < 805)) &&
                     ((touchController.touchPosY > 60) && (touchController.touchPosY < 220)) &&
                     ((touchController.detachPosX > 570) && (touchController.detachPosX < 805)) &&
                     ((touchController.detachPosY > 60) && (touchController.detachPosY < 220)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveBackPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 5 || tutorialTurn == 6 || tutorialTurn == 7) ||
                      ((touchController.touchPosX > 570) && (touchController.touchPosX < 805)) &&
                     ((touchController.touchPosY > 60) && (touchController.touchPosY < 220)) &&
                     ((touchController.detachPosX > 570) && (touchController.detachPosX < 805)) &&
                     ((touchController.detachPosY > 60) && (touchController.detachPosY < 220)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveBackPossibleFlag) && (tutorialFlag == true) &&
                      (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6))
                {
                    alice.arrowDrawFlag = false;
                    touchController.TouchPostionInitialize();

                    if (tutorialImageFlag == true)
                    {
                        if (tutorialCount == 60)
                        {
                            tutorialImageFlag = false;
                            waitingTime = 0;
                            tutorialTurn++;
                            tutorialCount = 0;
                            if (stageNumber == 1)
                            {
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 5)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(5);
                                }
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 8)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(7);
                                }
                            }
                            else if (stageNumber == 2)
                            {
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 2)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(1);
                                }
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 4)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(2);
                                }
                            }
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
                            alice.SetAnimation(Player.Motion.CLIMB, true);
                            alice.SetAnimation(Player.Motion.CLIMB_START, false);
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
                            waitingTime = 0;
                            alice.moveDirection = Player.MoveDirection.BACK;
                            alice.inputKeyFlag = true;
                            alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 0.5f, alice.GetComponent<Player>().arrayPosZ);
                            alice.SetAnimation(Player.Motion.CLIMB_START, false);
                            alice.SetAnimation(Player.Motion.CLIMB, false);
                            alice.SetAnimation(Player.Motion.WALK_NEXT, true);
                            print("後移動");// デバッグ用コメント
                        }
                    }
                }

               // ▼画面左方向移動処理
                // Ａキーが押された時、行動が無しなら///////////////////////////////////////////////////////////////////////////////////////
                if (((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.Joystick1Button2))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveLeftPossibleFlag) && (tutorialFlag == false) ||
                     ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.Joystick1Button2))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveLeftPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7 || tutorialTurn == 8) ||
                     ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.Joystick1Button2))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveLeftPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6) ||
                     ((touchController.touchPosX > 380) && (touchController.touchPosX < 565)) &&
                     ((touchController.touchPosY > 170) && (touchController.touchPosY < 385)) &&
                     ((touchController.detachPosX > 380) && (touchController.detachPosX < 565)) &&
                     ((touchController.detachPosY > 170) && (touchController.detachPosY < 385)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveLeftPossibleFlag) && (tutorialFlag == false) ||
                     ((touchController.touchPosX > 380) && (touchController.touchPosX < 565)) &&
                     ((touchController.touchPosY > 170) && (touchController.touchPosY < 385)) &&
                     ((touchController.detachPosX > 380) && (touchController.detachPosX < 565)) &&
                     ((touchController.detachPosY > 170) && (touchController.detachPosY < 385)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveLeftPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7 || tutorialTurn == 8) ||
                     ((touchController.touchPosX > 380) && (touchController.touchPosX < 565)) &&
                     ((touchController.touchPosY > 170) && (touchController.touchPosY < 385)) &&
                     ((touchController.detachPosX > 380) && (touchController.detachPosX < 565)) &&
                     ((touchController.detachPosY > 170) && (touchController.detachPosY < 385)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveLeftPossibleFlag) && (tutorialFlag == true) &&
                     (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6))
                {
                    alice.arrowDrawFlag = false;
                    touchController.TouchPostionInitialize();
                    if (tutorialImageFlag == true)
                    {
                        if (tutorialCount == 60)
                        {
                            tutorialImageFlag = false;
                            waitingTime = 0;
                            tutorialTurn++;
                            tutorialCount = 0;
                            if (stageNumber == 1)
                            {
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 5)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(5);
                                }
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 8)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(7);
                                }
                            }
                            else if (stageNumber == 2)
                            {
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 2)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(1);
                                }
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 4)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(2);
                                }
                            }
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
                            alice.SetAnimation(Player.Motion.CLIMB, true);
                            alice.SetAnimation(Player.Motion.CLIMB_START, false);
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
                            waitingTime = 0;
                            alice.moveDirection = Player.MoveDirection.LEFT;
                            alice.inputKeyFlag = true;
                            alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 0.5f, alice.GetComponent<Player>().arrayPosZ);
                            alice.SetAnimation(Player.Motion.CLIMB_START, false);
                            alice.SetAnimation(Player.Motion.CLIMB, false);
                            alice.SetAnimation(Player.Motion.WALK_NEXT, true);
                            print("左移動");// デバッグ用コメント
                        }
                    }
                }

                 // ▼画面右方向移動処理
                // Ｄキーが押された時、行動が無しなら///////////////////////////////////////////////////////
                if (((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.Joystick1Button1))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveRightPossibleFlag) && (tutorialFlag == false) ||
                     ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.Joystick1Button1))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveRightPossibleFlag) && (tutorialFlag == true) &&
                    (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7) ||
                    ((Input.GetKeyDown(KeyCode.D)) || (Input.GetKeyDown(KeyCode.Joystick1Button1))) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveRightPossibleFlag) && (tutorialFlag == true) &&
                    (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6) ||
                    ((touchController.touchPosX > 705) && (touchController.touchPosX < 915)) &&
                    ((touchController.touchPosY > 220) && (touchController.touchPosY < 450)) &&
                    ((touchController.detachPosX > 705) && (touchController.detachPosX < 915)) &&
                    ((touchController.detachPosY > 220) && (touchController.detachPosY < 450)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveRightPossibleFlag) && (tutorialFlag == false) ||
                    ((touchController.touchPosX > 705) && (touchController.touchPosX < 915)) &&
                    ((touchController.touchPosY > 220) && (touchController.touchPosY < 450)) &&
                    ((touchController.detachPosX > 705) && (touchController.detachPosX < 915)) &&
                    ((touchController.detachPosY > 220) && (touchController.detachPosY < 450)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveRightPossibleFlag) && (tutorialFlag == true) &&
                    (stageNumber == 1) && (tutorialTurn == 2 || tutorialTurn == 4 || tutorialTurn == 6 || tutorialTurn == 7) ||
                    ((touchController.touchPosX > 705) && (touchController.touchPosX < 915)) &&
                    ((touchController.touchPosY > 220) && (touchController.touchPosY < 450)) &&
                    ((touchController.detachPosX > 705) && (touchController.detachPosX < 915)) &&
                    ((touchController.detachPosY > 220) && (touchController.detachPosY < 450)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (alice.moveRightPossibleFlag) && (tutorialFlag == true) &&
                    (stageNumber == 2) && (tutorialTurn == 1 || tutorialTurn == 3 || tutorialTurn == 4 || tutorialTurn == 6))
                {
                    alice.arrowDrawFlag = false;
                    touchController.TouchPostionInitialize();
                    if (tutorialImageFlag == true)
                    {
                        if (tutorialCount == 60)
                        {
                            tutorialImageFlag = false;
                            waitingTime = 0;
                            tutorialTurn++;
                            tutorialCount = 0;
                            if (stageNumber == 1)
                            {
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 5)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(5);
                                }
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 8)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(7);
                                }
                            }
                            else if (stageNumber == 2)
                            {
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 2)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(1);
                                }
                                if (GameObject.Find("GameMain").GetComponent<GameMain>().tutorialTurn == 4)
                                {
                                    GameObject.Find("CharacterTaklText").GetComponent<ChangeText>().TutorialNextNumber(2);
                                }
                            }
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
                            alice.SetAnimation(Player.Motion.CLIMB, true);
                            alice.SetAnimation(Player.Motion.CLIMB_START, false);
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
                            waitingTime = 0;
                            alice.moveDirection = Player.MoveDirection.RIGHT;
                            alice.inputKeyFlag = true;
                            alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 0.5f, alice.GetComponent<Player>().arrayPosZ);
                            alice.SetAnimation(Player.Motion.CLIMB_START, false);
                            alice.SetAnimation(Player.Motion.CLIMB, false);
                            alice.SetAnimation(Player.Motion.WALK_NEXT, true);
                            print("右移動");// デバッグ用コメント
                        }
                    }
                }

                
               
                 
                // ▼待機処理
                // Ｘキーが押された時、行動が無しなら///////////////////////////////////////////////////////
                if (((Input.GetKeyDown(KeyCode.X)) || (TrigerInput < -0.007f)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (tutorialFlag == false) && (alice.gameOverFlag == false) && (alice.arrayPosY != 0) ||
                    ((Input.GetKeyDown(KeyCode.X)) || (TrigerInput < -0.007f)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (tutorialFlag == true) && (stageNumber == 2) &&
                    (tutorialTurn == 0) ||
                    ((touchController.touchPosX > 1010) && (touchController.touchPosX < 1280)) &&
                    ((touchController.touchPosY > 0) && (touchController.touchPosY < 320)) &&
                    ((touchController.detachPosX > 1010) && (touchController.detachPosX < 1280)) &&
                    ((touchController.detachPosY > 0) && (touchController.detachPosY < 320)) &&
                    ((touchController.touchPosX + 30 > touchController.detachPosX) && (touchController.touchPosX - 30 < touchController.detachPosX)) &&
                    ((touchController.touchPosY + 30 > touchController.detachPosY) && (touchController.touchPosY - 30 < touchController.detachPosY)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (tutorialFlag == false) && (alice.gameOverFlag == false) ||
                    ((touchController.touchPosX > 1010) && (touchController.touchPosX < 1280)) &&
                    ((touchController.touchPosY > 0) && (touchController.touchPosY < 320)) &&
                    ((touchController.detachPosX > 1010) && (touchController.detachPosX < 1280)) &&
                    ((touchController.detachPosY > 0) && (touchController.detachPosY < 320)) &&
                    ((touchController.touchPosX + 30 > touchController.detachPosX) && (touchController.touchPosX - 30 < touchController.detachPosX)) &&
                    ((touchController.touchPosY + 30 > touchController.detachPosY) && (touchController.touchPosY - 30 < touchController.detachPosY)) && (action == PlayerAction.NONE) && (alice.moveCount > 0) && (tutorialFlag == true) && (stageNumber == 2) &&
                    (tutorialTurn == 0))
                {

                    touchController.TouchPostionInitialize();
                    action = PlayerAction.NEXT;     // 行動を進むに
                    turn = Turn.PLAYER;             // ターンをプレイヤーに
                    turnCountGimmick = 0;           // カウントを０に

                    alice.moveDirection = Player.MoveDirection.STOP;
                    alice.inputKeyFlag = true;
                    alice.SetAnimation(Player.Motion.STOP_NEXT, true);
                    print("待機");// デバッグ用コメント
                }
                
                // ▼巻き戻し処理
                // Ｑキーが押された時、行動が無しなら///////////////////////////////////////////////////////
                if (((Input.GetKeyDown(KeyCode.Q)) || (Input.GetKeyDown(KeyCode.Joystick1Button4))) && (action == PlayerAction.NONE) && (alice.saveCount > 0) && (tutorialFlag == false) ||
               ((Input.GetKeyDown(KeyCode.Q)) || (Input.GetKeyDown(KeyCode.Joystick1Button4))) && (action == PlayerAction.NONE) && (alice.saveCount > 0) && (tutorialFlag == true) && (stageNumber == 2) &&
               (tutorialTurn == 5 || tutorialTurn == 7) ||
               ((touchController.touchPosX > 1010) && (touchController.touchPosX < 1280)) &&
               ((touchController.touchPosY > 0) && (touchController.touchPosY < 320)) &&
               ((touchController.detachPosX > 1010) && (touchController.detachPosX < 1280)) &&
               ((touchController.detachPosY > 0) && (touchController.detachPosY < 320)) &&
               ((touchController.touchPosX < touchController.detachPosX) && (touchController.touchPosY < touchController.detachPosY)) && (action == PlayerAction.NONE) && (alice.saveCount > 0) && (tutorialFlag == false) ||
                   ((touchController.touchPosX > 1010) && (touchController.touchPosX < 1280)) &&
               ((touchController.touchPosY > 0) && (touchController.touchPosY < 320)) &&
               ((touchController.detachPosX > 1010) && (touchController.detachPosX < 1280)) &&
               ((touchController.detachPosY > 0) && (touchController.detachPosY < 320)) &&
               ((touchController.touchPosX < touchController.detachPosX) && (touchController.touchPosY < touchController.detachPosY)) && (action == PlayerAction.NONE) && (alice.saveCount > 0) && (tutorialFlag == true) && (stageNumber == 2) &&
               (tutorialTurn == 5 || tutorialTurn == 7))
                {
                    // ゲームオーバーだったらアニメーションを戻す
                    if (alice.gameOverFlag)
                    {
                        alice.SetAnimation(Player.Motion.GAMEOVER, false);
                    }
                  
                    touchController.TouchPostionInitialize();
                    //巻き戻しを押した時、下に穴があった時には、しょりをする、左側の分は丹羽君
                    if (alice.saveMoveDirection[alice.saveCount - 1] == Player.MoveDirection.NONE || stage.GetFootHole(alice))
                    {
                        stage.FootDecision(alice,Player.PlayerAction.NEXT);
                    }

                    action = PlayerAction.RETURN;   // 行動を戻るに
                    turn = Turn.GIMMICK;            // ターンをギミックに
                    turnCountGimmick = 0;           // カウントを０に
                    alice.moveReturnFlag = true;    // 巻き戻しフラグを真に

                    print("巻き戻し");// デバッグ用コメント
                }

                // ▼早送り処理
                // Ｅキーが押された時、行動が無しなら//////////////////////////////
                if (((Input.GetKeyDown(KeyCode.E)) || (Input.GetKeyDown(KeyCode.Joystick1Button5))) && (action == PlayerAction.NONE) ||
                   ((touchController.touchPosX > 1010) && (touchController.touchPosX < 1280)) &&
                   ((touchController.touchPosY > 0) && (touchController.touchPosY < 320)) &&
                   ((touchController.detachPosX > 1010) && (touchController.detachPosX < 1280)) &&
                   ((touchController.detachPosY > 0) && (touchController.detachPosY < 320)) &&
                   ((touchController.touchPosX > touchController.detachPosX) && (touchController.touchPosY > touchController.detachPosY)) && (action == PlayerAction.NONE))
                {
                    touchController.TouchPostionInitialize();
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
        Debug.Log("FRONT" + alice.moveFrontPossibleFlag);
        Debug.Log("BACK" + alice.moveBackPossibleFlag);
        Debug.Log("LEFT" + alice.moveLeftPossibleFlag);
        Debug.Log("RIGHT" + alice.moveRightPossibleFlag);
    
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

    //---------------------------------------------------
    //松村脩平追加部分
    //デバッグ用TextName変換
    public void ChangeTextName()
    {
        if(stageNumber == 1)
        {
            PlayerPrefs.SetInt("STAGE_NUM",1);
            PlayerPrefs.SetString("Text", "stage01.txt");
        }
        else if (stageNumber == 2)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 2);
            PlayerPrefs.SetString("Text", "stage02.txt");
        }
        else if (stageNumber == 3)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 3);
            PlayerPrefs.SetString("Text", "stage03.txt");
        }
        else if (stageNumber == 4)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 4);
            PlayerPrefs.SetString("Text", "stage04.txt");
        }
        else if (stageNumber == 5)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 5);
            PlayerPrefs.SetString("Text", "stage05.txt");
        }
        else if (stageNumber == 6)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 6);
            PlayerPrefs.SetString("Text", "stage06.txt");
        }
        else if (stageNumber == 7)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 7);
            PlayerPrefs.SetString("Text", "stage07.txt");
        }
        else if (stageNumber == 8)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 8);
            PlayerPrefs.SetString("Text", "stage08.txt");
        }
        else if (stageNumber == 9)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 9);
            PlayerPrefs.SetString("Text", "stage09.txt");
        }
        else if (stageNumber == 10)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 10);
            PlayerPrefs.SetString("Text", "stage10.txt");
        }
        else if (stageNumber == 11)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 11);
            PlayerPrefs.SetString("Text", "stage11.txt");
        }
        else if (stageNumber == 12)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 12);
            PlayerPrefs.SetString("Text", "stage12.txt");
        }
        else if (stageNumber == 13)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 13);
            PlayerPrefs.SetString("Text", "stage13.txt");
        }
        else if (stageNumber == 14)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 14);
            PlayerPrefs.SetString("Text", "stage14.txt");
        }
        else if (stageNumber == 15)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 15);
            PlayerPrefs.SetString("Text", "stage15.txt");
        }
        else if (stageNumber == 16)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 16);
            PlayerPrefs.SetString("Text", "stage16.txt");
        }
        else if (stageNumber == 17)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 17);
            PlayerPrefs.SetString("Text", "stage17.txt");
        }
        else if (stageNumber == 18)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 18);
            PlayerPrefs.SetString("Text", "stage18.txt");
        }
        else if (stageNumber == 19)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 19);
            PlayerPrefs.SetString("Text", "stage19.txt");
        }
        else if (stageNumber == 20)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 20);
            PlayerPrefs.SetString("Text", "stage20.txt");
        }
        else if (stageNumber == 21)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 21);
            PlayerPrefs.SetString("Text", "stage21.txt");
        }
        else if (stageNumber == 22)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 22);
            PlayerPrefs.SetString("Text", "stage22.txt");
        }
        else if (stageNumber == 23)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 23);
            PlayerPrefs.SetString("Text", "stage23.txt");
        }
        else if (stageNumber == 24)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 24);
            PlayerPrefs.SetString("Text", "stage24.txt");
        }
        else if (stageNumber == 25)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 25);
            PlayerPrefs.SetString("Text", "stage25.txt");
        }
        else if (stageNumber == 26)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 26);
            PlayerPrefs.SetString("Text", "stage26.txt");
        }
        else if (stageNumber == 27)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 27);
            PlayerPrefs.SetString("Text", "stage27.txt");
        }
        else if (stageNumber == 28)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 28);
            PlayerPrefs.SetString("Text", "stage28.txt");
        }
        else if (stageNumber == 29)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 29);
            PlayerPrefs.SetString("Text", "stage29.txt");
        }
        else if (stageNumber == 30)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 30);
            PlayerPrefs.SetString("Text", "stage30.txt");
        }
        else if (stageNumber == 31)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 31);
            PlayerPrefs.SetString("Text", "stage31.txt");
        }
        else if (stageNumber == 32)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 32);
            PlayerPrefs.SetString("Text", "stage32.txt");
        }
        else if (stageNumber == 33)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 33);
            PlayerPrefs.SetString("Text", "stage33.txt");
        }
        else if (stageNumber == 34)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 34);
            PlayerPrefs.SetString("Text", "stage34.txt");
        }
        else if (stageNumber == 35)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 35);
            PlayerPrefs.SetString("Text", "stage35.txt");
        }
        else if (stageNumber == 36)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 36);
            PlayerPrefs.SetString("Text", "stage36.txt");
        }
        else if (stageNumber == 37)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 37);
            PlayerPrefs.SetString("Text", "stage37.txt");
        }
        else if (stageNumber == 38)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 38);
            PlayerPrefs.SetString("Text", "stage38.txt");
        }
        else if (stageNumber == 39)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 39);
            PlayerPrefs.SetString("Text", "stage39.txt");
        }
        else if (stageNumber == 40)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 40);
            PlayerPrefs.SetString("Text", "stage40.txt");
        }
        else if (stageNumber == 41)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 41);
            PlayerPrefs.SetString("Text", "stage41.txt");
        }
        else if (stageNumber == 42)
        {
            PlayerPrefs.SetInt("STAGE_NUM", 42);
            PlayerPrefs.SetString("Text", "stage42.txt");
        }
    }
    //---------------------------------------------------
}
