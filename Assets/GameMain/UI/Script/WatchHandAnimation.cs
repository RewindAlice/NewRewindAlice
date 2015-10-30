using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
public class WatchHandAnimation : MonoBehaviour {
    private int count;

    private float rotRimit;

    private int rimitTime;
    private float rotNum;
    Vector3 LeftRot = new Vector3(0, 0, 1);
    Vector3 RightRot = new Vector3(0, 0, -1);
    public GameObject gameMain;
    private GameMain gameMainClass;


	// Use this for initialization
	void Start () {
        count = 0;
        rotRimit = 118.0f;
        rimitTime = 50;
        gameMainClass = gameMain.GetComponent<GameMain>();
        //ターン数上限を取得
        //rimitTime = gameMainClass.stageTurnNum; newRewindAliceの移行に従ってコメント化

        //回転の限界を、ターン数で÷
        rotNum = (float)rotRimit / (float)rimitTime;
	}
	
	// Update is called once per frame
	void Update () {
        //ターン数上限を取得
        //rimitTime = 

        ////回転の限界を、ターン数で÷
        //rotNum = (float)rotRimit / (float)rimitTime;
        //count++;
        //ターン数上限を取得
		//rimitTime = gameMainClass.stageTurnNum; newRewindAliceの移行に従ってコメント化

        //回転の限界を、ターン数で÷
        rotNum = (float)rotRimit / (float)rimitTime;

        //if (count == 30)
        //{
        //    NextTurn();
        //    count = 0;
        //}

	}

    public void NextTurn()
    {
        //ターンが立つたび
        this.transform.Rotate(RightRot, rotNum);
    }

    public void BackTurn()
    {
        //ターンが戻るたび
        this.transform.Rotate(RightRot, -rotNum);
    }
}
