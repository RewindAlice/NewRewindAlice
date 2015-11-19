using UnityEngine;
using System.Collections;

public class HeartSoldierTurnLeft : BaseGimmick
{

    //オブジェクト
    public GameObject alice;
    private Player moveScript;
    public GameObject stage;
    private Stage stageScript;
    private GameObject pause;
    private Pause pauseScript;

    //向き
    public int direction;
    //動いたターン数
    public int timeCount;

    //向きの値
    Vector3 enemyAngle1;
    Vector3 enemyAngle2;
    Vector3 enemyAngle3;
    Vector3 enemyAngle4;

    //オブジェクトの座標
    public int arrayPosX;
    public int arrayPosY;
    public int arrayPosZ;

    //------------------------
    //初期化関数
    //------------------------
    void Start()
    {
        //オブジェクトの検索
        pause = GameObject.Find("Pause");
        alice = GameObject.Find("Alice");
        pauseScript = pause.GetComponent<Pause>();
        moveScript = alice.GetComponent<Player>();

        //キャラの回転角初期化
        enemyAngle1 = new Vector3(0, 0, 0);
        enemyAngle2 = new Vector3(0, 90, 0);
        enemyAngle3 = new Vector3(0, 180, 0);
        enemyAngle4 = new Vector3(0, 270, 0);

        //経過したターン数
        timeCount = 0;

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

    //-----------------
    //アップデート関数
    //-----------------
    void Update()
    {
    }

    //---------------------------------
    //アリスが動いたときに呼ばれる関数
    //---------------------------------
    public override void OnAliceMoveNext(int aliceMoveTime)
    {
        //(moveScript)プレイヤーの歩数と(timeCount)歩数を比べる
        if (timeCount < aliceMoveTime)
        {
            //アリスを見つけたかのフラグ
            bool flag;

            //アリスを見つけたか判定
            flag = CaptureDecision();

            //アリスが見つかってなかったら向きを変える
            if (flag == false)
            {
                //右回転
                direction -= 1;
                //ターン数を増やす
                timeCount += 1;

                if (direction == 0)
                {
                    direction = 4;
                }

                //回転させる
                ChangeDirection();

                //アリスを見つけたか判定
                CaptureDecision();
            }

        }
    }

    //----------------------------------
    //アリスが時を戻した時に呼ばれる関数
    //----------------------------------
    public override void OnAliceMoveReturn(int aliceMoveTime)
    {
        if (timeCount >= aliceMoveTime)
        {
            direction += 1;
            timeCount -= 1;
            if (direction == 5)
            {
                direction = 1;
            }

            ChangeDirection();
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
                        moveScript.gameOverFlag = true;
                        return true;
                    }
                    break;
                case 2:
                    if ((playerArray.x == arrayPosX + 1) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ))
                    {
                        moveScript.gameOverFlag = true;
                        return true;
                    }
                    break;

                case 3:
                    if ((playerArray.x == arrayPosX) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ - 1))
                    {
                        moveScript.gameOverFlag = true;
                        return true;
                    }
                    break;

                case 4:
                    if ((playerArray.x == arrayPosX - 1) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ))
                    {
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
            this.transform.localEulerAngles = enemyAngle3;
       
        }
        if (direction == 3)
        {
            this.transform.localEulerAngles = enemyAngle2;
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
        if (direction == 2)
        {
            direction = 3;
            this.transform.localEulerAngles = enemyAngle2;

        }
        if (direction == 3)
        {
            direction = 2;
            this.transform.localEulerAngles = enemyAngle3;
        }
        if (direction == 4)
        {
            this.transform.localEulerAngles = enemyAngle4;
        }
    }
}
