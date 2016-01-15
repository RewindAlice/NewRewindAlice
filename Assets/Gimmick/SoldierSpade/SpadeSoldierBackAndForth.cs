using UnityEngine;
using System.Collections;

public class SpadeSoldierBackAndForth : BaseGimmick
{

    const float MOVE_SPEED = 1.0f;    // 移動速度
    const float SPEED = 0.04f;        // 移動速度

    //移動可能フラグ
    public bool moveFlag;
    //ターンが戻ったか
    public bool returnFlag;
    public bool foundTheAliceAfter;
    public bool returnAnimationFlag;
    public int returnAnimationCount;
    public int foundCount;

    //オブジェクト
    public GameObject alice;
    private Player moveScript;
    public GameObject stage;
    private Stage stageScript;
    private GameObject pause;
    private Pause pauseScript;

    //追加
    public int direction;
    //トランプ兵の歩数計算
    public int timeCount;

    //回転の初期値
    Vector3 enemyAngle1;
    Vector3 enemyAngle2;
    Vector3 enemyAngle3;
    Vector3 enemyAngle4;

    //過去の向きを保存
    public int[] beforeDirection;
    //動いてないターン保存
    public int[] notMoveTurn;
    //回転しただけのターン保存
    public int[] TurnTime;

    public enum ArrayMove
    {
        PLUS_X,     // X方向にプラス
        MINUS_X,    // X方向にマイナス
        PLUS_Y,     // Y方向にプラス
        MINUS_Y,    // Y方向にマイナス
        PLUS_Z,     // Z方向にプラス
        MINUS_Z,    // Z方向にマイナス
    }

    public int arrayPosX;                // 配列上での座標Ｘ
    public int arrayPosY;                // 配列上での座標Ｙ
    public int arrayPosZ;                // 配列上での座標Ｚ

    public Vector3 buttonInputPosition;  // ボタン入力時の座標
    public bool discoveryFlag;
    //------------------------
    //初期化関数
    //------------------------
    void Start()
    {
        //オブジェクトの検索
        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();
        alice = GameObject.Find("Alice");
        moveScript = alice.GetComponent<Player>();
        stage = GameObject.Find("Stage");
        stageScript = stage.GetComponent<Stage>();
        
        //トランプ兵の回転処理
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
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        notMoveTurn = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


        TurnTime = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                                      0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                           
        //初期の向きを代入
        beforeDirection[0] = direction;
        ChangeDirection();

        foundTheAliceAfter = false;
        discoveryFlag = false;
        foundCount = 0;
        returnAnimationFlag = false;
        returnAnimationCount = 0;
	}


    //-----------------
    //アップデート関数
    //-----------------
    void Update()
    {
        //動かす
        Move();

        if (returnAnimationFlag == true)
        {
            returnAnimationCount++;

            if (returnAnimationCount == 26)
            {
                returnAnimationCount = 0;
                returnAnimationFlag = false;

                //向きの変更
                ChangeDirection();

            }
        }
    }

    //------------------------
    //座標・向きの初期化関数
    //------------------------
    public void Initialize(int direction, int x, int y, int z)
    {
        //向きの初期化
        this.direction = direction;
        
        //座標の初期化
        arrayPosX = x;
        arrayPosY = y;
        arrayPosZ = z;

        InitChangeDirection();
    }



    //---------------------------------
    //アリスが動いたときに呼ばれる関数
    //---------------------------------
    public override void OnAliceMoveNext(int aliceMoveTime)
    {
        //アリスを発見したかどうかのフラグ
        //bool discoveryFlag;

        //向きを保存
        beforeDirection[timeCount] = direction;
        
        //アリスが前にいるか判定
        discoveryFlag = CaptureDecision();
        foundTheAliceAfter = false;

        //アリスが見つかっていなければ
        if(discoveryFlag == false)
        {
            returnFlag = false;

            //(moveScript)プレイヤーの歩数と(timeCount)歩数を比べる
            if (timeCount < aliceMoveTime)
            {
                int roopMax = 2;
                for (int i = 0; i < roopMax; i++)
                {
                    discoveryFlag = false;

                    //前に進めるか判定
                    switch (direction)
                    {
                        case 1:
                            if ((stageScript.BesideDecision(arrayPosX, arrayPosY, arrayPosZ + 1,false)) && (stageScript.BesideDownDecision(arrayPosX, arrayPosY, arrayPosZ + 1)))
                            {
                                discoveryFlag = CaptureDecision();
                                foundTheAliceAfter = false;
                                if (discoveryFlag == false)
                                {
                                    moveFlag = true;
                                }
                            }
                            break;
                        case 3:
                            if ((stageScript.BesideDecision(arrayPosX, arrayPosY, arrayPosZ - 1, false)) && (stageScript.BesideDownDecision(arrayPosX, arrayPosY, arrayPosZ - 1)))
                            {
                                discoveryFlag = CaptureDecision();
                                foundTheAliceAfter = false;
                                if (discoveryFlag == false)
                                {
                                    moveFlag = true;
                                }
                            }
                            break;
                        case 4:
                            if ((stageScript.BesideDecision(arrayPosX - 1, arrayPosY, arrayPosZ,false)) && (stageScript.BesideDownDecision(arrayPosX - 1, arrayPosY, arrayPosZ)))
                            {
                                discoveryFlag = CaptureDecision();
                                foundTheAliceAfter = false;
                                if (discoveryFlag == false)
                                {
                                    moveFlag = true;
                                }
                            }
                            break;
                        case 2:
                            if ((stageScript.BesideDecision(arrayPosX + 1, arrayPosY, arrayPosZ,false)) && (stageScript.BesideDownDecision(arrayPosX + 1, arrayPosY, arrayPosZ)))
                            {
                                discoveryFlag = CaptureDecision();
                                foundTheAliceAfter = false;
                                if (discoveryFlag == false)
                                {
                                    moveFlag = true;
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    //もし動くフラグがtrueなら
                    if (moveFlag == true)
                    {
                        GetComponent<Animator>().SetBool("MoveNext", true);
                        foundTheAliceAfter = true;
                        break;
                    }
                    else
                    {
                        if(i == 0 )
                        {
                            //左回転させる
                            direction += 2;
                        }

                        //向きが一周したら最初の向きに戻す
                        if (direction == 5)
                        {
                            direction = 1;
                        }
                        else if(direction == 6)
                        {
                            direction = 2;
                        }

                        //もし動けなかったら
                        if (i == 1)
                        {
                            notMoveTurn[timeCount] = 1;
                        }
                        //向きの変更
                        ChangeDirection();

                        //アリスが前にいるか判定
                        discoveryFlag = CaptureDecision();

                        //アリスが回転しただけなら
                        if(discoveryFlag && i==0)
                        {
                            TurnTime[timeCount] = 1;
                        }
                    }                    
                }

                //現在座標を保存
                buttonInputPosition = this.transform.localPosition;
  
                //向きの変更
                ChangeDirection();

                timeCount++;                
            }
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
            if (discoveryFlag == false)
                GetComponent<Animator>().SetBool("MoveReturn", true);


            if (notMoveTurn[timeCount - 1] == 0)
            {
                moveFlag = true;
            }

            //構えなおす
            if (foundTheAliceAfter == false)
            {
                discoveryFlag = false;
                returnAnimationFlag = true;
                GetComponent<Animator>().SetBool("FoundTheAlice", false);
                GetComponent<Animator>().SetBool("MoveNext", false);
                GetComponent<Animator>().SetBool("AfterFound", false);
            }
            //構えなおして歩かせる
            else if (foundTheAliceAfter == true)
            {
                //discoveryFlag = false;
                GetComponent<Animator>().SetBool("FoundTheAlice", false);
                GetComponent<Animator>().SetBool("MoveNext", false);
                GetComponent<Animator>().SetBool("AfterFound", true);
                GetComponent<Animator>().SetBool("MoveReturn", true);
            }
            //カウントを戻す
            timeCount -= 1;

           
        }
        else
        {
            //構えなおす
            if (foundTheAliceAfter == false)
            {
                discoveryFlag = false;
                returnAnimationFlag = true;
                GetComponent<Animator>().SetBool("FoundTheAlice", false);
                GetComponent<Animator>().SetBool("MoveNext", false);
                GetComponent<Animator>().SetBool("AfterFound", false);
            }
            //構えなおして歩かせる
            else if (foundTheAliceAfter == true)
            {
                //discoveryFlag = false;
                GetComponent<Animator>().SetBool("FoundTheAlice", false);
                GetComponent<Animator>().SetBool("MoveNext", false);
                GetComponent<Animator>().SetBool("AfterFound", true);
                GetComponent<Animator>().SetBool("MoveReturn", true);
            }
        }
       
        if (TurnTime[timeCount] == 1)
        {
            moveFlag = false;
            //向きを保存
            direction = beforeDirection[timeCount];
            //向きの変更
            if (returnAnimationFlag == false)
            ChangeDirection();
        }

        //待機フラグを初期化
        notMoveTurn[timeCount] = 0;

        TurnTime[timeCount] = 0; 

        //仮の保存座標に現在座標に入れる
        buttonInputPosition = this.transform.localPosition;
	}

    //---------------
    //移動させる関数
    //---------------
    public void Move()
    {
        switch (returnFlag)
        {
            //ターン数が進むとき
            case false:
                if (moveFlag)
                {
                    //動くスピードを設定
                    transform.Translate(Vector3.forward * SPEED);

                    //以下停止コード
                    switch (direction)
                    {
                        //Z軸を調整する
                        case 1:
                            //目的地に着いたとき
                            if (transform.localPosition.z >= buttonInputPosition.z + 1)
                            {
                                GetComponent<Animator>().SetBool("MoveNext", false);
                                //誤差の調節
                                Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, buttonInputPosition.z + 1);
                                //移動を終える
                                MoveFinish(position, ArrayMove.PLUS_Z);
                            }
                            break;
                        case 3:
                            //目的地に着いたとき
                            if (transform.localPosition.z <= buttonInputPosition.z - 1)
                            {
                                GetComponent<Animator>().SetBool("MoveNext", false);
                                //誤差の調節
                                Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, buttonInputPosition.z - 1);
                                //移動を終える
                                MoveFinish(position, ArrayMove.MINUS_Z);
                            }
                            break;

                        //X軸を調整する
                        case 4:
                            //目的地に着いたとき
                            if (transform.localPosition.x <= buttonInputPosition.x - 1)
                            {
                                GetComponent<Animator>().SetBool("MoveNext", false);
                                //誤差の調節
                                Vector3 position = new Vector3(buttonInputPosition.x - 1, transform.localPosition.y, transform.localPosition.z);
                                //移動を終える
                                MoveFinish(position, ArrayMove.MINUS_X);
                            }
                            break;
                        case 2:
                            //目的地に着いたとき
                            if (transform.localPosition.x >= buttonInputPosition.x + 1)
                            {
                                GetComponent<Animator>().SetBool("MoveNext", false);
                                //誤差の調節
                                Vector3 position = new Vector3(buttonInputPosition.x + 1, transform.localPosition.y, transform.localPosition.z);
                                //移動を終える
                                MoveFinish(position, ArrayMove.PLUS_X);
                            }
                            break;
                    }
                }
                else
                {
                    moveFlag = false;
                }
                break;

            //ターン数が戻るとき
            case true:

                if (moveFlag)
                {
                    if (discoveryFlag == true)
                    {
                        foundCount++;
                        if (foundCount == 25)
                        {
                            discoveryFlag = false;
                            foundCount = 0;
                        }
                    }
                    else
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
                                    GetComponent<Animator>().SetBool("MoveReturn", false);
                                    //誤差の調節
                                    Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, buttonInputPosition.z - 1);
                                    //移動を終える
                                    MoveFinish(position, ArrayMove.MINUS_Z);

                                }
                                break;
                            case 3:
                                //目的地に着いたとき
                                if (transform.localPosition.z >= buttonInputPosition.z + 1)
                                {
                                    GetComponent<Animator>().SetBool("MoveReturn", false);
                                    //誤差の調節
                                    Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, buttonInputPosition.z + 1);
                                    //移動を終える
                                    MoveFinish(position, ArrayMove.PLUS_Z);

                                }
                                break;

                            //X軸を調整する
                            case 4:
                                //目的地に着いたとき
                                if (transform.localPosition.x >= buttonInputPosition.x + 1)
                                {
                                    GetComponent<Animator>().SetBool("MoveReturn", false);
                                    //誤差の調節
                                    Vector3 position = new Vector3(buttonInputPosition.x + 1, transform.localPosition.y, transform.localPosition.z);
                                    //移動を終える
                                    MoveFinish(position, ArrayMove.PLUS_X);
                                }
                                break;
                            case 2:
                                //目的地に着いたとき
                                if (transform.localPosition.x <= buttonInputPosition.x - 1)
                                {
                                    GetComponent<Animator>().SetBool("MoveReturn", false);
                                    //誤差の調節
                                    Vector3 position = new Vector3(buttonInputPosition.x - 1, transform.localPosition.y, transform.localPosition.z);
                                    //移動を終える
                                    MoveFinish(position, ArrayMove.MINUS_X);

                                }
                                break;
                        }
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

        CaptureDecision();
        discoveryFlag = CaptureDecision();
    }

    //---------------------------
    //配列上の位置を変更する関数
    //---------------------------
    public void ChangeArrayPosition(ArrayMove arrayMove)
    {
       
        switch (arrayMove)
        {
            case ArrayMove.PLUS_X: arrayPosX++; break;      // 配列上の座標Xに１プラス
            case ArrayMove.MINUS_X: arrayPosX--; break;     // 配列上の座標Xに１マイナス
            case ArrayMove.PLUS_Y: arrayPosY++; break;      // 配列上の座標Yに１プラス
            case ArrayMove.MINUS_Y: arrayPosY--; break;     // 配列上の座標Yに１マイナス
            case ArrayMove.PLUS_Z: arrayPosZ++; break;      // 配列上の座標Zに１プラス
            case ArrayMove.MINUS_Z: arrayPosZ--; break;     // 配列上の座標Zに１マイナス
        }

    }

    //---------------------------
    //アリスが前にいるか判定処理
    //---------------------------
    public bool CaptureDecision()
    {
        //アリスの位置を取得
        Vector3 playerArray = moveScript.GetArray();

        if (moveScript.GetInvisible() == false)
        {
            //アリスが前にいるか判定
            switch (direction)
            {
                case 1:
                    if ((playerArray.x == arrayPosX) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ + 1))
                    {
                        GetComponent<Animator>().SetBool("FoundTheAlice", true);
                        moveScript.gameOverFlag = true;
                        return true;
                    }
                    break;
                case 2:
                    if ((playerArray.x == arrayPosX + 1) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ))
                    {
                        GetComponent<Animator>().SetBool("FoundTheAlice", true);
                        moveScript.gameOverFlag = true;
                        return true;
                    }
                    break;

                case 3:
                    if ((playerArray.x == arrayPosX) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ - 1))
                    {
                        GetComponent<Animator>().SetBool("FoundTheAlice", true);
                        moveScript.gameOverFlag = true;
                        return true;
                    }
                    break;

                case 4:
                    if ((playerArray.x == arrayPosX - 1) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ))
                    {
                        GetComponent<Animator>().SetBool("FoundTheAlice", true);
                        moveScript.gameOverFlag = true;
                        return true;
                    }
                    break;
            }
        }

        return false;
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
