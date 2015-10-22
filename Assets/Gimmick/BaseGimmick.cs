using UnityEngine;
using System.Collections;

public class BaseGimmick : MonoBehaviour
{
    protected bool gimmickFlag;             // ギミックが有効か判断するフラグ
    protected int gimmickStartTurn;         // ギミックが動き始めるターン数
    protected int gimmickStartTurnCount;    // ギミックが動き始めてからのターン数

    protected bool besideDecisionFlag;      // 横判定時の移動可能判定用フラグ
    protected bool besideDownDecisionFlag;  // 横下判定時の移動可能判定用フラグ

	// 初期化
	void Start ()
    {
        gimmickFlag = false;
        gimmickStartTurn = 0;
        gimmickStartTurnCount = 0;
	}
	
	// 更新
	void Update ()
    {
	
	}

    // アリスが進んだときに呼ばれる関数
    public virtual void OnAliceNext(int aliceMoveCount)
    {

    }

    // アリスが戻ったときに呼ばれる関数
    public virtual void OnAliceReturn(int aliceMoveCount)
    {

    }

    // ギミックが有効になるターン数を設定する
    public virtual void setGimmickStartTurn(int startTurn){ gimmickStartTurn = startTurn; }

    // 横判定時の移動可能判定用フラグの取得
    public virtual bool getBesideDecisionFlag() { return besideDecisionFlag; }

    // 横下判定時の移動可能判定用フラグの取得
    public virtual bool getBesideDownDecisionFlag() { return besideDownDecisionFlag; }
}
