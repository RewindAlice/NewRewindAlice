using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;

public class ChangeCharacterImage : MonoBehaviour
{

    //アリス(オリジナル、笑顔、驚き)
    public Sprite alice_On;
    public Sprite alice_Off;
    public Sprite aliceSmile_On;
    public Sprite aliceSmile_Off;
    public Sprite aliceAmazing_On;
    public Sprite aliceAmazing_Off;


    //帽子屋(オリジナル、いかれ顔)
    public Sprite madHatter_On;
    public Sprite madHatter_Off;
    public Sprite madHatterCrazy_On;
    public Sprite madHatterCrazy_Off;

    //アリスの姉
    public Sprite sisterAlice_On;
    public Sprite sisterAlice_Off;

    //白うさぎ
    public Sprite whiteRabbit_On;
    public Sprite whiteRabbit_Off;

    //チェシャ猫
    public Sprite cheshireCat_On;
    public Sprite cheshireCat_Off;

    //ハートの女王(オリジナル、怒鳴り、青ざめ、笑顔) 
    public Sprite queenOfHeart_On;
    public Sprite queenOfHeart_Off;
    public Sprite queenOfHeartAnger_On;
    public Sprite queenOfHeartAnger_Off;
    public Sprite queenOfHeartPale_On;
    public Sprite queenOfHeartPale_Off;
    public Sprite queenOfHeartSmile_On;
    public Sprite queenOfHeartSmile_Off;

    //イモムシ
    public Sprite greenCaterpillarOn;
    public Sprite greenCaterpillarOff;

    //トランプ兵
    public Sprite trumpSoldier_On;
    public Sprite trumpSoldier_Off;

    //透明のスプライト
    public Sprite limpidity;

    //左側かどうか
    public bool leftSide;

    // シナリオを格納する一行格納する
    public string[] scenarios;

    //出力するテキストのファイルパス
    private string filepath;

    //ステージの番号
    private int stageNum;

    //フラグマネージャー(クリック回数を入手するため)
    private FlagManager flagManager;
    public GameObject sentenceTextController;

    //イメージ変更用
    public Image image;

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


    void Start()
    {

        //オブジェクトのImageを入手
        image = gameObject.GetComponent<Image>();

        //ステージの番号を取得
        stageNum = PlayerPrefs.GetInt("STORY_NUM");

        //ステージの番号によって、取得するパスの変更
        if (stageNum == 11)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara1-1";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara1-1";
            }
        }
        else if (stageNum == 12)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara1-2";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara1-2";
            }
        }
        else if (stageNum == 21)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara2-1";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara2-1";
            }
        }
        else if (stageNum == 22)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara2-2";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara2-2";
            }
        }
        else if (stageNum == 31)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara3-1";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara3-1";
            }
        }
        else if (stageNum == 32)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara3-2";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara3-2";
            }
        }
        else if (stageNum == 41)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara4-1";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara4-1";
            }
        }
        else if (stageNum == 42)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara4-2";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara4-2";
            }
        }
        else if (stageNum == 51)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara5-1";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara5-1";
            }
        }
        else if (stageNum == 52)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara5-2";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara5-2";
            }
        }
        else if (stageNum == 53)
        {
            if (leftSide == true)
            {
                filepath = "Text/LeftCharacter/leftChara5-3";
            }
            else
            {
                filepath = "Text/RightCharacter/rightChara5-3";
            }
        }
        else
        {
            filepath = "Text/error";
        }

//        this.read();
        //読み込み方法変更
        ReadTextData();
        flagManager = sentenceTextController.GetComponent<FlagManager>();


        //キャラクターのチェンジ
        if (scenarios[flagManager.clickCounter] == "0")
        {
            image.sprite = limpidity;
        }
        else if (scenarios[flagManager.clickCounter] == "1")
        {
            image.sprite = alice_On;
        }
        else if (scenarios[flagManager.clickCounter] == "2")
        {
            image.sprite = alice_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "3")
        {
            image.sprite = aliceSmile_On;
        }
        else if (scenarios[flagManager.clickCounter] == "4")
        {
            image.sprite = aliceSmile_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "5")
        {
            image.sprite = aliceAmazing_On;
        }
        else if (scenarios[flagManager.clickCounter] == "6")
        {
            image.sprite = aliceAmazing_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "7")
        {
            image.sprite = madHatter_On;
        }
        else if (scenarios[flagManager.clickCounter] == "8")
        {
            image.sprite = madHatter_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "9")
        {
            image.sprite = madHatterCrazy_On;
        }
        else if (scenarios[flagManager.clickCounter] == "10")
        {
            image.sprite = madHatterCrazy_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "11")
        {
            image.sprite = sisterAlice_On;
        }
        else if (scenarios[flagManager.clickCounter] == "12")
        {
            image.sprite = sisterAlice_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "13")
        {
            image.sprite = whiteRabbit_On;
        }
        else if (scenarios[flagManager.clickCounter] == "14")
        {
            image.sprite = whiteRabbit_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "15")
        {
            image.sprite = cheshireCat_On;
        }
        else if (scenarios[flagManager.clickCounter] == "16")
        {
            image.sprite = cheshireCat_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "17")
        {
            image.sprite = queenOfHeart_On;
        }
        else if (scenarios[flagManager.clickCounter] == "18")
        {
            image.sprite = queenOfHeart_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "19")
        {
            image.sprite = queenOfHeartAnger_On;
        }
        else if (scenarios[flagManager.clickCounter] == "20")
        {
            image.sprite = queenOfHeartAnger_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "21")
        {
            image.sprite = queenOfHeartPale_On;
        }
        else if (scenarios[flagManager.clickCounter] == "22")
        {
            image.sprite = queenOfHeartPale_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "23")
        {
            image.sprite = queenOfHeartSmile_On;
        }
        else if (scenarios[flagManager.clickCounter] == "24")
        {
            image.sprite = queenOfHeartSmile_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "25")
        {
            image.sprite = greenCaterpillarOn;
        }
        else if (scenarios[flagManager.clickCounter] == "26")
        {
            image.sprite = greenCaterpillarOff;
        }
        else if (scenarios[flagManager.clickCounter] == "27")
        {
            image.sprite = trumpSoldier_On;
        }
        else if (scenarios[flagManager.clickCounter] == "28")
        {
            image.sprite = trumpSoldier_Off;
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

    void Update()
    {


    }

    public void CharacterChange()
    {

        //キャラクターのチェンジ
        if (scenarios[flagManager.clickCounter] == "0")
        {
            image.sprite = limpidity;
        }
        else if (scenarios[flagManager.clickCounter] == "1")
        {
            image.sprite = alice_On;
        }
        else if (scenarios[flagManager.clickCounter] == "2")
        {
            image.sprite = alice_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "3")
        {
            image.sprite = aliceSmile_On;
        }
        else if (scenarios[flagManager.clickCounter] == "4")
        {
            image.sprite = aliceSmile_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "5")
        {
            image.sprite = aliceAmazing_On;
        }
        else if (scenarios[flagManager.clickCounter] == "6")
        {
            image.sprite = aliceAmazing_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "7")
        {
            image.sprite = madHatter_On;
        }
        else if (scenarios[flagManager.clickCounter] == "8")
        {
            image.sprite = madHatter_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "9")
        {
            image.sprite = madHatterCrazy_On;
        }
        else if (scenarios[flagManager.clickCounter] == "10")
        {
            image.sprite = madHatterCrazy_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "11")
        {
            image.sprite = sisterAlice_On;
        }
        else if (scenarios[flagManager.clickCounter] == "12")
        {
            image.sprite = sisterAlice_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "13")
        {
            image.sprite = whiteRabbit_On;
        }
        else if (scenarios[flagManager.clickCounter] == "14")
        {
            image.sprite = whiteRabbit_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "15")
        {
            image.sprite = cheshireCat_On;
        }
        else if (scenarios[flagManager.clickCounter] == "16")
        {
            image.sprite = cheshireCat_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "17")
        {
            image.sprite = queenOfHeart_On;
        }
        else if (scenarios[flagManager.clickCounter] == "18")
        {
            image.sprite = queenOfHeart_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "19")
        {
            image.sprite = queenOfHeartAnger_On;
        }
        else if (scenarios[flagManager.clickCounter] == "20")
        {
            image.sprite = queenOfHeartAnger_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "21")
        {
            image.sprite = queenOfHeartPale_On;
        }
        else if (scenarios[flagManager.clickCounter] == "22")
        {
            image.sprite = queenOfHeartPale_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "23")
        {
            image.sprite = queenOfHeartSmile_On;
        }
        else if (scenarios[flagManager.clickCounter] == "24")
        {
            image.sprite = queenOfHeartSmile_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "25")
        {
            image.sprite = greenCaterpillarOn;
        }
        else if (scenarios[flagManager.clickCounter] == "26")
        {
            image.sprite = greenCaterpillarOff;
        }
        else if (scenarios[flagManager.clickCounter] == "27")
        {
            image.sprite = trumpSoldier_On;
        }
        else if (scenarios[flagManager.clickCounter] == "28")
        {
            image.sprite = trumpSoldier_Off;
        }
        else if (scenarios[flagManager.clickCounter] == "99")
        {
            //キャラの読み込みを終了する
            Debug.Log("読み込み終わり");
        }
    }

    //.txtを読み込むときにreadの部分で差し換えてください
    void ReadTextData()
    {

        // TextAssetとして、Resourcesフォルダからテキストデータをロードする
        stageTextAsset = Resources.Load(filepath, typeof(TextAsset)) as TextAsset;
        // 文字列を代入
        stageData = stageTextAsset.text + "99";
        string[] test02 = { "\r\n" };
        //.txt内の改行に合わせて改行する
        scenarios = stageData.Split(test02, System.StringSplitOptions.RemoveEmptyEntries);
    }
}
