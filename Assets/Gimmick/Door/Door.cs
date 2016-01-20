using UnityEngine;
using System.Collections;

public class Door : BaseGimmick
{
	public int moveCount = 0;
	public bool openFlag;
	public int openTimingTurn;
	public int turnNum;
    public bool testflag;
    public bool firstOpenDoorFlag;

    public GameObject stage;

	// ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start()
	{
        firstOpenDoorFlag = false;
        stage = GameObject.Find("Stage");
		openFlag = false;
		openTimingTurn = 0;
		turnNum = 0;
	}

	// ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update() { 
        //もしアリスが穴を通っていたら
        if (GameObject.Find("Stage").GetComponent<Stage>().wapAndDoorFlag){}
        else
        {
            if (openTimingTurn > turnNum)
            {
                CloseDoor();
                GameObject.Find("Stage").GetComponent<Stage>().wapAndDoorFlag4 = false;
            }
        }
    }

	// ★アリスが進んだ時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public override void OnAliceMoveNext(int aliceMove)
	{
		// ギミックの開始ターンとアリスの移動数が同じになったら
		if (startActionTurn == aliceMove) { gimmickFlag = true; }  // ギミックを有効にする

		turnNum++;
        if (firstOpenDoorFlag)
        {
            openTimingTurn++;
            firstOpenDoorFlag = false;
        }

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
			openTimingTurn = turnNum;
			openFlag = true;
			gimmickFlag = true;
            GetComponent<Animator>().SetBool("OpenMotionFlag_Next", true);
            GetComponent<Animator>().SetBool("OpenMotionFlag_Return", false);
            firstOpenDoorFlag = true;
		}
	}

	// ★扉を閉じる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void CloseDoor()
	{
        stage.GetComponent<Stage>().wapAndDoorFlag = false;
			openTimingTurn = 0;
            openFlag = false;
            GetComponent<Animator>().SetBool("OpenMotionFlag_Return", true);
            GetComponent<Animator>().SetBool("OpenMotionFlag_Next", false);
            firstOpenDoorFlag = false;
	}
}
