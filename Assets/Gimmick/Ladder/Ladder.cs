using UnityEngine;
using System.Collections;

public class Ladder : BaseGimmick 
{
    public int moveCount;

    public bool breakFlag;
    public bool climbPossibleFlag;  // 登れるかのフラグ
 
    private GameObject pause;
    private Pause pauseScript;
    private Renderer renderer;
    public bool drawFlag;       // ギミックの表示フラグ
    // 初期化
    void Start()
    {
        breakFlag = false;
        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();

        renderer = GetComponentInChildren<Renderer>();
        drawFlag = true;    // ギミックの初期表示フラグを真に

        gimmickFlag = false;        // ギミックが有効か判断するフラグに偽を保存
        //startActionTurn = 4;        // ギミックを動かし始めるターン数を１に
        gimmickCount = 0;           // ギミックが有効になってからのターン数を０に
        moveCount = 0;
        climbPossibleFlag = true;
    }

    // 更新
    void Update()
    {
        if (pauseScript.pauseFlag == false)
        {

        }

        // 描画フラグが
        if (drawFlag)
        {
            // 真なら
            renderer.enabled = true;    // ギミックを表示
        }
        else
        {
            // 偽なら
            renderer.enabled = false;   // ギミックを非表示
        }
    }

    //アリスが動いたときに呼ばれる関数
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
                        breakFlag = false;
                        drawFlag = true;
                        climbPossibleFlag = false;
                        break;
                    // ▼１なら////////////////////////////////////////////
                    case 1:
                        breakFlag = true;
                        drawFlag = false;
                        climbPossibleFlag = false;
                        break;
                }

                gimmickCount++;
            }

            moveCount++;
        }
    }

    //アリスが戻った時に呼ばれる関数
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
                    // ▼０なら////////////////////////////////////////////
                    case 0:
                        breakFlag = false;
                        drawFlag = true;
                        climbPossibleFlag = true;
                        break;
                    // ▼１なら////////////////////////////////////////////
                    case 1:
                        breakFlag = false;
                        drawFlag = true;
                        climbPossibleFlag = false;
                        break;
                }
            }

            moveCount--;
        }
    }
}
