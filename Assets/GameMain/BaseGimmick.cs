using UnityEngine;
using System.Collections;

public class BaseGimmick : MonoBehaviour
{
    protected bool gimmickFlag;                     // ギミックフラグ
    public int startActionTurn;                     // ギミック開始ターン
    protected int gimmickCount;                     // ギミック開始後のターン数
    public bool besideDicisionMovePossibleFlag;     // 横判定用移動可能フラグ
    public bool besideDownDicisionMovePossibleFlag; // 横下判定用移動可能フラグ


    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        gimmickFlag = false;    // 初期のギミックフラグを偽に
        startActionTurn = 0;    // 初期のギミック開始ターンを０に
        gimmickCount = 0;       // 初期のギミック開始後のターン数を０に

        besideDicisionMovePossibleFlag = false;         // 初期の横判定用移動可能フラグを偽に
        besideDownDicisionMovePossibleFlag = false;     // 初期の横下判定用移動可能フラグを偽に
	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {

	}

    // ★アリスが進んだ時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public virtual  void OnAliceMoveNext(int aliceMove)
    {
        // ここにギミックごとのターン経過による変化を描く
    }

    // ★アリスが戻った時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public virtual void OnAliceMoveReturn(int aliceMove)
    {
        // ここにギミックごとのターン経過による変化を描く
    }

    // ★ギミックフラグを設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public virtual void SetGimmickFlag(bool flag)
    {
        gimmickFlag = flag;
    }

    // ★ギミックフラグを取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public virtual bool GetGimmickFlag()
    {
        return gimmickFlag; // ギミックフラグを返す
    }

    // ★ギミックの動き始めるターン数を設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public virtual void SetStartActionTurn(int actionTurn)
    {
        startActionTurn = actionTurn;   // ギミック開始ターンを設定
    }

    // ★横判定用移動可能フラグを設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public virtual void SetBesideDicisionMovePossibleFlag(bool movePossibleFlag)
    {
        besideDicisionMovePossibleFlag = movePossibleFlag;  // 移動可能フラグを設定
    }

    // ★横判定用移動可能フラグを取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public virtual bool GetBesideDicisionMovePossibleFlag()
    {
        return besideDicisionMovePossibleFlag;  // 移動可能フラグを返す
    }

    // ★横下判定用移動可能フラグを設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public virtual void SetBesideDownDicisionMovePossibleFlag(bool movePossibleFlag)
    {
        besideDownDicisionMovePossibleFlag = movePossibleFlag;  // 移動可能フラグを設定
    }

    // ★横下判定用移動可能フラグを取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public virtual bool GetBesideDownDicisionMovePossibleFlag()
    {
        return besideDownDicisionMovePossibleFlag;  // 移動可能フラグを返す
    }

    public int GetGimmckCount()
    {
        return gimmickCount;
    }
}