using UnityEngine;
using System.Collections;
using System.Collections.Generic;   // 動的配列で使用

using System.IO;    // ファイル入出力
using System.Text;  //

public class Stage : MonoBehaviour
{
    const int STAGE_X = 11; // ステージの横幅
    const int STAGE_Y = 5;  // ステージの高さ
    const int STAGE_Z = 11; // ステージの奥行

    // ステージギミック番号
    const int NONE = 1;     // 何も無い
    const int START = 2;    // スタート地点
    const int GOAL = 3;     // ゴール地点
    const int BLOCK = 4;    // ブロック
    const int ENEMY = 5;    // 敵

    // 蔦ギミック（仮）
    const int IVY_BLOCK = 6;
    const int IVY_FRONT = 7;
    const int IVY_BACK = 8;
    const int IVY_LEFT = 9;
    const int IVY_RIGHT = 10;


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

    const int DARKFOREST_BLOCK_GROUND = 12;    // 暗い森ステージの足場ブロック（前段）

    const int GARDEN_BLOCK_GROUND = 13;        // 暗い森ステージの足場ブロック（1段目）
    const int GARDEN_BLOCK_FLOWER = 14;        // 暗い森ステージの足場ブロック（2段目以降）

    const int STEP_ONE_TREE = 21;   // 高さ1の木
    const int STEP_TWO_TREE = 22;   // 高さ2の木

    //const int IVY_BLOCK = 23;       // 蔦ブロック
    const int IVY_GIMMCIK = 24;     // 蔦ギミック

    const int LADDER_BLOCK = 25;    // 梯子ブロック
    const int LADDER_GIMMCIK = 26;  // 梯子ギミック

    const int MUSHROOM_SMALL = 27;  // キノコ（小さくなる）
    const int MUSHROOM_BIG = 28;    // キノコ（大きくなる）
    const int POTION_SMALL = 29;    // 薬（小さくなる）
    const int POTION_BIG = 30;      // 薬（大きくなる）

    const int DOOR_RED_KEY = 31;    // 赤扉（鍵）
    const int DOOR_RED = 32;        // 赤扉

    const int DOOR_BLUE_KEY = 33;   // 青扉（鍵）
    const int DOOR_BLUE = 34;       // 青扉

    const int DOOR_YELLOW_KEY = 35; // 黄扉（鍵）
    const int DOOR_YELLOW = 36;     // 黄扉

    const int DOOR_GREEN_KEY = 37;  // 緑扉（鍵）
    const int DOOR_GREEN = 38;      // 緑扉

    const int WARP_HOLE_ONE = 41;   // 穴１
    const int WARP_HOLE_TWO = 42;   // 穴２
    const int WARP_HOLE_TRHEE = 43; // 穴３
    const int WARP_HOLE_FOUR = 44;  // 穴４
    const int WARP_HOLE_FIVE = 45;  // 穴５

    const int RED_FLOWER = 46;      // 花１
    const int BLUE_FLOWER = 47;     // 花２
    const int PURPLE_FLOWER = 48;   // 花３
    const int CHESHIRE_CAT = 49;    // チェシャ猫
    const int BRAMBLE = 50;         // 茨
   
    const int TWEEDLEDUM = 51;           //トゥイードルダム
    const int TWEEDLEDEE = 52;           //トゥイードルディ
    const int SOLDIER_HEART_RIGHT = 53;  //ハート兵右回り
//    const int SOLDIER_HEART_LEFT = 54;　 //ハート兵左回り
//    const int SOLDIER_SPADE_RIGHT = 55;　//スペード兵右回り
//    const int SOLDIER_SPADE_LEFT = 56;　 //スペード兵左回り
 //   const int SOLDIER_SPADE_BAF = 57;　　//スペード兵行ったり来たり
    const int ROCK = 58;                 //岩
    const int HAMPTYDUMPTY = 59;         //ハンプティダンプティ




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
    int[, ,] gimmickStartTurnArray = new int[STAGE_Y, STAGE_X, STAGE_Z];            // ステージの配置（開始ターン数）
    List<int> moveGimmickStartTurnList = new List<int>();                           // 移動系ギミック（開始ターン数）
    int turnNum;                                                                    // ステージのターン数

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        
	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {
	
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
            case NONE:  // ▼何も無い//////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickStartTurnArray[y, x, z] = gimmickStartTurn;
                break;
            case START: // ▼スタート地点///////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickStart, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickStartTurnArray[y, x, z] = gimmickStartTurn;
                break;
            case GOAL:  // ▼ゴール地点////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickGoal, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickStartTurnArray[y, x, z] = gimmickStartTurn;
                break;
            case BLOCK: // ▼ブロック///////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickBlock, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickStartTurnArray[y, x, z] = gimmickStartTurn;
                break;
            case ENEMY: // ▼敵////////////////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickStartTurnArray[y, x, z] = 1;
                gimmickNumArray[y, x, z] = NONE;
                moveGimmickObjectList.Add(GameObject.Instantiate(gimmickEnemy, new Vector3(x, y, z), Quaternion.identity) as GameObject);
                moveGimmickStartTurnList.Add(gimmickNumArray[y, x, z] % 100);
                moveGimmickNumList.Add(ENEMY);
                break;
            case IVY_BLOCK: // ▼蔦ブロック/////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickBlock, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickStartTurnArray[y, x, z] = gimmickStartTurn;
                break;
            case IVY_FRONT: // ▼蔦（前）///////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_BACK:  // ▼蔦（後）///////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_LEFT:  // ▼蔦（左）///////////////////////////////////////////////////////////////////////////////////////////////////
            case IVY_RIGHT: // ▼蔦（右）///////////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickNone, new Vector3(x, y, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickStartTurnArray[y, x, z] = gimmickStartTurn;
                break;
            case STEP_ONE_TREE: // ▼木/////////////////////////////////////////////////////////////////////////////////////////////
                gimmickObjectArray[y, x, z] = GameObject.Instantiate(gimmickTree, new Vector3(x, y - 0.5f, z), Quaternion.identity) as GameObject;
                gimmickObjectArray[y, x, z].transform.localEulerAngles = getGimmickDirection(gimmickDirection);
                gimmickNumArray[y, x, z] = gimmickPattern;
                gimmickStartTurnArray[y, x, z] = gimmickStartTurn;
                gimmickObjectArray[y, x, z].GetComponent<Tree>().SetStartActionTurn(gimmickStartTurn);
                break;
        }
    }

    // ★選択されたステージを設定★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void setSelectStage(int stageNum)
    {
        switch (stageNum)
        {
            case 1: Stage1(); break;
            case 2: Stage2(); break;
			case 3: Stage3(); break;
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
		int stageTurnNum = 30;

		int[, ,] stage = new int[,,]
        {
            {
                // １段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
                {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
                {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
                {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
                {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
                {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
                {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
                {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
                {  10101,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401,  10401},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
            },
            {
                // ２段目
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10201,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  12101,  10101,  11001,  10101,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10701,  10601,  10801,  10101,  10101,  10101,  10101,  10101},
                {  10101,  10101,  10101,  10101,  10901,  10101,  10101,  10101,  10101,  10101,  10101},
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
                    if (gimmickNumArray[y, x, z] == START)
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
                    if (gimmickNumArray[y, x, z] == START)
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
            case NONE:      // 何も無い
            case START:     // スタート地点
            case GOAL:      // ゴール地点
            case IVY_BLOCK: // 蔦ブロック
            case IVY_FRONT: // 蔦（前）
            case IVY_BACK:  // 蔦（後）
            case IVY_LEFT:  // 蔦（左）
            case IVY_RIGHT: // 蔦（右）
                flag = true;
                break;

            default:
                flag = false;
                break;
        }

        // 移動系ギミックの判定
        for (int num = 0; num < moveGimmickObjectList.Count; num++)
        {
            // 配列の中にギミックがある場合
            if(moveGimmickNumList[num] != NONE)
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
            case NONE:      // 何も無い
            case START:     // スタート地点
            case GOAL:      // ゴール地点
            case BLOCK:     // ブロック
            case IVY_BLOCK: // 蔦ブロック
            case IVY_FRONT: // 蔦（前）
            case IVY_BACK:  // 蔦（後）
            case IVY_LEFT:  // 蔦（左）
            case IVY_RIGHT: // 蔦（右）
                flag = true;
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
            case GOAL:      // ▼ゴール
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
                case NONE:
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
}