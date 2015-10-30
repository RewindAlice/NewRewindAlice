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

        if (stageNumber == 1)
        {
            stageNumber = 111;
        }
        else if (stageNumber == 2)
        {
            stageNumber = 112;
        }
        else if (stageNumber == 3)
        {
            stageNumber = 113;
        }
        else if (stageNumber == 4)
        {
            stageNumber = 114;
        }
        else if (stageNumber == 5)
        {
            stageNumber = 115;
        }
        else if (stageNumber == 6)
        {
            stageNumber = 121;
        }
        else if (stageNumber == 7)
        {
            stageNumber = 122;
        }
        else if (stageNumber == 8)
        {
            stageNumber = 123;
        }
        else if (stageNumber == 9)
        {
            stageNumber = 124;
        }
        else if (stageNumber == 10)
        {
            stageNumber = 125;
        }
        else if (stageNumber == 11)
        {
            stageNumber = 131;
        }
        else if (stageNumber == 12)
        {
            stageNumber = 132;
        }
        else if (stageNumber == 13)
        {
            stageNumber = 133;
        }
        else if (stageNumber == 14)
        {
            stageNumber = 134;
        }
        else if (stageNumber == 15)
        {
            stageNumber = 135;
        }

        else if (stageNumber == 16)
        {
            stageNumber = 141;
        }
        else if (stageNumber == 17)
        {
            stageNumber = 142;
        }
        else if (stageNumber == 18)
        {
            stageNumber = 143;
        }
        else if (stageNumber == 19)
        {
            stageNumber = 144;
        }
        else if (stageNumber == 20)
        {
            stageNumber = 145;
        }
        else if (stageNumber == 21)
        {
            stageNumber = 151;
        }
        else if (stageNumber == 22)
        {
            stageNumber = 152;
        }
        else if (stageNumber == 23)
        {
            stageNumber = 153;
        }
        else if (stageNumber == 24)
        {
            stageNumber = 154;
        }
        else if (stageNumber == 25)
        {
            stageNumber = 201;
        }

        //stageNumber = 111;
       
            //ステージの番号によって、取得するパスの変更(Story用)
            if (stageNumber == 111)
            {
                limitTalk = 1;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage1-1talk";
            }
            else if (stageNumber == 112)
            {
                limitTalk = 2;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage1-2talk";
            }
            else if (stageNumber == 113)
            {
                limitTalk = 3;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage1-3talk";
            }
            else if (stageNumber == 114)
            {
                limitTalk = 3;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage1-4talk";
            }
            else if (stageNumber == 115)
            {
                limitTalk = 4;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage1-5talk";
            }
            else if (stageNumber == 121)
            {
                limitTalk = 2;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage2-1talk";
            }
            else if (stageNumber == 122)
            {
                limitTalk = 2;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage2-2talk";
            }
            else if (stageNumber == 123)
            {
                limitTalk = 3;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage2-3talk";
            }
            else if (stageNumber == 124)
            {
                limitTalk = 4;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage2-4talk";
            }
            else if (stageNumber == 125)
            {
                limitTalk = 4;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage2-5talk";
            }
            else if (stageNumber == 131)
            {
                limitTalk = 2;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage3-1talk";
            }
            else if (stageNumber == 132)
            {
                limitTalk = 3;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage3-2talk";
            }
            else if (stageNumber == 133)
            {
                limitTalk = 3;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage3-3talk";
            }
            else if (stageNumber == 134)
            {
                limitTalk = 4;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage3-4talk";
            }
            else if (stageNumber == 135)
            {
                limitTalk = 4;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage3-5talk";
            }
            else if (stageNumber == 141)
            {
                limitTalk = 2;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage4-1talk";
            }
            else if (stageNumber == 142)
            {
                limitTalk = 3;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage4-2talk";
            }
            else if (stageNumber == 143)
            {
                limitTalk = 3;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage4-3talk";
            }
            else if (stageNumber == 144)
            {
                limitTalk = 4;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage4-4talk";
            }
            else if (stageNumber == 145)
            {
                limitTalk = 4;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage4-5talk";
            }
            else if (stageNumber == 151)
            {
                limitTalk = 2;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage5-1talk";
            }
            else if (stageNumber == 152)
            {
                limitTalk = 3;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage5-2talk"; 
            }
            else if (stageNumber == 153)
            {
                limitTalk = 3;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage5-3talk";
            }
            else if (stageNumber == 154)
            {

                limitTalk = 4;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage5-4talk";
            }

            if (stageNumber == 201)
            {
                limitTalk = 6;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage1-1talk";
            }



          
        

        talkSpeed = 600;
        ReadTextData();
       //this.read();

        this.SetNextLine(lineCount);
        lineCount++;
        
	
	}
	
	// Update is called once per frame
	void Update () {

        if(pauseScript.pauseFlag == true)
        {

        }
        else
        {
            timeCount++;
        }
            //Debug.Log("hoge");
            //Debug.Log( flagManager.sentenceEndFlag );  
        if (timeCount % talkSpeed == 0)
        {
            if (lineCount == limitTalk)
            {
                lineCount = talkNumber;
            }
            SetNextLine(lineCount);
            lineCount++;
            
           

        }

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
}
