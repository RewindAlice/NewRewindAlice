using UnityEngine;
using System.Collections;

public class Tree : BaseGimmick
{
    // ★BaseGimmickで宣言した変数
    //protected bool gimmickFlag;     // ギミックフラグ
    //public int startActionTurn;     // ギミック開始ターン
    //protected int gimmickCount;     // ギミック開始後のターン数
    //public bool movePossibleFlag;   // 移動可能フラグ

    public int moveCount = 0;   //
    public int growCount = 0;   // 木の成長段階

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void Start()
    {
        
    }

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void Update()
    {

    }

    // ★アリスが進んだ時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public override void OnAliceMoveNext(int aliceMove)
    {
        print("Tree");  // デバッグ用コメント

        // ギミックの開始ターンとアリスの移動数が同じになったら
        if (startActionTurn == aliceMove)
        {
            gimmickFlag = true; // ギミックを有効にする
        }

        // アリスの移動数が多かったら
        if (moveCount < aliceMove)
        {
            // ギミックが有効なら
            if (gimmickFlag)
            {
                // ▽ギミックが有効になってからのターン数が
                switch (gimmickCount)
                {
                    // ▼０なら////////////////////////////////////////////
                    case 0:
                        growCount = 1;              // 成長段階を１に
                        movePossibleFlag = true;    // 移動可能フラグを真に
                        break;
                    // ▼１なら////////////////////////////////////////////
                    case 1:
                        growCount = 2;              // 成長段階を２に
                        movePossibleFlag = false;   // 移動可能フラグを偽に
                        break;
                    // ▼２なら////////////////////////////////////////////
                    case 2:
                        growCount = 3;              // 成長段階を３に
                        movePossibleFlag = false;   // 移動可能フラグを偽に
                        break;
                }

                gimmickCount++;
                GetComponent<Animator>().SetInteger("GrowCount", growCount);
            }

            moveCount++;
        }
    }

    // ★アリスが戻った時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public override void OnAliceMoveReturn(int aliceMove)
    {
        // ギミックの開始ターンがアリスの移動数より大きかったら
        if (startActionTurn > aliceMove)
        {
            gimmickFlag = false;    // ギミックを無効にする
        }

        // ギミックが有効なら
        if (gimmickFlag)
        {
            //
            if (moveCount == aliceMove)
            {
                gimmickCount--;

                switch (gimmickCount)
                {
                    case 0:
                        growCount = 0;
                        movePossibleFlag = true;
                        gimmickFlag = false;
                        break;
                    case 1:
                        growCount = 1;
                        movePossibleFlag = true;
                        break;
                    case 2:
                        growCount = 2;
                        movePossibleFlag = false;
                        break;
                }

                GetComponent<Animator>().SetInteger("GrowCount", growCount);
            }

            moveCount--;
        }
    }
}
