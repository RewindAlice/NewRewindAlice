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

    public GameObject nameTextController;
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

    private int fadeCount;
    public TouchController touchController;

    public GameObject pause;
    // 文字の表示が完了しているかどうか
    public bool IsCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeUntilDisplay; }
    }


    void Start()
    {
        fadeCount = 0;
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
        flagManager = GetComponent<FlagManager>();
        nameTextController = GameObject.Find("NameTextController");
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
        
        if (stopText == false)
        {
            fadeCount++;

            //pause起動
            if (((touchController.touchPosX > 1200) && (touchController.touchPosX < 1280)) &&
                ((touchController.touchPosY > 640) && (touchController.touchPosY < 720)) &&
                ((touchController.detachPosX > 1200) && (touchController.detachPosX < 1280)) &&
                ((touchController.detachPosY > 640) && (touchController.detachPosY < 720)))
            {
                touchController.TouchPostionInitialize();
                pause.GetComponent<StoryPause>().text.stopText = true;//ストーリーのテキストを止める
                pause.GetComponent<StoryPause>().name.stopText = true;//名前のテキストを止める
                pause.GetComponent<StoryPause>().pauseImageManager1.GetComponent<Image>().enabled = true;
                pause.GetComponent<StoryPause>().pauseImageManager2.GetComponent<Image>().enabled = true;
                //pauseImageManager3.GetComponent<Image>().enabled = true;
                pause.GetComponent<StoryPause>().pauseImageManager4.GetComponent<Image>().enabled = true;
                pause.GetComponent<StoryPause>().pauseImageManager5.GetComponent<Image>().enabled = true;
                pause.GetComponent<StoryPause>().pauseImageManager6.GetComponent<Image>().enabled = true;
                pause.GetComponent<StoryPause>().Initialize();
       
            }
            else
            {
                if(pause.GetComponent<StoryPause>().pauseFlag == false && fadeCount > 50)
                {
                    if (IsCompleteDisplayText)
                    {

                        flagManager.sentenceEndFlag = true;
                        //Debug.Log("hoge");
                        //Debug.Log( flagManager.sentenceEndFlag );  
                        if ((currentLine < scenarios.Length && Input.GetMouseButtonDown(0)) ||
                            (currentLine < scenarios.Length && Input.GetKeyDown(KeyCode.Joystick1Button0)) ||
                            (currentLine < scenarios.Length && Input.GetKeyDown(KeyCode.Joystick1Button1)) ||
                            (currentLine < scenarios.Length && Input.GetKeyDown(KeyCode.Joystick1Button2)) ||
                            (currentLine < scenarios.Length && Input.GetKeyDown(KeyCode.Joystick1Button3)) ||
                            (currentLine < scenarios.Length && Input.GetKeyDown(KeyCode.W)) ||
                            (currentLine < scenarios.Length && Input.GetKeyDown(KeyCode.Space)))
                        {
                            seManager.SEStop();
                            if (storyEndFlag == false)
                            {

                                SetNextLine();
                                nameTextController.GetComponent<NameTextController>().TextUpdate();
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
                        PlayerPrefs.SetInt("Story1_1Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();

                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 1);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                        }
                    }
                    else if (stageNum == 12 && crickNum == 7)
                    {
                        PlayerPrefs.SetInt("Story1_2Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 7);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                        }
                    }
                    else if (stageNum == 21 && crickNum == 17)
                    {
                        PlayerPrefs.SetInt("Story2_1Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 8);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                        }
                    }
                    else if (stageNum == 22 && crickNum == 5)
                    {
                        PlayerPrefs.SetInt("Story2_2Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 14);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                        }
                    }
                    else if (stageNum == 31 && crickNum == 12)
                    {
                        PlayerPrefs.SetInt("Story3_1Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();

                            PlayerPrefs.SetInt("STAMP_NUM", 15);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                        }
                    }
                    else if (stageNum == 32 && crickNum == 14)
                    {
                        PlayerPrefs.SetInt("Story3_2Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 21);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                        }
                    }
                    else if (stageNum == 41 && crickNum == 5)
                    {
                        PlayerPrefs.SetInt("Story4_1Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 22);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                        }
                    }
                    else if (stageNum == 42 && crickNum == 17)
                    {
                        PlayerPrefs.SetInt("Story4_2Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 28);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                        }
                    }
                    else if (stageNum == 51 && crickNum == 7)
                    {
                        PlayerPrefs.SetInt("Story5_1Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 29);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });
                        }
                    }
                    else if (stageNum == 52 && crickNum == 22)
                    {
                        PlayerPrefs.SetInt("Story5_2Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 34);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("EndingScene"); });
                        }
                    }
                    else if (stageNum == 53 && crickNum == 33)
                    {
                        PlayerPrefs.SetInt("Story5_3Clear", 1);
                        storyEndFlag = true;
                        EndBgmfunc();
                        if (BGMTimer > BGMFadeTime)
                        {
                            BGMTimer = 0;
                            BGMDeleter = false;
                            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                            PlayerPrefs.SetInt("STAMP_NUM", 35);
                            CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });
                        }
                    }

                    if (stageNum == 52 && crickNum == 10)
                    {
                        Singleton<SoundPlayer>.instance.playBGM("bgm010", 1.0f);
                    }
                }
               
            }

            //Debug.Log( flagManager.sentenceEndFlag );        
            // 文字の表示が完了してるならクリック時に次の行を表示する
            
        }
        else
        {
             if(pause.GetComponent<StoryPause>().pauseFlag == true)
            {
                 if(touchController.detachPosX != 0)
                 {
                     //pause起動
                     if (((touchController.touchPosX > 250) && (touchController.touchPosX < 750)) &&
                         ((touchController.touchPosY > 500) && (touchController.touchPosY < 595)) &&
                         ((touchController.detachPosX > 250) && (touchController.detachPosX < 750)) &&
                         ((touchController.detachPosY > 500) && (touchController.detachPosY < 595)))
                     {
                         touchController.TouchPostionInitialize();
                         pause.GetComponent<StoryPause>().pauseImageManager1.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().pauseImageManager2.GetComponent<Image>().enabled = false;
                         //pauseImageManager3.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().pauseImageManager4.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().pauseImageManager5.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().pauseImageManager6.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().EscapePause();


                     }
                     else if (((touchController.touchPosX > 250) && (touchController.touchPosX < 750)) &&
                            ((touchController.touchPosY > 220) && (touchController.touchPosY < 310)) &&
                            ((touchController.detachPosX > 250) && (touchController.detachPosX < 750)) &&
                            ((touchController.detachPosY > 220) && (touchController.detachPosY < 310)))
                     {
                         touchController.TouchPostionInitialize();
                         pause.GetComponent<StoryPause>().EscapePause();
                         CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                     }
                     else
                     {
                         touchController.TouchPostionInitialize();
                         pause.GetComponent<StoryPause>().pauseImageManager1.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().pauseImageManager2.GetComponent<Image>().enabled = false;
                         //pauseImageManager3.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().pauseImageManager4.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().pauseImageManager5.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().pauseImageManager6.GetComponent<Image>().enabled = false;
                         pause.GetComponent<StoryPause>().EscapePause();
                     }
                 }
                
            }

        }
        if(BGMDeleter)
        {
            BGMTimer++;
        }

        if(touchController.detachPosX != 0)
        {
            touchController.TouchPostionInitialize();
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