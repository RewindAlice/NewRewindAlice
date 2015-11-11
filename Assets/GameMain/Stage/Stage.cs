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

    //新配列番号
    const int NONE_BLOCK = 0;
    const int START_POINT = 1;
    const int STAGE_GOOL = 2;
    const int WATER = 3;    // 水

    const int FOREST_BLOCK_GROUND = 4;         // 森ステージの足場ブロック（1段目）
    const int FOREST_BLOCK_GRASS = 5;          // 森ステージの足場ブロック（2段目）
    const int FOREST_BLOCK_ALLGRASS = 6;       // 森ステージの足場ブロック（3段目以降）

    const int ROOM_BLOCK_FLOOR = 7;            // 家ステージの足場ブロック（1段目）
    const int ROOM_BLOCK_BOOKSHELF = 8;        // 家ステージの足場ブロック（2段目）

    const int REDFOREST_BLOCK_GROUND = 9;      // 森ステージの足場ブロック（1段目）
    const int REDFOREST_BLOCK_GRASS = 10;      // 森ステージの足場ブロック（2段目）
    const int REDFOREST_BLOCK_ALLGRASS = 11;   // 森ステージの足場ブロック（3段目以降）

    const int DARKFOREST_BLOCK_GROUND = 12;    // 暗い森ステージの足場ブロック（全段）

    const int GARDEN_BLOCK_GROUND = 13;        // ガーデンステージの足場ブロック（1段目）
    const int GARDEN_BLOCK_FLOWER = 14;        // ガーデンテージの足場ブロック（2段目以降）



    //// 蔦ギミック（仮）
    const int IVY_BLOCK = 21;
    const int IVY_FRONT = 22;
    const int IVY_BACK = 23;
    const int IVY_LEFT = 24;
    const int IVY_RIGHT = 25;

    //梯子
    const int LADDER_BLOCK = 26;
    const int LADDER_FRONT = 27;
    const int LADDER_BACK = 28;
    const int LADDER_LEFT = 29;
    const int LADDER_RIGHT = 30;

    const int TREE = 31;   // 高さ1の木
    const int DUMMY_TREE = 32;   // 高さ2の木

    const int MUSHROOM_SMALL = 33;  // キノコ（小さくなる）
    const int MUSHROOM_BIG = 34;    // キノコ（大きくなる）
    const int POTION_SMALL = 35;    // 薬（小さくなる）
    const int POTION_BIG = 36;      // 薬（大きくなる）

    const int DOOR_RED_KEY = 37;    // 赤扉（鍵）
    const int DOOR_RED = 38;        // 赤扉

    const int DOOR_BLUE_KEY = 39;   // 青扉（鍵）
    const int DOOR_BLUE = 40;       // 青扉

    const int DOOR_YELLOW_KEY = 41; // 黄扉（鍵）
    const int DOOR_YELLOW = 42;     // 黄扉

    const int DOOR_GREEN_KEY = 43;  // 緑扉（鍵）
    const int DOOR_GREEN = 44;      // 緑扉

    const int WARP_HOLE_ONE = 45;   // 穴１
    const int WARP_HOLE_TWO = 46;   // 穴２
    const int WARP_HOLE_TRHEE = 47; // 穴３
    const int WARP_HOLE_FOUR = 48;  // 穴４
    const int WARP_HOLE_FIVE = 49;  // 穴５

    const int BRAMBLE = 50;         // 茨
    const int RED_FLOWER = 51;      // 花１
    const int BLUE_FLOWER = 52;     // 花２
    const int PURPLE_FLOWER = 53;   // 花３
    const int CHESHIRE_CAT = 54;    // チェシャ猫  
 
    const int TWEEDLEDUM = 55;           //トゥイードルダム
    const int TWEEDLEDEE = 56;           //トゥイードルディ
    const int SOLDIER_HEART_RIGHT = 57;  //ハート兵右回り
    const int SOLDIER_HEART_LEFT = 58;//ハート兵左回り
    const int SOLDIER_SPADE_RIGHT = 59;//スペード兵右回り
    const int SOLDIER_SPADE_LEFT = 60;//スペード兵左回り
    const int SOLDIER_SPADE_BAF = 61;//スペード兵行ったり来たり
    const int ROCK = 62;                 //岩
    const int HAMPTYDUMPTY = 64;         //ハンプティダンプティ




    public Player alice;

    public GameObject gimmickNone;  // 何も無い
    public GameObject gimmickStart; // スタート地点
    public GameObject gimmickGoal;  // ゴール地点
    public GameObject gimmickBlock; // ブロック
    public GameObject gimmickEnemy; // 敵（移動系配列）

    public GameObject gimmickIVY;   // 蔦
    public GameObject gimmickTree;  // 木

    int gimmick = 0;            // 配列の数字
    int gimmickPattern = 0;     // ギミックの種類
    int gimmickDirection = 0;   // ギミックの向き
    int gimmickStartTurn = 0;   // ギミックの開始ターン

    int[, ,] gimmickArray = new int[STAGE_Y, STAGE_X, STAGE_Z];                     // ステージの配置（数字）
    int[, ,] gimmickNumArray = new int[STAGE_Y, STAGE_X, STAGE_Z];                  // ステージの配置（ギミック番号）
    List<int> moveGimmickNumList = new List<int>();                                 // 移動系ギミック（数字）
    GameObject[, ,] gimmickObjectArray = new GameObject[STAGE_Y, STAGE_X, STAGE_Z]; // ステージの配置（オブジェクト）
    List<GameObject> moveGimmickObjectList = new List<GameObject>();                // 移動系ギミック（オブジェクト）
    //int[, ,] gimmickStartTurnArray = new int[STAGE_Y, STAGE_X, STAGE_Z];            // ステージの配置（開始ターン数）
    //List<int> moveGimmickStartTurnList = new List<int>();                           // 移動系ギミック（開始ターン数）
    int turnNum;                                                                    // ステージのターン数
    private string guitxt = "";
    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        
	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {
	
	}

    void ReadFile()
    {
        FileInfo fi = new FileInfo("Assets/stage.txt");

        StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8);

        guitxt = sr.ReadLine();
        turnNum = Int32.Parse(guitxt);

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
    // ★ステージの生成★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void CreateStage()
    {
        for(int x = 0; x < STAGE_X; x++)
        {
            for(int y = 0; y < STAGE_Y; y++)
            {
                for(int z = 0; z < STAGE_Z; z++)
                {
                    // ギミックの生成
                    CreateGimmcik(x, y, z);
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
        gimmick = gimmick % 100;            // 配列の数字を百で割った余りを入れる
        gimmickStartTurn = gimmick;         // ギミックの開始ターンを入れる

        switch (gimmickPattern)
        {
            case NONE_BLOCK:  // ▼何も無い//////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case START_POINT: // ▼スタート地点///////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickStart, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case STAGE_GOOL:  // ▼ゴール地点////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickGoal, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case FOREST_BLOCK_GROUND: // ▼ブロック///////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickBlock, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            //case ENEMY: // ▼敵////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //    gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y, z), Quaternion.identity) as GameObject;
            //    gimmickNumArray[y, x, z] = NONE_BLOCK;
            //    moveGimmickObjectList.Add(GameObject.Instantiate(gimmickEnemy, new Vector3(x, y, z), Quaternion.identity) as GameObject);
            //    moveGimmickNumList.Add(ENEMY);
            //    break;
            case IVY_BLOCK: // ▼蔦ブロック/////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickBlock, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case IVY_FRONT: // ▼蔦（前）///////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_BACK:  // ▼蔦（後）///////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_LEFT:  // ▼蔦（左）///////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_RIGHT: // ▼蔦（右）///////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case TREE: // ▼木//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickTree, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;  // ギミックのオブジェクトを配列に設定
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);                                     // ギミックを指定された向きに変更
                gimmickNumArray[y, x, z] = gimmickPattern;                                                                                          // ギミックを数字として配列に設定
                gimmickObjectArray[y, x, z].GetComponent<Tree>().SetStartActionTurn(gimmickStartTurn);                                              // ギミックの開始ターンを配列に設定
                break;

        }
    }

    // ★選択されたステージを設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void setSelectStage(int stageNum)
    {
        switch (stageNum)
        {
            case 1: Stage1(); break;    // ステージ１を設定
            case 2: Stage2(); break;    // ステージ２を設定
			case 3: Stage3(); break;    // ステージ３を設定
        }
    }

    // ★ステージ１★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void Stage1()
    {
        int stageTurnNum = 30;

        int[, ,] stage = new int[,,]
        {
            {
                // １段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10401,  10401,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10401,  10401,  10401,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10401,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
            {
                // ２段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  11001,  10101,  10601,  10801,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10201,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10301,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
            {
                // ３段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
            {
                // ４段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
            {
                // ５段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
        };

        turnNum = stageTurnNum;

        for (int x = 0; x < STAGE_X; x++)
        {
            for (int y = 0; y < STAGE_Y; y++)
            {
                for (int z = 0; z < STAGE_Z; z++)
                {
                    gimmickArray[y, x, z] = stage[y, x, z];
                }
            }
        }
    }

    // ★ステージ2★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void Stage2()
    {
        int stageTurnNum = 30;

        int[, ,] stage = new int[,,]
        {
            {
                // １段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10401,  10401,  10401,  10401,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
            {
                // ２段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  11001,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10201,  10101,  10101,  10301,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
            {
                // ３段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
            {
                // ４段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
            {
                // ５段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
        };

        turnNum = stageTurnNum;

        for (int x = 0; x < STAGE_X; x++)
        {
            for (int y = 0; y < STAGE_Y; y++)
            {
                for (int z = 0; z < STAGE_Z; z++)
                {
                    gimmickArray[y, x, z] = stage[y, x, z];
                }
            }
        }
    }

	// ★ステージ3★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void Stage3()
	{
        //int stageTurnNum = 30;

        //int[, ,] stage = new int[,,]
        //{
        //    {
        //        // １段目
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
        //        {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
        //        {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
        //        {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
        //        {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
        //        {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
        //        {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
        //        {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
        //        {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //    },
        //    {
        //        // ２段目
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10201,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  12101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  11001,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10701,  10601,  10801,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10901,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //    },
        //    {
        //        // ３段目
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //    },
        //    {
        //        // ４段目
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //    },
        //    {
        //        // ５段目
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //        {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
        //    },
        //};

        //turnNum = stageTurnNum;

        //for (int x = 0; x < STAGE_X; x++)
        //{
        //    for (int y = 0; y < STAGE_Y; y++)
        //    {
        //        for (int z = 0; z < STAGE_Z; z++)
        //        {
        //            gimmickArray[y, x, z] = stage[y, x, z];
        //        }
        //    }
        //}
        ReadFile();
	}

    // ★ターン数の取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public int getStageTurnNum()
    {
        return turnNum;
    }

    // ★スタート位置の取得★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public Vector3 getStartPosition()
    {
        Vector3 startPosition = new Vector3(0, 0, 0);

        for (int x = 0; x < STAGE_X; x++)
        {
            for (int y = 0; y < STAGE_Y; y++)
            {
                for (int z = 0; z < STAGE_Z; z++)
                {
                    if (gimmickNumArray[y, x, z] == START_POINT)
                    {
                        startPosition = new Vector3(x, y - 0.5f, z);
                    }
                }
            }
        }
        return startPosition;
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
                    if (gimmickNumArray[y, x, z] == START_POINT)
                    {
                        switch (c)
                        {
                            case 'x': arrayPos = x; break;
                            case 'y': arrayPos = y; break;
                            case 'z': arrayPos = z; break;
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
        Vector3 direction = new Vector3(0, 0, 0);

        Vector3 direction1 = new Vector3(0, 0, 0);
        Vector3 direction2 = new Vector3(0, 90, 0);
        Vector3 direction3 = new Vector3(0, 180, 0);
        Vector3 direction4 = new Vector3(0, 270, 0);

        switch (directionPattern)
        {
            case 1: direction = direction1; break;
            case 2: direction = direction2; break;
            case 3: direction = direction3; break;
            case 4: direction = direction4; break;
        }

        return direction;
    }

    // ★移動可能判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public bool MovePossibleDecision(Player alice, Player.MoveDirection direction, PlayerCamera.CameraAngle cameraAngle)
    {
        bool moveDirectionflag = false;
        bool moveDirectionDownflag = false;

        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

        // アリスがステージ外にいる場合
        if((posY == 0) || (posX == 0) || (posX == 10) || (posZ == 0) || (posZ == 10))
        {
            return false;
        }
        else
        {
            // カメラの向きが
            switch(cameraAngle)
            {
                // 前なら
                case PlayerCamera.CameraAngle.FRONT:
                    // 移動方向が
                    switch (direction)
                    {
                        // 前なら
                        case Player.MoveDirection.FRONT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX, posZ - 1], posX, posY, posZ - 1);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ - 1], posX, posY - 1, posZ - 1);
                            break;
                        // 後なら
                        case Player.MoveDirection.BACK:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX, posZ + 1], posX, posY, posZ + 1);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ + 1], posX, posY - 1, posZ + 1);
                            break;
                        // 左なら
                        case Player.MoveDirection.LEFT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX + 1, posZ], posX + 1, posY, posZ);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX + 1, posZ], posX + 1, posY - 1, posZ);
                            break;
                        // 右なら
                        case Player.MoveDirection.RIGHT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX - 1, posZ], posX - 1, posY, posZ);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX - 1, posZ], posX - 1, posY - 1, posZ);
                            break;
                    }
                    break;
                // 後なら
                case PlayerCamera.CameraAngle.BACK:
                    // 移動方向が
                    switch (direction)
                    {
                        // 前なら
                        case Player.MoveDirection.FRONT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX, posZ + 1], posX, posY, posZ + 1);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ + 1], posX, posY - 1, posZ + 1);
                            break;
                        // 後なら
                        case Player.MoveDirection.BACK:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX, posZ - 1], posX, posY, posZ - 1);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ - 1], posX, posY - 1, posZ - 1);
                            break;
                        // 左なら
                        case Player.MoveDirection.LEFT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX - 1, posZ], posX - 1, posY, posZ);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX - 1, posZ], posX - 1, posY - 1, posZ);
                            break;
                        // 右なら
                        case Player.MoveDirection.RIGHT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX + 1, posZ], posX + 1, posY, posZ);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX + 1, posZ], posX + 1, posY - 1, posZ);
                            break;

                    }
                    break;
                // 左なら
                case PlayerCamera.CameraAngle.LEFT:
                    // 移動方向が
                    switch (direction)
                    {
                        // 前なら
                        case Player.MoveDirection.FRONT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX + 1, posZ], posX + 1, posY, posZ);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX + 1, posZ], posX + 1, posY - 1, posZ);
                            break;
                        // 後なら
                        case Player.MoveDirection.BACK:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX - 1, posZ], posX - 1, posY, posZ);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX - 1, posZ], posX - 1, posY - 1, posZ);
                            break;
                        // 左なら
                        case Player.MoveDirection.LEFT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX, posZ + 1], posX, posY, posZ + 1);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ + 1], posX, posY - 1, posZ + 1);
                            break;
                        // 右なら
                        case Player.MoveDirection.RIGHT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX, posZ - 1], posX, posY, posZ - 1);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ - 1], posX, posY - 1, posZ - 1);
                            break;
                    }
                    break;
                // 右なら
                case PlayerCamera.CameraAngle.RIGHT:
                    // 移動方向が
                    switch (direction)
                    {
                        // 前なら
                        case Player.MoveDirection.FRONT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX - 1, posZ], posX - 1, posY, posZ);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX - 1, posZ], posX - 1, posY - 1, posZ);
                            break;
                        // 後なら
                        case Player.MoveDirection.BACK:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX + 1, posZ], posX + 1, posY, posZ);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX + 1, posZ], posX + 1, posY - 1, posZ);
                            break;
                        // 左なら
                        case Player.MoveDirection.LEFT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX, posZ - 1], posX, posY, posZ - 1);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ - 1], posX, posY - 1, posZ - 1);
                            break;
                        // 右なら
                        case Player.MoveDirection.RIGHT:
                            moveDirectionflag = BesideDicision(gimmickNumArray[posY, posX, posZ + 1], posX, posY, posZ + 1);
                            moveDirectionDownflag = BesideDownDicision(gimmickNumArray[posY - 1, posX, posZ + 1], posX, posY - 1, posZ + 1);
                            break;
                    }
                    break;
            }

            // 横と横下が真の時のみ真を返す
            if (moveDirectionflag && moveDirectionDownflag)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    // ★横判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    bool BesideDicision(int gimmick, int posX, int posY, int posZ)
    {
        bool flag = false;

        switch(gimmick)
        {
            // 移動できる
            case NONE_BLOCK:      // 何も無い
            case START_POINT:     // スタート地点
            case STAGE_GOOL:      // ゴール地点
            case IVY_BLOCK: // 蔦ブロック
            case IVY_FRONT: // 蔦（前）
            case IVY_BACK:  // 蔦（後）
            case IVY_LEFT:  // 蔦（左）
            case IVY_RIGHT: // 蔦（右）
                flag = true;
                break;

            // 特定の条件の時は移動可能
            case TREE:
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().movePossibleFlag == true) { flag = true; }
                else { flag = false; }
                break;

            default:
                flag = false;
                break;
        }

        // 移動系ギミックの判定
        for (int num = 0; num < moveGimmickObjectList.Count; num++)
        {
            // 配列の中にギミックがある場合
            if(moveGimmickNumList[num] != NONE_BLOCK)
            {
                if ((posX == (int)moveGimmickObjectList[num].transform.position.x) && (posY == (int)moveGimmickObjectList[num].transform.position.y) && (posZ == (int)moveGimmickObjectList[num].transform.position.z))
                {
                    flag = false;
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
            // 移動できる
            case NONE_BLOCK:      // 何も無い
            case START_POINT:     // スタート地点
            case STAGE_GOOL:      // ゴール地点
            case FOREST_BLOCK_GROUND:     // ブロック
            case IVY_BLOCK: // 蔦ブロック
            case IVY_FRONT: // 蔦（前）
            case IVY_BACK:  // 蔦（後）
            case IVY_LEFT:  // 蔦（左）
            case IVY_RIGHT: // 蔦（右）
                flag = true;
                break;

            // 特定の条件の時は移動可能
            case TREE:
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().growCount <= 2) { flag = true; }
                else { flag = false; }
                break;


        }

        return flag;
    }

    // ★ギミックとの判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void GimmickDecision(Player alice)
    {
        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

        alice.climb1Flag = false;

        switch(gimmickNumArray[posY, posX, posZ])
        {
            case STAGE_GOOL:      // ▼ゴール
                GoalCheck();
                break;
            case IVY_FRONT: // 蔦（前）
                Climb1(Player.PlayerAngle.FRONT);
                break;
            case IVY_BACK:  // 蔦（後）
                Climb1(Player.PlayerAngle.BACK);
                break;
            case IVY_LEFT:  // 蔦（左）
                Climb1(Player.PlayerAngle.LEFT);
                break;
            case IVY_RIGHT: // 蔦（右）
                Climb1(Player.PlayerAngle.RIGHT);
                break;
            case TREE:
                if(gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().growCount == 1)
                {
                    alice.AutoMoveSetting(Player.MoveDirection.UP);
                    print("木の成長");
                }
                break;


        }
    }

    // ★足元との判定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void FootDecision(Player alice)
    {
        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

        alice.climb2Flag = false;

        // アリスが地面についていないなら
        if(posY >= 1)
        {
            switch(gimmickNumArray[posY - 1, posX, posZ])
            {
                // 落下するもの
                case NONE_BLOCK:
                    alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                    print("落下");
                    break;

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

    // ★ゴールしているかチェック★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void GoalCheck()
    {
        print("ゴール到着");
        // ここにゴール処理を書く
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

    // ★一部ギミックの配列上の位置を変更★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void ChangeArrayGimmick()
    {
        for(int x = 0; x < STAGE_X; x++)
        {
            for(int y = 0; y < STAGE_Y; y++)
            {
                for(int z = 0; z < STAGE_Z; z++)
                {
                    // ▽ギミックが
                    switch(gimmickNumArray[y, x, z])
                    {
                        // ▼木（成長段階１）なら
                        case TREE:
                            // 成長段階が２以下なら１つ上の配列を変更
                            if(gimmickObjectArray[y, x, z].GetComponent<Tree>().growCount <= 1){ gimmickNumArray[y + 1, x, z] = NONE_BLOCK; }
                            // 成長段階が３なら１つ上の配列を変更
                            else if(gimmickObjectArray[y, x, z].GetComponent<Tree>().growCount == 2){ gimmickNumArray[y + 1, x, z] = DUMMY_TREE; }
                            break;
                    }
                }
            }
        }
    }
}