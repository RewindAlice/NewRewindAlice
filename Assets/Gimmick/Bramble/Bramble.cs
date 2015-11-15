using UnityEngine;
using System.Collections;

public class Bramble : BaseGimmick
{
    private Renderer renderer;
    public bool trapFlag;
    private GameObject pause;
    private Pause pauseScript;
    // 初期化
    void Start()
    {
        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();
        renderer = GetComponentInChildren<Renderer>();
        //renderer.enabled = false;
        trapFlag = false;

        gimmickFlag = false;    // ギミックを非有効に
        gimmickCount = 0;       //
        startActionTurn = 1;
    }

    // 更新
    void Update()
    {
        if(pauseScript.pauseFlag ==false)
        {
            if (trapFlag == true)
            {
                renderer.enabled = true;
            }
            GetComponent<Animator>().SetBool("TrapFlag", trapFlag);
        }
      
    }

    //アリスが動いたときに呼ばれる関数
    public override void OnAliceMoveNext(int aliceMoveTime)
    {
        
    }


    //アリスが戻ったs時に呼ばれる関数
    public override void OnAliceMoveReturn(int aliceMoveTime)
    {
        trapFlag = false;
    }

}
