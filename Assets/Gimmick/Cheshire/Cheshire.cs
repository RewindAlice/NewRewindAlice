using UnityEngine;
using System.Collections;

public class Cheshire : BaseGimmick
{
	// ★BaseGimmickで宣言した変数
	//protected bool gimmickFlag;     // ギミックフラグ
	//public int startActionTurn;     // ギミック開始ターン
	//protected int gimmickCount;     // ギミック開始後のターン数
	//public bool movePossibleFlag;   // 移動可能フラグ

	public int moveCount = 0;   //
	public int turnNum;
	public bool invisibleFlag;
	public int startInvisibleTurn; // 入手ターン
	private Renderer renderer;

	// ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start()
	{
		turnNum = 0;
		startInvisibleTurn = 0;
		invisibleFlag = false;
		renderer = GetComponentInChildren<Renderer>();
	}

	// ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update()
	{
		RemoveInvisible();
	}

	// ★アリスが進んだ時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public override void OnAliceMoveNext(int aliceMove)
	{
		print("Cheshire");  // デバッグ用コメント

		// ギミックの開始ターンとアリスの移動数が同じになったら
		if (startActionTurn == aliceMove)
		{
			gimmickFlag = true; // ギミックを有効にする
		}

		// アリスの移動数が多かったら
		if (moveCount < aliceMove)
		{
			moveCount++;
		}
		turnNum++;
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
			}
			moveCount--;
		}
		turnNum--;
	}

	// ★透明化処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void StartInvisible()
	{
		if (!invisibleFlag)
		{
			startInvisibleTurn = turnNum + 1;
			invisibleFlag = true;
			renderer.enabled = false;    // 描画しない
		}
	}

	// ★透明化解除処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void RemoveInvisible()
	{
		if (startInvisibleTurn > turnNum)
		{
			startInvisibleTurn = 0;
			invisibleFlag = false;
			renderer.enabled = true;    // 描画しない
		}
	}
}
