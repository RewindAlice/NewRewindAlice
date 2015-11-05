using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameMain gameMain;       // ゲームメイン
    public PlayerCamera camera;     // カメラ
    public Player alice;            // プレイヤー

    // ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start ()
    {
        // ▼カメラ左回転///////////////////////////////
        gameMain.cameraTurnLeftEvent += camera.TurnLeft;
        
        // ▼カメラ右回転/////////////////////////////////
        gameMain.cameraTurnRightEvent += camera.TurnRight;

        // ▼キー入力によるプレイヤーの移動////////////////////////////////////
        gameMain.inputPlayerMoveEvent += alice.MoveStart;       // 移動開始処理

        // ▼キー入力によるプレイヤーの巻き戻し////////////////////////////////
        gameMain.inputPlayerMoveReturnEvent += alice.MoveStart; // 移動開始処理

        // ▼キー入力によるプレイヤーの早送り//////////////////////////////////
        gameMain.inputPlayerMoveNextEvent += alice.MoveStart;   // 移動開始処理

        // ▼自動移動処理//////////////////////////////////////////////////
        gameMain.autoPlayerMoveEvent += alice.MoveStart;    // 移動開始処理
	}

    // ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update ()
    {
	
	}
}
