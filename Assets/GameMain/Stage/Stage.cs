using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // 動的配列で使用

using System.IO;    // ファイル入出力
using System.Text;  //
using System;

public class Stage : MonoBehaviour
{
    const int STAGE_X = 11; // ステージの横幅
    const int STAGE_Y = 5;  // ステージの高さ
    const int STAGE_Z = 11; // ステージの奥行

    // 配列番号                                 // No       ギミックの種類
    const int NONE_BLOCK = 0;                   // No.0     透明ブロック
    const int START_POINT = 1;                  // No.1     スタート地点
    const int STAGE_GOOL = 2;                   // No.2     ゴール地点
    const int WATER = 3;                        // No.3     水
    const int FOREST_BLOCK_GROUND = 4;          // No.4     森ステージの足場ブロック（1段目）
    const int FOREST_BLOCK_GRASS = 5;           // No.5     森ステージの足場ブロック（2段目）
    const int FOREST_BLOCK_ALLGRASS = 6;        // No.6     森ステージの足場ブロック（3段目以降）
    const int ROOM_BLOCK_FLOOR = 7;             // No.7     家ステージの足場ブロック（1段目）
    const int ROOM_BLOCK_BOOKSHELF = 8;         // No.8     家ステージの本棚
    const int REDFOREST_BLOCK_GROUND = 9;       // No.9     赤い森ステージの足場ブロック（1段目）
    const int REDFOREST_BLOCK_GRASS = 10;       // No.10    赤い森ステージの足場ブロック（2段目）
    const int REDFOREST_BLOCK_ALLGRASS = 11;    // No.11    赤い森ステージの足場ブロック（3段目以降）
    const int DARKFOREST_BLOCK_GROUND = 12;     // No.12    暗い森ステージの足場ブロック（全段）
    const int GARDEN_BLOCK_GROUND = 13;         // No.13    庭園ステージの足場ブロック（1段目）
    const int GARDEN_BLOCK_FLOWER = 14;         // No.14    庭園ステージの足場ブロック（2段目以降）
                                                // No.15 ～ No.20は無し
    const int IVY_BLOCK = 21;                   // No.21    蔦ブロック
    const int IVY_FRONT = 22;                   // No.22    蔦（前）
    const int IVY_BACK = 23;                    // No.23    蔦（後）
    const int IVY_LEFT = 24;                    // No.24    蔦（左）
    const int IVY_RIGHT = 25;                   // No.25    蔦（右）
    const int LADDER_BLOCK = 26;                // No.26    梯子ブロック
    const int LADDER_FRONT = 27;                // No.27    梯子（前）
    const int LADDER_BACK = 28;                 // No.28    梯子（後）
    const int LADDER_LEFT = 29;                 // No.29    梯子（左）
    const int LADDER_RIGHT = 30;                // No.30    梯子（右）
    const int TREE = 31;                        // No.31    木
    const int DUMMY_TREE = 32;                  // No.32    木（成長後判定用）
    const int MUSHROOM_SMALL = 33;              // No.33    キノコ（小さくなる）
    const int MUSHROOM_BIG = 34;                // No.34    キノコ（大きくなる）
    const int POTION_SMALL = 35;                // No.35    薬（小さくなる）
    const int POTION_BIG = 36;                  // No.36    薬（大きくなる）
    const int DOOR_RED_KEY = 37;                // No.37    赤扉（鍵）
    const int DOOR_RED = 38;                    // No.38    赤扉（扉）
    const int DOOR_BLUE_KEY = 39;               // No.39    青扉（鍵）
    const int DOOR_BLUE = 40;                   // No.40    青扉（扉）
    const int DOOR_YELLOW_KEY = 41;             // No.41    黄扉（鍵）
    const int DOOR_YELLOW = 42;                 // No.42    黄扉（扉）
    const int DOOR_GREEN_KEY = 43;              // No.43    緑扉（鍵）
    const int DOOR_GREEN = 44;                  // No.44    緑扉（扉）
    const int WARP_HOLE_ONE = 45;               // No.45    穴１
    const int WARP_HOLE_TWO = 46;               // No.46    穴２
    const int WARP_HOLE_THREE = 47;             // No.47    穴３
    const int WARP_HOLE_FOUR = 48;              // No.48    穴４
    const int WARP_HOLE_FIVE = 49;              // No.49    穴５
    const int BRAMBLE = 50;                     // No.50    茨
    const int RED_FLOWER = 51;                  // No.51    花１（赤）
    const int BLUE_FLOWER = 52;                 // No.52    花２（青）
    const int PURPLE_FLOWER = 53;               // No.53    花３（紫）
    const int CHESHIRE_CAT = 54;                // No.54    チェシャ猫  
    const int TWEEDLEDUM = 55;                  // No.55    トゥイードルダム
    const int TWEEDLEDEE = 56;                  // No.56    トゥイードルディ
    const int SOLDIER_HEART_RIGHT = 57;         // No.57    ハート兵（右回り）
    const int SOLDIER_HEART_LEFT = 58;          // No.58    ハート兵（左回り）
    const int SOLDIER_SPADE_RIGHT = 59;         // No.59    スペード兵（右回り）
    const int SOLDIER_SPADE_LEFT = 60;          // No.60    スペード兵（左回り）
    const int SOLDIER_SPADE_BAF = 61;           // No.61    スペード兵（往復）
    const int ROCK = 62;                        // No.62    岩
    const int HAMPTYDUMPTY = 64;                // No.64    ハンプティダンプティ
    const int DUMMY_HEART = 77;

    public GameObject gameMain;                     // ゲームメイン
    public Player alice;                            // アリス
    public GameObject gimmickNone;                  // No.0     透明ブロック
    public GameObject gimmickStart;                 // No.1     スタート地点
    public GameObject gimmickGoal;                  // No.2     ゴール地点
    public GameObject waterBlock;                   // No.3     水
    public GameObject forestBlockGround;            // No.4     森ステージの足場ブロック（1段目）
    public GameObject forestBlockGrass;             // No.5     森ステージの足場ブロック（2段目）
    public GameObject forestBlockAllGrass;          // No.6     森ステージの足場ブロック（3段目以降）
    public GameObject roomBlockFloorBlack;          // No.7     家ステージの足場ブロック（黒）
    public GameObject roomBlockFloorWhite;          // No.7     家ステージの足場ブロック（白）
    public GameObject roomBlockBookShelf;           // No.8     家ステージの本棚
    public GameObject redForestBlockGround;         // No.9     赤い森ステージの足場ブロック（1段目）
    public GameObject redForestBlockGrass;          // No.10    赤い森ステージの足場ブロック（2段目）
    public GameObject redForestBlockAllGrass;       // No.11    赤い森ステージの足場ブロック（3段目以降）
    public GameObject darkForestBlockRedAllGrass;   // No.12    暗い森ステージの足場ブロック（赤）
    public GameObject darkForestBlockGreenAllGrass; // No.12    暗い森ステージの足場ブロック（青）
    public GameObject gardenBlockGrass;             // No.13    庭園ステージの足場ブロック（1段目）
    public GameObject gardenBlockRoseRed;           // No.14    庭園ステージの足場ブロック（2段目以降）（赤）
    public GameObject gardenBlockRoseWhite;         // No.14    庭園ステージの足場ブロック（2段目以降）（白）
    public GameObject gimmickIvyBlock;              // No.21    蔦ブロック
    public GameObject gimmickIVY;                   // No.22~   蔦
    public GameObject gimmickLadder;                // No.27~   梯子
    public GameObject gimmickTree;                  // No.31~   木
    public GameObject gimmickMushroomSmall;         // No.33    キノコ（小さくなる）
    public GameObject gimmickMushroomBig;           // No.34    キノコ（大きくなる）
    public GameObject gimmickPotionSmall;           // No.35    薬（小さくなる）
    public GameObject gimmickPotionBig;             // No.36    薬（大きくなる）
    public GameObject gimmickKey_Red;               // No.37    赤扉（鍵）
    public GameObject gimmickDoor_Red;              // No.38    赤扉（扉）
    public GameObject gimmickKey_Blue;              // No.39    青扉（鍵）
    public GameObject gimmickDoor_Blue;             // No.40    青扉（扉）
    public GameObject gimmickKey_Yellow;            // No.41    黄扉（鍵）
    public GameObject gimmickDoor_Yellow;           // No.42    黄扉（扉）
    public GameObject gimmickKey_Green;             // No.43    緑扉（鍵）
    public GameObject gimmickDoor_Green;            // No.44    緑扉（扉）
    public GameObject gimmickHole;                  // No.45~   穴
    public GameObject gimmickBramble;               // No.50    茨
    public GameObject gimmickRedFlower;             // No.51    花１（赤）
    public GameObject gimmickBuleFlower;            // No.52    花２（青）
    public GameObject gimmickPurpleFlower;          // No.53    花３（紫）
    public GameObject cheshire;                     // No.54    チェシャ猫
    public GameObject gimmickTweedleDum;            // No.55    トゥイードルダム
    public GameObject gimmickTweedleDee;            // No.56    トゥイードルディ
    public GameObject gimmickSoldierHeartRight;     // No.57    ハート兵（右回り）
    public GameObject gimmickSoldierHeartLeft;      // No.58    ハート兵（左回り）
    public GameObject gimmickSoldierSpadeRight;     // No.59    スペード兵（右回り）
    public GameObject gimmickSoldierSpadeLeft;      // No.60    スペード兵（左回り）
    public GameObject gimmickSoldierSpadeBAF;       // No.61    スペード兵（往復）
    public GameObject rock;                         // No.62    岩

    public int field;               // フィールド番号
    GameObject fieldObject;         // ステージ天球のオブジェクト
    public GameObject fieldStage1;  // ステージ１の天球
    public GameObject fieldStage2;  // ステージ２の天球
    public GameObject fieldStage3;  // ステージ３の天球
    public GameObject fieldStage4;  // ステージ４の天球
    public GameObject fieldStage5;  // ステージ５の天球



    public GameObject Effect01;
    int gimmick = 0;            // 配列の数字
    int gimmickPattern = 0;     // ギミックの種類
    int gimmickDirection = 0;   // ギミックの向き
    int gimmickStartTurn = 0;   // ギミックの開始ターン

    int[, ,] gimmickArray = new int[STAGE_Y, STAGE_X, STAGE_Z];                     // ステージの配置（数字）
    public int[, ,] gimmickNumArray = new int[STAGE_Y, STAGE_X, STAGE_Z];                  // ステージの配置（ギミック番号）
    List<int> moveGimmickNumList = new List<int>();                                 // 移動系ギミック（数字）
    GameObject[, ,] gimmickObjectArray = new GameObject[STAGE_Y, STAGE_X, STAGE_Z]; // ステージの配置（オブジェクト）
    List<GameObject> moveGimmickObjectList = new List<GameObject>();                // 移動系ギミック（オブジェクト）
    int turnNum;                                                                    // ステージのターン数
    private string guitxt = "";

	public int[, ,] pushGimmickNumArray = new int[STAGE_Y, STAGE_X, STAGE_Z];                  // ステージの配置（押されるギミック番号）
	GameObject[, ,] pushGimmickObjectArray = new GameObject[STAGE_Y, STAGE_X, STAGE_Z]; // ステージの配置（押されるオブジェクト）

    int[] ModeChangeSaveNum;                                                       //石が移動したときにもともと何があったかを保存する配列
    int[] ModeChangeSaveTurn;                                                      //上記のターンを保存する配列
    int SaveTurnArrayController = 0;
    bool stoneFallController = false;

    public TextAsset stageTextAsset;    //テキスト読み込み用

    public string stageData;            //テキスト代入用
    public string[] scenarios;          //ステージ読み込み用変数
    private string filepath;            //テキスト名
    //

    bool Twinstest = false;                                                        //DeeとDumが被ったら
    Vector3 nextMovePosTwin;
    public Vector3 Twins1;
    public Vector3 Twins2;
    public bool oncetime;
    public bool goalFlag;
	public bool playerRizeFlag = false;

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        goalFlag = false;
        ModeChangeSaveTurn = new int[] {0,0,0,0,0,0,0,0,0,0,
                                      0,0,0,0,0,0,0,0,0,0,
                                      0,0,0,0,0,0,0,0,0,0};

        ModeChangeSaveNum = new int[]{0,0,0,0,0,0,0,0,0,0,
                                      0,0,0,0,0,0,0,0,0,0,
                                      0,0,0,0,0,0,0,0,0,0};
        nextMovePosTwin = new Vector3(0, 0, 0);
        Twins1 = new Vector3(0, 0, 0);
        Twins2 = new Vector3(0, 0, 0);

		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				for (int k = 0; k < 11; k++)
				{
					pushGimmickNumArray[i, j, k] = 0;
					pushGimmickObjectArray[i, j, k] = gimmickNone;
				}
			}
		}
	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {
	
	}

    // ★ステージ情報の読み込み★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void ReadFile()
    {
        FileInfo fi = new FileInfo("Assets/GameMain/Stage/StageData/"+PlayerPrefs.GetString("Text"));

        StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8);

        guitxt = sr.ReadLine();
        turnNum = Int32.Parse(guitxt);
        guitxt = sr.ReadLine();
        field = Int32.Parse(guitxt);

        for(int i = 0; i< 5;i++)
        {
            guitxt = sr.ReadLine();
            for (int j = 0; j < 11; j++)
            {
                guitxt = sr.ReadLine();
                string[] fields = guitxt.Split(',');
                for (int k = 0; k < 11; k++)
                {
                    gimmickArray[i, j, k] = Int32.Parse(fields[k]);
                }
            }
        }
    }

    // ★ステージ情報の読み込み★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    void ReadTextData()
    {

        //Resourcesフォルダからテキストデータをロードする
        stageTextAsset = Resources.Load(filepath, typeof(TextAsset)) as TextAsset;

        // 文字列を代入
        stageData = stageTextAsset.text;

        string[] limmit = { "\r", "\n", "," };
        scenarios = stageData.Split(limmit, System.StringSplitOptions.RemoveEmptyEntries);

        turnNum = Int32.Parse(scenarios[0]);

        field = Int32.Parse(scenarios[1]);

        int stageObjectNumber = 2;
        
        //ステージ情報の代入
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                for (int k = 0; k < 11; k++)
                {
                    gimmickArray[i, j, k] = Int32.Parse(scenarios[stageObjectNumber]);
                    stageObjectNumber++;
                }
            }
        }
    }

    // ★ステージの生成★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void CreateStage()
    {
        Vector3 fieldPosition = new Vector3(5, -0.5f, 5);   // フィールドの座標を設定

        // ▽フィールド番号が
        switch (field)
        {
            // ▼１なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 1: fieldObject = GameObject.Instantiate(fieldStage1, fieldPosition, Quaternion.identity) as GameObject; break;     // ステージ１の天球を設定
            // ▼２なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 2: fieldObject = GameObject.Instantiate(fieldStage2, fieldPosition, Quaternion.identity) as GameObject; break;     // ステージ２の天球を設定
            // ▼３なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 3: fieldObject = GameObject.Instantiate(fieldStage3, fieldPosition, Quaternion.identity) as GameObject; break;     // ステージ３の天球を設定
            // ▼４なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 4: fieldObject = GameObject.Instantiate(fieldStage4, fieldPosition, Quaternion.identity) as GameObject; break;     // ステージ４の天球を設定
            // ▼５なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case 5: fieldObject = GameObject.Instantiate(fieldStage5, fieldPosition, Quaternion.identity) as GameObject; break;     // ステージ５の天球を設定
        }

        for(int x = 0; x < STAGE_X; x++)
        {
            for(int y = 0; y < STAGE_Y; y++)
            {
                for(int z = 0; z < STAGE_Z; z++)
                {
                    CreateGimmcik(x, y, z);     // ギミックの生成
                }
            }
        }
    }

    // ★ギミックの生成★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void CreateGimmcik(int x, int y, int z)
    {
        gimmick = 0;            // 配列の数字（一時保存用）
        gimmickPattern = 0;     // ギミックの種類
        gimmickDirection = 0;   // ギミックの向き
        gimmickStartTurn = 0;   // ギミックの開始ターン

        gimmick = gimmickArray[y, x, z];    // 配列の数字を一時的に保存する
        gimmickDirection = gimmick / 10000; // 配列の数字を一万で割りギミックの向きを求める
        gimmick = gimmick % 10000;          // 配列の数字を一万で割った余りを入れる
        gimmickPattern = gimmick / 100;     // 配列の数字を百で割りギミックの種類を求める
        gimmick = gimmick % 100;            // 配列の数字を百で割った余りを入れるF
        gimmickStartTurn = gimmick;         // ギミックの開始ターンを入れる

        // ▽ギミックの種類が////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        switch (gimmickPattern)
        {
            // ▼No.0     透明ブロック///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case NONE_BLOCK://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.1     スタート地点///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case START_POINT:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickStart, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.2     ゴール地点/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case STAGE_GOOL://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickGoal, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.3     水/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case WATER://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(waterBlock, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.4     森ステージの足場ブロック（1段目）//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case FOREST_BLOCK_GROUND:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(forestBlockGround, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.5     森ステージの足場ブロック（2段目）//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case FOREST_BLOCK_GRASS://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(forestBlockGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.6     森ステージの足場ブロック（3段目以降）//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case FOREST_BLOCK_ALLGRASS://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(forestBlockAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.7     家ステージの足場ブロック（1段目）//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case ROOM_BLOCK_FLOOR:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (x % 2)
                {
                    case 0:
                        switch (z % 2)
                        {
                            case 0:
                                // 家ステージの足場ブロック（白）////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                            case 1:
                                // 家ステージの足場ブロック（黒）////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorBlack, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                        }
                        break;
                    case 1:
                        switch (z % 2)
                        {
                            case 0:
                                // 家ステージの足場ブロック（黒）////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorBlack, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                            case 1:
                                // 家ステージの足場ブロック（白）////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                        }
                        break;
                }
                break;
            // ▼No.8     家ステージの本棚///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case ROOM_BLOCK_BOOKSHELF:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickNumArray[y, x, z] = NONE_BLOCK;

				pushGimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockBookShelf, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
				pushGimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
				pushGimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(270, 0, 0);
				pushGimmickNumArray[y, x, z] = ROCK;
				pushGimmickObjectArray[y, x, z].GetComponent<Rock>().SetStartActionTurn(gimmickStartTurn);
				pushGimmickObjectArray[y, x, z].GetComponent<Rock>().Initialize(x, y, z);
                break;
            // ▼No.9     赤い森ステージの足場ブロック（1段目）//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case REDFOREST_BLOCK_GROUND://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(redForestBlockGround, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.10    赤い森ステージの足場ブロック（2段目）//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case REDFOREST_BLOCK_GRASS://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(redForestBlockGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.11    赤い森ステージの足場ブロック（3段目以降）//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case REDFOREST_BLOCK_ALLGRASS:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(redForestBlockAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.12    暗い森ステージの足場ブロック（全段）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DARKFOREST_BLOCK_GROUND:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(darkForestBlockRedAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    case 1://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(darkForestBlockGreenAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                }
                break;
            // ▼No.13    庭園ステージの足場ブロック（1段目）////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case GARDEN_BLOCK_GROUND:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gardenBlockGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.14    庭園ステージの足場ブロック（2段目以降）////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case GARDEN_BLOCK_FLOWER:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gardenBlockRoseRed, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    case 1://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gardenBlockRoseWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                }
                break;
            // ▼No.21    蔦ブロック/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_BLOCK://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIvyBlock, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<BlockMaterialChanger>().MatelialChange(field);
                break;
            // ▼No.22~  蔦//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_FRONT: // ▼No.22    蔦（前）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_BACK:  // ▼No.23    蔦（後）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_LEFT:  // ▼No.24    蔦（左）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_RIGHT: // ▼No.25    蔦（右）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (gimmickPattern)
                {
                    // ▼No.22    蔦（前）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case IVY_FRONT://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 ivyFrontPosition = new Vector3(x, y - 0.4f, z + 0.5f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyFrontPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 180, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.23    蔦（後）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case IVY_BACK:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 ivyBackPosition = new Vector3(x, y - 0.4f, z - 0.5f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyBackPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.24    蔦（左）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case IVY_LEFT:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 ivyLeftPosition = new Vector3(x - 0.5f, y - 0.4f, z);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyLeftPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 90, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.25    蔦（右）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case IVY_RIGHT://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 ivyRightPosition = new Vector3(x + 0.5f, y - 0.4f, z);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyRightPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 270, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                }
                gimmickObjectArray[y, x, z].GetComponent<Ivy>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.26    梯子ブロック///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_BLOCK:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (x % 2)
                {
                    case 0:
                        switch (z % 2)
                        {
                            case 0:// 家ステージの足場ブロック（白）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                            case 1:// 家ステージの足場ブロック（黒）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorBlack, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                        }
                        break;
                    case 1:
                        switch (z % 2)
                        {
                            case 0:// 家ステージの足場ブロック（黒）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorBlack, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                            case 1:// 家ステージの足場ブロック（白）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                        }
                        break;
                }
                break;
            // ▼N0.27~  梯子////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_FRONT:  // ▼No.27    梯子（前）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_BACK:   // ▼No.28    梯子（後）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_LEFT:   // ▼No.29    梯子（左）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_RIGHT:  // ▼No.30    梯子（右）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (gimmickPattern)
                {
                    // ▼No.27    梯子（前）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case LADDER_FRONT:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 LadderFrontPosition = new Vector3(x - 0.9f, y - 0.5f, z + 0.45f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickLadder, LadderFrontPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 180, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.28    梯子（後）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case LADDER_BACK:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 LadderBackPosition = new Vector3(x + 0.9f, y - 0.5f, z - 0.45f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickLadder, LadderBackPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.29    梯子（左）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case LADDER_LEFT:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 LadderLeftPosition = new Vector3(x - 0.45f, y - 0.5f, z - 0.9f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickLadder, LadderLeftPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 90, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.30    梯子（右）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case LADDER_RIGHT:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 LadderRightPosition = new Vector3(x + 0.45f, y - 0.5f, z + 0.9f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickLadder, LadderRightPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 270, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                }
                gimmickObjectArray[y, x, z].GetComponent<Ladder>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.31    木/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case TREE:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickTree, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Tree>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.33    キノコ（小さくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case MUSHROOM_SMALL://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickMushroomSmall, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetStartActionTurn(gimmickStartTurn);
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetItemCode(MUSHROOM_SMALL);
                break;
            // ▼No.34    キノコ（大きくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case MUSHROOM_BIG:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickMushroomBig, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetStartActionTurn(gimmickStartTurn);
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetItemCode(MUSHROOM_BIG);
                break;
            // ▼No.35    薬（小さくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case POTION_SMALL:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickPotionSmall, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetStartActionTurn(gimmickStartTurn);
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetItemCode(POTION_SMALL);
                break;
            // ▼No.36    薬（大きくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case POTION_BIG://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickPotionBig, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetStartActionTurn(gimmickStartTurn);
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetItemCode(POTION_BIG);
                break;
			// ▼No.37    赤扉（鍵）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case DOOR_RED_KEY:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickKey_Red, new Vector3(x, y - 0.35f, z - 0.2f), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection) + new Vector3(-45, 0, 0);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Key>().SetStartActionTurn(gimmickStartTurn);
				break;
			// ▼No.38    赤扉（扉）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case DOOR_RED:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickDoor_Red, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Door>().SetStartActionTurn(gimmickStartTurn);
				break;
			// ▼No.39    青扉（鍵）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case DOOR_BLUE_KEY://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickKey_Blue, new Vector3(x, y - 0.35f, z - 0.2f), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection) + new Vector3(-45, 0, 0);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Key>().SetStartActionTurn(gimmickStartTurn);
				break;
			// ▼No.40    青扉（扉）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case DOOR_BLUE://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickDoor_Blue, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Door>().SetStartActionTurn(gimmickStartTurn);
				break;
			// ▼No.41    黄扉（鍵）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case DOOR_YELLOW_KEY:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickKey_Yellow, new Vector3(x, y - 0.35f, z - 0.2f), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection) + new Vector3(-45, 0, 0);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Key>().SetStartActionTurn(gimmickStartTurn);
				break;
			// ▼No.42    黄扉（扉）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case DOOR_YELLOW:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickDoor_Yellow, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Door>().SetStartActionTurn(gimmickStartTurn);
				break;
			// ▼No.43    緑扉（鍵）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case DOOR_GREEN_KEY://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickKey_Green, new Vector3(x, y - 0.35f, z - 0.2f), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection) + new Vector3(-45, 0, 0);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Key>().SetStartActionTurn(gimmickStartTurn);
				break;
			// ▼No.44    緑扉（扉）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case DOOR_GREEN://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickDoor_Green, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Door>().SetStartActionTurn(gimmickStartTurn);
				break;
			// ▼No.45~  穴//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case WARP_HOLE_ONE:     // ▼No.45    穴１///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case WARP_HOLE_TWO:     // ▼No.46    穴２///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case WARP_HOLE_THREE:   // ▼No.47    穴３///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case WARP_HOLE_FOUR:    // ▼No.48    穴４///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case WARP_HOLE_FIVE:    // ▼No.49    穴５///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickHole, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
				gimmickNumArray[y, x, z] = gimmickPattern;
				break;
			// ▼No.50    茨/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case BRAMBLE:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickBramble, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
				gimmickNumArray[y, x, z] = gimmickPattern;
				break;
			// ▼No.51    花１（赤）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case RED_FLOWER://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickRedFlower, new Vector3(x, y - 0.2f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Flower3>().changeMaterial(field);
				break;
			// ▼No.52    花２（青）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case BLUE_FLOWER:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickBuleFlower, new Vector3(x, y - 0.2f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Flower2>().changeMaterial(field);
				break;
			// ▼No.53    花３（紫）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case PURPLE_FLOWER://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickPurpleFlower, new Vector3(x, y - 0.2f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Flower1>().changeMaterial(field);
				break;
			// ▼No.54    チェシャ猫/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			case CHESHIRE_CAT:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				gimmickObjectArray[y, x, z] = GameObject.Instantiate(cheshire, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
				gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
				gimmickNumArray[y, x, z] = gimmickPattern;
				gimmickObjectArray[y, x, z].GetComponent<Cheshire>().SetStartActionTurn(gimmickStartTurn);
				break;
            // ▼No.55    トゥイードルダム///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case TWEEDLEDUM://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickTweedleDum, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(TWEEDLEDUM);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<TweedleDum>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.56    トゥイードルディ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case TWEEDLEDEE://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickTweedleDee, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(TWEEDLEDEE);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<TweedleDee>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.57    ハート兵（右回り）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_HEART_RIGHT:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickSoldierHeartRight, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnRight>().Initialize(gimmickDirection, x, y, z);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.58    ハート兵（左回り）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_HEART_LEFT://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickSoldierHeartLeft, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnLeft>().Initialize(gimmickDirection, x, y, z);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.59    スペード兵（右回り）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_SPADE_RIGHT:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickSoldierSpadeRight, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_SPADE_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<SpadeSoldierTurnRight>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.60    スペード兵（左回り）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_SPADE_LEFT://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickSoldierSpadeLeft, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_SPADE_LEFT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<SpadeSoldierTurnLeft>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.61    スペード兵（往復）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_SPADE_BAF://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickSoldierSpadeBAF, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_SPADE_BAF);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<SpadeSoldierBackAndForth>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.62    岩/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case ROCK:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickNumArray[y, x, z] = NONE_BLOCK;

				pushGimmickObjectArray[y, x, z] = GameObject.Instantiate(rock, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
				pushGimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
				pushGimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(270, 0, 0);
				pushGimmickNumArray[y, x, z] = gimmickPattern;
				pushGimmickObjectArray[y, x, z].GetComponent<Rock>().SetStartActionTurn(gimmickStartTurn);
				pushGimmickObjectArray[y, x, z].GetComponent<Rock>().Initialize(x, y, z);
				break;
        }
    }

    // ★選択されたステージを設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void setSelectStage(int stageNum)
    {
        //ステージテキストの設定
        if (stageNum == 1)
        {
            filepath = "StageData/stage01";
        }
        else if (stageNum == 2)
        {
            filepath = "StageData/stage02";
        }
        else if (stageNum == 3)
        {
            filepath = "StageData/stage03";
        }
        else if (stageNum == 4)
        {
            filepath = "StageData/stage04";
        }
        else if (stageNum == 5)
        {
            filepath = "StageData/stage05";
        }
        else if (stageNum == 6)
        {
            filepath = "StageData/stage06";
        }
        else if (stageNum == 7)
        {
            filepath = "StageData/stage07";
        }
        else if (stageNum == 8)
        {
            filepath = "StageData/stage08";
        }
        else if (stageNum == 9)
        {
            filepath = "StageData/stage09";
        }
        else if (stageNum == 10)
        {
            filepath = "StageData/stage10";
        }
        else if (stageNum == 11)
        {
            filepath = "StageData/stage11";
        }
        else if (stageNum == 12)
        {
            filepath = "StageData/stage12";
        }
        else if (stageNum == 13)
        {
            filepath = "StageData/stage13";
        }
        else if (stageNum == 14)
        {
            filepath = "StageData/stage14";
        }
        else if (stageNum == 15)
        {
            filepath = "StageData/stage15";
        }
        else if (stageNum == 16)
        {
            filepath = "StageData/stage16";
        }
        else if (stageNum == 17)
        {
            filepath = "StageData/stage17";
        }
        else if (stageNum == 18)
        {
            filepath = "StageData/stage18";
        }
        else if (stageNum == 19)
        {
            filepath = "StageData/stage19";
        }
        else if (stageNum == 20)
        {
            filepath = "StageData/stage20";
        }
        else if (stageNum == 21)
        {
            filepath = "StageData/stage21";
        }
        else if (stageNum == 22)
        {
            filepath = "StageData/stage22";
        }
        else if (stageNum == 23)
        {
            filepath = "StageData/stage23";
        }
        else if (stageNum == 24)
        {
            filepath = "StageData/stage24";
        }
        else if (stageNum == 25)
        {
            filepath = "StageData/stage25";
        }
        else if (stageNum == 26)
        {
            filepath = "StageData/stage26";
        }
        else if (stageNum == 27)
        {
            filepath = "StageData/stage27";
        }
        else if (stageNum == 28)
        {
            filepath = "StageData/stage28";
        }
        else if (stageNum == 29)
        {
            filepath = "StageData/stage29";
        }
        else if (stageNum == 30)
        {
            filepath = "StageData/stage30";
        }
        else if (stageNum == 31)
        {
            filepath = "StageData/stage31";
        }
        else if (stageNum == 32)
        {
            filepath = "StageData/stage32";
        }
        else if (stageNum == 33)
        {
            filepath = "StageData/stage33";
        }
        else if (stageNum == 34)
        {
            filepath = "StageData/stage34";
        }
        else if (stageNum == 35)
        {
            filepath = "StageData/stage35";
        }
        else if (stageNum == 36)
        {
            filepath = "StageData/stage36";
        }
        else if (stageNum == 37)
        {
            filepath = "StageData/stage37";
        }
        else if (stageNum == 38)
        {
            filepath = "StageData/stage38";
        }
        else if (stageNum == 39)
        {
            filepath = "StageData/stage39";
        }
        else if (stageNum == 40)
        {
            filepath = "StageData/stage40";
        }
        else if (stageNum == 41)
        {
            filepath = "StageData/stage41";
        }
        else if (stageNum == 42)
        {
            filepath = "StageData/stage42";
        }
        else if (stageNum == 43)
        {
            filepath = "StageData/stage43";
        }
        else if (stageNum == 44)
        {
            filepath = "StageData/stage44";
        }
        else if (stageNum == 45)
        {
            filepath = "StageData/stage45";
        }


        ReadTextData(); //ステージ情報の読み込み
        //ReadFile();
    }

    // ★ターン数の取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public int getStageTurnNum()
    {
        return turnNum;     // ターン数を返す
    }

    // ★スタート位置の取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public Vector3 getStartPosition()
    {
        Vector3 startPosition = new Vector3(0, 0, 0);   // スタート位置の初期化

        for (int x = 0; x < STAGE_X; x++)
        {
            for (int y = 0; y < STAGE_Y; y++)
            {
                for (int z = 0; z < STAGE_Z; z++)
                {
                    // 配列の内容がスタート位置なら
                    if (gimmickNumArray[y, x, z] == START_POINT)
                    {
                        startPosition = new Vector3(x, y - 0.5f, z);    // スタート位置を設定
                    }
                }
            }
        }

        return startPosition;   // スタート位置を返す
    }

    // ★配列上の位置の取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public int getStartArrayPosition(char c)
    {
        int arrayPos = 0;

        for (int x = 0; x < STAGE_X; x++)
        {
            for (int y = 0; y < STAGE_Y; y++)
            {
                for (int z = 0; z < STAGE_Z; z++)
                {
                    // 配列の内容がスタート位置なら
                    if (gimmickNumArray[y, x, z] == START_POINT)
                    {
                        // ▽送られてきた文字が
                        switch (c)
                        {
                            // ▼Xなら////////////////////////////////////////////////
                            case 'x': arrayPos = x; break;  // 配列座標にXの座標を設定
                            // ▼Yなら////////////////////////////////////////////////
                            case 'y': arrayPos = y; break;  // 配列座標にXの座標を設定
                            // ▼Zなら////////////////////////////////////////////////
                            case 'z': arrayPos = z; break;  // 配列座標にXの座標を設定
                        }
                    }
                }
            }
        }

        return arrayPos;
    }

    // ★ギミックの向きを取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    Vector3 getGimmickDirection(int directionPattern)
    {
        Vector3 direction = new Vector3(0, 0, 0);   // 向きを初期化

        Vector3 direction1 = new Vector3(0, 0, 0);      // 向き１の角度を設定
        Vector3 direction2 = new Vector3(0, 90, 0);     // 向き２の角度を設定
        Vector3 direction3 = new Vector3(0, 180, 0);    // 向き３の角度を設定
        Vector3 direction4 = new Vector3(0, 270, 0);    // 向き４の角度を設定

        // ▽送られてきた向きが
        switch (directionPattern)
        {
            // ▼向き１なら//////////////////////////////////////////////
            case 1: direction = direction1; break;  // 向きに向き１を設定
            // ▼向き２なら//////////////////////////////////////////////
            case 2: direction = direction2; break;  // 向きに向き２を設定
            // ▼向き３なら//////////////////////////////////////////////
            case 3: direction = direction3; break;  // 向きに向き３を設定
            // ▼向き４なら//////////////////////////////////////////////
            case 4: direction = direction4; break;  // 向きに向き４を設定
        }

        return direction;   // 向きを返す
    }

    // ★移動可能判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public bool MovePossibleDecision(Player alice, Player.MoveDirection direction, PlayerCamera.CameraAngle cameraAngle)
    {
        bool besideDicisionflag = false;        // 横判定の結果
        bool besideDownDicisionflag = false;    // 横下判定の結果

        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

        // アリスがステージ外にいる場合///////////////////////////////////////////////
        if ((posY == 0) || (posX == 0) || (posX == 10) || (posZ == 0) || (posZ == 10))
        {
            return false;   // 移動できない
        }
        else
        {
            // ▽カメラの向きが
            switch (cameraAngle)
            {
                // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                case PlayerCamera.CameraAngle.FRONT://///////////////////////////////////////////////////////////////////////////////////////
                    // ▽移動方向が
                    switch (direction)
                    {
                        // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.FRONT://///////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX, posZ - 1], posX, posY, posZ - 1);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ - 1], posX, posY - 1, posZ - 1);
                            break;
                        // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.BACK://////////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX, posZ + 1], posX, posY, posZ + 1);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ + 1], posX, posY - 1, posZ + 1);
                            break;
                        // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.LEFT://////////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX + 1, posZ], posX + 1, posY, posZ);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX + 1, posZ], posX + 1, posY - 1, posZ);
                            break;
                        // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.RIGHT://///////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX - 1, posZ], posX - 1, posY, posZ);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX - 1, posZ], posX - 1, posY - 1, posZ);
                            break;
                    }
                    break;
                // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                case PlayerCamera.CameraAngle.BACK://////////////////////////////////////////////////////////////////////////////////////////
                    // ▽移動方向が
                    switch (direction)
                    {
                        // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.FRONT://///////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX, posZ + 1], posX, posY, posZ + 1);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ + 1], posX, posY - 1, posZ + 1);
                            break;
                        // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.BACK://////////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX, posZ - 1], posX, posY, posZ - 1);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ - 1], posX, posY - 1, posZ - 1);
                            break;
                        // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.LEFT://////////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX - 1, posZ], posX - 1, posY, posZ);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX - 1, posZ], posX - 1, posY - 1, posZ);
                            break;
                        // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.RIGHT://///////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX + 1, posZ], posX + 1, posY, posZ);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX + 1, posZ], posX + 1, posY - 1, posZ);
                            break;

                    }
                    break;
                // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                case PlayerCamera.CameraAngle.LEFT://////////////////////////////////////////////////////////////////////////////////////////
                    // ▽移動方向が
                    switch (direction)
                    {
                        // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.FRONT://///////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX + 1, posZ], posX + 1, posY, posZ);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX + 1, posZ], posX + 1, posY - 1, posZ);
                            break;
                        // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.BACK://////////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX - 1, posZ], posX - 1, posY, posZ);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX - 1, posZ], posX - 1, posY - 1, posZ);
                            break;
                        // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.LEFT://////////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX, posZ + 1], posX, posY, posZ + 1);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ + 1], posX, posY - 1, posZ + 1);
                            break;
                        // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.RIGHT://///////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX, posZ - 1], posX, posY, posZ - 1);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ - 1], posX, posY - 1, posZ - 1);
                            break;
                    }
                    break;
                // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                case PlayerCamera.CameraAngle.RIGHT://///////////////////////////////////////////////////////////////////////////////////////
                    // ▽移動方向が
                    switch (direction)
                    {
                        // ▼前なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.FRONT://///////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX - 1, posZ], posX - 1, posY, posZ);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX - 1, posZ], posX - 1, posY - 1, posZ);
                            break;
                        // ▼後なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.BACK://////////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX + 1, posZ], posX + 1, posY, posZ);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX + 1, posZ], posX + 1, posY - 1, posZ);
                            break;
                        // ▼左なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.LEFT://////////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX, posZ - 1], posX, posY, posZ - 1);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ - 1], posX, posY - 1, posZ - 1);
                            break;
                        // ▼右なら//////////////////////////////////////////////////////////////////////////////////////////////////////////
                        case Player.MoveDirection.RIGHT://///////////////////////////////////////////////////////////////////////////////////
                            besideDicisionflag = BesideDicision(gimmickNumArray[posY, posX, posZ + 1], posX, posY, posZ + 1);
                            besideDownDicisionflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ + 1], posX, posY - 1, posZ + 1);
                            break;
                    }
                    break;
            }

            // 横判定と横下判定が真の時のみ真を返す/////////////////////////////////////////
            if (besideDicisionflag && besideDownDicisionflag && alice.gameOverFlag == false)
            {
                return true;    // 移動できる
            }
            else
            {
                return false;   // 移動できない
            }
        }
    }

    // ★横判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    bool BesideDicision(int gimmick, int posX, int posY, int posZ)
    {
        bool flag = false;

        switch (gimmick)
        {
            // 移動できるギミック////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case NONE_BLOCK:        // No.0     透明ブロック
            case START_POINT:       // No.1     スタート地点
            case STAGE_GOOL:        // No.2     ゴール地点
            case IVY_FRONT:         // No.22    蔦（前）
            case IVY_BACK:          // No.23    蔦（後）
            case IVY_LEFT:          // No.24    蔦（左）
            case IVY_RIGHT:         // No.25    蔦（右）
            case LADDER_FRONT:      // No.27    梯子（前）
            case LADDER_BACK:       // No.28    梯子（後）
            case LADDER_LEFT:       // No.29    梯子（左）
            case LADDER_RIGHT:      // No.30    梯子（右）
            case MUSHROOM_SMALL:    // No.33    キノコ（小さくなる）
            case MUSHROOM_BIG:      // No.34    キノコ（大きくなる）
            case POTION_SMALL:      // No.35    薬（小さくなる）
            case POTION_BIG:        // No.36    薬（大きくなる）
            case DOOR_RED_KEY:      // No.37    赤扉（鍵）
            case DOOR_BLUE_KEY:     // No.39    青扉（鍵）
            case DOOR_YELLOW_KEY:   // No.41    黄扉（鍵）
            case DOOR_GREEN_KEY:    // No.43    緑扉（鍵）
            case WARP_HOLE_ONE:     // No.45    穴１
            case WARP_HOLE_TWO:     // No.46    穴２
            case WARP_HOLE_THREE:   // No.47    穴３
            case WARP_HOLE_FOUR:    // No.48    穴４
            case WARP_HOLE_FIVE:    // No.49    穴５
            case BRAMBLE:           // No.50    茨
            case RED_FLOWER:        // No.51    花１（赤）
            case BLUE_FLOWER:       // No.52    花２（青）
            case PURPLE_FLOWER:     // No.53    花３（紫）
            case CHESHIRE_CAT:      // No.54    チェシャ猫  
            case 77:
                flag = true;
                break;

            // 移動できないギミック//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case WATER:                     // No.3     水
            case FOREST_BLOCK_GROUND:       // No.4     森ステージの足場ブロック（1段目）
            case FOREST_BLOCK_GRASS:        // No.5     森ステージの足場ブロック（2段目）
            case FOREST_BLOCK_ALLGRASS:     // No.6     森ステージの足場ブロック（3段目以降）
            case ROOM_BLOCK_FLOOR:          // No.7     家ステージの足場ブロック（1段目）
            case ROOM_BLOCK_BOOKSHELF:      // No.8     家ステージの本棚
            case REDFOREST_BLOCK_GROUND:    // No.9     赤い森ステージの足場ブロック（1段目）
            case REDFOREST_BLOCK_GRASS:     // No.10    赤い森ステージの足場ブロック（2段目）
            case REDFOREST_BLOCK_ALLGRASS:  // No.11    赤い森ステージの足場ブロック（3段目以降）
            case DARKFOREST_BLOCK_GROUND:   // No.12    暗い森ステージの足場ブロック（全段）
            case GARDEN_BLOCK_GROUND:       // No.13    庭園ステージの足場ブロック（1段目）
            case GARDEN_BLOCK_FLOWER:       // No.14    庭園ステージの足場ブロック（2段目以降）
            case DUMMY_TREE:                // No.32    木（成長後判定用）
            default:
                flag = false;
                break;

            // 条件によって変わるギミック////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_BLOCK:     // No.21    蔦ブロック/////////////////////////////////////
            case LADDER_BLOCK:  // No.26    梯子ブロック///////////////////////////////////
                switch (gimmickNumArray[alice.arrayPosY, alice.arrayPosX, alice.arrayPosZ])
                {
                    case IVY_FRONT: // No.22    蔦（前）///////////////////////////////////////////////////////////////////////////
                    case IVY_BACK:  // No.23    蔦（後）///////////////////////////////////////////////////////////////////////////
                    case IVY_LEFT:  // No.24    蔦（左）///////////////////////////////////////////////////////////////////////////
                    case IVY_RIGHT: // No.25    蔦（右）///////////////////////////////////////////////////////////////////////////
                        if (gimmickObjectArray[alice.arrayPosY, alice.arrayPosX, alice.arrayPosZ].GetComponent<Ivy>().eraseFlag)
                        {
                            flag = false;   // 移動できない
                        }
                        else
                        {
                            flag = true;    // 移動できる
                        }
                        break;
                    case LADDER_FRONT: // No.27    梯子（前）//////////////////////////////////////////////////////////////////////
                    case LADDER_BACK:  // No.28    梯子（後）//////////////////////////////////////////////////////////////////////
                    case LADDER_LEFT:  // No.29    梯子（左）//////////////////////////////////////////////////////////////////////
                    case LADDER_RIGHT: // No.30    梯子（右）//////////////////////////////////////////////////////////////////////
                        if (gimmickObjectArray[alice.arrayPosY, alice.arrayPosX, alice.arrayPosZ].GetComponent<Ladder>().breakFlag)
                        {
                            flag = false;   // 移動できない
                        }
                        else
                        {
                            flag = true;    // 移動できる
                        }
                        break;
                }
                break;
            case TREE:  // No.31    木///////////////////////////////////////////////////////////////////////////////
                flag = gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().GetBesideDicisionMovePossibleFlag();
                break;
            case DOOR_RED: // No.38    赤扉（扉）////////////////////////////////////////
                if (alice.getKeyColor_Red)
                {
                    flag = true;    // 移動できる
                    gimmickObjectArray[posY, posX, posZ].GetComponent<Door>().OpenDoor();
                }
                break;
            case DOOR_BLUE: // No.40    青扉（扉）///////////////////////////////////////
                if (alice.getKeyColor_Blue)
                {
                    flag = true;    // 移動できる
                    gimmickObjectArray[posY, posX, posZ].GetComponent<Door>().OpenDoor();
                }
                break;
            case DOOR_YELLOW: // No.42    黄扉（扉）/////////////////////////////////////
                if (alice.getKeyColor_Yellow)
                {
                    flag = true;    // 移動できる
                    gimmickObjectArray[posY, posX, posZ].GetComponent<Door>().OpenDoor();
                }
                break;
            case DOOR_GREEN: // No.44    緑扉（扉）//////////////////////////////////////
                if (alice.getKeyColor_Green)
                {
                    flag = true;    // 移動できる
                    gimmickObjectArray[posY, posX, posZ].GetComponent<Door>().OpenDoor();
                }
                break;

			case SOLDIER_HEART_RIGHT: // No.57    ハート兵(右回り)////////////////////////////////////////////////////////////
				int heartPositionByAliceX_R = posX - alice.arrayPosX; // アリスから見た兵士の位置x
				int heartPositionByAliceZ_R = posZ - alice.arrayPosZ;// アリスから見た兵士の位置z

				int heartPushDirectionX_R = alice.arrayPosX - posX; // 押した方向x
				int heartPushDirectionZ_R = alice.arrayPosZ - posZ; //  押した方向z

				// 押した先にギミックが無ければ移動可能
				if (HeartGimmickDecision(posX, posY, posZ, heartPushDirectionX_R, heartPushDirectionZ_R))
				{
					if (((heartPushDirectionX_R == 1) && (heartPushDirectionZ_R == 0) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().direction == 4)) ||
					((heartPushDirectionX_R == -1) && (heartPushDirectionZ_R == 0) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().direction == 2) ||
					((heartPushDirectionX_R == 0) && (heartPushDirectionZ_R == 1) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().direction == 3)) ||
					((heartPushDirectionX_R == 0) && (heartPushDirectionZ_R == -1) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().direction == 1))))
					{
						flag = true;

						if ((alice.GetMoveDirection() == 3) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().direction == 4) ||
							(alice.GetMoveDirection() == 4) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().direction == 2) ||
							(alice.GetMoveDirection() == 2) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().direction == 3) ||
							(alice.GetMoveDirection() == 1) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().direction == 1))
						{
							gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().PushMove(posX, posY, posZ, heartPushDirectionX_R, heartPushDirectionZ_R);
                            //GameObject objectTemp;
                            //objectTemp = gimmickObjectArray[posY, posX - heartPushDirectionX_R, posZ - heartPushDirectionZ_R];
                            //gimmickObjectArray[posY, posX - heartPushDirectionX_R, posZ - heartPushDirectionZ_R] = gimmickObjectArray[posY, posX, posZ];
                            //gimmickObjectArray[posY, posX, posZ] = objectTemp;

                            //gimmickNumArray[posY, posX - heartPushDirectionX_R, posZ - heartPushDirectionZ_R] = SOLDIER_HEART_RIGHT;
                            //gimmickNumArray[posY, posX, posZ] = NONE_BLOCK;
						}
					}
				}
				break;

			case SOLDIER_HEART_LEFT: // No.58    ハート兵(左回り)////////////////////////////////////////////////////////////
				int heartPositionByAliceX_L = posX - alice.arrayPosX; // アリスから見た兵士の位置x
				int heartPositionByAliceZ_L = posZ - alice.arrayPosZ;// アリスから見た兵士の位置z

				int heartPushDirectionX_L = alice.arrayPosX - posX; // 押した方向x
				int heartPushDirectionZ_L = alice.arrayPosZ - posZ; //  押した方向z

				// 押した先にギミックが無ければ移動可能
				if (HeartGimmickDecision(posX, posY, posZ, heartPushDirectionX_L, heartPushDirectionZ_L))
				{
					if (((heartPushDirectionX_L == 1) && (heartPushDirectionZ_L == 0) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().direction == 4)) ||
					((heartPushDirectionX_L == -1) && (heartPushDirectionZ_L == 0) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().direction == 2) ||
					((heartPushDirectionX_L == 0) && (heartPushDirectionZ_L == 1) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().direction == 3)) ||
					((heartPushDirectionX_L == 0) && (heartPushDirectionZ_L == -1) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().direction == 1))))
					{
						flag = true;

						if ((alice.GetMoveDirection() == 3) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().direction == 4) ||
							(alice.GetMoveDirection() == 4) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().direction == 2) ||
							(alice.GetMoveDirection() == 2) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().direction == 3) ||
							(alice.GetMoveDirection() == 1) && (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().direction == 1))
						{
							gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().PushMove(posX, posY, posZ, heartPushDirectionX_L, heartPushDirectionZ_L);
                            //GameObject objectTemp;
                            //objectTemp = gimmickObjectArray[posY, posX - heartPushDirectionX_L, posZ - heartPushDirectionZ_L];
                            //gimmickObjectArray[posY, posX - heartPushDirectionX_L, posZ - heartPushDirectionZ_L] = gimmickObjectArray[posY, posX, posZ];
                            //gimmickObjectArray[posY, posX, posZ] = objectTemp;

                            //gimmickNumArray[posY, posX - heartPushDirectionX_L, posZ - heartPushDirectionZ_L] = SOLDIER_HEART_LEFT;
                            //gimmickNumArray[posY, posX, posZ] = NONE_BLOCK;
						}
					}
				}
				break;
        }

		switch (pushGimmickNumArray[posY, posX, posZ])
		{
			// 岩
			case ROCK:
				flag = false;
				// アリスが大きければ
				if (alice.GetBig())
				{
					int rockPositionByAliceX = posX - alice.arrayPosX;  // アリスから見た岩の位置X
					int rockPositionByAliceZ = posZ - alice.arrayPosZ;  // アリスから見た岩の位置Z

					int pushDirectionX = alice.arrayPosX - posX;    // 押した方向X
					int pushDirectionZ = alice.arrayPosZ - posZ;    // 押した方向Z

					// 押した先にギミックが無ければ移動可能
					if (RockGimmickDecision(posX, posY, posZ, pushDirectionX, pushDirectionZ))
					{
						flag = true;    // 移動できる
						if (((pushDirectionX == 1) && (pushDirectionZ == 0) && (alice.GetMoveDirection() == 3)) ||
						((pushDirectionX == -1) && (pushDirectionZ == 0) && (alice.GetMoveDirection() == 4)) ||
						((pushDirectionX == 0) && (pushDirectionZ == 1) && (alice.GetMoveDirection() == 2)) ||
						((pushDirectionX == 0) && (pushDirectionZ == -1) && (alice.GetMoveDirection() == 1)))
						{
							pushGimmickObjectArray[posY, posX, posZ].GetComponent<Rock>().PushMove(posX, posY, posZ, pushDirectionX, pushDirectionZ);

							GameObject objectTemp;
							objectTemp = pushGimmickObjectArray[posY, posX - pushDirectionX, posZ - pushDirectionZ];
							pushGimmickObjectArray[posY, posX - pushDirectionX, posZ - pushDirectionZ] = pushGimmickObjectArray[posY, posX, posZ];
							pushGimmickObjectArray[posY, posX, posZ] = objectTemp;

							pushGimmickNumArray[posY, posX - pushDirectionX, posZ - pushDirectionZ] = ROCK;
							pushGimmickNumArray[posY, posX, posZ] = NONE_BLOCK;
						}
					}
				}
				break;
		}


        // 移動系ギミックの判定////////////////////////////////////
        for (int num = 0; num < moveGimmickObjectList.Count; num++)
        {
            // 配列の中にギミックがある場合
            if (moveGimmickNumList[num] != NONE_BLOCK)
            {
                if ((posX == (int)moveGimmickObjectList[num].transform.position.x) && (posY == (int)(moveGimmickObjectList[num].transform.position.y + 0.5f)) && (posZ == (int)moveGimmickObjectList[num].transform.position.z))
                {
                    if (alice.invisibleFlag == false)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }

                }
            }
        }

        return flag;
    }

    // ★横下判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    bool BesideDownDicision(int gimmick, int posX, int posY, int posZ)
    {
        bool flag = false;

        switch (gimmick)
        {
            // 移動できるギミック////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //case NONE_BLOCK:                // No.0     透明ブロック
            case START_POINT:               // No.1     スタート地点
            case STAGE_GOOL:                // No.2     ゴール地点
            case WATER:                     // No.3     水
            case FOREST_BLOCK_GROUND:       // No.4     森ステージの足場ブロック（1段目）
            case FOREST_BLOCK_GRASS:        // No.5     森ステージの足場ブロック（2段目）
            case FOREST_BLOCK_ALLGRASS:     // No.6     森ステージの足場ブロック（3段目以降）
            case ROOM_BLOCK_FLOOR:          // No.7     家ステージの足場ブロック（1段目）
            case ROOM_BLOCK_BOOKSHELF:      // No.8     家ステージの本棚
            case REDFOREST_BLOCK_GROUND:    // No.9     赤い森ステージの足場ブロック（1段目）
            case REDFOREST_BLOCK_GRASS:     // No.10    赤い森ステージの足場ブロック（2段目）
            case REDFOREST_BLOCK_ALLGRASS:  // No.11    赤い森ステージの足場ブロック（3段目以降）
            case DARKFOREST_BLOCK_GROUND:   // No.12    暗い森ステージの足場ブロック（全段）
            case GARDEN_BLOCK_GROUND:       // No.13    庭園ステージの足場ブロック（1段目）
            case GARDEN_BLOCK_FLOWER:       // No.14    庭園ステージの足場ブロック（2段目以降）
            case IVY_BLOCK:                 // No.21    蔦ブロック
            case IVY_FRONT:                 // No.22    蔦（前）
            case IVY_BACK:                  // No.23    蔦（後）
            case IVY_LEFT:                  // No.24    蔦（左）
            case IVY_RIGHT:                 // No.25    蔦（右）
            case LADDER_BLOCK:              // No.26    梯子ブロック
            case LADDER_FRONT:              // No.27    梯子（前）
            case LADDER_BACK:               // No.28    梯子（後）
            case LADDER_LEFT:               // No.29    梯子（左）
            case LADDER_RIGHT:              // No.30    梯子（右）
            case DUMMY_TREE:                // No.32    木（成長後判定用）
            case MUSHROOM_SMALL:            // No.33    キノコ（小さくなる）
            case MUSHROOM_BIG:              // No.34    キノコ（大きくなる）
            case POTION_SMALL:              // No.35    薬（小さくなる）
            case POTION_BIG:                // No.36    薬（大きくなる）
            case DOOR_RED_KEY:              // No.37    赤扉（鍵）
            case DOOR_BLUE_KEY:             // No.39    青扉（鍵）
            case DOOR_YELLOW_KEY:           // No.41    黄扉（鍵）
            case DOOR_GREEN_KEY:            // No.43    緑扉（鍵）
            case WARP_HOLE_ONE:             // No.45    穴１
            case WARP_HOLE_TWO:             // No.46    穴２
            case WARP_HOLE_THREE:           // No.47    穴３
            case WARP_HOLE_FOUR:            // No.48    穴４
            case WARP_HOLE_FIVE:            // No.49    穴５
            case BRAMBLE:                   // No.50    茨
            case CHESHIRE_CAT:              // No.54    チェシャ猫
            case ROCK:                      // No.62    岩
            case 77:
                flag = true;
                break;

            // 条件によって変わるギミック////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case TREE:  // No.31    木///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                flag = gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().GetBesideDownDicisionMovePossibleFlag();   // 木の横下判定用移動可能フラグを取得
                break;
            case RED_FLOWER:    // No.51    花１（赤）///////////////////////////////////////////////////////////////////////////////////////
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<Flower3>().besideDownDicisionMovePossibleFlag == true) { flag = true; }
                else { flag = false; }
                break;
            case BLUE_FLOWER:   // No.52    花２（青）///////////////////////////////////////////////////////////////////////////////////////
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<Flower2>().besideDownDicisionMovePossibleFlag == true) { flag = true; }
                else { flag = false; }
                break;
            case PURPLE_FLOWER: // No.53    花３（紫）///////////////////////////////////////////////////////////////////////////////////////
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<Flower1>().besideDownDicisionMovePossibleFlag == true) { flag = true; }
                else { flag = false; }
                break;

			case SOLDIER_HEART_RIGHT: // No.57		ハート兵（右回り）///////////////////////////////////////////////////////////////////////////////////////
				//if (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnRight>().downFlag == true) { flag = true; }
				break;

			case SOLDIER_HEART_LEFT: // No.58		ハート兵（左回り）///////////////////////////////////////////////////////////////////////////////////////
				//if (gimmickObjectArray[posY, posX, posZ].GetComponent<HeartSoldierTurnLeft>().downFlag == true) { flag = true; }
				break;

			case NONE_BLOCK:
				//switch (pushGimmickNumArray[posY, posX, posZ])
				//{
				//	case ROCK: // No.62    岩
						flag = true;
				//		break;
				//}
				break;
        }

        return flag;
    }

    // ★ギミックとの判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void GimmickDecision(Player alice, Player.PlayerAction action)
    {
        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

        alice.climb1Flag = false;

        // ▽ギミックが//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        switch(gimmickNumArray[posY, posX, posZ])
        {
            // ▼No.2    ゴール地点//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case STAGE_GOOL:
                GoalCheck();
                break;

            // ▼No.22    蔦（前）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_FRONT:
                // ターンが進むの時
                if (action == Player.PlayerAction.NEXT)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Ivy>().climbPossibleFlag)
                    {
                        Climb1(Player.PlayerAngle.FRONT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                // ターンが戻るの時
                else if (action == Player.PlayerAction.RETURN)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (!gimmickObjectArray[posY, posX, posZ].GetComponent<Ivy>().eraseFlag)
                    {
                        Climb1(Player.PlayerAngle.FRONT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                break;
            // ▼No.27    梯子（前）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_FRONT:
                if (action == Player.PlayerAction.NEXT)
                {
                    // 梯子が登れる状態なら梯子の方向を向いて登り状態に
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Ladder>().climbPossibleFlag)
                    {
                        Climb1(Player.PlayerAngle.FRONT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                // ターンが戻るの時
                else if (action == Player.PlayerAction.RETURN)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (!gimmickObjectArray[posY, posX, posZ].GetComponent<Ladder>().breakFlag)
                    {
                        Climb1(Player.PlayerAngle.FRONT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                break;
            // ▼No.23    蔦（後）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_BACK:
                // ターンが進むの時
                if (action == Player.PlayerAction.NEXT)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Ivy>().climbPossibleFlag)
                    {
                        Climb1(Player.PlayerAngle.BACK);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                // ターンが戻るの時
                else if (action == Player.PlayerAction.RETURN)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (!gimmickObjectArray[posY, posX, posZ].GetComponent<Ivy>().eraseFlag)
                    {
                        Climb1(Player.PlayerAngle.BACK);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                break;
            // ▼No.28    梯子（後）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_BACK:
                if (action == Player.PlayerAction.NEXT)
                {
                    // 梯子が登れる状態なら梯子の方向を向いて登り状態に
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Ladder>().climbPossibleFlag)
                    {
                        Climb1(Player.PlayerAngle.BACK);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                // ターンが戻るの時
                else if (action == Player.PlayerAction.RETURN)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (!gimmickObjectArray[posY, posX, posZ].GetComponent<Ladder>().breakFlag)
                    {
                        Climb1(Player.PlayerAngle.BACK);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                break;
            // ▼No.24    蔦（左）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_LEFT:
                // ターンが進むの時
                if (action == Player.PlayerAction.NEXT)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Ivy>().climbPossibleFlag)
                    {
                        Climb1(Player.PlayerAngle.LEFT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                // ターンが戻るの時
                else if (action == Player.PlayerAction.RETURN)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (!gimmickObjectArray[posY, posX, posZ].GetComponent<Ivy>().eraseFlag)
                    {
                        Climb1(Player.PlayerAngle.LEFT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                break;
            // ▼No.29    梯子（左）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_LEFT:
                if (action == Player.PlayerAction.NEXT)
                {
                    // 梯子が登れる状態なら梯子の方向を向いて登り状態に
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Ladder>().climbPossibleFlag)
                    {
                        Climb1(Player.PlayerAngle.LEFT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                // ターンが戻るの時
                else if (action == Player.PlayerAction.RETURN)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (!gimmickObjectArray[posY, posX, posZ].GetComponent<Ladder>().breakFlag)
                    {
                        Climb1(Player.PlayerAngle.LEFT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                break;
            // ▼No.25    蔦（右）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_RIGHT:
                // ターンが進むの時
                if (action == Player.PlayerAction.NEXT)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Ivy>().climbPossibleFlag)
                    {
                        Climb1(Player.PlayerAngle.RIGHT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                // ターンが戻るの時
                else if (action == Player.PlayerAction.RETURN)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (!gimmickObjectArray[posY, posX, posZ].GetComponent<Ivy>().eraseFlag)
                    {
                        Climb1(Player.PlayerAngle.RIGHT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                break;
            // ▼No.30    梯子（右）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_RIGHT:
                if (action == Player.PlayerAction.NEXT)
                {
                    // 梯子が登れる状態なら梯子の方向を向いて登り状態に
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Ladder>().climbPossibleFlag)
                    {
                        Climb1(Player.PlayerAngle.RIGHT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                // ターンが戻るの時
                else if (action == Player.PlayerAction.RETURN)
                {
                    // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                    if (!gimmickObjectArray[posY, posX, posZ].GetComponent<Ladder>().breakFlag)
                    {
                        Climb1(Player.PlayerAngle.RIGHT);
                        alice.SetAnimation(Player.Motion.CLIMB_START, true);
                    }
                }
                break;
            // ▼No.31    木/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case TREE:
                if (action == Player.PlayerAction.NEXT)
                {
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().growCount == 1)
                    {
						playerRizeFlag = true;
                        alice.AutoMoveSetting(Player.MoveDirection.UP);
                    }
                }
                
                break;
            // ▼No.33    キノコ（小さくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // ▼No.35    薬（小さくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case MUSHROOM_SMALL:
            case POTION_SMALL:
                // ギミックフラグが真なら
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<ModeChange>().GetGimmickFlag())
                {
                    alice.playerMode = Player.PlayerMode.SMALL;                                             // 状態を小さいに
                    alice.countSmall = 3;                                                                   // 小さくなっているカウントを３に
                    alice.ModeChange();                                                                     // 状態の切り替え
                    gimmickObjectArray[posY, posX, posZ].GetComponent<ModeChange>().drawFlag = false;       // 描画フラグを偽に
                    gimmickObjectArray[posY, posX, posZ].GetComponent<ModeChange>().OutPutEffect();         // エフェクトを出す
                    gimmickObjectArray[posY, posX, posZ].GetComponent<ModeChange>().SetGimmickFlag(false);  // ギミックフラグを偽に
                }
                break;
            // ▼No.34    キノコ（大きくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // ▼No.36    薬（大きくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case MUSHROOM_BIG:
            case POTION_BIG:
                // ギミックフラグが真なら
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<ModeChange>().GetGimmickFlag())
                {
                    alice.playerMode = Player.PlayerMode.BIG;                                               // 状態を大きいに
                    alice.countBig = 3;                                                                     // 大きくなっているカウントを３に
                    alice.ModeChange();                                                                     // 状態の切り替え
                    gimmickObjectArray[posY, posX, posZ].GetComponent<ModeChange>().OutPutEffect();         // エフェクトを出す
                    gimmickObjectArray[posY, posX, posZ].GetComponent<ModeChange>().drawFlag = false;       // 描画フラグを偽に
                    gimmickObjectArray[posY, posX, posZ].GetComponent<ModeChange>().SetGimmickFlag(false);  // ギミックフラグを偽に
                }
                break;
            // ▼No.50    茨/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case BRAMBLE:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[posY, posX, posZ].GetComponent<Bramble>().trapFlag = true;
                alice.gameOverFlag = true;
                break;
            // ▼No.37    赤扉（鍵）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_RED_KEY:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				alice.GetKey(Player.GetKeyColor.RED);
				gimmickObjectArray[posY, posX, posZ].GetComponent<Key>().GetKey();
				break;
            // ▼No.39    青扉（鍵）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_BLUE_KEY://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				alice.GetKey(Player.GetKeyColor.BLUE);
				gimmickObjectArray[posY, posX, posZ].GetComponent<Key>().GetKey();
				break;
            // ▼No.41    黄扉（鍵）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_YELLOW_KEY:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				alice.GetKey(Player.GetKeyColor.YELLOW);
				gimmickObjectArray[posY, posX, posZ].GetComponent<Key>().GetKey();
				break;
            // ▼No.43    緑扉（鍵）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_GREEN_KEY://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				alice.GetKey(Player.GetKeyColor.GREEN);
				gimmickObjectArray[posY, posX, posZ].GetComponent<Key>().GetKey();
				break;
            // ▼No.54    チェシャ猫/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case CHESHIRE_CAT:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
				alice.TouchCheshire();
				gimmickObjectArray[posY, posX, posZ].GetComponent<Cheshire>().StartInvisible();
				break;
        }
    }

    // ★足元との判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void FootDecision(Player alice,Player.PlayerAction action)
    {
        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

		bool DontFallFlag = false;
        //alice.climb2Flag = false;

        // アリスが地面についていないなら
        if(posY >= 1)
        {
			switch (pushGimmickNumArray[posY - 1, posX, posZ])
			{
				case ROCK:
					DontFallFlag = true;
					break;

				//default:
				//	alice.AutoMoveSetting(Player.MoveDirection.DOWN);
				//	alice.SetAnimation(Player.Motion.DROP_NEXT, true);
				//	print("落下");
				//	break;

			}

			if (!DontFallFlag)
			{
				switch (gimmickNumArray[posY - 1, posX, posZ])
				{
					// 落下するもの
					case NONE_BLOCK:
					case WATER:
					case STAGE_GOOL:
					case DOOR_RED_KEY: // 鍵（赤）
					case DOOR_BLUE_KEY: // 鍵（青）
					case DOOR_YELLOW_KEY: // 鍵（黄）
					case DOOR_GREEN_KEY: // 鍵（緑）
					case CHESHIRE_CAT: // チェシャ
					case MUSHROOM_SMALL:            // No.33    キノコ（小さくなる）
					case MUSHROOM_BIG:              // No.34    キノコ（大きくなる）
					case POTION_SMALL:              // No.35    薬（小さくなる）
					case POTION_BIG:                // No.36    薬（大きくなる）
                        if (!(gimmickNumArray[posY, posX, posZ] == 77))
                        {
                            if (action == Player.PlayerAction.NEXT)
                            {
                                alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                                alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                                print("落下");
                            }
                        }
						
						
						break;

                    case IVY_FRONT:     // 蔦（前）
                        // ターンが進むの時
                        if (action == Player.PlayerAction.NEXT)
                        {
                            // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                            if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Ivy>().climbPossibleFlag)
                            {
                                Climb2(Player.PlayerAngle.FRONT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                                alice.saveClimbMidst[alice.saveCount - 1] = true;
                            }
                            // 蔦が枯れていたら落下
                            else
                            {
                                alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                                alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                            }
                        }
                        // ターンが戻るの時
                        else if (action == Player.PlayerAction.RETURN)
                        {
                            if (alice.saveClimbMidst[alice.saveCount - 1])
                            {
                                Climb2(Player.PlayerAngle.FRONT);
                                alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 1, alice.GetComponent<Player>().arrayPosZ);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                            else
                            {
                                Climb2(Player.PlayerAngle.FRONT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                        }
                        break;
                    case LADDER_FRONT:  // 梯子（前）
                        // ターンが進むの時
                        if (action == Player.PlayerAction.NEXT)
                        {
                            // 梯子が登れる状態なら梯子の方向を向いて登り状態に
                            if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Ladder>().climbPossibleFlag)
                            {
                                Climb2(Player.PlayerAngle.FRONT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                                alice.saveClimbMidst[alice.saveCount - 1] = true;
                            }
                            // 梯子が壊れていたら落下
                            else
                            {
                                alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                                alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                            }
                        }
                        // ターンが戻るの時
                        else if (action == Player.PlayerAction.RETURN)
                        {
                            if (alice.saveClimbMidst[alice.saveCount - 1])
                            {
                                Climb2(Player.PlayerAngle.FRONT);
                                alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 1, alice.GetComponent<Player>().arrayPosZ);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                            else
                            {
                                Climb2(Player.PlayerAngle.FRONT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                        }
                        break;
                    case IVY_BACK:      // 蔦（後）
                        // ターンが進むの時
                        if (action == Player.PlayerAction.NEXT)
                        {
                            // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                            if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Ivy>().climbPossibleFlag)
                            {
                                Climb2(Player.PlayerAngle.BACK);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                                alice.saveClimbMidst[alice.saveCount - 1] = true;
                            }
                            // 蔦が枯れていたら落下
                            else
                            {
                                alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                                alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                            }
                        }
                        // ターンが戻るの時
                        else if (action == Player.PlayerAction.RETURN)
                        {
                            if (alice.saveClimbMidst[alice.saveCount - 1])
                            {
                                Climb2(Player.PlayerAngle.BACK);
                                alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 1, alice.GetComponent<Player>().arrayPosZ);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                            else
                            {
                                Climb2(Player.PlayerAngle.BACK);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                        }
                        break;
                    case LADDER_BACK:   // 梯子（後）
                        // ターンが進むの時
                        if (action == Player.PlayerAction.NEXT)
                        {
                            // 梯子が登れる状態なら梯子の方向を向いて登り状態に
                            if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Ladder>().climbPossibleFlag)
                            {
                                Climb2(Player.PlayerAngle.BACK);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                                alice.saveClimbMidst[alice.saveCount - 1] = true;
                            }
                            // 梯子が壊れていたら落下
                            else
                            {
                                alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                                alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                            }
                        }
                        // ターンが戻るの時
                        else if (action == Player.PlayerAction.RETURN)
                        {
                            if (alice.saveClimbMidst[alice.saveCount - 1])
                            {
                                Climb2(Player.PlayerAngle.BACK);
                                alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 1, alice.GetComponent<Player>().arrayPosZ);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                            else
                            {
                                Climb2(Player.PlayerAngle.BACK);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                        }
                        break;
                    case IVY_LEFT:      // 蔦（左）
                        // ターンが進むの時
                        if (action == Player.PlayerAction.NEXT)
                        {
                            // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                            if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Ivy>().climbPossibleFlag)
                            {
                                Climb2(Player.PlayerAngle.LEFT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                                alice.saveClimbMidst[alice.saveCount - 1] = true;
                            }
                            // 蔦が枯れていたら落下
                            else
                            {
                                alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                                alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                            }
                        }
                        // ターンが戻るの時
                        else if (action == Player.PlayerAction.RETURN)
                        {
                            if (alice.saveClimbMidst[alice.saveCount - 1])
                            {
                                Climb2(Player.PlayerAngle.LEFT);
                                alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 1, alice.GetComponent<Player>().arrayPosZ);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                            else
                            {
                                Climb2(Player.PlayerAngle.LEFT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                        }
                        break;
                    case LADDER_LEFT:   // 梯子（左）
                        // ターンが進むの時
                        if (action == Player.PlayerAction.NEXT)
                        {
                            // 梯子が登れる状態なら梯子の方向を向いて登り状態に
                            if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Ladder>().climbPossibleFlag)
                            {
                                Climb2(Player.PlayerAngle.LEFT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                                alice.saveClimbMidst[alice.saveCount - 1] = true;
                            }
                            // 梯子が壊れていたら落下
                            else
                            {
                                alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                                alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                            }
                        }
                        // ターンが戻るの時
                        else if (action == Player.PlayerAction.RETURN)
                        {
                            if (alice.saveClimbMidst[alice.saveCount - 1])
                            {
                                Climb2(Player.PlayerAngle.LEFT);
                                alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 1, alice.GetComponent<Player>().arrayPosZ);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                            else
                            {
                                Climb2(Player.PlayerAngle.LEFT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                        }
                        break;
                    case IVY_RIGHT:     // 蔦（右）
                        // ターンが進むの時
                        if (action == Player.PlayerAction.NEXT)
                        {
                            // 蔦が登れる状態なら蔦の方向を向いて登り状態に
                            if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Ivy>().climbPossibleFlag)
                            {
                                Climb2(Player.PlayerAngle.RIGHT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                                alice.saveClimbMidst[alice.saveCount - 1] = true;
                            }
                            // 蔦が枯れていたら落下
                            else
                            {
                                alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                                alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                            }
                        }
                        // ターンが戻るの時
                        else if (action == Player.PlayerAction.RETURN)
                        {
                            if (alice.saveClimbMidst[alice.saveCount - 1])
                            {
                                Climb2(Player.PlayerAngle.RIGHT);
                                alice.transform.position = new Vector3(alice.GetComponent<Player>().arrayPosX, alice.GetComponent<Player>().arrayPosY - 1, alice.GetComponent<Player>().arrayPosZ);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                            else
                            {
                                Climb2(Player.PlayerAngle.RIGHT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                        }
                        break;
                    case LADDER_RIGHT:  // 梯子（右）
                        // ターンが進むの時
                        if (action == Player.PlayerAction.NEXT)
                        {
                            // 梯子が登れる状態なら梯子の方向を向いて登り状態に
                            if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Ladder>().climbPossibleFlag)
                            {
                                Climb2(Player.PlayerAngle.RIGHT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                                alice.saveClimbMidst[alice.saveCount - 1] = true;
                            }
                            // 梯子が壊れていたら落下
                            else
                            {
                                alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                                alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                            }
                        }
                        // ターンが戻るの時
                        else if (action == Player.PlayerAction.RETURN)
                        {
                            if (alice.saveClimbMidst[alice.saveCount - 1])
                            {
                                Climb2(Player.PlayerAngle.RIGHT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                            else
                            {
                                Climb2(Player.PlayerAngle.RIGHT);
                                alice.SetAnimation(Player.Motion.CLIMB, true);
                            }
                        }
                        break;
						break;
					case TREE:  // ▼木//////////////////////////////////////////////////////////////////
						if (action == Player.PlayerAction.NEXT)
						{
							// 木の成長段階が１以下なら
							if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Tree>().growCount == 2)
							{
								playerRizeFlag = true;
								alice.AutoMoveSetting(Player.MoveDirection.UP);
							}
							else if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Tree>().growCount < 1)
							{
								alice.AutoMoveSetting(Player.MoveDirection.DOWN);
								alice.SetAnimation(Player.Motion.DROP_NEXT, true);
								print("落下");
							}
						}
						break;
					case WARP_HOLE_ONE:
					case WARP_HOLE_TWO:
					case WARP_HOLE_THREE:
					case WARP_HOLE_FOUR:
					case WARP_HOLE_FIVE:

						if (action == Player.PlayerAction.NEXT || action == Player.PlayerAction.RETURN)
						{
							for (int y = 0; y < STAGE_Y; y++)
							{
								for (int x = 0; x < STAGE_X; x++)
								{
									for (int z = 0; z < STAGE_Z; z++)
									{
										switch (gimmickNumArray[posY - 1, posX, posZ])
										{
											case WARP_HOLE_ONE:
												if (alice.playerMode == Player.PlayerMode.SMALL)
												{
													if (gimmickNumArray[y, x, z] == WARP_HOLE_ONE && ((y != posY - 1) || (x != posX) || (z != posZ)))
													{
														alice.transform.position = new Vector3(x, y + 0.5f, z);
														alice.arrayPosX = x;
														alice.arrayPosY = y + 1;
														alice.arrayPosZ = z;
														break;
													}
												}

												break;
											case WARP_HOLE_TWO:
												if (alice.playerMode == Player.PlayerMode.SMALL)
												{
													if (gimmickNumArray[y, x, z] == WARP_HOLE_TWO && ((y != posY - 1) || (x != posX) || (z != posZ)))
													{
														alice.transform.position = new Vector3(x, y + 0.5f, z);
														alice.arrayPosX = x;
														alice.arrayPosY = y + 1;
														alice.arrayPosZ = z;
														break;
													}
												}
												break;
											case WARP_HOLE_THREE:
												if (alice.playerMode == Player.PlayerMode.SMALL)
												{
													if (gimmickNumArray[y, x, z] == WARP_HOLE_THREE && ((y != posY - 1) || (x != posX) || (z != posZ)))
													{
														alice.transform.position = new Vector3(x, y + 0.5f, z);
														alice.arrayPosX = x;
														alice.arrayPosY = y + 1;
														alice.arrayPosZ = z;
														break;
													}
												}
												break;
											case WARP_HOLE_FOUR:
												if (alice.playerMode == Player.PlayerMode.SMALL)
												{
													if (gimmickNumArray[y, x, z] == WARP_HOLE_FOUR && ((y != posY - 1) || (x != posX) || (z != posZ)))
													{
														alice.transform.position = new Vector3(x, y + 0.5f, z);
														alice.arrayPosX = x;
														alice.arrayPosY = y + 1;
														alice.arrayPosZ = z;
														break;
													}
												}
												break;
											case WARP_HOLE_FIVE:
												if (alice.playerMode == Player.PlayerMode.SMALL)
												{
													if (gimmickNumArray[y, x, z] == WARP_HOLE_FIVE && ((y != posY - 1) || (x != posX) || (z != posZ)))
													{
														alice.transform.position = new Vector3(x, y + 0.5f, z);
														alice.arrayPosX = x;
														alice.arrayPosY = y + 1;
														alice.arrayPosZ = z;
														break;
													}
												}
												break;
										}
									}
								}
							}
						}
						break;
				}
			}
        }
    }

    public void FlowerFootDecision(Player alice)
    {
        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

        //alice.climb2Flag = false;

        // アリスが地面についていないなら
        if (posY >= 1)
        {
            switch (gimmickNumArray[posY - 1, posX, posZ])
            {
                case RED_FLOWER:
                    if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Flower3>().besideDownDicisionMovePossibleFlag == false) { alice.gameOverFlag = true; }

                    break;
                case BLUE_FLOWER:
                    if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Flower2>().besideDownDicisionMovePossibleFlag == false) { alice.gameOverFlag = true; }

                    break;
                case PURPLE_FLOWER:
                    if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Flower1>().besideDownDicisionMovePossibleFlag == false) { alice.gameOverFlag = true; }

                    break;
            }
        }
    }

    public void IvyDecision(Player alice)
    {
        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

        alice.climb2Flag = false;

        // アリスが地面についていないなら
        if (posY >= 1)
        {
            switch (gimmickNumArray[posY - 1, posX, posZ])
            {

                case IVY_FRONT: // 蔦（前）
                    Climb2(Player.PlayerAngle.FRONT);
                    break;
                case IVY_BACK:  // 蔦（後）
                    Climb2(Player.PlayerAngle.BACK);
                    break;
                case IVY_LEFT:  // 蔦（左）
                    Climb2(Player.PlayerAngle.LEFT);
                    break;
                case IVY_RIGHT: // 蔦（右）
                    Climb2(Player.PlayerAngle.RIGHT);
                    break;
            }
        }
    }

    public void FlowerDecision(Player alice)
    {
        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

 
        // アリスが地面についていないなら
        if (posY >= 1)
        {
            switch (gimmickNumArray[posY - 1, posX, posZ])
            {

                case RED_FLOWER:
                    if (!gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Flower3>().GetBesideDicisionMovePossibleFlag())
                        alice.gameOverFlag = true;
                    break;
                case BLUE_FLOWER:
                    if (!gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Flower2>().GetBesideDicisionMovePossibleFlag())
                        alice.gameOverFlag = true;
                    break;
                case PURPLE_FLOWER:
                    if (!gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Flower1>().GetBesideDicisionMovePossibleFlag())
                        alice.gameOverFlag = true;
                    break;
            }
        }
    }

    // ★ゴールしているかチェック★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void GoalCheck()
    {
        print("ゴール到着");
        // ここにゴール処理を書く
        goalFlag = true;
        GameObject.Find("Camera").GetComponent<PlayerCamera>().clearFlag = true;
        Singleton<SoundPlayer>.instance.PlaySE("se004");
        
        // タッチした画面座標からワールド座標へ変換
        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);

        for (int x = 0; x < STAGE_X; x++)
        {
            for (int y = 0; y < STAGE_Y; y++)
            {
                for (int z = 0; z < STAGE_Z; z++)
                {
                    if(gimmickNumArray[y,x,z] == STAGE_GOOL)
                    {
                        pos = new Vector3(gimmickObjectArray[y, x, z].transform.position.x, gimmickObjectArray[y, x, z].transform.position.y-0.5f, gimmickObjectArray[y, x, z].transform.position.z);
                    }
                }
            }
        }
        GameObject go = (GameObject)Instantiate(Effect01, pos, Quaternion.identity);

        if(alice.GetDirection() == 1)
        {
            go.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if(alice.GetDirection() == 3)
        {
            go.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if(alice.GetDirection() == 2)
        {
            go.transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        if(alice.GetDirection() == 4)
        {
            go.transform.localEulerAngles = new Vector3(0, 270, 0);
        }
        
        // エフェクトを消す
        //Destroy(go, 5.0f);
    }

    // ★登り状態に変更する★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void Climb1(Player.PlayerAngle angle)
    {
        // 向きが
        switch(angle)
        {
            case Player.PlayerAngle.FRONT:
                alice.playerAngle = Player.PlayerAngle.FRONT;
                alice.transform.localEulerAngles = new Vector3(0, 0, 0);    // 前方向の角度を指定
                break;
            case Player.PlayerAngle.BACK:
                alice.playerAngle = Player.PlayerAngle.BACK;
                alice.transform.localEulerAngles = new Vector3(0, 180, 0);  // 後方向の角度を指定
                break;
            case Player.PlayerAngle.LEFT:
                alice.playerAngle = Player.PlayerAngle.LEFT;
                alice.transform.localEulerAngles = new Vector3(0, 270, 0);  // 左方向の角度を指定
                break;
            case Player.PlayerAngle.RIGHT:
                alice.playerAngle = Player.PlayerAngle.RIGHT;
                alice.transform.localEulerAngles = new Vector3(0, 90, 0);   // 右方向の角度を指定
                break;
        }

        alice.climb1Flag = true;
    }

    // ★登り状態に変更する★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void Climb2(Player.PlayerAngle angle)
    {
        // 向きが
        switch (angle)
        {
            case Player.PlayerAngle.FRONT:
                alice.playerAngle = Player.PlayerAngle.FRONT;
                alice.transform.localEulerAngles = new Vector3(0, 0, 0);    // 前方向の角度を指定
                break;
            case Player.PlayerAngle.BACK:
                alice.playerAngle = Player.PlayerAngle.BACK;
                alice.transform.localEulerAngles = new Vector3(0, 180, 0);  // 後方向の角度を指定
                break;
            case Player.PlayerAngle.LEFT:
                alice.playerAngle = Player.PlayerAngle.LEFT;
                alice.transform.localEulerAngles = new Vector3(0, 270, 0);  // 左方向の角度を指定
                break;
            case Player.PlayerAngle.RIGHT:
                alice.playerAngle = Player.PlayerAngle.RIGHT;
                alice.transform.localEulerAngles = new Vector3(0, 90, 0);   // 右方向の角度を指定
                break;
        }

        alice.climb2Flag = true;
    }

    // ★一部ギミックの配列上の位置を変更（進める）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void ChangeArrayGimmickNext()
    {
        for (int x = 0; x < STAGE_X; x++)
        {
            for (int y = 0; y < STAGE_Y; y++)
            {
                for (int z = 0; z < STAGE_Z; z++)
                {
                    // ▽ギミックが
                    switch (gimmickNumArray[y, x, z])
                    {
						// ▼木なら
						case TREE:
							// 成長段階が２以下なら１つ上の配列を変更
							if (gimmickObjectArray[y, x, z].GetComponent<Tree>().growCount <= 1)
							{
								gimmickNumArray[y + 1, x, z] = NONE_BLOCK;
							}

							// 成長段階が2なら木と重なっている場所の配列を変更
							if (gimmickObjectArray[y, x, z].GetComponent<Tree>().growCount == 1)
							{
								if (pushGimmickNumArray[y, x, z] == ROCK)
								{
									if ((alice.arrayPosX == x) &&
										(alice.arrayPosZ == z))
									{
										playerRizeFlag = true;
	
										if(alice.arrayPosY == y + 1)
											alice.AutoMoveSetting(Player.MoveDirection.UP);
									}
									pushGimmickObjectArray[y, x, z].GetComponent<Rock>().Rize();

									GameObject objectTemp;
									objectTemp = pushGimmickObjectArray[y + 1, x, z];
									pushGimmickObjectArray[y + 1, x, z] = pushGimmickObjectArray[y, x, z];
									pushGimmickObjectArray[y, x, z] = objectTemp;

									pushGimmickNumArray[y + 1, x, z] = ROCK;
									pushGimmickNumArray[y, x, z] = NONE_BLOCK;
								}
							}
							// 成長段階が3なら一つ上の配列を変更
							if (gimmickObjectArray[y, x, z].GetComponent<Tree>().growCount == 2)
							{
								Debug.Log("nextTree");
								gimmickNumArray[y + 1, x, z] = DUMMY_TREE;
								if (pushGimmickNumArray[y + 1, x, z] == ROCK)
								{
									if ((alice.arrayPosY == y + 2) &&
										(alice.arrayPosX == x) &&
										(alice.arrayPosZ == z))
									{
										playerRizeFlag = true;
										alice.AutoMoveSetting(Player.MoveDirection.UP);
									}
									pushGimmickObjectArray[y + 1, x, z].GetComponent<Rock>().Rize();
									GameObject objectTemp;
									objectTemp = pushGimmickObjectArray[y + 2, x, z];
									pushGimmickObjectArray[y + 2, x, z] = pushGimmickObjectArray[y + 1, x, z];
									pushGimmickObjectArray[y + 1, x, z] = objectTemp;

									pushGimmickNumArray[y + 2, x, z] = ROCK;
									pushGimmickNumArray[y + 1, x, z] = NONE_BLOCK;
								}
							}
							break;

						case NONE_BLOCK:
						case WATER:
						case IVY_BLOCK:
						case IVY_FRONT:
						case IVY_BACK:
						case IVY_LEFT:
						case IVY_RIGHT:
						case LADDER_BLOCK:
						case LADDER_FRONT:
						case LADDER_BACK:
						case LADDER_LEFT:
						case LADDER_RIGHT:
							switch (pushGimmickNumArray[y, x, z])
							{
								case ROCK:
									if (y > 0)
									{
										switch (gimmickNumArray[y - 1, x, z])
										{

											case NONE_BLOCK:
											case WATER:
											case IVY_BLOCK:
											case IVY_FRONT:
											case IVY_BACK:
											case IVY_LEFT:
											case IVY_RIGHT:
											case LADDER_BLOCK:
											case LADDER_FRONT:
											case LADDER_BACK:
											case LADDER_LEFT:
											case LADDER_RIGHT:
											case TREE:

												if (gimmickNumArray[y - 1, x, z] == TREE)
												{
													if (gimmickObjectArray[y - 1, x, z].GetComponent<Tree>().growCount > 0)
														break;
												}

												if (pushGimmickNumArray[y - 1, x, z] == NONE_BLOCK)
												{
													Debug.Log("ROCKFALLCHECK");
													pushGimmickObjectArray[y, x, z].GetComponent<Rock>().Fall();
													GameObject objectTemp;
													objectTemp = pushGimmickObjectArray[y - 1, x, z];
													pushGimmickObjectArray[y - 1, x, z] = pushGimmickObjectArray[y, x, z];
													pushGimmickObjectArray[y, x, z] = objectTemp;

													pushGimmickNumArray[y - 1, x, z] = ROCK;
													pushGimmickNumArray[y, x, z] = NONE_BLOCK;
													//stoneFallController = true;
												}
												break;
										}
										break;
									}
									break;
							}
							break;

						// ▼ハート兵(右回転)なら
						case SOLDIER_HEART_RIGHT:
                            if (gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnRight>().downFlag == true)
                            {
                                switch (gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnRight>().direction)
                                {
                                    case 1:
                                        gimmickNumArray[y, x, z + 1] = 77;
                                        Debug.Log("direction1 start");
                                        break;

                                    case 2:
                                        gimmickNumArray[y, x + 1, z] = 77;
                                        Debug.Log("direction2 start");
                                        break;

                                    case 3:
                                        gimmickNumArray[y, x, z - 1] = 77;
                                        Debug.Log("direction1 start");
                                        break;

                                    case 4:
                                        gimmickNumArray[y, x - 1, z] = 77;
                                        Debug.Log("direction1 start");
                                        break;

                                }
                            }
							break;


						// ▼ハート兵(左回転)なら
						case SOLDIER_HEART_LEFT:
                            if (gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnLeft>().downFlag == true)
                            {
                                switch (gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnLeft>().direction)
                                {
                                    case 1:
                                        gimmickNumArray[y, x, z + 1] = 77;
                                        Debug.Log("direction1 start");
                                        break;

                                    case 2:
                                        gimmickNumArray[y, x + 1, z] = 77;
                                        Debug.Log("direction2 start");
                                        break;

                                    case 3:
                                        gimmickNumArray[y, x, z - 1] = 77;
                                        Debug.Log("direction1 start");
                                        break;

                                    case 4:
                                        gimmickNumArray[y, x - 1, z] = 77;
                                        Debug.Log("direction1 start");
                                        break;

                                }
                            }
							break;
                    }
                }
            }
        }
    }

	// ★一部ギミックの配列上の位置を変更（戻る）★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void ChangeArrayGimmickReturn()
	{
		for (int x = 0; x < STAGE_X; x++)
		{
			for (int y = 0; y < STAGE_Y; y++)
			{
				for (int z = 0; z < STAGE_Z; z++)
				{
					// ▽ギミックが
					switch (gimmickNumArray[y, x, z])
					{
						// ▼木なら
						case TREE:
							// 成長段階が２以下なら１つ上の配列を変更
							if (gimmickObjectArray[y, x, z].GetComponent<Tree>().growCount == 3)
							{
								gimmickNumArray[y + 1, x, z] = NONE_BLOCK;
							}

							if (gimmickObjectArray[y, x, z].GetComponent<Tree>().growCount >= 2)
							{
								//if (pushGimmickNumArray[y + 1, x, z] == ROCK)
								//{
								//	pushGimmickObjectArray[y + 1, x, z].GetComponent<Rock>().Fall();
								//	GameObject objectTemp;
								//	objectTemp = pushGimmickObjectArray[y, x, z];
								//	pushGimmickObjectArray[y, x, z] = pushGimmickObjectArray[y + 1, x, z];
								//	pushGimmickObjectArray[y + 1, x, z] = objectTemp;

								//	pushGimmickNumArray[y, x, z] = ROCK;
								//	pushGimmickNumArray[y + 1, x, z] = NONE_BLOCK;
								//}
							}
							break;

						// ▼岩なら
						case ROCK:
							if (y > 0)
							{
								if (gimmickNumArray[y - 1, x, z] == NONE_BLOCK)
								{
									pushGimmickObjectArray[y, x, z].GetComponent<Rock>().Fall();
									GameObject objectTemp;
									objectTemp = pushGimmickObjectArray[y - 1, x, z];
									pushGimmickObjectArray[y - 1, x, z] = pushGimmickObjectArray[y, x, z];
									pushGimmickObjectArray[y, x, z] = objectTemp;

									pushGimmickNumArray[y - 1, x, z] = ROCK;
									pushGimmickNumArray[y, x, z] = NONE_BLOCK;
								}
							}
							break;

						// ▼ハート兵(右回転)なら
						case SOLDIER_HEART_RIGHT:
                            if (gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnRight>().downFlag == true)
                            {
                                switch (gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnRight>().direction)
                                {
                                    case 1:
                                        gimmickNumArray[y, x, z + 1] = 0;
                                        Debug.Log("direction1 back");
                                        break;

                                    case 2:
                                        gimmickNumArray[y, x + 1, z] = 0;
                                        Debug.Log("direction2 back");
                                        break;

                                    case 3:
                                        gimmickNumArray[y, x, z - 1] = 0;
                                        Debug.Log("direction3 back");
                                        break;

                                    case 4:
                                        gimmickNumArray[y, x - 1, z] = 0;
                                        Debug.Log("direction4 back");
                                        break;

                                }
                            }
							break;

						// ▼ハート兵(左回転)なら
						case SOLDIER_HEART_LEFT:
                            if (gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnLeft>().downFlag == true)
                            {
                                switch (gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnLeft>().direction)
                                {
                                    case 1:
                                        gimmickNumArray[y, x, z + 1] = 0;
                                        Debug.Log("direction1 back");
                                        break;

                                    case 2:
                                        gimmickNumArray[y, x + 1, z] = 0;
                                        Debug.Log("direction2 back");
                                        break;

                                    case 3:
                                        gimmickNumArray[y, x, z - 1] = 0;
                                        Debug.Log("direction3 back");
                                        break;

                                    case 4:
                                        gimmickNumArray[y, x - 1, z] = 0;
                                        Debug.Log("direction4 back");
                                        break;

                                }
                            }
							break;
					}
				}
			}
		}
	}


    //----------------------------------------------------------------------------
    //松村脩平追加部分
    //移動系ギミック横判定////////////////////////////////////////////////////////
    public bool BesideDecision(int posX, int posY, int posZ, bool twins)
    {
        bool flag = false;

        if ((posY == -1) ||
            (posX == -1) || (posX == 11) ||
            (posZ == -1) || (posZ == 11))
            return false;

        switch (gimmickNumArray[posY, posX, posZ])
        {
            //横にあるとき通れるオブジェクト
            //case NONE_BLOCK:      // 何も無し
            case START_POINT:     // スタート地点
            case STAGE_GOOL:      // ゴール地点
            case IVY_FRONT:
            case IVY_BACK:
            case IVY_LEFT:
            case IVY_RIGHT:
            case LADDER_FRONT:
            case LADDER_BACK:
            case LADDER_LEFT:
            case LADDER_RIGHT:
            case MUSHROOM_SMALL:            // No.33    キノコ（小さくなる）
            case MUSHROOM_BIG:              // No.34    キノコ（大きくなる）
            case POTION_SMALL:              // No.35    薬（小さくなる）
            case POTION_BIG:                // No.36    薬（大きくなる）
                flag = true;
                break;

            //横にあるとき通れないオブジェクト
            case FOREST_BLOCK_GROUND:
            case FOREST_BLOCK_GRASS:
            case FOREST_BLOCK_ALLGRASS:

            case ROOM_BLOCK_FLOOR:
            case ROOM_BLOCK_BOOKSHELF:

            case REDFOREST_BLOCK_GROUND:
            case REDFOREST_BLOCK_GRASS:
            case REDFOREST_BLOCK_ALLGRASS:

            case DARKFOREST_BLOCK_GROUND:

            case GARDEN_BLOCK_GROUND:
            case GARDEN_BLOCK_FLOWER:

            case WATER:             // 水

            case IVY_BLOCK:         // 蔦ブロック
            case LADDER_BLOCK:      // 梯子ブロック
                flag = false;
                break;

            case TREE:
            case DUMMY_TREE:
                flag = gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().GetBesideDicisionMovePossibleFlag();   // 木の横判定用移動可能フラグを取得
                break;

			case NONE_BLOCK:
                flag = true;
                switch (pushGimmickNumArray[posY, posX, posZ])
                {
                    case ROCK:
                        flag = false;
                        break;

                }

				break;
        }

        
        //アリス
        if ((posX == alice.arrayPosX) &&
            (posY == alice.arrayPosY) &&
            (posZ == alice.arrayPosZ))
        {
             flag = twins;

            if(alice.invisibleFlag == true)
            {
                flag = true;
            }
        }
            
        return flag;
    }

	// 移動系ギミック横下判定//////////////////////////////////////////////////////////
	public bool BesideDownDecision(int posX, int posY, int posZ)
	{
		bool flag = false;

		switch (gimmickNumArray[posY - 1, posX, posZ])
		{

			//進行方向下にあるとき通れるオブジェクト
			case FOREST_BLOCK_GROUND:
			case FOREST_BLOCK_GRASS:
			case FOREST_BLOCK_ALLGRASS:

			case ROOM_BLOCK_FLOOR:
			case ROOM_BLOCK_BOOKSHELF:

			case REDFOREST_BLOCK_GROUND:
			case REDFOREST_BLOCK_GRASS:
			case REDFOREST_BLOCK_ALLGRASS:

			case DARKFOREST_BLOCK_GROUND:

			case GARDEN_BLOCK_GROUND:
			case GARDEN_BLOCK_FLOWER:

			case START_POINT:             // スタート地点
			case STAGE_GOOL:              // ゴール地点
			case IVY_BLOCK:
			case IVY_FRONT:
			case IVY_BACK:
			case IVY_LEFT:
			case IVY_RIGHT:
			case LADDER_BLOCK:
			case LADDER_FRONT:
			case LADDER_BACK:
			case LADDER_LEFT:
			case LADDER_RIGHT:
			case ROCK:
			case WARP_HOLE_ONE:
			case WARP_HOLE_TWO:
			case WARP_HOLE_THREE:
			case WARP_HOLE_FOUR:
			case WARP_HOLE_FIVE:
			case POTION_BIG:
			case POTION_SMALL:
				flag = true;
				break;

			//進行方向下にあるとき通れないオブジェクト
			case WATER:             // 水
				flag = false;
				break;
			// 木
			case TREE:
				if (gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().growCount <= 2)
				{
					flag = true;
				}
				else
				{
					flag = false;
				}
				break;

			case NONE_BLOCK:
				switch (pushGimmickNumArray[posY-1, posX, posZ])
				{
					case ROCK:
                    case ROOM_BLOCK_BOOKSHELF:
						flag = true;
						break;
				}
				break;
		}
		return flag;
	}


    public bool GetFootHole(Player alice)
    {
        int posX = alice.arrayPosX;       // 配列上の座標Ｘ
        int posY = alice.arrayPosY;       // 配列上の座標Ｙ
        int posZ = alice.arrayPosZ;       // 配列上の座標Ｚ

        if (posY - 1 >= 0)
        {
            if ((gimmickNumArray[posY - 1, posX, posZ] == WARP_HOLE_ONE) || (gimmickNumArray[posY - 1, posX, posZ] == WARP_HOLE_TWO) ||
                (gimmickNumArray[posY - 1, posX, posZ] == WARP_HOLE_THREE) || (gimmickNumArray[posY - 1, posX, posZ] == WARP_HOLE_FOUR) ||
                ((gimmickNumArray[posY - 1, posX, posZ] == WARP_HOLE_FIVE)))
            {
                return true;
            }
        }

        return false;
    }

	// ★岩とギミックの判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public bool RockGimmickDecision(int posX, int posY, int posZ, int pushDirectionX, int pushDirectionZ)
	{
		// 移動先がステージを超えておらず、何もなければ押して通れる
		if (((pushDirectionX == 1) && (posX > 1)) ||
			 ((pushDirectionX == -1) && (posX < STAGE_X - 2)) ||
			 ((pushDirectionZ == 1) && (posZ > 1)) ||
			 ((pushDirectionZ == -1) && (posZ < STAGE_Z - 2)))
		{
			// アリスとの判定
			if ((alice.arrayPosY == posY) &&
				(alice.arrayPosX == posX - pushDirectionX) &&
				(alice.arrayPosZ == posZ - pushDirectionZ))
				return false;

			switch (gimmickNumArray[posY, posX - pushDirectionX, posZ - pushDirectionZ])
			{
				case NONE_BLOCK:
					switch (pushGimmickNumArray[posY, posX - pushDirectionX, posZ - pushDirectionZ])
					{
						case ROCK:
							return false;
						default:
							return true;
					}
				case TREE:
					{
						if (gimmickObjectArray[posY, posX - pushDirectionX, posZ - pushDirectionZ].GetComponent<Tree>().growCount <= 1)
							return true;
						return false;
					}

				case START_POINT:
				case STAGE_GOOL:

				case IVY_FRONT:
				case IVY_BACK:
				case IVY_LEFT:
				case IVY_RIGHT:
				case LADDER_FRONT:
				case LADDER_BACK:
				case LADDER_LEFT:
				case LADDER_RIGHT:
					return true;

				case POTION_BIG:
				case POTION_SMALL:
				case MUSHROOM_BIG:
				case MUSHROOM_SMALL:
					//現在のターンを入れる
					//ModeChangeSaveTurn[SaveTurnArrayController] = GameObject.Find("GameMain").GetComponent<GameMain>().turnNum;
					//ModeChangeSaveNum[ModeChangeSaveTurn[SaveTurnArrayController]] = gimmickNumArray[posY, posX - pushDirectionX, posZ - pushDirectionZ];
					//SaveTurnArrayController++;
					if (gimmickObjectArray[posY, posX - pushDirectionX, posZ - pushDirectionZ].GetComponent<ModeChange>().GetRendererEnabled() == false)
					{
						return true;
					}
					else
					{
						return false;
					}

				default: break;
			}
		}
		return false;
	}

	// ★ギミックの巻き戻し処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void GimmickReturn(int posX, int posY, int posZ, int moveDirectionX, int moveDirectionY, int moveDirectionZ, int kind)
	{
		Debug.Log("MODOTTAYO");
		Debug.Log(posX);
		Debug.Log(posY);
		Debug.Log(posZ);
		GameObject objectTemp;
		objectTemp = pushGimmickObjectArray[posY + moveDirectionY, posX - moveDirectionX, posZ - moveDirectionZ];
		pushGimmickObjectArray[posY + moveDirectionY, posX - moveDirectionX, posZ - moveDirectionZ] = pushGimmickObjectArray[posY, posX, posZ];
		pushGimmickObjectArray[posY, posX, posZ] = objectTemp;

		pushGimmickNumArray[posY + moveDirectionY, posX - moveDirectionX, posZ - moveDirectionZ] = NONE_BLOCK;

		switch (kind)
		{
			case 1:
				pushGimmickNumArray[posY, posX, posZ] = ROCK;
                int localSetTurn = (GameObject.Find("GameMain").GetComponent<GameMain>().turnNum)+1;
     
            if (SaveTurnArrayController != 0)
            {   //現在のターン数を入れる
                if (localSetTurn == ModeChangeSaveTurn[SaveTurnArrayController - 1])
                {
                    gimmickNumArray[posY + moveDirectionY, posX - moveDirectionX, posZ - moveDirectionZ] = ModeChangeSaveNum[ModeChangeSaveTurn[SaveTurnArrayController - 1]];
                    ModeChangeSaveNum[ModeChangeSaveTurn[SaveTurnArrayController - 1]] = 0;
                    ModeChangeSaveTurn[SaveTurnArrayController - 1] = 0;
                    SaveTurnArrayController--;
                }

            }
				break;
			case 2:
				//gimmickNumArray[posY, posX, posZ] = SOLDIER_HEART_RIGHT;
				break;
			case 3:
				gimmickNumArray[posY, posX, posZ] = SOLDIER_HEART_LEFT;
				break;
			default:
				break;
		}
	}

	// ★ハートとギミックの判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public bool HeartGimmickDecision(int posX, int posY, int posZ, int pushDirectionX, int pushDirectionZ)
	{
		bool flag = false;

		// 移動先とその下がステージを超えておらず、何もなければ押して通れる
		if (((pushDirectionX == 1) && (posX > 1)) ||
			 ((pushDirectionX == -1) && (posX < STAGE_X - 2)) ||
			 ((pushDirectionZ == 1) && (posZ > 1)) ||
			 ((pushDirectionZ == -1) && (posZ < STAGE_Z - 2)))
		{
			switch (gimmickNumArray[posY, posX - pushDirectionX, posZ - pushDirectionZ])
			{
				case NONE_BLOCK:
				case START_POINT:
				case STAGE_GOOL:
					flag = true;
					break;
				default:
					break;
			}
		}

		if (flag)
		{
			switch (gimmickNumArray[posY - 1, posX - pushDirectionX, posZ - pushDirectionZ])
			{
				case NONE_BLOCK:
				case START_POINT:
				case STAGE_GOOL:
					return true;
				default:
					break;
			}
		}
		return false;

	}

	// ★アリスが自動移動しているか確認★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public bool CheckAutoMove()
	{
		return alice.GetAutoMove();
	}
    //----------------------------------------------------------------------------
    //ダムが横の方向見る
    public bool TwinBesideDecision(int posX, int posY, int posZ, Vector3 mine, int direction)
    {
        int TwinDirection = direction;
        bool flag = false;

        if ((posY == -1) ||
            (posX == -1) || (posX == 11) ||
            (posZ == -1) || (posZ == 11))
            return false;
        //・・・・・・・・・・・・・・・・・・・・
        //石を押すと石の番号が消える
        //・・・・・・・・・・・・・・・・・・・・
        switch (gimmickNumArray[posY, posX, posZ])
        {
            //横にあるとき通れるオブジェクト
            //case NONE_BLOCK:      // 何も無し
            case START_POINT:     // スタート地点
            case STAGE_GOOL:      // ゴール地点
            case IVY_FRONT:
            case IVY_BACK:
            case IVY_LEFT:
            case IVY_RIGHT:
            case LADDER_FRONT:
            case LADDER_BACK:
            case LADDER_LEFT:
            case LADDER_RIGHT:

                flag = true;
                break;

            //横にあるとき通れないオブジェクト
            case FOREST_BLOCK_GROUND:
            case FOREST_BLOCK_GRASS:
            case FOREST_BLOCK_ALLGRASS:

            case ROOM_BLOCK_FLOOR:
            case ROOM_BLOCK_BOOKSHELF:

            case REDFOREST_BLOCK_GROUND:
            case REDFOREST_BLOCK_GRASS:
            case REDFOREST_BLOCK_ALLGRASS:

            case DARKFOREST_BLOCK_GROUND:

            case GARDEN_BLOCK_GROUND:
            case GARDEN_BLOCK_FLOWER:

            case WATER:             // 水

            case IVY_BLOCK:         // 蔦ブロック
            case LADDER_BLOCK:      // 梯子ブロック
                flag = false;
                break;

            case TREE:
            case DUMMY_TREE:
                flag = gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().GetBesideDicisionMovePossibleFlag();   // 木の横判定用移動可能フラグを取得
                break;
            case POTION_BIG:
            case POTION_SMALL:
            case MUSHROOM_BIG:
            case MUSHROOM_SMALL:
                //if (gimmickObjectArray[posY, posX, posZ].GetComponent<ModeChange>().GetRendererEnabled() == false)
                    flag = true;
                break;

			case NONE_BLOCK:      // 何も無し
				switch (pushGimmickNumArray[posY, posX, posZ])
				{
					case ROCK:

						int rockPositionByAliceX = posX - alice.arrayPosX; // アリスから見た岩の位置x
						int rockPositionByAliceZ = posZ - alice.arrayPosZ;// アリスから見た岩の位置z

						int pushDirectionX = (int)mine.x - posX; // 押した方向x
						int pushDirectionZ = (int)mine.z - posZ; //  押した方向z
						//Debug.Log(pushDirectionX);
						Debug.Log(pushDirectionZ);
						//Debug.Log(alice.GetMoveDirection());

						// 押した先にギミックが無ければ移動可能
						if (RockGimmickDecision(posX, posY, posZ, pushDirectionX, pushDirectionZ))
						{
							flag = true;
							//// 
							//int dum = gimmickObjectArray[(int)mine.y,(int)mine.x,(int)mine.z].GetComponent<TweedleDum>().direction;

							if (((pushDirectionX == 1) && (pushDirectionZ == 0) && (TwinDirection == 4)) ||
							((pushDirectionX == -1) && (pushDirectionZ == 0) && (TwinDirection == 2)) ||
							((pushDirectionX == 0) && (pushDirectionZ == 1) && (TwinDirection == 3)) ||
							((pushDirectionX == 0) && (pushDirectionZ == -1) && (TwinDirection == 1)))
							{
								pushGimmickObjectArray[posY, posX, posZ].GetComponent<Rock>().GimmickPushMove(posX, posY, posZ, pushDirectionX, pushDirectionZ);

								GameObject objectTemp;
								objectTemp = pushGimmickObjectArray[posY, posX - pushDirectionX, posZ - pushDirectionZ];
								pushGimmickObjectArray[posY, posX - pushDirectionX, posZ - pushDirectionZ] = pushGimmickObjectArray[posY, posX, posZ];
								pushGimmickObjectArray[posY, posX, posZ] = objectTemp;

								pushGimmickNumArray[posY, posX - pushDirectionX, posZ - pushDirectionZ] = ROCK;
								pushGimmickNumArray[posY, posX, posZ] = NONE_BLOCK;
							}
						}

						break;
						
					default:
						flag = true;
						break;
				}
				break;
        }
        return flag;
    }

    public void SearchRockFallAgain()
    {

        for (int x = 0; x < STAGE_X; x++)
        {
            for (int y = 0; y < STAGE_Y; y++)
            {
                for (int z = 0; z < STAGE_Z; z++)
                {
                    // ▽ギミックが
                    switch (gimmickNumArray[y, x, z])
                    {

                        case NONE_BLOCK:
                            switch (pushGimmickNumArray[y, x, z])
                            {
                                case ROCK:
                                    if (y > 0)
                                    {
                                        if ((gimmickNumArray[y - 1, x, z] == NONE_BLOCK) ||
                                            (gimmickNumArray[y - 1, x, z] == WATER))
                                        {
                                            if (pushGimmickNumArray[y - 1, x, z] == NONE_BLOCK)
                                            {
                                                pushGimmickObjectArray[y, x, z].GetComponent<Rock>().Fall();
                                                GameObject objectTemp;
                                                objectTemp = pushGimmickObjectArray[y - 1, x, z];
                                                pushGimmickObjectArray[y - 1, x, z] = pushGimmickObjectArray[y, x, z];
                                                pushGimmickObjectArray[y, x, z] = objectTemp;

                                                pushGimmickNumArray[y - 1, x, z] = ROCK;
                                                pushGimmickNumArray[y, x, z] = NONE_BLOCK;
                                                stoneFallController = true;

                                                GameObject.Find("GameMain").GetComponent<GameMain>().turnCountGimmick = 0;
                                            }
                                        }
                                    }
                                    break;
                            }
                            break;

                    }
                }
            }
        }
    }

    public bool TwinsTestFunc(Vector3 TwinsPos, int direction, bool dee)
    {

        int deeNum = 0;
        int dumNum = 0;

        // 移動系ギミックの判定////////////////////////////////////
        for (int num = 0; num < moveGimmickObjectList.Count; num++)
        {
            // 配列の中にギミックがある場合
            if (moveGimmickNumList[num] == TWEEDLEDEE)
            {
                deeNum = num;
            }

            // 配列の中にギミックがある場合
            if (moveGimmickNumList[num] == TWEEDLEDUM)
            {
                dumNum = num;
            }

        }

        bool testcode = false;
        if (Twins1 == new Vector3(0, 0, 0))
        {
            Twins1 = TwinsPos;
        }
        else
        {
            Twins2 = TwinsPos;
        }
        if (oncetime != true)
        {
            switch (direction)
            {
                case 1:

                    if ((moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().arrayPosZ + 2 == moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().arrayPosZ) && (dee))
                    {
                        if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction == 1 && moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction == 3)
                        {
                            moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 3;
                            int move = moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction;
                            switch (move)
                            {
                                case 1:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 3;
                                    break;
                                case 3:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 1;
                                    break;
                                case 4:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 2;
                                    break;
                                case 2:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 4;
                                    break;
                            }
                        }
                    }
                    else if (moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().arrayPosZ + 2 == moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().arrayPosZ && (!dee))
                    {
                        if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction == 3 && moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction == 1)
                        {
                            moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 3;
                            int move = moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction;
                            switch (move)
                            {
                                case 1:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 3;
                                    break;
                                case 3:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 1;
                                    break;
                                case 4:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 2;
                                    break;
                                case 2:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 4;
                                    break;
                            }
                        }
                    }
                    break;

                case 3:
                    //z-2
                    //array.Z-2の座標にdee又はdumがいる時
                    if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().arrayPosZ - 2 == moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().arrayPosZ && (dee))
                    {
                        if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction == 3 && moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction == 1)
                        {
                            moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 1;
                            int move = moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction;
                            switch (move)
                            {
                                case 1:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 3;
                                    break;
                                case 3:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 1;
                                    break;
                                case 4:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 2;
                                    break;
                                case 2:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 4;
                                    break;
                            }
                        }
                    }
                    else if (moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().arrayPosZ - 2 == moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().arrayPosZ && (!dee))
                    {
                        if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction == 1 && moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction == 3)
                        {
                            moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 1;
                            int move = moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction;
                            switch (move)
                            {
                                case 1:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 3;
                                    break;
                                case 3:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 1;
                                    break;
                                case 4:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 2;
                                    break;
                                case 2:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 4;
                                    break;
                            }
                        }
                    }
                    break;

                case 4:
                    //array.X-2の座標にdee又はdumがいる時
                    if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().arrayPosX - 2 == moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().arrayPosX && (dee))
                    {
                        if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction == 4 && moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction == 2)
                        {
                            moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 2;
                            int move = moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction;
                            switch (move)
                            {
                                case 1:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 3;
                                    break;
                                case 3:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 1;
                                    break;
                                case 4:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 2;
                                    break;
                                case 2:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 4;
                                    break;
                            }
                        }
                    }
                    else if (moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().arrayPosX - 2 == moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().arrayPosX && (!dee))
                    {
                        if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction == 2 && moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction == 4)
                        {
                            moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 2;
                            int move = moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction;

                            switch (move)
                            {
                                case 1:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 3;
                                    break;
                                case 3:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 1;
                                    break;
                                case 4:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 2;
                                    break;
                                case 2:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 4;
                                    break;
                            }
                        }
                    }
                    break;

                case 2:

                    if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().arrayPosX + 2 == moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().arrayPosX && (dee))
                    {
                        if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction == 2 && moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction == 4)
                        {
                            moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 4;
                            int move = moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction;
                            switch (move)
                            {
                                case 1:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 3;
                                    break;
                                case 3:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 1;
                                    break;
                                case 4:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 2;
                                    break;
                                case 2:
                                    moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 4;
                                    break;
                            }
                        }

                    }
                    else if (moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().arrayPosX - 2 == moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().arrayPosX && (!dee))
                    {

                        if (moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction == 4 && moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction == 2)
                        {
                            moveGimmickObjectList[dumNum].GetComponent<TweedleDum>().direction = 4;
                            int move = moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction;

                            switch (move)
                            {
                                case 1:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 3;
                                    break;
                                case 3:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 1;
                                    break;
                                case 4:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 2;
                                    break;
                                case 2:
                                    moveGimmickObjectList[deeNum].GetComponent<TweedleDee>().direction = 4;
                                    break;
                            }
                        }
                    }
                    break;
            }
            oncetime = true;
        }
        else
        {
            oncetime = false;
        }
        return false;
    }
    //DEEとDUMが両方ともステージにいるか？
    public bool findTwins()
    {
        bool twin1 = false;
        bool twin2 = false;
        for (int num = 0; num < moveGimmickObjectList.Count; num++)
        {
            // 配列の中にギミックがある場合
            if (moveGimmickNumList[num] == TWEEDLEDEE)
            {
                twin1 = true;
            }
            if(moveGimmickNumList[num] == TWEEDLEDUM)
            {
                twin2 = true;
            }
            if((twin1)&&(twin2))
            {
                return true;
            }
        }
        return false;

    }
	public void StartMove(int type)
	{
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				for (int k = 0; k < 11; k++)
				{
					if (pushGimmickNumArray[i, j, k] == ROCK)
					{
						//Debug.Log("yobareta");
						//Debug.Log(i);
						//Debug.Log(j);
						//Debug.Log(k);
						if (playerRizeFlag)
							playerRizeFlag = false;
						else
							pushGimmickObjectArray[i, j, k].GetComponent<Rock>().StartMove(type);
					}
				}
			}
		}
	}

    public bool GimmickMoveMidst()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                for (int k = 0; k < 11; k++)
                {
                    if (pushGimmickNumArray[i, j, k] == ROCK || pushGimmickNumArray[i, j, k] == ROOM_BLOCK_BOOKSHELF)
                    {
                        if (pushGimmickObjectArray[i, j, k].GetComponent<Rock>().moveFlag == true)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

}