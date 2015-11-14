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

    //------------------------------------
    //松村脩平追加部分
    //------------------------------------
    //ハートのトランプ兵のモデル
    public GameObject gimmickSoldierHeartLeft;
    public GameObject gimmickSoldierHeartRight;
    //スペードのトランプ兵のモデル
    public GameObject gimmickSoldierSpadeLeft;
    public GameObject gimmickSoldierSpadeRight;
    public GameObject gimmickSoldierSpadeBAF;
    
    public GameObject gimmickTweedleDum;
    public GameObject gimmickTweedleDee;
    
    public int field;
    GameObject fieldObject;                                                     // ステージ天球のオブジェクト
    

    public GameObject fieldStage1;  // ステージ１の天球
    public GameObject fieldStage2;  // ステージ２の天球
    public GameObject fieldStage3;  // ステージ３の天球
    public GameObject fieldStage4;  // ステージ４の天球
    public GameObject fieldStage5;  // ステージ５の天球



    //----------------------------
   

    //const int GARDEN_BLOCK_GROUND = 13;        // ガーデンステージの足場ブロック（1段目）
    //const int GARDEN_BLOCK_FLOWER = 14;        // ガーデンテージの足場ブロック（2段目以降）

    public GameObject forestBlockGround;           // 森ステージの足場ブロック（１段目）
    public GameObject forestBlockGrass;                // 森ステージの足場ブロック（２段目）
    public GameObject forestBlockAllGrass;                // 森ステージの足場ブロック（３段目）
    
    public GameObject roomBlockFloorBlack;                 // 家ステージの足場ブロック（黒）
    public GameObject roomBlockFloorWhite;                 // 家ステージの足場ブロック（白）
    public GameObject roomBlockBookShelf;                  // 家ステージの足場ブロック（本棚）

    public GameObject redForestBlockGround;     // 森(ステージ4)ステージの足場ブロック（１段目）
    public GameObject redForestBlockGrass;          // 森(ステージ4)ステージの足場ブロック（２段目）
    public GameObject redForestBlockAllGrass;          // 森(ステージ4)ステージの足場ブロック（３段目）
    
    public GameObject darkForestBlockRedGround;               // 暗い森ステージの足場ブロック（茶）
    public GameObject darkForestBlockGreenGround;               // 暗い森ステージの足場ブロック（青）

    public GameObject darkForestBlockRedAllGrass;       // 暗い森ステージの足場ブロック（青）（２段目以降）
    public GameObject darkForestBlockGreenAllGrass;        // 暗い森ステージの足場ブロック（赤）（２段目以降）
 
    public GameObject gardenBlockGrass;           // 庭園ステージの足場ブロック（一段目）
    public GameObject gardenBlockRoseRed;         // 庭園ステージの足場ブロック（赤）（２段目以降）
    public GameObject gardenBlockRoseWhite;       // 庭園ステージの足場ブロック（白）（２段目以降）
    //----------------------------
    //------------------------------------

    int gimmick = 0;            // 配列の数字
    int gimmickPattern = 0;     // ギミックの種類
    int gimmickDirection = 0;   // ギミックの向き
    int gimmickStartTurn = 0;   // ギミックの開始ターン

    int[, ,] gimmickArray = new int[STAGE_Y, STAGE_X, STAGE_Z];                     // ステージの配置（数字）
    public int[, ,] gimmickNumArray = new int[STAGE_Y, STAGE_X, STAGE_Z];                  // ステージの配置（ギミック番号）
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
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickGoal, new Vector3(x, y-0.5f, z), Quaternion.identity) as GameObject;
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
                switch (gimmickPattern)
                {
                    case IVY_FRONT:
                        Vector3 ivyFrontPosition = new Vector3(x, y - 0.4f, z + 0.5f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyFrontPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 180, 0);
                        break;
                    case IVY_BACK:
                        Vector3 ivyBackPosition = new Vector3(x, y - 0.4f, z - 0.5f);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyBackPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 0, 0);
                        break;
                    case IVY_LEFT:
                        Vector3 ivyLeftPosition = new Vector3(x - 0.5f, y - 0.4f, z);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyLeftPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 90, 0);
                        break;
                    case IVY_RIGHT:
                        Vector3 ivyRightPosition = new Vector3(x + 0.5f, y - 0.4f, z);
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickIVY, ivyRightPosition, Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(0, 270, 0);
                        break;
                }
                break;
            case TREE: // ▼木//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickTree, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;  // ギミックのオブジェクトを配列に設定
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);                                     // ギミックを指定された向きに変更
                gimmickNumArray[y, x, z] = gimmickPattern;                                                                                          // ギミックを数字として配列に設定
                gimmickObjectArray[y, x, z].GetComponent<Tree>().SetStartActionTurn(gimmickStartTurn);                                              // ギミックの開始ターンを配列に設定
                break;


            //------------------------------------
            //松村脩平追加部分
            //------------------------------------
            case FOREST_BLOCK_GROUND: // 森ステージの足場ブロック（1段目）
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(forestBlockGround, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case FOREST_BLOCK_GRASS:          // 森ステージの足場ブロック（2段目）
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(forestBlockGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case FOREST_BLOCK_ALLGRASS:       // 森ステージの足場ブロック（3段目以降）
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(forestBlockAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case ROOM_BLOCK_FLOOR:           // 家ステージの足場ブロック（1段目）
                 switch (x)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 9:
                        if (z % 2 == 0)
                        {
                            // 家ステージの足場ブロック（黒）
                            gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorBlack, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                            gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                            gimmickNumArray[y, x, z] = gimmickPattern;
                        }
                        else
                        {
                            // 家ステージの足場ブロック（白）
                            gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                            gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                            gimmickNumArray[y, x, z] = gimmickPattern;
                        }
                        break;
                    case 2:
                    case 4:
                    case 6:
                    case 8:
                    case 10:
                        if (z % 2 == 1)
                        {
                            // 家ステージの足場ブロック（黒）
                            gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorBlack, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                            gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                            gimmickNumArray[y, x, z] = gimmickPattern;
                        }
                        else
                        {
                            // 家ステージの足場ブロック（白）
                            gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockFloorWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                            gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                            gimmickNumArray[y, x, z] = gimmickPattern;
                        }
                        break;
                }
                break;

            case ROOM_BLOCK_BOOKSHELF:        // 家ステージの足場ブロック（2段目）
                // 家ステージの足場ブロック（白）
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(roomBlockBookShelf, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;

            case REDFOREST_BLOCK_GROUND:      // 森ステージの足場ブロック（1段目）
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(redForestBlockGround, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case REDFOREST_BLOCK_GRASS:      // 森ステージの足場ブロック（2段目）
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(redForestBlockGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case REDFOREST_BLOCK_ALLGRASS:   // 森ステージの足場ブロック（3段目以降）
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(redForestBlockAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;
            case DARKFOREST_BLOCK_GROUND:    // 暗い森ステージの足場ブロック（全段）
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0:
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(darkForestBlockRedAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    case 1:
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(darkForestBlockGreenAllGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        gimmickNumArray[y, x, z] = gimmickPattern;
                        break;
                    //case 2:
                    //    gimmickObjectArray[y, x, z] = GameObject.Instantiate(blockInsect3, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                    //    gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                    //    gimmickNumArray[y, x, z] = gimmickPattern;
                        //break;
                }
                break;

            case GARDEN_BLOCK_GROUND:        // ガーデンステージの足場ブロック（1段目）
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gardenBlockGrass, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                break;

            case GARDEN_BLOCK_FLOWER:        // ガーデンテージの足場ブロック（2段目以降）
                switch (UnityEngine.Random.Range(0, 2))
                {
                    case 0:
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gardenBlockRoseRed, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        break;
                    case 1:
                        gimmickObjectArray[y, x, z] = GameObject.Instantiate(gardenBlockRoseWhite, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                        gimmickObjectArray[y, x, z].transform.localEulerAngles = new Vector3(-90.0f, 0, 0);
                        break;
                }
                break;

            case SOLDIER_HEART_RIGHT:
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickSoldierHeartRight, new Vector3(x, y-0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnRight>().Initialize(gimmickDirection, x, y, z);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;

            case SOLDIER_HEART_LEFT:
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickSoldierHeartLeft, new Vector3(x, y-0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].GetComponent<HeartSoldierTurnLeft>().Initialize(gimmickDirection, x, y, z);
                gimmickNumArray[y, x, z] = gimmickPattern;
                break;

            case SOLDIER_SPADE_RIGHT:
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = gimmickPattern;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickSoldierSpadeRight, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<SpadeSoldierTurnRight>().Initialize(gimmickDirection, x, y, z);
                break;

            case SOLDIER_SPADE_LEFT:
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = gimmickPattern;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickSoldierSpadeLeft, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<SpadeSoldierTurnLeft>().Initialize(gimmickDirection, x, y, z);
                break;

            case SOLDIER_SPADE_BAF:
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickSoldierSpadeBAF, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<SpadeSoldierBackAndForth>().Initialize(gimmickDirection, x, y, z);
                break;

            case TWEEDLEDUM:
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickTweedleDum, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<TweedleDum>().Initialize(gimmickDirection, x, y, z);
                break;
            case TWEEDLEDEE:
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickNumArray[y, x, z] = NONE_BLOCK;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickTweedleDee, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject);
                moveGimmickNumList.Add(SOLDIER_HEART_RIGHT);
                moveGimmickObjectList[moveGimmickObjectList.Count - 1].GetComponent<TweedleDee>().Initialize(gimmickDirection, x, y, z);
                break;
            //-----------------------------------------------------------------

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

            //------------------------------
            //松村脩平追加部分
            case 4: Stage3(); break;    
            case 5: Stage3(); break;    
            case 6: Stage3(); break;    
            case 7: Stage3(); break;    
            case 8: Stage3(); break;    
            case 9: Stage3(); break;    
            case 10: Stage3(); break;   
            case 11: Stage3(); break;   
            case 12: Stage3(); break;   
            case 13: Stage3(); break;   
            case 14: Stage3(); break;   
            case 15: Stage3(); break;   
            case 16: Stage3(); break;   
            case 17: Stage3(); break;   
            case 18: Stage3(); break;   
            case 19: Stage3(); break;   
            case 20: Stage3(); break;   
            case 21: Stage3(); break;   
            case 22: Stage3(); break;   
            case 23: Stage3(); break;   
            case 24: Stage3(); break;   
            //--------------------------------
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
            case MUSHROOM_SMALL:  // キノコ（小さくなる）
            case MUSHROOM_BIG:    // キノコ（大きくなる）
            case POTION_SMALL:    // 薬（小さくなる）
            case POTION_BIG:      // 薬（大きくなる）
            case DOOR_RED_KEY:    // 赤扉（鍵）
            case DOOR_BLUE_KEY:   // 青扉（鍵）
            case DOOR_YELLOW_KEY: // 黄扉（鍵）
            case DOOR_GREEN_KEY:  // 緑扉（鍵）
            case WARP_HOLE_ONE:   // 穴１
            case WARP_HOLE_TWO:   // 穴２
            case WARP_HOLE_TRHEE: // 穴３
            case WARP_HOLE_FOUR:  // 穴４
            case WARP_HOLE_FIVE:  // 穴５
            case BRAMBLE:     
            case CHESHIRE_CAT:
                flag = true;
                break;

            case DOOR_BLUE:       // 青扉
            case DOOR_YELLOW:     // 黄扉
            case DOOR_GREEN:      // 緑扉
            case DOOR_RED:        // 赤扉
                
                break;
            case ROCK:
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
                if ((posX == (int)moveGimmickObjectList[num].transform.position.x) && (posY == (int)(moveGimmickObjectList[num].transform.position.y+0.5f)) && (posZ == (int)moveGimmickObjectList[num].transform.position.z))
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
                if (gimmickObjectArray[posY, posX, posZ].GetComponent<Tree>().movePossibleFlag == true) { flag = true; }
                else { flag = false; }
                break;

        }

        
        //アリス
        if ((posX == alice.arrayPosX) &&
            (posY == alice.arrayPosY) &&
            (posZ == alice.arrayPosZ))
        {
             flag = twins;
        }
            
        return flag;
    }

    // 移動系ギミック横下判定//////////////////////////////////////////////////////////
    public bool BesideDownDecision(int posX, int posY, int posZ)
    {
        bool flag = false;
      
        switch (gimmickNumArray[posY - 1, posX, posZ])
        {

            //進行方向下にあるとき通れないオブジェクト
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


                flag = true;
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
            //case HOLE1:
            //case HOLE2:
            //case HOLE3:
            //case HOLE4:
            //case HOLE5:
            //    flag = false;
            //    break;
        }
        return flag;
    }

    //----------------------------------------------------------------------------
}