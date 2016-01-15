using UnityEngine;
using System.Collections;

public class Cheshire : BaseGimmick
{
	public int moveCount = 0; 
	public int turnNum;
	public bool invisibleFlag;
	public int startInvisibleTurn;
	private Renderer renderer;
	public bool animationFlag;
	public bool returnAnimationFlag;
	public int animationTimer;
	// ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start()
	{
		turnNum = 0;
		startInvisibleTurn = 0;
		invisibleFlag = false;
		animationFlag = false;
		returnAnimationFlag = false;
		animationTimer = 0;
		renderer = GetComponentInChildren<Renderer>();
	}

	// ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update() 
	{ 
		RemoveInvisible();

		Animation();
	}

	// ★アリスが進んだ時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public override void OnAliceMoveNext(int aliceMove)
	{
		// ギミックの開始ターンとアリスの移動数が同じになったら
		if (startActionTurn == aliceMove) { gimmickFlag = true; } // ギミックを有効にする

		// アリスの移動数が多かったら
		if (moveCount < aliceMove) { moveCount++; }
		animationFlag = true;
		turnNum++;
	}

	// ★アリスが戻った時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public override void OnAliceMoveReturn(int aliceMove)
	{
		// ギミックの開始ターンがアリスの移動数より大きかったら
		if (startActionTurn > aliceMove) { gimmickFlag = false; } // ギミックを無効にする

		// ギミックが有効なら
		if (gimmickFlag)
		{
			if (moveCount == aliceMove) { gimmickCount--; }
			
			moveCount--;
		}
		returnAnimationFlag = true; 
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

	// ★アニメーション処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void Animation()
	{
		if (animationFlag)
		{
			GetComponent<Animator>().SetBool("motionFlag", animationFlag);


			animationTimer++;

			if (animationTimer == 30   )
			{
				animationFlag = false;
				animationTimer = 0;
				GetComponent<Animator>().SetBool("motionFlag", animationFlag);
			}

		}

		else if (returnAnimationFlag)
		{
			GetComponent<Animator>().SetBool("returnMotionFlag", returnAnimationFlag);


			animationTimer++;

			if (animationTimer == 30)
			{
				returnAnimationFlag = false;
				animationTimer = 0;
				GetComponent<Animator>().SetBool("returnMotionFlag", returnAnimationFlag);
			}
		}
	}
}
