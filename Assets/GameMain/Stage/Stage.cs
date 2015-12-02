﻿using UnityEngine;
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
    const int WARP_HOLE_TRHEE = 47;             // No.47    穴３
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
    // ★ステージの生成★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void CreateStage()
    {
        //----------------------------------------------
        //松村脩平追加部分
        Vector3 fieldPosition = new Vector3(5, -0.5f, 5);

        switch (field)
        {
            case 1: fieldObject = GameObject.Instantiate(fieldStage1, fieldPosition, Quaternion.identity) as GameObject; break;
            case 2: fieldObject = GameObject.Instantiate(fieldStage2, fieldPosition, Quaternion.identity) as GameObject; break;
            case 3: fieldObject = GameObject.Instantiate(fieldStage3, fieldPosition, Quaternion.identity) as GameObject; break;
            case 4: fieldObject = GameObject.Instantiate(fieldStage4, fieldPosition, Quaternion.identity) as GameObject; break;
            case 5: fieldObject = GameObject.Instantiate(fieldStage5, fieldPosition, Quaternion.identity) as GameObject; break;
        }
        //----------------------------------------------

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
        gimmick = gimmick % 100;            // 配列の数字を百で割った余りを入れるF
        gimmickStartTurn = gimmick;         // ギミックの開始ターンを入れる

        // ▽ギミックの種類が
        switch (gimmickPattern)
        {
            // ▼No.0     透明ブロック/////////////////////////////////////////////////////////////////////////////////////////////////////
            case NONE_BLOCK:///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.1     スタート地点//////////////////////////////////////////////////////////////////////////////////////////////////////
            case START_POINT:///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickStart, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.2     ゴール地点////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case STAGE_GOOL:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickGoal, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.3     水//////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case WATER:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(waterBlock, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.4     森ステージの足場ブロック（1段目）/////////////////////////////////////////////////////////////////////////////////////////////
            case FOREST_BLOCK_GROUND:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(forestBlockGround, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.5     森ステージの足場ブロック（2段目）////////////////////////////////////////////////////////////////////////////////////////////
            case FOREST_BLOCK_GRASS:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(forestBlockGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.6     森ステージの足場ブロック（3段目以降）///////////////////////////////////////////////////////////////////////////////////////////
            case FOREST_BLOCK_ALLGRASS:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(forestBlockAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.7     家ステージの足場ブロック（1段目）///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case ROOM_BLOCK_FLOOR:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (x % 2)
                {
                    case 0:
                        switch (z % 2)
                        {
                            case 0:
                                // 家ステージの足場ブロック（白）/////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                            case 1:
                                // 家ステージの足場ブロック（黒）/////////////////////////////////////////////////////////////////////////////////////////////////////////
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
                                // 家ステージの足場ブロック（黒）/////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorBlack, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                            case 1:
                                // 家ステージの足場ブロック（白）/////////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                        }
                        break;
                }
                break;
            // ▼No.8     家ステージの本棚///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case ROOM_BLOCK_BOOKSHELF:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockBookShelf, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(270, 0, 0);
                gimmickNumArray[y, x, z] = ROCK;
                gimmickObjectArray[y, x, z].GetComponent<Rock>().SetStartActionTurn(gimmickStartTurn);
                gimmickObjectArray[y, x, z].GetComponent<Rock>().Initialize(x, y, z);
                break;
            // ▼No.9     赤い森ステージの足場ブロック（1段目）////////////////////////////////////////////////////////////////////////////////////////////
            case REDFOREST_BLOCK_GROUND:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(redForestBlockGround, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.10    赤い森ステージの足場ブロック（2段目）///////////////////////////////////////////////////////////////////////////////////////////
            case REDFOREST_BLOCK_GRASS:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(redForestBlockGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.11    赤い森ステージの足場ブロック（3段目以降）//////////////////////////////////////////////////////////////////////////////////////////
            case REDFOREST_BLOCK_ALLGRASS:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(redForestBlockAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.12    暗い森ステージの足場ブロック（全段）///////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DARKFOREST_BLOCK_GROUND:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(darkForestBlockRedAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    case 1:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(darkForestBlockGreenAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                }
                break;
            // ▼No.13    庭園ステージの足場ブロック（1段目）//////////////////////////////////////////////////////////////////////////////////////////
            case GARDEN_BLOCK_GROUND://////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gardenBlockGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.14    庭園ステージの足場ブロック（2段目以降）////////////////////////////////////////////////////////////////////////////////////////////////
            case GARDEN_BLOCK_FLOWER:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gardenBlockRoseRed, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    case 1:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gardenBlockRoseWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                }
                break;
            // ▼No.21    蔦ブロック//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_BLOCK:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIvyBlock, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<BlockMaterialChanger>().MatelialChange(field);
                break;
            // ▼No.22~  蔦////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_FRONT: // ▼No.22    蔦（前）/////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_BACK:  // ▼No.23    蔦（後）/////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_LEFT:  // ▼No.24    蔦（左）/////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_RIGHT: // ▼No.25    蔦（右）/////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (gimmickPattern)
                {
                    // ▼No.22    蔦（前）////////////////////////////////////////////////////////////////////////////////////////////////////
                    case IVY_FRONT:///////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 ivyFrontPosition = new Vector3(x, y - 0.4f, z + 0.5f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyFrontPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 180, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.23    蔦（後）///////////////////////////////////////////////////////////////////////////////////////////////////
                    case IVY_BACK:///////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 ivyBackPosition = new Vector3(x, y - 0.4f, z - 0.5f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyBackPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.24    蔦（左）///////////////////////////////////////////////////////////////////////////////////////////////////
                    case IVY_LEFT:///////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 ivyLeftPosition = new Vector3(x - 0.5f, y - 0.4f, z);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyLeftPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 90, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.25    蔦（右）////////////////////////////////////////////////////////////////////////////////////////////////////
                    case IVY_RIGHT:///////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 ivyRightPosition = new Vector3(x + 0.5f, y - 0.4f, z);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyRightPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 270, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                }
                gimmickObjectArray[y, x, z].GetComponent<Ivy>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.26    梯子ブロック////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_BLOCK:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                switch (x % 2)
                {
                    case 0:
                        switch (z % 2)
                        {
                            case 0:// 家ステージの足場ブロック（白）//////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                            case 1:// 家ステージの足場ブロック（黒）//////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorBlack, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                        }
                        break;
                    case 1:
                        switch (z % 2)
                        {
                            case 0:// 家ステージの足場ブロック（黒）//////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorBlack, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                            case 1:// 家ステージの足場ブロック（白）//////////////////////////////////////////////////////////////////////////////////////////////////////
                                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                                gimmickNumArray[y, x, z] = gimmickPattern;
                                break;
                        }
                        break;
                }
                break;
            // ▼N0.27~  梯子///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_FRONT:  // ▼No.27    梯子（前）////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_BACK:   // ▼No.28    梯子（後）////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_LEFT:   // ▼No.29    梯子（左）////////////////////////////////////////////////////////////////////////////////////////////
            case LADDER_RIGHT:  // ▼No.30    梯子（右）////////////////////////////////////////////////////////////////////////////////////////////
                switch (gimmickPattern)
                {
                    // ▼No.27    梯子（前）////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case LADDER_FRONT://////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 LadderFrontPosition = new Vector3(x - 0.9f, y - 0.5f, z + 0.45f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickLadder, LadderFrontPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 180, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.28    梯子（後）///////////////////////////////////////////////////////////////////////////////////////////////////////
                    case LADDER_BACK://////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 LadderBackPosition = new Vector3(x + 0.9f, y - 0.5f, z - 0.45f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickLadder, LadderBackPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.29    梯子（左）///////////////////////////////////////////////////////////////////////////////////////////////////////
                    case LADDER_LEFT://////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 LadderLeftPosition = new Vector3(x - 0.45f, y - 0.5f, z - 0.9f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickLadder, LadderLeftPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 90, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    // ▼No.30    梯子（右）////////////////////////////////////////////////////////////////////////////////////////////////////////
                    case LADDER_RIGHT://////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        Vector3 LadderRightPosition = new Vector3(x + 0.45f, y - 0.5f, z + 0.9f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickLadder, LadderRightPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 270, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                }
                //gimmickObjectArray[y, x, z].GetComponent<Ladder>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.31    木//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case TREE:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickTree, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Tree>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.33    キノコ（小さくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////
            case MUSHROOM_SMALL://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickMushroomSmall, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.34    キノコ（大きくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////
            case MUSHROOM_BIG:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickMushroomBig, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.35    薬（小さくなる）///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case POTION_SMALL:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickPotionSmall, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.36    薬（大きくなる）/////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case POTION_BIG:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickPotionBig, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<ModeChange>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.37    赤扉（鍵）//////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_RED_KEY:////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickKey_Red, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection) + new Vector3(-45, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Key>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.38    赤扉（扉）//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_RED:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickDoor_Red, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Door>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.39    青扉（鍵）///////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_BLUE_KEY:////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickKey_Blue, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection) + new Vector3(-45, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Key>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.40    青扉（扉）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_BLUE:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickDoor_Blue, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Door>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.41    黄扉（鍵）/////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_YELLOW_KEY:////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickKey_Yellow, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection) + new Vector3(-45, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Key>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.42    黄扉（扉）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_YELLOW:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickDoor_Yellow, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Door>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.43    緑扉（鍵）////////////////////////////////////////////////////////////////////////////////////////////ll//////////////
            case DOOR_GREEN_KEY:////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickKey_Green, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection) + new Vector3(-45, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Key>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.44    緑扉（扉）////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case DOOR_GREEN:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickDoor_Green, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Door>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.45~  穴///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case WARP_HOLE_ONE:     // ▼No.45    穴１////////////////////////////////////////////////////////////////////////////////////////////
            case WARP_HOLE_TWO:     // ▼No.46    穴２////////////////////////////////////////////////////////////////////////////////////////////
            case WARP_HOLE_TRHEE:   // ▼No.47    穴３////////////////////////////////////////////////////////////////////////////////////////////
            case WARP_HOLE_FOUR:    // ▼No.48    穴４////////////////////////////////////////////////////////////////////////////////////////////
            case WARP_HOLE_FIVE:    // ▼No.49    穴５////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickHole, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.50    茨/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case BRAMBLE:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickBramble, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.51    花１（赤）///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case RED_FLOWER:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickRedFlower, new Vector3(x, y - 0.2f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Flower3>().changeMaterial(field);
                break;
            // ▼No.52    花２（青）////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case BLUE_FLOWER:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickBuleFlower, new Vector3(x, y - 0.2f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Flower2>().changeMaterial(field);
                break;
            // ▼No.53    花３（紫）//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case PURPLE_FLOWER:///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickPurpleFlower, new Vector3(x, y - 0.2f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Flower1>().changeMaterial(field);
                break;
            // ▼No.54    チェシャ猫////////////////////////////////////////////////////////////////////////////////////////////////////
            case CHESHIRE_CAT://////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(cheshire, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Cheshire>().SetStartActionTurn(gimmickStartTurn);
                break;
            // ▼No.55    トゥイードルダム////////////////////////////////////////////////////////////////////////////////////////////////////////
            case TWEEDLEDUM://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickTweedleDum, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<TweedleDum>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.56    トゥイードルディ////////////////////////////////////////////////////////////////////////////////////////////////////////
            case TWEEDLEDEE://////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickTweedleDee, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<TweedleDee>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.57    ハート兵（右回り）/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_HEART_RIGHT:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickSoldierHeartRight, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnRight>().Initialize(gimmickDirection, x, y, z);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.58    ハート兵（左回り）////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_HEART_LEFT:////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickSoldierHeartLeft, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnLeft>().Initialize(gimmickDirection, x, y, z);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            // ▼No.59    スペード兵（右回り）////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_SPADE_RIGHT://///////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = gimmickPattern;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickSoldierSpadeRight, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<SpadeSoldierTurnRight>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.60    スペード兵（左回り）////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_SPADE_LEFT://////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = gimmickPattern;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickSoldierSpadeLeft, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<SpadeSoldierTurnLeft>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.61    スペード兵（往復）//////////////////////////////////////////////////////////////////////////////////////////////////////
            case SOLDIER_SPADE_BAF:///////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickSoldierSpadeBAF, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<SpadeSoldierBackAndForth>().Initialize(gimmickDirection, x, y, z);
                break;
            // ▼No.62    岩///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case ROCK://///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(rock, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(270, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickObjectArray[y, x, z].GetComponent<Rock>().SetStartActionTurn(gimmickStartTurn);
                gimmickObjectArray[y, x, z].GetComponent<Rock>().Initialize(x, y, z);
                break;
        }
    }

    // ★選択されたステージを設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void setSelectStage(int stageNum)
    {
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
        bool besideDicisionflag = false;        // 横判定の結果
        bool besideDownDicisionflag = false;    // 横下判定の結果

        int posX = alice.arrayPosX; // アリスの配列上の座標Ｘを取得
        int posY = alice.arrayPosY; // アリスの配列上の座標Ｙを取得
        int posZ = alice.arrayPosZ; // アリスの配列上の座標Ｚを取得

        // アリスがステージ外にいる場合//////////////////////////////////////////////
        if ((posY == 0) || (posX == 0) || (posX == 10) || (posZ == 0) || (posZ == 10))
        {
            return false;
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
            // 移動できるギミック
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
            case WARP_HOLE_TRHEE:   // No.47    穴３
            case WARP_HOLE_FOUR:    // No.48    穴４
            case WARP_HOLE_FIVE:    // No.49    穴５
            case BRAMBLE:           // No.50    茨
            case RED_FLOWER:        // No.51    花１（赤）
            case BLUE_FLOWER:       // No.52    花２（青）
            case PURPLE_FLOWER:     // No.53    花３（紫）
            case CHESHIRE_CAT:      // No.54    チェシャ猫  
                flag = true;
                break;

            // 移動できないギミック
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

            // 条件によって変わるギミック
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_BLOCK:     // No.21    蔦ブロック/////////////////////////////////////
            case LADDER_BLOCK:  // No.26    梯子ブロック///////////////////////////////////
                switch (gimmickNumArray[alice.arrayPosY, alice.arrayPosX, alice.arrayPosZ])
                {
                    case IVY_FRONT: // No.22    蔦（前）///////////////////////////////////////////////////////////////////////////
                    case IVY_BACK:  // No.23    蔦（後）///////////////////////////////////////////////////////////////////////////
                    case IVY_LEFT:  // No.24    蔦（左）///////////////////////////////////////////////////////////////////////////
                    case IVY_RIGHT: // No.25    蔦（右）///////////////////////////////////////////////////////////////////////////
                        if (gimmickObjectArray[alice.arrayPosY, alice.arrayPosX, alice.arrayPosZ].GetComponent<Ivy>().getBrownFlag)
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
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            case TREE:  // No.31    木///////////////////////////////////////////////////////////////////////////////
                flag = gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().GetBesideDicisionMovePossibleFlag();
                break;
            /////////////////////////////////////////////////////////////////////////////
            case DOOR_RED: // No.38    赤扉（扉）////////////////////////////////////////
                if (alice.getKeyColor_Red)
                {
                    flag = true;    // 移動できる
                    gimmickObjectArray[posY, posX, posZ].GetComponent<Door>().OpenDoor();
                }
                break;
            /////////////////////////////////////////////////////////////////////////////
            case DOOR_BLUE: // No.40    青扉（扉）///////////////////////////////////////
                if (alice.getKeyColor_Blue)
                {
                    flag = true;    // 移動できる
                    gimmickObjectArray[posY, posX, posZ].GetComponent<Door>().OpenDoor();
                }
                break;
            /////////////////////////////////////////////////////////////////////////////
            case DOOR_YELLOW: // No.42    黄扉（扉）/////////////////////////////////////
                if (alice.getKeyColor_Yellow)
                {
                    flag = true;    // 移動できる
                    gimmickObjectArray[posY, posX, posZ].GetComponent<Door>().OpenDoor();
                }
                break;
            /////////////////////////////////////////////////////////////////////////////
            case DOOR_GREEN: // No.44    緑扉（扉）//////////////////////////////////////
                if (alice.getKeyColor_Green)
                {
                    flag = true;    // 移動できる
                    gimmickObjectArray[posY, posX, posZ].GetComponent<Door>().OpenDoor();
                }
                break;
            //////////////////////////////////////////////////////////////////////////////////////
            case ROCK:  // No.62    岩////////////////////////////////////////////////////////////
                // アリスが大きければ
                if (alice.GetBig())
                {
                    int rockPositionByAliceX = posX - alice.arrayPosX;  // アリスから見た岩の位置X
                    int rockPositionByAliceZ = posZ - alice.arrayPosZ;  // アリスから見た岩の位置Z

                    int pushDirectionX = alice.arrayPosX - posX;    // 押した方向X
                    int pushDirectionZ = alice.arrayPosZ - posZ;    // 押した方向Z
                    Debug.Log(pushDirectionZ);

                    // 押した先にギミックが無ければ移動可能
                    if (RockGimmickDecision(posX, posY, posZ, pushDirectionX, pushDirectionZ))
                    {
                        flag = true;    // 移動できる
                        if (((pushDirectionX == 1) && (pushDirectionZ == 0) && (alice.GetMoveDirection() == 3)) ||
                        ((pushDirectionX == -1) && (pushDirectionZ == 0) && (alice.GetMoveDirection() == 4)) ||
                        ((pushDirectionX == 0) && (pushDirectionZ == 1) && (alice.GetMoveDirection() == 2)) ||
                        ((pushDirectionX == 0) && (pushDirectionZ == -1) && (alice.GetMoveDirection() == 1)))
                        {
                            gimmickObjectArray[posY, posX, posZ].GetComponent<Rock>().PushMove(posX, posY, posZ, pushDirectionX, pushDirectionZ);

                            GameObject objectTemp;
                            objectTemp = gimmickObjectArray[posY, posX - pushDirectionX, posZ - pushDirectionZ];
                            gimmickObjectArray[posY, posX - pushDirectionX, posZ - pushDirectionZ] = gimmickObjectArray[posY, posX, posZ];
                            gimmickObjectArray[posY, posX, posZ] = objectTemp;

                            gimmickNumArray[posY, posX - pushDirectionX, posZ - pushDirectionZ] = ROCK;
                            gimmickNumArray[posY, posX, posZ] = NONE_BLOCK;
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
            // 移動できる
            case NONE_BLOCK:      // 何も無い
            case START_POINT:     // スタート地点
            case STAGE_GOOL:      // ゴール地点
            case FOREST_BLOCK_GROUND:     // ブロック
            case FOREST_BLOCK_GRASS:          // 森ステージの足場ブロック（2段目）
            case FOREST_BLOCK_ALLGRASS:       // 森ステージの足場ブロック（3段目以降）
         
            case ROOM_BLOCK_FLOOR:            // 家ステージの足場ブロック（1段目）
            case ROOM_BLOCK_BOOKSHELF:        // 家ステージの足場ブロック（2段目）
           
            case REDFOREST_BLOCK_GROUND:      // 森ステージの足場ブロック（1段目）
            case REDFOREST_BLOCK_GRASS:      // 森ステージの足場ブロック（2段目）
            case REDFOREST_BLOCK_ALLGRASS:   // 森ステージの足場ブロック（3段目以降）
          
            case DARKFOREST_BLOCK_GROUND:    // 暗い森ステージの足場ブロック（全段）
            
            case GARDEN_BLOCK_GROUND:        // ガーデンステージの足場ブロック（1段目）
            case GARDEN_BLOCK_FLOWER:        // ガーデンテージの足場ブロック（2段目以降）
            case IVY_BLOCK: // 蔦ブロック
            case IVY_FRONT: // 蔦（前）
            case IVY_BACK:  // 蔦（後）
            case IVY_LEFT:  // 蔦（左）
            case IVY_RIGHT: // 蔦（右）

            case LADDER_BLOCK: // 蔦ブロック
            case LADDER_FRONT: // 蔦（前）
            case LADDER_BACK:  // 蔦（後）
            case LADDER_LEFT:  // 蔦（左）
            case LADDER_RIGHT: // 蔦（右）
            case WARP_HOLE_ONE:
            case WARP_HOLE_TWO:
            case WARP_HOLE_TRHEE:
            case WARP_HOLE_FOUR:
            case WARP_HOLE_FIVE:
			case DOOR_RED: // 扉（赤）
			case DOOR_BLUE: // 扉（青）
			case DOOR_YELLOW: // 扉（黄）
			case DOOR_GREEN: // 扉（緑）
			case DOOR_RED_KEY: // 鍵（赤）
			case DOOR_BLUE_KEY: // 鍵（青）
			case DOOR_YELLOW_KEY: // 鍵（黄）
			case DOOR_GREEN_KEY: // 鍵（緑）
			case CHESHIRE_CAT: // チェシャ
            case DUMMY_TREE:        // ▼木（ダミー）
            case MUSHROOM_BIG:      // ▼キノコ（大きくなる）
            case MUSHROOM_SMALL:    // ▼キノコ（小さくなる）
            case POTION_BIG:        // ▼薬（大きくなる）
            case POTION_SMALL:      // ▼薬（小さくなる）
			case ROCK:						// ▼岩
            case WATER:
                flag = true;
                break;

            // 特定の条件の時は移動可能
            case TREE:  // ▼木//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                flag = gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().GetBesideDownDicisionMovePossibleFlag();   // 木の横下判定用移動可能フラグを取得
                break;

            // 花
            case RED_FLOWER:
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<Flower3>().besideDownDicisionMovePossibleFlag == true) { flag = true; }
                else { flag = false; }
                break;
            case BLUE_FLOWER:
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<Flower2>().besideDownDicisionMovePossibleFlag == true) { flag = true; }
                else { flag = false; }
                break;
            case PURPLE_FLOWER:
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<Flower1>().besideDownDicisionMovePossibleFlag == true) { flag = true; }
                else { flag = false; }
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

        switch(gimmickNumArray[posY, posX, posZ])
        {
            case STAGE_GOOL:      // ▼ゴール
                GoalCheck();
                break;
            case IVY_FRONT: // 蔦（前）
            case LADDER_FRONT:
                Climb1(Player.PlayerAngle.FRONT);
                break;
            case IVY_BACK:  // 蔦（後）
            case LADDER_BACK:
                Climb1(Player.PlayerAngle.BACK);
                break;
            case IVY_LEFT:  // 蔦（左）
            case LADDER_LEFT:
                Climb1(Player.PlayerAngle.LEFT);
                break;
            case IVY_RIGHT: // 蔦（右）
            case LADDER_RIGHT://               
            Climb1(Player.PlayerAngle.RIGHT);
                break;
            case TREE:  // ▼木//////////////////////////////////////////////////////////////
                if (action == Player.PlayerAction.NEXT)
                {
                    if (gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().growCount == 1)
                    {
                        alice.AutoMoveSetting(Player.MoveDirection.UP);
                    }
                }
                
                break;
            case MUSHROOM_BIG:  // ▼キノコ（大きくなる）////////////////////////////////////////////////////////////////////////////////////
            case POTION_BIG:    // ▼薬（大きくなる）////////////////////////////////////////////////////////////////////////////////////////
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
            case MUSHROOM_SMALL:    // ▼キノコ（小さくなる）////////////////////////////////////////////////////////////////////////////////
            case POTION_SMALL:      // ▼薬（小さくなる）////////////////////////////////////////////////////////////////////////////////////
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
            // 茨
            case BRAMBLE:
                gimmickObjectArray[posY, posX, posZ].GetComponent<Bramble>().trapFlag = true;
                alice.gameOverFlag = true;
                break;

			case DOOR_RED_KEY: // 鍵（赤）
				alice.GetKey(Player.GetKeyColor.RED);
				gimmickObjectArray[posY, posX, posZ].GetComponent<Key>().GetKey();
				break;
			case DOOR_BLUE_KEY: // 鍵（青）
				alice.GetKey(Player.GetKeyColor.BLUE);
				gimmickObjectArray[posY, posX, posZ].GetComponent<Key>().GetKey();
				break;
			case DOOR_YELLOW_KEY: // 鍵（黄）
				alice.GetKey(Player.GetKeyColor.YELLOW);
				gimmickObjectArray[posY, posX, posZ].GetComponent<Key>().GetKey();
				break;
			case DOOR_GREEN_KEY: // 鍵（緑）
				alice.GetKey(Player.GetKeyColor.GREEN);
				gimmickObjectArray[posY, posX, posZ].GetComponent<Key>().GetKey();
				break;

			case CHESHIRE_CAT: // チェシャ
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

        //alice.climb2Flag = false;

        // アリスが地面についていないなら
        if(posY >= 1)
        {
            switch(gimmickNumArray[posY - 1, posX, posZ])
            {
                // 落下するもの
                case NONE_BLOCK:
                case STAGE_GOOL:
				case DOOR_RED_KEY: // 鍵（赤）
				case DOOR_BLUE_KEY: // 鍵（青）
				case DOOR_YELLOW_KEY: // 鍵（黄）
				case DOOR_GREEN_KEY: // 鍵（緑）
				case CHESHIRE_CAT: // チェシャ
                case WATER:
                    if (action == Player.PlayerAction.NEXT)
                    {
                        alice.AutoMoveSetting(Player.MoveDirection.DOWN);
                        alice.SetAnimation(Player.Motion.DROP_NEXT, true);
                        print("落下");
                    }
                   
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
                case TREE:  // ▼木//////////////////////////////////////////////////////////////////
                    if (action == Player.PlayerAction.NEXT)
                    {
                        // 木の成長段階が１以下なら
                        if (gimmickObjectArray[posY - 1, posX, posZ].GetComponent<Tree>().growCount == 2)
                        {
                            alice.AutoMoveSetting(Player.MoveDirection.UP);
                        }
                    }
                    break;
                case WARP_HOLE_ONE:
                case WARP_HOLE_TWO:
                case WARP_HOLE_TRHEE:
                case WARP_HOLE_FOUR:
                case WARP_HOLE_FIVE:

                    if(action == Player.PlayerAction.NEXT)
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
                                        case WARP_HOLE_TRHEE:
                                            if (alice.playerMode == Player.PlayerMode.SMALL)
                                            {
                                                if (gimmickNumArray[y, x, z] == WARP_HOLE_TRHEE && ((y != posY - 1) || (x != posX) || (z != posZ)))
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

    public void IvytDecision(Player alice)
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

        GameObject.Find("Camera").GetComponent<PlayerCamera>().clearFlag = true;
        //switch (PlayerPrefs.GetInt("STAGE_NUM"))
        //{
        //    case 1: PlayerPrefs.SetInt("STAMP_NUM", 2); break;
        //    case 2: PlayerPrefs.SetInt("STAMP_NUM", 3); break;
        //    case 3: PlayerPrefs.SetInt("STAMP_NUM", 4); break;
        //    case 4: PlayerPrefs.SetInt("STAMP_NUM", 5); break;
        //    case 5: PlayerPrefs.SetInt("STAMP_NUM", 6); break;
        //    case 6: PlayerPrefs.SetInt("STAMP_NUM", 9); break;
        //    case 7: PlayerPrefs.SetInt("STAMP_NUM", 10); break;
        //    case 8: PlayerPrefs.SetInt("STAMP_NUM", 11); break;
        //    case 9: PlayerPrefs.SetInt("STAMP_NUM", 12); break;
        //    case 10: PlayerPrefs.SetInt("STAMP_NUM", 13); break;
        //    case 11: PlayerPrefs.SetInt("STAMP_NUM", 16); break;
        //    case 12: PlayerPrefs.SetInt("STAMP_NUM", 17); break;
        //    case 13: PlayerPrefs.SetInt("STAMP_NUM", 18); break;
        //    case 14: PlayerPrefs.SetInt("STAMP_NUM", 19); break;
        //    case 15: PlayerPrefs.SetInt("STAMP_NUM", 20); break;
        //    case 16: PlayerPrefs.SetInt("STAMP_NUM", 23); break;
        //    case 17: PlayerPrefs.SetInt("STAMP_NUM", 24); break;
        //    case 18: PlayerPrefs.SetInt("STAMP_NUM", 25); break;
        //    case 19: PlayerPrefs.SetInt("STAMP_NUM", 26); break;
        //    case 20: PlayerPrefs.SetInt("STAMP_NUM", 27); break;
        //    case 21: PlayerPrefs.SetInt("STAMP_NUM", 30); break;
        //    case 22: PlayerPrefs.SetInt("STAMP_NUM", 31); break;
        //    case 23: PlayerPrefs.SetInt("STAMP_NUM", 32); break;
        //    case 24: PlayerPrefs.SetInt("STAMP_NUM", 33); break;
        //}
        //CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });
                
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
                            // 成長段階が３なら１つ上の配列を変更
                            else if (gimmickObjectArray[y, x, z].GetComponent<Tree>().growCount == 2)
                            {
                                gimmickNumArray[y + 1, x, z] = DUMMY_TREE;
                            }
                            break;

						// ▼岩なら
						case ROCK:
							if (y > 0)
							{
								if (gimmickNumArray[y - 1, x, z] == NONE_BLOCK)
								{
									gimmickObjectArray[y, x, z].GetComponent<Rock>().Fall();
									GameObject objectTemp;
									objectTemp = gimmickObjectArray[y - 1, x, z];
									gimmickObjectArray[y - 1, x, z] = gimmickObjectArray[y, x, z];
									gimmickObjectArray[y, x, z] = objectTemp;

									gimmickNumArray[y - 1, x, z] = ROCK;
									gimmickNumArray[y, x, z] = NONE_BLOCK;

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
                            break;

						// ▼岩なら
						case ROCK:
							if (y > 0)
							{
								if (gimmickNumArray[y - 1, x, z] == NONE_BLOCK)
								{
									gimmickObjectArray[y, x, z].GetComponent<Rock>().Fall();
									GameObject objectTemp;
									objectTemp = gimmickObjectArray[y - 1, x, z];
									gimmickObjectArray[y - 1, x, z] = gimmickObjectArray[y, x, z];
									gimmickObjectArray[y, x, z] = objectTemp;

									gimmickNumArray[y - 1, x, z] = ROCK;
									gimmickNumArray[y, x, z] = NONE_BLOCK;
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
            case NONE_BLOCK:      // 何も無し
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

            //case NONE:              // 何も無し
            //case WATER:             // 水
            //case WARP_HOLE_ONE:
            //case WARP_HOLE_TWO:
            //case WARP_HOLE_THREE:
            //case WARP_HOLE_FOUR:
            //case WARP_HOLE_FIVE:
            //    flag = false;
            //    break;
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
                (gimmickNumArray[posY - 1, posX, posZ] == WARP_HOLE_TRHEE) || (gimmickNumArray[posY - 1, posX, posZ] == WARP_HOLE_FOUR) ||
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
			switch (gimmickNumArray[posY, posX - pushDirectionX, posZ - pushDirectionZ])
			{
				case NONE_BLOCK:
				case START_POINT:
				case STAGE_GOOL:
					return true;

				default: break;
			}
		}
		return false;
	}

	// ★岩の巻き戻し処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void RockReturn(int posX, int posY, int posZ, int moveDirectionX, int moveDirectionY, int moveDirectionZ)
	{
		Debug.Log("MODOTTAYO");
		Debug.Log(posX);
		Debug.Log(posY);
		Debug.Log(posZ);
		GameObject objectTemp;
		objectTemp = gimmickObjectArray[posY + moveDirectionY, posX - moveDirectionX, posZ - moveDirectionZ];
		gimmickObjectArray[posY + moveDirectionY, posX - moveDirectionX, posZ - moveDirectionZ] = gimmickObjectArray[posY, posX, posZ];
		gimmickObjectArray[posY, posX, posZ] = objectTemp;

		gimmickNumArray[posY + moveDirectionY, posX - moveDirectionX, posZ - moveDirectionZ] = NONE_BLOCK;
		gimmickNumArray[posY, posX, posZ] = ROCK;
	}

	// ★アリスが自動移動しているか確認★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public bool CheckAutoMove()
	{
		return alice.GetAutoMove();
	}
    //----------------------------------------------------------------------------
}