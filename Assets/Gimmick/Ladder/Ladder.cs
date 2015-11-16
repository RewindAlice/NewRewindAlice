using UnityEngine;
using System.Collections;

public class Ladder : BaseGimmick 
{

    public int growCount;           // 成長段階
    public bool movePossibleFlag;   // 移動可能フラグ
    public int moveCount;

 
    private GameObject pause;
    private Pause pauseScript;
    // 初期化
    void Start()
    {
        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();

        gimmickFlag = false;        // ギミックが有効か判断するフラグに偽を保存
        startActionTurn = 4;        // ギミックを動かし始めるターン数を１に
        gimmickCount = 0;           // ギミックが有効になってからのターン数を０に
        growCount = 0;              // 初期の成長段階
        movePossibleFlag = true;    // 移動可能フラグを真に
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
    public override void OnAliceMoveNext(int aliceMoveCount)
    {
        moveCount++;
        if (startActionTurn <= aliceMoveCount)
        {
            //GetComponent<Renderer>().material.color = new Color(1.0f, 0.7f, 0.3f, 1.0f);
           
            movePossibleFlag = false;    // 移動可能フラグを真に
        }
        else
        {
            
            //GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            movePossibleFlag = true;    // 移動可能フラグを真に
        }
    }

    //アリスが戻った時に呼ばれる関数
    public override void OnAliceMoveReturn(int aliceMoveCount)
    {

        if (startActionTurn >= aliceMoveCount)
        {
            //GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            
            
            movePossibleFlag = true;    // 移動可能フラグを真に
        }
        else
        {
            //GetComponent<Renderer>().material.color = new Color(1.0f, 0.7f, 0.3f, 1.0f);
           
            movePossibleFlag = false;    // 移動可能フラグを真に
        }

        moveCount--;
    }
}
