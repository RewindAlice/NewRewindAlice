using UnityEngine;
using System.Collections;

public class Door : BaseGimmick
{
	public int moveCount = 0;
	public bool openFlag;
	public int openTimingTurn;
	public int turnNum;

	// ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start()
	{
		openFlag = false;
		openTimingTurn = 0;
		turnNum = 0;
	}

	// ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update() { if (openTimingTurn > turnNum) { CloseDoor(); } }

	// ★アリスが進んだ時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public override void OnAliceMoveNext(int aliceMove)
	{
		// ギミックの開始ターンとアリスの移動数が同じになったら
		if (startActionTurn == aliceMove) { gimmickFlag = true; }  // ギミックを有効にする

		turnNum++;
	}

	// ★アリスが戻った時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public override void OnAliceMoveReturn(int aliceMove)
	{
		// ギミックの開始ターンがアリスの移動数より大きかったら
		if (startActionTurn > aliceMove) { gimmickFlag = false; } // ギミックを無効にする
		
		turnNum--;
	}

	// ★鍵を入手し、隣接した時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void OpenDoor()
	{
		if (!openFlag)
		{
			openTimingTurn = turnNum + 1;
			openFlag = true;
			gimmickFlag = true;
            GetComponent<Animator>().SetBool("OpenMotionFlag_Next", true);
            GetComponent<Animator>().SetBool("OpenMotionFlag_Return", false);
		}
	}

	// ★扉を閉じる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void CloseDoor()
	{
			openTimingTurn = 0;
			openFlag = false;
            GetComponent<Animator>().SetBool("OpenMotionFlag_Next", false);
            GetComponent<Animator>().SetBool("OpenMotionFlag_Return", true);
	}
}
