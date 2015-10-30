using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Collections;
public class WatchHandAnimation : MonoBehaviour {
    private int count;

    private float rotRimit;

    private int limitTime;
    private float rotNum;
    Vector3 LeftRot = new Vector3(0, 0, 1);
    Vector3 RightRot = new Vector3(0, 0, -1);
    public GameObject gameMain;
    private GameMain gameMainClass;


	// Use this for initialization
	void Start () {
        count = 0;
        rotRimit = 120.0f;
        limitTime = 0;
        gameMainClass = gameMain.GetComponent<GameMain>();
        //ターン数上限を取得
        limitTime = gameMainClass.limitTurn;

        //回転の限界を、ターン数で÷
        rotNum = (float)rotRimit / (float)limitTime;
	}
	
	// Update is called once per frame
	void Update () {
        //ターン数上限を取得
        limitTime = gameMainClass.limitTurn; ;

        //回転の限界を、ターン数で÷
        rotNum = (float)rotRimit / (float)limitTime;
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
