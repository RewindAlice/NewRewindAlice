using UnityEngine;
using System.Collections;
using UnityEngine.UI;	// uGUIの機能を使うお約束
using System;
using System.IO;

public class NameTextController : MonoBehaviour
{
    public string[] scenarios; // シナリオを格納する
    public Text uiText;	// uiTextへの参照を保つ
    public GameObject sentenceTextController;
    private string filepath;
    private FlagManager flagManager;

    int currentLine = 0; // 現在の行番号

    //読み込みの改善
    //////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 読み込んだテキストデータを格納するテキストアセット
    /// </summary>
    public TextAsset stageTextAsset;
    /// <summary>
    /// ステージの文字列データ
    /// </summary>
    public string stageData;
    /// /////////////////////////////////////////////////////////////////////////

    //メニュー作成
    public bool stopText;
    public bool funcFlag;
    //タイマー
    private int timer;
    const int breakTime = 16;

    void Start()
    {

        funcFlag = false;
        stopText = false;
        CameraFade.StartAlphaFade(Color.black, true, 1.0f, 0.5f);

        int num = PlayerPrefs.GetInt("STORY_NUM");

        if (num == 11)
        {
            filepath = "Text/Name/name1-1";
        }
        else if (num == 12)
        {
            filepath = "Text/Name/name1-2";
        }
        else if (num == 21)
        {
            filepath = "Text/Name/name2-1";
        }
        else if (num == 22)
        {
            filepath = "Text/Name/name2-2";
        }
        else if (num == 31)
        {
            filepath = "Text/Name/name3-1";
        }
        else if (num == 32)
        {
            filepath = "Text/Name/name3-2";
        }
        else if (num == 41)
        {
            filepath = "Text/Name/name4-1";
        }
        else if (num == 42)
        {
            filepath = "Text/Name/name4-2";
        }
        else if (num == 51)
        {
            filepath = "Text/Name/name5-1";
        }
        else if (num == 52)
        {
            filepath = "Text/Name/name5-2";
        }
        else if (num == 53)
        {
            filepath = "Text/Name/name5-3";
        }
        else
        {
            filepath = "Text/error";
        }
        //this.read();

        ReadTextData();

        flagManager = sentenceTextController.GetComponent<FlagManager>();

        TextUpdate();

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
        //if (Input.GetKeyDown(KeyCode.Escape)&&(timer ==0))
        //{
        //    if (stopText == false)
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
            // 現在の行番号がラストまで行ってない状態でクリックすると、テキストを更新する
            if (currentLine < scenarios.Length && flagManager.sentenceEndFlag == true && Input.GetMouseButtonDown(0))
            {
                TextUpdate();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))//Wへ変更予定
            {
                funcFlag = true;
            }
        }
        //backGame();
    }


    //void backGame()
    //{
    //    if (funcFlag)
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
    //            timer++;
    //        }
    //    }
    //}
    // テキストを更新する
    void TextUpdate()
    {
        // 現在の行のテキストをuiTextに流し込み、現在の行番号を一つ追加する
        uiText.text = scenarios[currentLine];
        currentLine++;
    }

    void ReadTextData()
    {

        // TextAssetとして、Resourcesフォルダからテキストデータをロードする

        stageTextAsset = Resources.Load(filepath, typeof(TextAsset)) as TextAsset;
        // 文字列を代入
        stageData = stageTextAsset.text;
        scenarios = stageData.Split("\n"[0]);
        //// 空白を置換で削除
        //stageData = stageData.Replace(" ", "");
    }
}