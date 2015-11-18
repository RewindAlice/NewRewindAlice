using UnityEngine;
using System.Collections;

public class Tree : BaseGimmick
{
    // ★BaseGimmickで宣言した変数
    //protected bool gimmickFlag;                       // ギミックフラグ
    //public int startActionTurn;                       // ギミック開始ターン
    //protected int gimmickCount;                       // ギミック開始後のターン数
    //public bool besideDicisionMovePossibleFlag;       // 横判定用移動可能フラグ
    //public bool besideDownDicisionMovePossibleFlag;   // 横下判定用移動可能フラグ

    public int moveCount = 0;   //
    public int growCount = 0;   // 木の成長段階

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void Start()
    {
        besideDicisionMovePossibleFlag = true;      // 初期の横判定用移動可能フラグを真に
        besideDownDicisionMovePossibleFlag = true;  // 初期の横下判定用移動可能フラグを真に
    }

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void Update()
    {

    }

    // ★アリスが進んだ時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public override void OnAliceMoveNext(int aliceMove)
    {
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
                        growCount = 1;                          // 成長段階を１に
                        besideDicisionMovePossibleFlag = true;  // 横判定用移動可能フラグを真に
                        break;
                    // ▼１なら////////////////////////////////////////////
                    case 1:
                        growCount = 2;                          // 成長段階を２に
                        besideDicisionMovePossibleFlag = false; // 横判定用移動可能フラグを真に
                        break;
                    // ▼２なら////////////////////////////////////////////
                    case 2:
                        growCount = 3;                          // 成長段階を３に
                        besideDicisionMovePossibleFlag = false; // 横判定用移動可能フラグを偽に
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

                // ▽ギミックが有効になってからのターン数が
                switch (gimmickCount)
                {
                    case 0:
                        growCount = 0;                          // 成長段階を０に
                        besideDicisionMovePossibleFlag = true;  // 横下判定用移動可能フラグを真に
                        gimmickFlag = false;                    // ギミックフラグを偽に
                        break;
                    case 1:
                        growCount = 1;                          // 成長段階を１に
                        besideDicisionMovePossibleFlag = true;  // 横下判定用移動可能フラグを真に
                        break;
                    case 2:
                        growCount = 2;                          // 成長段階を２に
                        besideDicisionMovePossibleFlag = false; // 横下判定用移動可能フラグを真に
                        break;
                }

                GetComponent<Animator>().SetInteger("GrowCount", growCount);
            }

            moveCount--;
        }
    }
}