using UnityEngine;
using System.Collections;

public class ModeChange : BaseGimmick
{
    // ★BaseGimmickで宣言した変数
    //protected bool gimmickFlag;                       // ギミックフラグ
    //public int startActionTurn;                       // ギミック開始ターン
    //protected int gimmickCount;                       // ギミック開始後のターン数
    //public bool besideDicisionMovePossibleFlag;       // 横判定用移動可能フラグ
    //public bool besideDownDicisionMovePossibleFlag;   // 横下判定用移動可能フラグ

    public int itemCode;
    const int setEffectTimer = 180;
    public int effectTimer = 0;
    public bool playingEffectFlag;
    private Renderer renderer;
    public int touchCount;      // キノコに触れてからのターン数
    public bool drawFlag;       // ギミックの表示フラグ

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void Start()
    {
        playingEffectFlag = false;
        gimmickFlag = true;                         // 初期のギミックフラグを真に
        besideDicisionMovePossibleFlag = true;      // 初期の横判定用移動可能フラグを真に
        besideDownDicisionMovePossibleFlag = true;  // 初期の横下判定用移動可能フラグを真に

        renderer = GetComponentInChildren<Renderer>();
        touchCount = 0;
        drawFlag = true;    // ギミックの初期表示フラグを真に
    }

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void Update()
    {
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

    // ★アリスが進んだ時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public override void OnAliceMoveNext(int aliceMove)
    {
        // ギミックが有効でない（既に触れている）
        if(!gimmickFlag)
        {
            touchCount++;   // 触れてからのターン数を増やす
        }
    }

    // ★アリスが戻った時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public override void OnAliceMoveReturn(int aliceMove)
    {
        // ギミックが有効でない（既に触れている）
        if (!gimmickFlag)
        {
            touchCount--;   // 触れてからのターン数を減らす

            // 触れてからのターン数が－１なら
            if(touchCount == 0)
            {
                gimmickCount = 0;
                gimmickFlag = true;
                drawFlag = true;
                OutPutEffect();
            }
        }
    }

    public void OutPutEffect()
    {
        //空のオブジェクトを生成する
        GameObject prefab = null;
        bool effctBigFlag = false;
        //アイコムコードでエフェクトの色を識別する
        switch (itemCode)
        {
            //キノコ小さい
            case 33:
                effctBigFlag = false;
                break;
            //キノコ大きい
            case 34:
                effctBigFlag = true;
                break;
            //ポーション小さい
            case 35:
                effctBigFlag = false;
                break;
            //ポーション大きい
            case 36:
                effctBigFlag = true;
                break;
        }
        //大きくするなら
        if (effctBigFlag)
        {
            prefab = (GameObject)Resources.Load("Particles/GiganticEffect");
        }
        else 
        {
            prefab = (GameObject)Resources.Load("Particles/MinuteEffect");            
        }
        if (prefab != null)
        {
            //オブジェクトを作ると破棄.
            GameObject instant_object = (GameObject)GameObject.Instantiate(prefab,
                           this.transform.localPosition, Quaternion.identity);
            GameObject.Destroy(instant_object, 3);
        }

    }
    public void SetItemCode(int num)
    {
        itemCode = num;
    }
    public bool GetRendererEnabled()
    {
        return renderer.enabled;
    }
}