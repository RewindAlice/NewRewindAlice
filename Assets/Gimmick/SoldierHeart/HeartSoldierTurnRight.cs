using UnityEngine;
using System.Collections;

public class HeartSoldierTurnRight : BaseGimmick
{
	// ★移動方向★
	public enum MoveDirection
	{
		NONE,
		FRONT,  // 前
		BACK,   // 後
		LEFT,   // 左
		RIGHT,  // 右
		UP,     // 上
		DOWN,   // 下
		STOP,   // 止
	}

	// ★アリスの行動★
	public enum PlayerAction
	{
		NONE,       // 無し
		NEXT,       // 進む
		RETURN,     // 戻る
	}

	//オブジェクト
	public GameObject alice;
	private Player moveScript;
	public GameObject stage;
	private Stage stageScript;
	private GameObject pause;
	private Pause pauseScript;

	//向き
	public int direction;
	public float directionRot;
	//動いたターン数
	public int timeCount;

	//向きの値
	Vector3 enemyAngle1;
	Vector3 enemyAngle2;
	Vector3 enemyAngle3;
	Vector3 enemyAngle4;

	//オブジェクトの座標
	public int arrayPosX;
	public int arrayPosY;
	public int arrayPosZ;

	// 倒れているかどうか
	public bool downFlag;
	public bool returnDownFlag;

	// 倒れたターン	
	public int downTurn;
	public bool moveFlag;
	public int moveTimer;
	public int moveCount = 0;
	public MoveDirection[] moveMemory = new MoveDirection[100]; // 保存用配列(移動方向)
	private MoveDirection moveDirection = 0; // 移動方向
	public GameObject gameMain; // ゲームメイン
	public int turnNum;
	public PlayerAction playerAction; // アリスの行動
	public int[] angleMemory = new int[100]; // 保存用配列(向き)
	public bool captureFlag;

	public bool animationFlag;
	public bool returnAnimationFlag;
	public int animationTimer;

	public bool afterCaptureFlag;

	//------------------------
	//初期化関数
	//------------------------
	void Start()
	{
		//オブジェクトの検索
		pause = GameObject.Find("Pause");
		alice = GameObject.Find("Alice");
		stage = GameObject.Find("Stage");
		pauseScript = pause.GetComponent<Pause>();
		moveScript = alice.GetComponent<Player>();

		//キャラの回転角初期化
		enemyAngle1 = new Vector3(0, 0, 0);
		enemyAngle2 = new Vector3(0, 90, 0);
		enemyAngle3 = new Vector3(0, 180, 0);
		enemyAngle4 = new Vector3(0, 270, 0);

		//経過したターン数
		timeCount = 0;

		// 倒れているかどうか
		downFlag = false;
		returnDownFlag = false;

		// 倒れたターン
		downTurn = 0;

		moveTimer = 0;
		moveFlag = false;
		turnNum = 0;

		for (int i = 0; i < 100; i++)
		{
			moveMemory[i] = MoveDirection.NONE;
			angleMemory[i] = 0;
		}

		animationFlag = false;
		returnAnimationFlag = false;
		animationTimer = 0;

		// 向きの調整
		switch (direction)
		{
			case 1:
				directionRot = 180;
				break;
			case 2:
				directionRot = 90;
				break;
			case 3:
				directionRot = 0;
				break;
			case 4:
				directionRot = 270;
				break;
		}
		transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, directionRot, transform.localEulerAngles.z);

		afterCaptureFlag = false;
	}

	//------------------------
	//座標・向きの初期化関数
	//------------------------
	public void Initialize(int direction, int x, int y, int z)
	{
		//向きの初期化
		this.direction = direction;
		InitChangeDirection();
		//座標の初期化
		arrayPosX = x;
		arrayPosY = y;
		arrayPosZ = z;
	}

	//-----------------
	//アップデート関数
	//-----------------
	void Update()
	{
		if (moveFlag)
			Move();
	}

	public override void OnAliceMoveNext(int aliceMoveTime)
	{
		//(moveScript)プレイヤーの歩数と(timeCount)歩数を比べる
		if (timeCount < aliceMoveTime)
		{
			//アリスを見つけたかのフラグ
			bool flag;

			//アリスを見つけたか判定
			flag = CaptureDecision();

			//アリスが見つかってなかったら向きを変える
			if (flag == false)
			{
				//右回転
				direction -= 1;
				//ターン数を増やす
				timeCount += 1;

				if (direction == 0)
				{
					direction = 4;
				}

				// 倒れていないなら回転させる
				//if(!downFlag)
				//	ChangeDirection();

				// 倒れておらず、アリスを見つけていないなら回転させる
				if ((!downFlag) && (!captureFlag))
					ChangeDirection();

				// アリスの移動数が多かったら
				if (moveCount < aliceMoveTime)
				{
					moveCount++;
				}
			}
			if (!stage.GetComponent<Stage>().CheckAutoMove())
			{
				moveDirection = moveMemory[turnNum];      // 保存されている移動方向を設定		
				playerAction = PlayerAction.NEXT;
				moveFlag = true;
			}
		}
		//回転後、アリスを見つけたか判定
		if (!captureFlag)
		{
			if (CaptureDecision()) { afterCaptureFlag = true; }
		}
	}

	//----------------------------------
	//アリスが時を戻した時に呼ばれる関数
	//----------------------------------
	public override void OnAliceMoveReturn(int aliceMoveTime)
	{
		if (timeCount >= aliceMoveTime)
		{
			direction += 1;
			timeCount -= 1;
			if (direction == 5)
			{
				direction = 1;
			}

			// 倒れていないなら回転させる
			if (!downFlag)
				ChangeDirection();

			//if (captureFlag)
			//{
			//	captureFlag = false;
			//	GetComponent<Animator>().SetBool("captureFlag", captureFlag);
			//}
			if (!stage.GetComponent<Stage>().CheckAutoMove())
			{
				moveDirection = moveMemory[turnNum];      // 保存されている移動方向を設定
				moveFlag = true;
			}
		}
		playerAction = PlayerAction.RETURN;
		if (captureFlag)
		{
			captureFlag = false;
			GetComponent<Animator>().SetBool("captureFlag", captureFlag);
		}
		afterCaptureFlag = false;
	}

	//---------------------------
	//アリスが前にいるか判定処理
	//---------------------------
	public bool CaptureDecision()
	{
		//アリスの位置を取得
		Vector3 playerArray = moveScript.GetArray();

		//if(playerAction == PlayerAction.RETURN)
		//{
		//	captureFlag = false;
		//	return false;
		//}

		if (moveScript.GetInvisible() == false)
		{
			//アリスが前にいるか判定
			switch (direction)
			{
				case 1:
					if ((playerArray.x == arrayPosX) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ + 1))
					{
						moveScript.gameOverFlag = true;
						captureFlag = true;
						return true;
					}
					break;
				case 2:
					if ((playerArray.x == arrayPosX + 1) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ))
					{
						moveScript.gameOverFlag = true;
						captureFlag = true;
						return true;
					}
					break;

				case 3:
					if ((playerArray.x == arrayPosX) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ - 1))
					{
						moveScript.gameOverFlag = true;
						captureFlag = true;
						return true;
					}
					break;

				case 4:
					if ((playerArray.x == arrayPosX - 1) && (playerArray.y == arrayPosY) && (playerArray.z == arrayPosZ))
					{
						moveScript.gameOverFlag = true;
						captureFlag = true;
						return true;
					}
					break;
			}
		}

		return false;
	}

	//---------------------
	//向きの変更をする関数
	//---------------------
	public void ChangeDirection()
	{
		// 倒れていなければ
		if (!downFlag)
		{
			////変数に応じて回転を代入する
			//if (direction == 1)
			//{
			//	this.transform.localEulerAngles = enemyAngle1;
			//}
			//if (direction == 2)
			//{
			//	this.transform.localEulerAngles = enemyAngle2;

			//}
			//if (direction == 3)
			//{
			//	this.transform.localEulerAngles = enemyAngle3;
			//}
			//if (direction == 4)
			//{
			//	this.transform.localEulerAngles = enemyAngle4;
			//}
			GetComponent<Animator>().SetBool("rotationFlag", true);
			Debug.Log("rot");
		}
	}

	public void InitChangeDirection()
	{

		//キャラの回転処理
		enemyAngle1 = new Vector3(0, 0, 0);
		enemyAngle2 = new Vector3(0, 90, 0);
		enemyAngle3 = new Vector3(0, 180, 0);
		enemyAngle4 = new Vector3(0, 270, 0);

		//変数に応じて回転を代入する
		if (direction == 1)
		{
			this.transform.localEulerAngles = enemyAngle1;
		}
		else if (direction == 2)
		{
			direction = 3;
			this.transform.localEulerAngles = enemyAngle3;
		}
		else if (direction == 3)
		{
			direction = 2;
			this.transform.localEulerAngles = enemyAngle2;

		}
		else if (direction == 4)
		{
			this.transform.localEulerAngles = enemyAngle4;
		}
	}
	// ★兵士が押されたときに呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void PushMove(int x, int y, int z, int directionX, int directionZ)
	{
		if (directionZ == -1) { moveMemory[turnNum] = MoveDirection.FRONT; }
		else if (directionZ == 1) { moveMemory[turnNum] = MoveDirection.BACK; }
		else if (directionX == 1) { moveMemory[turnNum] = MoveDirection.LEFT; }
		else if (directionX == -1) { moveMemory[turnNum] = MoveDirection.RIGHT; }

		downTurn = turnNum;
		downFlag = true;
		GetComponent<Animator>().SetBool("downFlag", downFlag);
	}

	// ★自動移動する★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void Move()
	{
		if (downFlag)
		{
			if (playerAction == PlayerAction.NEXT)
			{
				if ((moveMemory[turnNum] == MoveDirection.LEFT) ||
						(moveMemory[turnNum] == MoveDirection.RIGHT) ||
						(moveMemory[turnNum] == MoveDirection.FRONT) ||
						(moveMemory[turnNum] == MoveDirection.BACK)) { transform.Translate(Vector3.forward * 0.01f); }
			}

			if (playerAction == PlayerAction.RETURN)
			{
				if ((moveMemory[turnNum - 1] == MoveDirection.LEFT) ||
						(moveMemory[turnNum - 1] == MoveDirection.RIGHT) ||
						(moveMemory[turnNum - 1] == MoveDirection.FRONT) ||
						(moveMemory[turnNum - 1] == MoveDirection.BACK)) { transform.Translate(Vector3.forward * -0.01f); }
			}
		}
		else if ((!captureFlag) || (afterCaptureFlag))
		{
			if (playerAction == PlayerAction.NEXT)
			{
				if (moveMemory[turnNum] == MoveDirection.NONE)
				{
					directionRot -= 1.767f;
					transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, directionRot, transform.localEulerAngles.z);
				}
			}

			else if (playerAction == PlayerAction.RETURN)
			{
				if (moveMemory[turnNum] == MoveDirection.NONE)
				{
					directionRot += 1.767f;
					transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, directionRot, transform.localEulerAngles.z);
				}
			}
		}

		// 移動終了処理
		if (moveTimer == 50)
		{
			moveFlag = false;
			moveTimer = 0;
			GetComponent<Animator>().SetBool("rotationFlag", false);

			// 兵士の座標を動かす
			if (playerAction == PlayerAction.NEXT)
			{
				if (moveMemory[turnNum] == MoveDirection.FRONT) { arrayPosZ++; }
				else if (moveMemory[turnNum] == MoveDirection.BACK) { arrayPosZ--; }
				else if (moveMemory[turnNum] == MoveDirection.LEFT) { arrayPosX--; }
				else if (moveMemory[turnNum] == MoveDirection.RIGHT) { arrayPosX++; }
				else if (moveMemory[turnNum] == MoveDirection.DOWN) { arrayPosY--; }
			}
			else if (playerAction == PlayerAction.RETURN)
			{
				//if (moveMemory[turnNum - 1] == MoveDirection.LEFT) { arrayPosZ--; }
				//else if (moveMemory[turnNum - 1] == MoveDirection.RIGHT) { arrayPosZ++; }
				//else if (moveMemory[turnNum - 1] == MoveDirection.BACK) { arrayPosX++; }
				//else if (moveMemory[turnNum - 1] == MoveDirection.FRONT) { arrayPosX--; }
				//else if (moveMemory[turnNum - 1] == MoveDirection.DOWN) { arrayPosY++; }

				if (moveMemory[turnNum - 1] == MoveDirection.FRONT) { arrayPosZ--; }
				else if (moveMemory[turnNum - 1] == MoveDirection.BACK) { arrayPosZ++; }
				else if (moveMemory[turnNum - 1] == MoveDirection.LEFT) { arrayPosX++; }
				else if (moveMemory[turnNum - 1] == MoveDirection.RIGHT) { arrayPosX--; }
				else if (moveMemory[turnNum - 1] == MoveDirection.DOWN) { arrayPosY++; }
			}

			// 時間を戻したなら、Stageの方でも兵士を移動させる
			if (playerAction == PlayerAction.RETURN)
			{
				if (moveMemory[turnNum - 1] == MoveDirection.FRONT) { stage.GetComponent<Stage>().GimmickReturn(arrayPosX, arrayPosY, arrayPosZ, 0, 0, -1, 2); }
				if (moveMemory[turnNum - 1] == MoveDirection.BACK) { stage.GetComponent<Stage>().GimmickReturn(arrayPosX, arrayPosY, arrayPosZ, 0, 0, 1, 2); }
				if (moveMemory[turnNum - 1] == MoveDirection.LEFT) { stage.GetComponent<Stage>().GimmickReturn(arrayPosX, arrayPosY, arrayPosZ, 1, 0, 0, 2); }
				if (moveMemory[turnNum - 1] == MoveDirection.RIGHT) { stage.GetComponent<Stage>().GimmickReturn(arrayPosX, arrayPosY, arrayPosZ, -1, 0, 0, 2); }
				if (moveMemory[turnNum - 1] == MoveDirection.UP) { stage.GetComponent<Stage>().GimmickReturn(arrayPosX, arrayPosY, arrayPosZ, 0, 1, 0, 2); }
				if (moveMemory[turnNum - 1] == MoveDirection.DOWN) { stage.GetComponent<Stage>().GimmickReturn(arrayPosX, arrayPosY, arrayPosZ, 0, -1, 0, 2); }

				// 落下、もしくは上昇なら続けて移動
				if ((moveMemory[turnNum - 1] == MoveDirection.DOWN) || (moveMemory[turnNum - 1] == MoveDirection.UP))
				{
					moveFlag = true;
				}
				moveMemory[turnNum - 1] = MoveDirection.NONE;
			}

			// 落下、もしくは上昇なら続けて移動
			if (playerAction == PlayerAction.NEXT)
			{
				if ((moveMemory[turnNum + 1] == MoveDirection.DOWN) || (moveMemory[turnNum + 1] == MoveDirection.UP))
				{
					moveFlag = true;
				}
			}

			// 内部ターンの経過
			if (playerAction == PlayerAction.NEXT) { turnNum++; }
			else if (playerAction == PlayerAction.RETURN)
			{
				turnNum--;
				captureFlag = false;
			}

			if (turnNum <= downTurn)
			{
				downFlag = false;
				downTurn = 100;
				GetComponent<Animator>().SetBool("downFlag", downFlag);
			}

			// 向きの調整
			switch (direction)
			{
				case 1:
					directionRot = 0;
					break;
				case 2:
					directionRot = 90;
					break;
				case 3:
					directionRot = 180;
					break;
				case 4:
					directionRot = 270;
					break;
				//case 1:
				//	directionRot = 180;
				//	break;
				//case 2:
				//	directionRot = 90;
				//	break;
				//case 3:
				//	directionRot = 0;
				//	break;
				//case 4:
				//	directionRot = 270;
				//	break;
			}
		}
		moveTimer++;
	}
	// ★落ちるときに呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void Fall()
	{
		moveMemory[turnNum + 1] = MoveDirection.DOWN;
	}

	// ★アニメーション処理★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void Animation()
	{
		{
			GetComponent<Animator>().SetBool("captureFlag", captureFlag);
		}
	}
}