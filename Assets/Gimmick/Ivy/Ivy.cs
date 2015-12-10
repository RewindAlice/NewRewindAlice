using UnityEngine;
using System.Collections;

public class Ivy : BaseGimmick {


    // ★BaseGimmickで宣言した変数
    //protected bool gimmickFlag;                       // ギミックフラグ
    //public int startActionTurn;                       // ギミック開始ターン
    //protected int gimmickCount;                       // ギミック開始後のターン数
    //public bool besideDicisionMovePossibleFlag;       // 横判定用移動可能フラグ
    //public bool besideDownDicisionMovePossibleFlag;   // 横下判定用移動可能フラグ

    public int growCount;           // 成長段階
    public int moveCount;
    public bool eraseFlag;           //ステージからフラグを回収させる


    public GameObject Ivychild_one;
    public GameObject Ivychild_two;

    public MaterialChanger Ivy_one;
    public MaterialChanger Ivy_two;
    private GameObject pause;
    private Pause pauseScript;

    // 初期化
    void Start()
    {
        eraseFlag = false;
        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();
        Ivy_one = Ivychild_one.GetComponent<MaterialChanger>();
        Ivy_two = Ivychild_two.GetComponent<MaterialChanger>();

        gimmickFlag = false;        // ギミックが有効か判断するフラグに偽を保存
        //startActionTurn = 4;        // ギミックを動かし始めるターン数を１に
        gimmickCount = 0;           // ギミックが有効になってからのターン数を０に
        growCount = 0;              // 初期の成長段階
        besideDicisionMovePossibleFlag = true;    // 移動可能フラグを真に
        moveCount = 0;
    }

    // 更新
    void Update()
    {
        if (pauseScript.pauseFlag == false)
        {

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
                        Ivy_one.changeMaterialBrown();
                        Ivy_two.changeMaterialBrown();
                        eraseFlag = false;
                        break;
                    // ▼１なら////////////////////////////////////////////
                    case 1:
                        Ivy_one.changeMaterialErase();
                        Ivy_two.changeMaterialErase();
                        eraseFlag = true;
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
                        Ivy_one.changeMaterialOrizinal();
                        Ivy_two.changeMaterialOrizinal();
                        eraseFlag = false;
                        break;
                    // ▼１なら////////////////////////////////////////////
                    case 1:
                        Ivy_one.changeMaterialBrown();
                        Ivy_two.changeMaterialBrown();
                        eraseFlag = false;
                        break;
                }
            }

            moveCount--;
        }
    }

    public bool GetErase()
    {
        return eraseFlag;
    }
}
