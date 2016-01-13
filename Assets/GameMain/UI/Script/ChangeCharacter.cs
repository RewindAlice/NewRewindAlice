using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;

public class ChangeCharacter : MonoBehaviour {


    //キャラクター
    public Sprite flower;
    public Sprite cheshireCat;
    public Sprite alice;
    public Sprite greenCaterpillar;
    public Sprite whiteRabbit;
    public Sprite madHatter;
    public Sprite queenOfHeart;
    public Sprite sisterAlice;

    // シナリオを格納する一行格納する
    public string[] scenarios;

    //出力するテキストのファイルパス
    private string filepath;


    //ステージの番号
    private int stageNumber;

    private int timeCount;

    //イメージ変更用
    public Image image;

    public int charaCount;

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


	// Use this for initialization
	void Start () {

        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();
        //オブジェクトのImageを入手
        image = gameObject.GetComponent<Image>();
        charaCount = 0;
        timeCount = 0;
        //ステージの番号を取得
        stageNumber = PlayerPrefs.GetInt("STAGE_NUM");
        
       
        //ステージの番号によって、取得するパスの変更(Story用)
        if (stageNumber == 1)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage1-1character";
        }
        else if (stageNumber == 2)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage1-2character";
        }
        else if (stageNumber == 3)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage1-3character";
        }
        else if (stageNumber == 4)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage1-4character";
        }
        else if (stageNumber == 5)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage1-5character";
        }
        else if (stageNumber == 6)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage2-1character";
        }
        else if (stageNumber == 7)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage2-2character";
        }
        else if (stageNumber == 8)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage2-3character";
        }
        else if (stageNumber == 9)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage2-4character";
        }
        else if (stageNumber == 10)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage2-5character";
        }
        else if (stageNumber == 11)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage3-1character";
        }
        else if (stageNumber == 12)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage3-2character";
        }
        else if (stageNumber == 13)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage3-3character";
        }
        else if (stageNumber == 14)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage3-4character";
        }
        else if (stageNumber == 15)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage3-5character";
        }
        else if (stageNumber == 16)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage4-1character";
        }
        else if (stageNumber == 17)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage4-2character"; 
        }
        else if (stageNumber == 18)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage4-3character";
        }
        else if (stageNumber == 19)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage4-4character";
        }
        else if (stageNumber == 20)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage4-5character";
        }
        else if (stageNumber == 21)
        {
            limitTalk = 2;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage5-1character";
        }
        else if (stageNumber == 22)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage5-2character";
        }
        else if (stageNumber == 23)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage5-3character";
        }
        else if (stageNumber == 24)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage5-4character";
        }
        else if (stageNumber == 25)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage25character";
        }
        else if (stageNumber == 26)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage26character";
        }
        else if (stageNumber == 27)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage27character";
        }
        else if (stageNumber == 28)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage28character";
        }
        else if (stageNumber == 29)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage29character";
        }
        else if (stageNumber == 30)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage30character";
        }
        else if (stageNumber == 31)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage31character";
        }
        else if (stageNumber == 32)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage32character";
        }
        else if (stageNumber == 33)
        {
            limitTalk = 4;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage33character";
        }
        else if (stageNumber == 34)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage34character";
        }
        else if (stageNumber == 35)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage35character";
        }
        else if (stageNumber == 36)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage36character";
        }
        else if (stageNumber == 37)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage37character";
        }
        else if (stageNumber == 38)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage38character";
        }
        else if (stageNumber == 39)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage39character";
        }
        else if (stageNumber == 40)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage40character";
        }
        else if (stageNumber == 41)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage41character";
        }
        else if (stageNumber == 42)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage42character";
        }
        else if (stageNumber == 43)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage43character";
        }
        else if (stageNumber == 44)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage44character";
        }
        else if (stageNumber == 45)
        {
            limitTalk = 3;
            talkNumber = 0;
            filepath = "UI/IntoGame/CharacterImage/stage45character";
        }
       

        talkSpeed = 600;
        ReadTextData();
        //this.read();
     
            if (scenarios[charaCount] == "1")
            {
                image.sprite = flower;
            }
            else if (scenarios[charaCount] == "2")
            {
                image.sprite = cheshireCat;
            }
            else if (scenarios[charaCount] == "3")
            {
                image.sprite = alice;
            }
            else if (scenarios[charaCount] == "4")
            {
                image.sprite = greenCaterpillar;
            }
            else if (scenarios[charaCount] == "5")
            {
                image.sprite = whiteRabbit;
            }
            else if (scenarios[charaCount] == "6")
            {
                image.sprite = madHatter;
            }
            else if (scenarios[charaCount] == "7")
            {
                image.sprite = queenOfHeart;
            }
            else if (scenarios[charaCount] == "8")
            {
                image.sprite = sisterAlice;
            }
        
        charaCount++;
      

}
	
	// Update is called once per frame
	void Update () {

        if(pauseScript.pauseFlag == false)
        {
            timeCount++;

        }
        

            if (timeCount % talkSpeed == 0)
            {
                if (charaCount == limitTalk)
                {
                    charaCount = talkNumber;
                }

                if (scenarios[charaCount] == "1")
                {
                    image.sprite = flower;
                }
                else if (scenarios[charaCount] == "2")
                {
                    image.sprite = cheshireCat;
                }
                else if (scenarios[charaCount] == "3")
                {
                    image.sprite = alice;
                }
                else if (scenarios[charaCount] == "4")
                {
                    image.sprite = greenCaterpillar;
                }
                else if (scenarios[charaCount] == "5")
                {
                    image.sprite = whiteRabbit;
                }
                else if (scenarios[charaCount] == "6")
                {
                    image.sprite = madHatter;
                }
                else if (scenarios[charaCount] == "7")
                {
                    image.sprite = queenOfHeart;
                }
                else if (scenarios[charaCount] == "8")
                {
                    image.sprite = sisterAlice;
                }

                charaCount++;




        }
        
        
	
	}

    // テキストの読み込み
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
        sr.Close();
    }

    //.txtを読み込むときにreadの部分で差し換えてください
    void ReadTextData()
    {
   
            // TextAssetとして、Resourcesフォルダからテキストデータをロードする
            stageTextAsset = Resources.Load(filepath, typeof(TextAsset)) as TextAsset;
            // 文字列を代入(絵の判定の場合)
            stageData = stageTextAsset.text + "99";
            string[] test02 = { "\r\n" };
            //.txt内の改行に合わせて改行する
            scenarios = stageData.Split(test02, System.StringSplitOptions.RemoveEmptyEntries);
      
    }
}
