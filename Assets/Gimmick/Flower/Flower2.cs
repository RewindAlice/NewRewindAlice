using UnityEngine;
using System.Collections;

public class Flower2 : BaseGimmick
{
    //public bool gimmickFlag;      // ギミックが有効か判断するフラグ
    //public int startActionTurn;   // ギミックを動かし始めるターン数
    //public int gimmickCount;      // ギミックが有効になってからのターン数

    //public bool movePossibleFlag;   // 移動可能フラグ
    public int moveCount;
    public int motionCount;
    private GameObject pause;
    private Pause pauseScript;
    // 初期化
    void Start()
    {
        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();
        gimmickFlag = true;         // ギミックが有効か判断するフラグに真を保存
        startActionTurn = 1;        // ギミックを動かし始めるターン数を１に
        gimmickCount = 0;           // ギミックが有効になってからのターン数を０に
        movePossibleFlag = true;    // 移動可能フラグを真に
        moveCount = 0;
        motionCount = 1;
        GetComponent<Animator>().SetInteger("MotionCount", motionCount);
    }

    // 更新
    void Update()
    {
        if (pauseScript.pauseFlag == false)
        {

        }
    }

    //アリスが動いたときに呼ばれる関数
    public override void OnAliceMoveNext(int aliceMoveCount)
    {
        // 保存されている移動数よりアリスの移動数が多かったら
        if (moveCount < aliceMoveCount)
        {
            gimmickCount++;
            motionCount++;

            switch (motionCount % 3)
            {
                case 0:
                    movePossibleFlag = true;    // 移動可能フラグを真に
                    break;
                case 1:
                    movePossibleFlag = false;   // 移動可能フラグを偽に
                    break;
                case 2:
                    movePossibleFlag = true;    // 移動可能フラグを真に
                    break;
            }
            GetComponent<Animator>().SetInteger("MotionCount", motionCount % 3);
            moveCount++;
        }
    }

    //アリスが戻った時に呼ばれる関数
    public override void OnAliceMoveReturn(int aliceMoveCount)
    {
        // 保存されている移動数よりアリスの移動数が同じなら
        if (moveCount == aliceMoveCount)
        {
            gimmickCount--;
            motionCount--;

            // ギミックが有効になってからのターン数が
            switch (motionCount % 3)
            {
                case 0:
                    movePossibleFlag = true;    // 移動可能フラグを真に
                    break;
                case 1:
                    movePossibleFlag = false;   // 移動可能フラグを偽に
                    break;
                case 2:
                    movePossibleFlag = true;    // 移動可能フラグを真に
                    break;
            }
            GetComponent<Animator>().SetInteger("MotionCount", motionCount % 3);
            moveCount--;
        }
    }

    public bool GetMovePossible()
    {
        return movePossibleFlag;
    }

    public void SetMovePossible(bool flag)
    {
        movePossibleFlag = flag;
    }
}