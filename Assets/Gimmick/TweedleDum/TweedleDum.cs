using UnityEngine;
using System.Collections;

public class TweedleDum : BaseGimmick
{
    //ステージオブジェクト
    public GameObject player;
    public GameObject stage;
    public Player playerScript;
    public Stage stageScript;

    // 移動方向
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

	const float MOVE_SPEED = 1.0f;    // 移動速度
	const float SPEED = 0.05f;        // 移動速度

    public bool moveFlag;             //動くかどうか
    public bool returnFlag;           //ターン数が戻るかどうか
    public bool oncetimeTest;

    //キャラクターの向き
	public int direction;
	//キャラの歩数計算
	public int timeCount;

	Vector3 enemyAngle1;
	Vector3 enemyAngle2;
	Vector3 enemyAngle3;
	Vector3 enemyAngle4;

	public enum ArrayMove
	{
		PLUS_X,     // X方向にプラス
		MINUS_X,    // X方向にマイナス
		PLUS_Y,     // Y方向にプラス
		MINUS_Y,    // Y方向にマイナス
		PLUS_Z,     // Z方向にプラス
		MINUS_Z,    // Z方向にマイナス
	}

	public int arrayPosX;                           // 配列上での座標Ｘ
	public int arrayPosY;                           // 配列上での座標Ｙ
	public int arrayPosZ;                           // 配列上での座標Ｚ
	public Vector3 buttonInputPosition;  // ボタン入力時の座標
    public int aliceMove;

    //過去の向きを保存
    public int[] beforeDirection;
    public int[] notMoveTrun;

    //------------------------
    //初期化関数
    //------------------------
	void Start()
	{
        //オブジェクトの検索
        player = GameObject.Find("Alice");
		stage = GameObject.Find("Stage");
        playerScript = player.GetComponent<Player>();
		stageScript = stage.GetComponent<Stage>();

		//キャラの回転角初期化
		enemyAngle1 = new Vector3(0, 0, 0);
		enemyAngle2 = new Vector3(0, 90, 0);
		enemyAngle3 = new Vector3(0, 180, 0);
		enemyAngle4 = new Vector3(0, 270, 0);

        //ボタン入力時のオブジェクトの位置
		buttonInputPosition = new Vector3(0, 0, 0);

        //経過したターン数
        timeCount = 0;

        //動けるかどうか
        moveFlag = false;
        //時を戻したときの挙動かどうか
        returnFlag = false;

        //過去の向きの保存用変数の初期化
        beforeDirection = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        notMoveTrun = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};

        //初期の向きを代入
        beforeDirection[0] = direction;


        oncetimeTest = false;
	}

    //------------------------
    //座標・向きの初期化関数
    //------------------------
	public void Initialize(int direction, int x, int y, int z)
	{
        //向きの初期化
        this.direction = direction;
        InitChangeDirection();
        //座標の初期化
        arrayPosX = x;
        arrayPosY = y;
        arrayPosZ = z;
	}

    //---------------------------------
    //アリスが動いたときに呼ばれる関数
    //---------------------------------
	public override void OnAliceMoveNext(int aliceMoveTime)
	{

        //リターンフラグの初期化
        returnFlag = false;

        //(moveScript)プレイヤーの歩数と(timeCount)歩数を比べる
        if (timeCount < aliceMoveTime)
        {
            if (stageScript.findTwins())
            {
                stageScript.TwinsTestFunc(new Vector3(arrayPosX, arrayPosY, arrayPosZ), direction, false);
            }
            int roopMax = 4;
            for (int i = 0; i < roopMax; i++)
            {

                switch (direction)
                {
                    case 1:
                        //横判定と横下判定
                        if ((stageScript.TwinBesideDecision(arrayPosX, arrayPosY, arrayPosZ + 1, new Vector3(arrayPosX, arrayPosY, arrayPosZ), direction))
                            && (stageScript.BesideDownDecision(arrayPosX, arrayPosY, arrayPosZ + 1)))
                        {
                            moveFlag = true;
                            GetComponent<Animator>().SetBool("WalkMotion_Next", true);

                                //アリスと位置が被ったら
                                if ((new Vector3(arrayPosX, arrayPosY, arrayPosZ + 1) == new Vector3(playerScript.arrayPosX, playerScript.arrayPosY, playerScript.arrayPosZ))
                                    && (stageScript.BesideDecision(arrayPosX, arrayPosY, arrayPosZ + 1, true)))
                                {
                                    switch (playerScript.cameraAngle)
                                    {
                                        case PlayerCamera.CameraAngle.FRONT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.BACK);
                                            break;
                                        case PlayerCamera.CameraAngle.BACK:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.FRONT);
                                            break;
                                        case PlayerCamera.CameraAngle.LEFT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.LEFT);
                                            break;
                                        case PlayerCamera.CameraAngle.RIGHT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.RIGHT);
                                            break;
                                    }

                                    GetComponent<Animator>().SetBool("PushMotion_Next", true);
                                    playerScript.SetAnimation(Player.Motion.WALK_NEXT, true);
                                    oncetimeTest = true;
                                }
                                else if ((new Vector3(arrayPosX, arrayPosY, arrayPosZ + 1) == new Vector3(playerScript.arrayPosX, playerScript.arrayPosY, playerScript.arrayPosZ))
                                    && (!stageScript.BesideDecision(arrayPosX, arrayPosY, arrayPosZ + 1, true)))
                                {
                                    moveFlag = false;
                                }


                        }
                        break;
                    case 3:
                        if ((stageScript.TwinBesideDecision(arrayPosX, arrayPosY, arrayPosZ - 1, new Vector3(arrayPosX, arrayPosY, arrayPosZ), direction))
                            && (stageScript.BesideDownDecision(arrayPosX, arrayPosY, arrayPosZ - 1)))
                        {
                            moveFlag = true;
                            GetComponent<Animator>().SetBool("WalkMotion_Next", true);

                                if ((new Vector3(arrayPosX, arrayPosY, arrayPosZ - 1) == new Vector3(playerScript.arrayPosX, playerScript.arrayPosY, playerScript.arrayPosZ)) && (stageScript.BesideDecision(arrayPosX, arrayPosY, arrayPosZ - 1, true)))
                                {
                                    switch (playerScript.cameraAngle)
                                    {
                                        case PlayerCamera.CameraAngle.FRONT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.FRONT);
                                            break;
                                        case PlayerCamera.CameraAngle.BACK:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.BACK);
                                            break;
                                        case PlayerCamera.CameraAngle.LEFT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.RIGHT);
                                            break;
                                        case PlayerCamera.CameraAngle.RIGHT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.LEFT);
                                            break;
                                    }

                                    GetComponent<Animator>().SetBool("PushMotion_Next", true);
                                    playerScript.SetAnimation(Player.Motion.WALK_NEXT, true);
                                    oncetimeTest = true;
                                }
                                else if ((new Vector3(arrayPosX, arrayPosY, arrayPosZ - 1) == new Vector3(playerScript.arrayPosX, playerScript.arrayPosY, playerScript.arrayPosZ))
                                    && (!stageScript.BesideDecision(arrayPosX, arrayPosY, arrayPosZ - 1, true)))
                                {
                                    moveFlag = false;
                                }
    

                        }
                        break;
                    case 4:
                        if ((stageScript.TwinBesideDecision(arrayPosX - 1, arrayPosY, arrayPosZ, new Vector3(arrayPosX, arrayPosY, arrayPosZ), direction))
                            && (stageScript.BesideDownDecision(arrayPosX - 1, arrayPosY, arrayPosZ)))
                        {
                            moveFlag = true;
                            GetComponent<Animator>().SetBool("WalkMotion_Next", true);

                                if ((new Vector3(arrayPosX - 1, arrayPosY, arrayPosZ) == new Vector3(playerScript.arrayPosX, playerScript.arrayPosY, playerScript.arrayPosZ)) && (stageScript.BesideDecision(arrayPosX - 2, arrayPosY, arrayPosZ, true)))
                                {
                                    switch (playerScript.cameraAngle)
                                    {
                                        case PlayerCamera.CameraAngle.FRONT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.RIGHT);
                                            break;
                                        case PlayerCamera.CameraAngle.BACK:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.LEFT);
                                            break;
                                        case PlayerCamera.CameraAngle.LEFT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.BACK);
                                            break;
                                        case PlayerCamera.CameraAngle.RIGHT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.FRONT);
                                            break;
                                    }

                                    GetComponent<Animator>().SetBool("PushMotion_Next", true);
                                    playerScript.SetAnimation(Player.Motion.WALK_NEXT, true);
                                    oncetimeTest = true;
                                }
                                else if ((new Vector3(arrayPosX - 1, arrayPosY, arrayPosZ) == new Vector3(playerScript.arrayPosX, playerScript.arrayPosY, playerScript.arrayPosZ)) && (!stageScript.BesideDecision(arrayPosX - 2, arrayPosY, arrayPosZ, true)))
                                {
                                    moveFlag = false;
                                }

                        }
                        break;
                    case 2:
                        if ((stageScript.TwinBesideDecision(arrayPosX + 1, arrayPosY, arrayPosZ, new Vector3(arrayPosX, arrayPosY, arrayPosZ), direction))
                            && (stageScript.BesideDownDecision(arrayPosX + 1, arrayPosY, arrayPosZ)))
                        {
                            moveFlag = true;
                            GetComponent<Animator>().SetBool("WalkMotion_Next", true);
                 
                                if ((new Vector3(arrayPosX + 1, arrayPosY, arrayPosZ) == new Vector3(playerScript.arrayPosX, playerScript.arrayPosY, playerScript.arrayPosZ)) && (stageScript.BesideDecision(arrayPosX + 2, arrayPosY, arrayPosZ, true)))
                                {
                                    switch (playerScript.cameraAngle)
                                    {
                                        case PlayerCamera.CameraAngle.FRONT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.LEFT);
                                            break;
                                        case PlayerCamera.CameraAngle.BACK:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.RIGHT);
                                            break;
                                        case PlayerCamera.CameraAngle.LEFT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.FRONT);
                                            break;
                                        case PlayerCamera.CameraAngle.RIGHT:
                                            playerScript.AutoMoveSetting(Player.MoveDirection.BACK);
                                            break;
                                    }

                                    GetComponent<Animator>().SetBool("PushMotion_Next", true);
                                    playerScript.SetAnimation(Player.Motion.WALK_NEXT, true);
                                    oncetimeTest = true;
                                }
                                else if ((new Vector3(arrayPosX + 1, arrayPosY, arrayPosZ) == new Vector3(playerScript.arrayPosX, playerScript.arrayPosY, playerScript.arrayPosZ)) && (!stageScript.BesideDecision(arrayPosX + 2, arrayPosY, arrayPosZ, true)))
                                {
                                    moveFlag = false;
                                }


                        }
                        break;
                    default:
                        break;
                }
                if (moveFlag == true)
                {
                    break;
                }
                else
                {
                    //右回転させる
                    direction++;

                    //向きが一周したら最初の向きに戻す
                    if (direction == 5)
                    {
                        direction = 1;
                    }

                    //もし動けなかったら
                    if (i == 3)
                    {
                        notMoveTrun[timeCount + 1] = 1;
                    }

                }
            }



            //仮の保存座標に現在座標を入れる
            buttonInputPosition = this.transform.localPosition;


            //カウントを進める
            timeCount++;

            //向きを保存
            beforeDirection[timeCount] = direction;
            //向きの変更
            ChangeDirection();
        }

    }

    //----------------------------------
    //アリスが時を戻した時に呼ばれる関数
    //----------------------------------
	public override void OnAliceMoveReturn(int aliceMoveTime)
	{
		returnFlag = true;
       
        if (timeCount >= aliceMoveTime)
        {
            if (notMoveTrun[timeCount] == 0)
            {
                moveFlag = true;
                GetComponent<Animator>().SetBool("WalkMotion_Return", true);
            }
        }

        //待機フラグを初期化
		notMoveTrun[timeCount] = 0;

        //カウントを戻す
        timeCount -= 1;
 
      
        //仮の保存座標に現在座標に入れる
        buttonInputPosition = this.transform.localPosition;

	}

    //-----------------
    //アップデート関数
    //-----------------
	void Update()
	{
        //動かす
		Move();
	}

    //---------------
    //移動させる関数
    //---------------
	public void Move()
	{
		switch (returnFlag)
		{
			case false://進むとき
											
												
				if (moveFlag)
				{
					transform.Translate(Vector3.forward * SPEED);
					//以下停止コード
					switch (direction)
					{
						//Z軸を調整する
						case 1:
							//目的地に着いたとき
							if (transform.localPosition.z >= buttonInputPosition.z + 1)
							{
								Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, buttonInputPosition.z + 1);
								MoveFinish(position, ArrayMove.PLUS_Z);
                                GetComponent<Animator>().SetBool("PushMotion_Next", false);
                                GetComponent<Animator>().SetBool("WalkMotion_Next", false);
							}
							break;
						case 3:
							//目的地に着いたとき
							if (transform.localPosition.z <= buttonInputPosition.z - 1)
							{
								Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, buttonInputPosition.z - 1);
								//移動終了時に呼ばれる
								MoveFinish(position, ArrayMove.MINUS_Z);
                                GetComponent<Animator>().SetBool("PushMotion_Next", false);
                                GetComponent<Animator>().SetBool("WalkMotion_Next", false);
							}
							break;

						//X軸を調整する
						case 4:
							//目的地に着いたとき
							if (transform.localPosition.x <= buttonInputPosition.x - 1)
							{
								Vector3 position = new Vector3(buttonInputPosition.x - 1, transform.localPosition.y, transform.localPosition.z);
								MoveFinish(position, ArrayMove.MINUS_X);
                                GetComponent<Animator>().SetBool("PushMotion_Next", false);
                                GetComponent<Animator>().SetBool("WalkMotion_Next", false);
							}
							break;
						case 2:
							//目的地に着いたとき
							if (transform.localPosition.x >= buttonInputPosition.x + 1)
							{
								Vector3 position = new Vector3(buttonInputPosition.x + 1, transform.localPosition.y, transform.localPosition.z);
								//移動終了
								MoveFinish(position, ArrayMove.PLUS_X);
                                GetComponent<Animator>().SetBool("PushMotion_Next", false);
                                GetComponent<Animator>().SetBool("WalkMotion_Next", false);
							}
							break;
					}
				}
				else
				{
					
					moveFlag = false;
				}
				break;

			case true://戻るとき
				if (moveFlag)
                {
                    //動くスピードを設定
                    transform.Translate(Vector3.back * SPEED);

                    //以下停止コード
                    switch (direction)
                    {
                        //Z軸を調整する
                        case 1:
                            if (transform.localPosition.z <= buttonInputPosition.z - 1)
                            {
                                //誤差の調節
                                Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, buttonInputPosition.z - 1);
                                //移動を終える
                                MoveFinish(position, ArrayMove.MINUS_Z);
                                GetComponent<Animator>().SetBool("WalkMotion_Return", false);
                            }
                            break;
                        case 3:
                            //目的地に着いたとき
                            if (transform.localPosition.z >= buttonInputPosition.z + 1)
                            {
                                //誤差の調節
                                Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, buttonInputPosition.z + 1);
                                //移動を終える
                                MoveFinish(position, ArrayMove.PLUS_Z);
                                GetComponent<Animator>().SetBool("WalkMotion_Return", false);
                            }
                            break;

                        //X軸を調整する
                        case 4:
                            //目的地に着いたとき
                            if (transform.localPosition.x >= buttonInputPosition.x + 1)
                            {
                                //誤差の調節
                                Vector3 position = new Vector3(buttonInputPosition.x + 1, transform.localPosition.y, transform.localPosition.z);
                                //移動を終える
                                MoveFinish(position, ArrayMove.PLUS_X);
                                GetComponent<Animator>().SetBool("WalkMotion_Return", false);
                            }
                            break;
                        case 2:
                            //目的地に着いたとき
                            if (transform.localPosition.x <= buttonInputPosition.x - 1)
                            {
                                //誤差の調節
                                Vector3 position = new Vector3(buttonInputPosition.x - 1, transform.localPosition.y, transform.localPosition.z);
                                //移動を終える
                                MoveFinish(position, ArrayMove.MINUS_X);
                                GetComponent<Animator>().SetBool("WalkMotion_Return", false);
                            }
                            break;
                    }
                }
                else
                {
                    moveFlag = false;
                }
                      
			    break;
		}
	}

    //-------------------
    //移動完了時呼ぶ関数
    //-------------------
    public void MoveFinish(Vector3 position, ArrayMove arrayMove)
    {
        transform.localPosition = position;     // 座標を変更
        ChangeArrayPosition(arrayMove);         // 配列上の位置を変更
        //stageScript.DumGimmickDecision(arrayPosX, arrayPosY, arrayPosZ);

        //ターン数が戻った時の処理なら
        if (returnFlag == true)
        {
            //前回の向きの取得
            direction = beforeDirection[timeCount];

            //向きの変更
            ChangeDirection();


        }
        //リターンフラグの初期化
        returnFlag = false;
        //動きを止める
        moveFlag = false;
    }

    //---------------------------
    //配列上の位置を変更する関数
    //---------------------------
    public void ChangeArrayPosition(ArrayMove arrayMove)
    {
        //GameObject.Find("Stage").GetComponent<Stage>().gimmickNumArray[arrayPosY, arrayPosX, arrayPosZ] = 0;

        switch (arrayMove)
        {
            case ArrayMove.PLUS_X: arrayPosX++; break;      // 配列上の座標Xに１プラス
            case ArrayMove.MINUS_X: arrayPosX--; break;     // 配列上の座標Xに１マイナス
            case ArrayMove.PLUS_Y: arrayPosY++; break;      // 配列上の座標Yに１プラス
            case ArrayMove.MINUS_Y: arrayPosY--; break;     // 配列上の座標Yに１マイナス
            case ArrayMove.PLUS_Z: arrayPosZ++; break;      // 配列上の座標Zに１プラス
            case ArrayMove.MINUS_Z: arrayPosZ--; break;     // 配列上の座標Zに１マイナス
        } 
        //GameObject.Find("Stage").GetComponent<Stage>().gimmickNumArray[arrayPosY, arrayPosX, arrayPosZ] = 0;

    }

    //---------------------
    //向きの変更をする関数
    //---------------------
    public void ChangeDirection()
    {
        //変数に応じて回転を代入する
        if (direction == 1)
        {
            this.transform.localEulerAngles = enemyAngle1;
        }
        if (direction == 2)
        {
            this.transform.localEulerAngles = enemyAngle2;
        }
        if (direction == 3)
        {
            this.transform.localEulerAngles = enemyAngle3;
        }
        if (direction == 4)
        {
            this.transform.localEulerAngles = enemyAngle4;
        }
    }

    public void InitChangeDirection()
    {

        //キャラの回転処理
        enemyAngle1 = new Vector3(0, 0, 0);
        enemyAngle2 = new Vector3(0, 90, 0);
        enemyAngle3 = new Vector3(0, 180, 0);
        enemyAngle4 = new Vector3(0, 270, 0);

        //変数に応じて回転を代入する
        if (direction == 1)
        {
            this.transform.localEulerAngles = enemyAngle1;
        }
        else if (direction == 2)
        {
            direction = 3;
            this.transform.localEulerAngles = enemyAngle3;
        }
        else if (direction == 3)
        {
            direction = 2;
            this.transform.localEulerAngles = enemyAngle2;
        }
        else if (direction == 4)
        {
            this.transform.localEulerAngles = enemyAngle4;
        }
    }

}