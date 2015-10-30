using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerChange : MonoBehaviour {

    public Text uiText;	// uiTextへの参照を保つ
    public int time = 0;
    public int limit = 5;
    public int limitTime = 5;

    //残りターン数がわかるもの；
    public GameObject gameMain;
    private GameMain gameMainClass;

	// Use this for initialization
    void Start()
    {
        gameMainClass = gameMain.GetComponent<GameMain>();
        GetComponent<Text>().color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {

        //ターンの上限の取得
        limitTime = gameMainClass.turnNum; 

        //残りターンが少なくなったと時文字の色を変更する
        if(limitTime <4)
        {
            GetComponent<Text>().color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
        }
        else
        {
            GetComponent<Text>().color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        }
        //表示する
        uiText.text = limitTime.ToString();

	}
}
