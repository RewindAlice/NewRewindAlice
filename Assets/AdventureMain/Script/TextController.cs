using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;

public class TextController : MonoBehaviour
{
    public string[] scenarios;
    [SerializeField]
    Text uiText;

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.05f;

    private string currentText = string.Empty;
    private float timeUntilDisplay = 0;
    private float timeElapsed = 1;
    private int currentLine = 0;
    private int lastUpdateCharacter = -1;
    

    private FlagManager flagManager;

    private int test;

    //メニュー作成
    public bool stopText;
    public bool funcFlag;
    //タイマー
    private int timer;
    const int breakTime = 16;


    private string filepath;


    public GameObject LeftCharacter;
    public GameObject RightCharacter;

    private ChangeCharacterImage leftCharacterImage;
    private ChangeCharacterImage rightCharacterImage;

    public GameObject SE;

    private SEManager seManager;

    public int crickNum;
    public int stageNum;


    public bool storyEndFlag;


    //BGMフェード調整
    const float BGMFadeTime = 60.0f;
    private float BGMTimer;
    private bool BGMDeleter;

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


    void Start()
    {
        storyEndFlag = false;
        stageNum = PlayerPrefs.GetInt ("STORY_NUM");
        crickNum = 0;
        funcFlag = false;
        stopText = false;
        timer = 0;
        BGMDeleter = false;
        BGMTimer = 0;
        switch(stageNum)
        {
            case 11:
            case 12:
                Singleton<SoundPlayer>.instance.playBGM("bgm003", 1.0f);
                break;

            case 21:
           
                Singleton<SoundPlayer>.instance.playBGM("bgm004", 1.0f);
                break;

            case 22:
            case 31: 
                Singleton<SoundPlayer>.instance.playBGM("bgm005", 1.0f);
                break;

            case 32:
                Singleton<SoundPlayer>.instance.playBGM("bgm006", 1.0f);
                break;

            case 41:
                Singleton<SoundPlayer>.instance.playBGM("bgm007", 1.0f);
                break;
            case 42:
                Singleton<SoundPlayer>.instance.playBGM("bgm008", 1.0f);
                break;
            case 51:
            case 52:
                Singleton<SoundPlayer>.instance.playBGM("bgm009", 1.0f);
                break;
            default:

                break;
        }

        seManager = SE.GetComponent<SEManager>();
        leftCharacterImage = LeftCharacter.GetComponent<ChangeCharacterImage>();
        rightCharacterImage = RightCharacter.GetComponent<ChangeCharacterImage>();

        if (stageNum == 11)
        {
            filepath = "Text/StageTalk/stage1-1";
        }
        else if (stageNum == 12)
        {
            filepath = "Text/StageTalk/stage1-2";
        }
        else if (stageNum == 21)
        {
            filepath = "Text/StageTalk/stage2-1";
        }
        else if (stageNum == 22)
        {
            filepath = "Text/StageTalk/stage2-2";
        }
        else if (stageNum == 31)
        {
            filepath = "Text/StageTalk/stage3-1";
        }
        else if (stageNum == 32)
        {
            filepath = "Text/StageTalk/stage3-2";
        }
        else if (stageNum == 41)
        {
            filepath = "Text/StageTalk/stage4-1";
        }
        else if (stageNum == 42)
        {
            filepath = "Text/StageTalk/stage4-2";
        }
        else if (stageNum == 51)
        {
            filepath = "Text/StageTalk/stage5-1";
        }
        else if (stageNum == 52)
        {
            filepath = "Text/StageTalk/stage5-2";
        }
        else if (stageNum == 53)
        {
            filepath = "Text/StageTalk/stage5-3n";
        }
        else
        {
            filepath = "Text/error";
        }

        ReadTextData();
        //this.read();
        flagManager = GetComponent<FlagManager>();
        SetNextLine();
    }


    // 読み込み
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

    void Update()
    {
        Singleton<SoundPlayer>.instance.update();
        //if (Input.GetKeyDown(KeyCode.Escape) && (timer == 0))
        //{
        //    if(stopText == false)
        //    {
        //        stopText = true;
        //    }
        //    else
        //    {
        //        funcFlag = true;
        //    }
        //}
        if (stopText == false)
        {
            //Debug.Log( flagManager.sentenceEndFlag );        
            // 文字の表示が完了してるならクリック時に次の行を表示する
            if (IsCompleteDisplayText)
            {

                flagManager.sentenceEndFlag = true;
                //Debug.Log("hoge");
                //Debug.Log( flagManager.sentenceEndFlag );  
                if ((currentLine < scenarios.Length && Input.GetMouseButtonDown(0)) ||
                    (Input.GetKeyDown(KeyCode.Joystick1Button0)) ||
                    (Input.GetKeyDown(KeyCode.Joystick1Button1)) ||
                    (Input.GetKeyDown(KeyCode.Joystick1Button2)) ||
                    (Input.GetKeyDown(KeyCode.Joystick1Button3)) ||
					(Input.GetKeyDown(KeyCode.W)) ||
					(Input.GetKeyDown(KeyCode.Space)))
                {
                    seManager.SEStop();
                    if (storyEndFlag ==false)
                    {
                        SetNextLine();
                        crickNum++;
                        flagManager.clickCounter++;
                        leftCharacterImage.CharacterChange();
                        rightCharacterImage.CharacterChange();
                        seManager.SEChanger(stageNum, crickNum);
                    }
                   
                }
            }
            else
            {
                // 完了してないなら文字をすべて表示する
                if ((Input.GetMouseButtonDown(0)) ||
					(Input.GetKeyDown(KeyCode.W)) ||
					(Input.GetKeyDown(KeyCode.Space)))
                {
                    timeUntilDisplay = 0;
                }
                flagManager.sentenceEndFlag = false;
            }

            int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
            if (displayCharacterCount != lastUpdateCharacter)
            {
                uiText.text = currentText.Substring(0, displayCharacterCount);
                lastUpdateCharacter = displayCharacterCount;
            }

            if (stageNum == 11 && crickNum == 19)
            {
                storyEndFlag = true;
                EndBgmfunc();
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 1);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            else if (stageNum == 12 && crickNum == 8)
            {
                storyEndFlag = true;
                BGMDeleter = true;
                Singleton<SoundPlayer>.instance.stopBGM(1.0f);
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 7);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            else if (stageNum == 21 && crickNum == 18)
            {
                storyEndFlag = true;
                EndBgmfunc(); 
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 8);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            else if (stageNum == 22 && crickNum == 6)
            {
                storyEndFlag = true;
                EndBgmfunc();
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 14);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            else if (stageNum == 31 && crickNum == 13)
            {
                storyEndFlag = true;
                EndBgmfunc(); 
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();

                    PlayerPrefs.SetInt("STAMP_NUM", 15);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            else if (stageNum == 32 && crickNum == 16)
            {
                storyEndFlag = true;
                EndBgmfunc();
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 21);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            else if (stageNum == 41 && crickNum == 6)
            {
                storyEndFlag = true;
                EndBgmfunc();
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 22);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            else if (stageNum == 42 && crickNum == 18)
            {
                storyEndFlag = true;
                EndBgmfunc();
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 28);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            else if (stageNum == 51 && crickNum == 8)
            {
                storyEndFlag = true;
                EndBgmfunc();
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 29);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            else if (stageNum == 52 && crickNum == 23)
            {
                storyEndFlag = true;    
                EndBgmfunc();
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 34);
                    Application.LoadLevel("EndingScene");
                }
            }
            else if (stageNum == 53 && crickNum == 34)
            {
                storyEndFlag = true;
                EndBgmfunc();
                if (BGMTimer > BGMFadeTime)
                {
                    Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                    PlayerPrefs.SetInt("STAMP_NUM", 35);
                    Application.LoadLevel("StageSelectScene");
                }
            }
            if(stageNum == 52 && crickNum == 10)
            {
                Singleton<SoundPlayer>.instance.playBGM("bgm010", 1.0f);
            }
        }
        else
        {
            //if(Input.GetKey(KeyCode.Space))//Wへ変更予定
            //{
            //    funcFlag = true;
            //}

        }
        if(BGMDeleter)
        {
            BGMTimer++;
        }

        //backGame();
    }
    //void backGame()
    //{
    //    if(funcFlag)
    //    {
    //        //タイマーが必要な場合につけてください
    //        if (timer > breakTime)
    //        {
    //            stopText = false;
    //            funcFlag = false;
    //            timer = 0;
    //        }
    //        else
    //        {
    //           timer++;
    //        }
    //    }
    //}
    
    void SetNextLine()
    {
        currentText = scenarios[currentLine];
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;
        currentLine++;
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

    void EndBgmfunc()
    {
        BGMDeleter = true;
        Singleton<SoundPlayer>.instance.stopBGM(1.0f);
    }
}