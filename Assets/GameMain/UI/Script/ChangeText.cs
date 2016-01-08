using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
public class ChangeText : MonoBehaviour {

    public string[] scenarios;
	[SerializeField] Text uiText;
    
    [SerializeField][Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.05f;

    private string currentText = string.Empty;
    private float timeUntilDisplay = 0;
    private float timeElapsed = 1;
    private int currentLine = 0;
    private int lastUpdateCharacter = -1;

    private int timeCount;
    private int lineCount;

    private string filepath;
    private int stageNumber;

    //会話の限界
    private int limitTalk;
    //ヒントなしの話の数
    private int talkNumber;

    private int talkSpeed;
    private GameObject pause;
    private Pause pauseScript;

    private int challengeFlag;

    //読み込み改善用
    //////////////////////////////////////////////////////////////////
    /// <summary>
    /// 読み込んだテキストデータを格納するテキストアセット
    /// </summary>
    public TextAsset stageTextAsset;
    /// <summary>
    /// ステージの文字列データ
    /// </summary>
    public string stageData;
    //////////////////////////////////////////////////////////////

    public GameObject gameMain;
    public int tutorialCount;

    public bool Android;

    // 文字の表示が完了しているかどうか
    public bool IsCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeUntilDisplay; }
    }

    public void read()
    {

        FileInfo fi = new FileInfo(Application.dataPath + "/" + filepath);
        StreamReader sr = new StreamReader(fi.OpenRead());

        int i = 0;
        while (sr.Peek() != -1)
        {
            scenarios[i] = sr.ReadLine();
            i++;
        }
        i = 0;
        Debug.Log("yomikomi");
        sr.Close();
    }


	// Use this for initialization
	void Start () {

        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();
        timeCount = 0;
        lineCount = 0;
        //ステージの番号を取得
        stageNumber = PlayerPrefs.GetInt("STAGE_NUM");
        challengeFlag = PlayerPrefs.GetInt("CHALLENGE");

     
        //ステージの番号によって、取得するパスの変更(Story用)
        if (stageNumber == 1)
        {
            limitTalk = 8;
            talkNumber = 0;
            if(Android)
            {
                filepath = "UI/IntoGame/CharacterTalk/stage1-1talkS";
            }
            else
            {
                filepath = "UI/IntoGame/CharacterTalk/stage1-1talk";
            }
        }
        else if (stageNumber == 2)
        {
            limitTalk = 4;
            talkNumber = 0;
            if (Android)
            {
                filepath = "UI/IntoGame/CharacterTalk/stage1-2talkS";
            }
            else
            {
                filepath = "UI/IntoGame/CharacterTalk/stage1-2talk";
            }
        }
        else if (stageNumber == 3)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage1-3talk";
        }
        else if (stageNumber == 4)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage1-4talk";
        }
        else if (stageNumber == 5)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage1-5talk";
        }
        else if (stageNumber == 6)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage2-1talk";
        }
        else if (stageNumber == 7)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage2-2talk";
        }
        else if (stageNumber == 8)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage2-3talk";
        }
        else if (stageNumber == 9)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage2-4talk";
        }
        else if (stageNumber == 10)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage2-5talk";
        }
        else if (stageNumber == 11)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage3-1talk";
        }
        else if (stageNumber == 12)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage3-2talk";
        }
        else if (stageNumber == 13)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage3-3talk";
        }
        else if (stageNumber == 14)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage3-4talk";
        }
        else if (stageNumber == 15)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage3-5talk";
        }
        else if (stageNumber == 16)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage4-1talk";
        }
        else if (stageNumber == 17)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage4-2talk";
        }
        else if (stageNumber == 18)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage4-3talk";
        }
        else if (stageNumber == 19)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage4-4talk";
        }
        else if (stageNumber == 20)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage4-5talk";
        }
        else if (stageNumber == 21)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage5-1talk";
        }
        else if (stageNumber == 22)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage5-2talk";
        }
        else if (stageNumber == 23)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage5-3talk";
        }
        else if (stageNumber == 24)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage5-4talk";
        }
        else if (stageNumber == 25)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage25talk";
        }
        else if (stageNumber == 26)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage26talk";
        }
        else if (stageNumber == 27)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage27talk";
        }
        else if (stageNumber == 28)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage28talk";
        }
        else if (stageNumber == 29)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage29talk";
        }
        else if (stageNumber == 30)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage30talk";
        }
        else if (stageNumber == 31)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage31talk";
        }
        else if (stageNumber == 32)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage32talk";
        }
        else if (stageNumber == 33)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage33talk";
        }
        else if (stageNumber == 34)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage34talk";
        }
        else if (stageNumber == 35)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage35talk";
        }
        else if (stageNumber == 36)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage36talk";
        }
        else if (stageNumber == 37)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage37talk";
        }
        else if (stageNumber == 38)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage38talk";
        }
        else if (stageNumber == 39)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage39talk";
        }
        else if (stageNumber == 40)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage40talk";
        }
        else if (stageNumber == 41)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage41talk";
        }
        else if (stageNumber == 42)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage42talk";
        }
        else if (stageNumber == 43)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage43talk";
        }
        else if (stageNumber == 44)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage44talk";
        }
        else if (stageNumber == 45)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterTalk/stage45talk";
        }
       
        gameMain = GameObject.Find("GameMain");

        talkSpeed = 300;

        //テキストの読み込み
        ReadTextData();

        //一ライン書き出し
        this.SetNextLine(lineCount);
        lineCount++;
    
        //チュートリアルのカウントの初期化
        tutorialCount = 1;
    
	}
	
	// Update is called once per frame
	void Update () {

        if (pauseScript.pauseFlag != true)
        {
            if (gameMain.GetComponent<GameMain>().tutorialFlag)
            {
                if (gameMain.GetComponent<GameMain>().stageNumber == 1)
                {
                    ChangeSpeed();
                    if (tutorialCount == 3 || tutorialCount == 5 || tutorialCount == 7 || tutorialCount == 8 || tutorialCount == 9)
                    {

                    }
                    else
                    {
                        timeCount++;
                    }
                }
                else if (gameMain.GetComponent<GameMain>().stageNumber == 2)
                {
                    if (tutorialCount == 1 || tutorialCount == 2 || tutorialCount == 3||tutorialCount == 4 ||tutorialCount == 5)
                    {

                    }
                    else
                    {
                        timeCount++;
                    }
                }
            }
            else
            {
                timeCount++;
            }
        }

        if (gameMain.GetComponent<GameMain>().tutorialFlag)
        {
            if (timeCount == talkSpeed)
            {

                SetNextLine(tutorialCount);
                timeCount = 0;
                tutorialCount++;
            }
        }
        else
        {
      
            if (timeCount % talkSpeed == 0)
            {
                if (lineCount == limitTalk)
                {
                    lineCount = talkNumber;
                }

                SetNextLine(lineCount);
                lineCount++;

            }
      
        }


        ////過去正規版
        //if (pauseScript.pauseFlag != true)
        //{
        //    timeCount++;
        //}
        
        //if (timeCount % talkSpeed == 0)
        //{
        //    if (lineCount == limitTalk)
        //    {
        //        lineCount = talkNumber;
        //    }

        //    SetNextLine(lineCount);
        //    lineCount++;

        //}
       

        //テキスト更新処理
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
        if (displayCharacterCount != lastUpdateCharacter)
        {
            uiText.text = currentText.Substring(0, displayCharacterCount);
            lastUpdateCharacter = displayCharacterCount;
        }
	}

    // 読み込み
   
    void SetNextLine(int line)
    {
    
            currentText = scenarios[line];
            timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
            timeElapsed = Time.time;
            //currentLine++;
            lastUpdateCharacter = -1;
     
        
    }

        //.txtを読み込むときにreadの部分で差し換えてください
    void ReadTextData()
    {
            // TextAssetとして、Resourcesフォルダからテキストデータをロードする
            stageTextAsset = Resources.Load(filepath, typeof(TextAsset)) as TextAsset;
            // 文字列を代入
            stageData = stageTextAsset.text;
            scenarios = stageData.Split("\n"[0]); 
    }

    public void TutorialNext()
    {
        SetNextLine(tutorialCount);
        tutorialCount++;
        timeCount = 0;
    }
    
    public void TutorialNextNumber(int number)
    {
        SetNextLine(number);
        tutorialCount = number+1;
        timeCount = 0;
    }

    public void ChangeSpeed()
    {
        if (gameMain.GetComponent<GameMain>().stageNumber == 1)
        {
         if(tutorialCount == 1)
         {
             talkSpeed = 270;

         }
         else if (tutorialCount == 2)
         {
             talkSpeed = 220;

         }
         else if (tutorialCount == 4)
         {
             talkSpeed = 270;
         }
         else if (tutorialCount == 6)
         {
             talkSpeed = 240;

         }
         else if (tutorialCount == 7)
         {
             talkSpeed = 180;

         }
        }
    }
}
