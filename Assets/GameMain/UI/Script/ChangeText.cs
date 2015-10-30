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

        if (challengeFlag == 0)
        {
            //ステージの番号によって、取得するパスの変更(Story用)
            if (stageNumber == 1)
            {
                limitTalk = 1;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage1-1talk";
            }
            else if (stageNumber == 2)
            {
                limitTalk = 2;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage1-2talk";
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
                limitTalk = 2;
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
        }
        else if (challengeFlag == 1)
        {
            if (stageNumber == 1)
            {
                limitTalk = 6;
                talkNumber = 0;
                filepath = "UI/IntoGame/CharacterTalk/stage1-1talk";
            }
        }       
        
            



          
        

        talkSpeed = 600;
        ReadTextData();
       //this.read();

        this.SetNextLine(lineCount);
        lineCount++;
        
	
	}
	
	// Update is called once per frame
	void Update () {

        if(pauseScript.pauseFlag != true)
        {
            timeCount++;
        }
        
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
