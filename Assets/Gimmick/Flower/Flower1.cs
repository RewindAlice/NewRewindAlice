using UnityEngine;
using System.Collections;

public class Flower1 : BaseGimmick
{
    // ★BaseGimmickで宣言した変数
    //protected bool gimmickFlag;                       // ギミックフラグ
    //public int startActionTurn;                       // ギミック開始ターン
    //protected int gimmickCount;                       // ギミック開始後のターン数
    //public bool besideDicisionMovePossibleFlag;       // 横判定用移動可能フラグ
    //public bool besideDownDicisionMovePossibleFlag;   // 横下判定用移動可能フラグ

    public int moveCount;
    public int motionCount;
    private GameObject pause;
    private Pause pauseScript;

    public GameObject children;
    public BlockMaterialChanger changer;

    // 初期化
    void Start()
    {
        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();
        gimmickFlag = true;         // ギミックが有効か判断するフラグに真を保存
        startActionTurn = 1;        // ギミックを動かし始めるターン数を１に
        gimmickCount = 0;           // ギミックが有効になってからのターン数を０に
        besideDicisionMovePossibleFlag = true;    // 移動可能フラグを真に
        moveCount = 0;
        motionCount = 0;
        GetComponent<Animator>().SetInteger("MotionCount", motionCount);

       
    }

    // 更新
    void Update()
    {
        if (pauseScript.pauseFlag == false)
        {

        }
    }


    public void changeMaterial(int number)
    {
        changer = children.GetComponent<BlockMaterialChanger>();
        changer.MatelialChange(number);
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
                    besideDicisionMovePossibleFlag = true;    // 移動可能フラグを真に
                    break;
                case 1:
                    besideDicisionMovePossibleFlag = false;   // 移動可能フラグを偽に
                    break;
                case 2:
                    besideDicisionMovePossibleFlag = true;    // 移動可能フラグを真に
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
                    besideDicisionMovePossibleFlag = true;    // 移動可能フラグを真に
                    break;
                case 1:
                    besideDicisionMovePossibleFlag = false;   // 移動可能フラグを偽に
                    break;
                case 2:
                    besideDicisionMovePossibleFlag = true;    // 移動可能フラグを真に
                    break;
            }
            GetComponent<Animator>().SetInteger("MotionCount", motionCount % 3);
            moveCount--;
        }
    }
}