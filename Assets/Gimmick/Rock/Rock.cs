using UnityEngine;
using System.Collections;

public class Rock : BaseGimmick
{
    const float moves = 0.04f;

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

	public int posX; // 配列上の座標x
	public int posY; // 配列上の座標y
	public int posZ; // 配列上の座標z

	public bool moveFlag;
	public int moveTimer;
	public int moveCount = 0;
	public MoveDirection[] moveMemory = new MoveDirection[100]; // 保存用配列(移動方向)
	private MoveDirection moveDirection = 0; // 移動方向
	public PlayerAction playerAction; // アリスの行動
	public GameObject gameMain; // ゲームメイン
	public GameObject stage; // ステージ
    public GameObject alice;
	public int turnNum;
	public bool fallFlag;

	// ★初期化★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Start()
	{
		gameMain = GameObject.Find("GameMain");
		stage = GameObject.Find("Stage");
        alice = GameObject.Find("Alice");
		moveTimer = 0;
		moveFlag = false;
		turnNum = 0;
		fallFlag = false;

		for(int i = 0; i <100; i++)
		{
			moveMemory[i] = MoveDirection.NONE;
		}
	}

	// ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void Initialize(int x, int y, int z)
	{
		posX = x;
		posY = y;
		posZ = z;
	}

	// ★更新★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	void Update()
	{
		FallCheck();

		Move();
	}

	// ★アリスが進んだ時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public override void OnAliceMoveNext(int aliceMove)
	{
		// ギミックの開始ターンとアリスの移動数が同じになったら
		if (startActionTurn == aliceMove)
		{
			gimmickFlag = true; // ギミックを有効にする
		}

        fallFunction();

		// アリスの移動数が多かったら
		if (moveCount < aliceMove)
		{
			moveCount++;
		}
	}

	// ★アリスが戻った時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public override void OnAliceMoveReturn(int aliceMove)
 	{
		// ギミックの開始ターンがアリスの移動数より大きかったら
		if (startActionTurn > aliceMove)
		{
			gimmickFlag = false;    // ギミックを無効にする
		}
        backs();

		// ギミックが有効なら
		if (gimmickFlag)
		{
			if (moveCount == aliceMove)
			{
				gimmickCount--;
			}
			moveCount--;
		}
	}

	// ★岩が押されたときに呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void PushMove(int x, int y, int z, int directionX, int directionZ)
	{
		Debug.Log("Push");
		if (turnNum > 0)
		{
			if (directionZ == -1)
			{
				moveMemory[turnNum] = MoveDirection.FRONT;
				alice.GetComponent<Player>().SetAnimation(Player.Motion.PUSH_NEXT, true);
			}
			else if (directionZ == 1)
			{
				moveMemory[turnNum] = MoveDirection.BACK;
				alice.GetComponent<Player>().SetAnimation(Player.Motion.PUSH_NEXT, true);
			}
			else if (directionX == 1)
			{
				moveMemory[turnNum] = MoveDirection.LEFT;
				alice.GetComponent<Player>().SetAnimation(Player.Motion.PUSH_NEXT, true);
			}
			else if (directionX == -1)
			{
				moveMemory[turnNum] = MoveDirection.RIGHT;
				alice.GetComponent<Player>().SetAnimation(Player.Motion.PUSH_NEXT, true);
			}
		}
	}

    public void GimmickPushMove(int x, int y, int z, int directionX, int directionZ)
    {
		moveTimer = 0;
		moveFlag = true;
        if (directionZ == -1)
        {
            moveMemory[turnNum] = MoveDirection.FRONT;
 //           alice.GetComponent<Player>().SetAnimation(Player.Motion.PUSH_NEXT, true);
        }
        else if (directionZ == 1)
        {
            moveMemory[turnNum] = MoveDirection.BACK;
//            alice.GetComponent<Player>().SetAnimation(Player.Motion.PUSH_NEXT, true);
        }
        else if (directionX == 1)
        {
            moveMemory[turnNum] = MoveDirection.LEFT;
//            alice.GetComponent<Player>().SetAnimation(Player.Motion.PUSH_NEXT, true);
        }
        else if (directionX == -1)
        {
            moveMemory[turnNum] = MoveDirection.RIGHT;
//            alice.GetComponent<Player>().SetAnimation(Player.Motion.PUSH_NEXT, true);
        }
    }

	// ★自動移動する★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void Move()
	{
		if (moveFlag)
		{
			if(moveTimer >= 0)
			{ 
				if (playerAction == PlayerAction.NEXT)
				{
					if (moveMemory[turnNum] == MoveDirection.FRONT) { transform.Translate(Vector3.down * moves); }
					else if (moveMemory[turnNum] == MoveDirection.BACK) { transform.Translate(Vector3.up * moves); }
					else if (moveMemory[turnNum] == MoveDirection.LEFT) { transform.Translate(Vector3.left * moves); }
					else if (moveMemory[turnNum] == MoveDirection.RIGHT) { transform.Translate(Vector3.right * moves); }
					else if (moveMemory[turnNum] == MoveDirection.DOWN) { transform.Translate(Vector3.back * moves); }
					else if (moveMemory[turnNum] == MoveDirection.UP) { transform.Translate(Vector3.back * -moves); }
				}
				else if (playerAction == PlayerAction.RETURN)
				{
					if (turnNum > 0)
					{
						if (moveMemory[turnNum+1] == MoveDirection.FRONT) { transform.Translate(Vector3.down * -moves); }
						else if (moveMemory[turnNum+1] == MoveDirection.BACK) { transform.Translate(Vector3.up * -moves); }
						else if (moveMemory[turnNum+1] == MoveDirection.LEFT) { transform.Translate(Vector3.left * -moves); }
						else if (moveMemory[turnNum+1] == MoveDirection.RIGHT) { transform.Translate(Vector3.right * -moves); }
						else if (moveMemory[turnNum+1] == MoveDirection.DOWN) { transform.Translate(Vector3.back * -moves); }
						else if (moveMemory[turnNum + 1] == MoveDirection.UP) { transform.Translate(Vector3.back * moves); }
					}
				}
			}
			// 座標移動
			if (moveTimer == 1)
				ArrayMove();

			fallFlag = false;
			moveTimer++;
		}


		// 移動終了処理
		if ((moveTimer == 25) && (moveFlag))
		{
			moveFlag = false;
			moveTimer = 0;
			{
				{
					// 落下、もしくは上昇なら続けて移動
					if ((playerAction == PlayerAction.RETURN) && (turnNum > 0)) 
					{
						if ((moveMemory[turnNum+1] == MoveDirection.DOWN) || (moveMemory[turnNum+1] == MoveDirection.UP))
						{
							moveFlag = true;
							moveTimer = 0;
							fallFlag = false;

							turnNum--;
						}
						//moveMemory[turnNum - 1] = MoveDirection.NONE;
					}
				}
			}
			// 落下、もしくは上昇なら続けて移動
			if ((playerAction == PlayerAction.NEXT) && (turnNum > 0))
			{
				if ((moveMemory[turnNum+1] == MoveDirection.DOWN) || (moveMemory[turnNum+1] == MoveDirection.UP))
				{
					moveFlag = true;
					moveTimer = 0;
					Debug.Log("fall");
					fallFlag = false;
				}
			}
		}
	}
	// ★岩が落ちるときに呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void Fall() 
	{ 
		moveMemory[turnNum + 1] = MoveDirection.DOWN;
		fallFlag = true;
	}

	// ★岩が上がる時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void Rize()
	{
		Debug.Log("Rize");
		moveMemory[turnNum + 1] = MoveDirection.UP;
		fallFlag = true;
	}

    public void fallFunction()
    {

		if (!stage.GetComponent<Stage>().CheckAutoMove())
		{
			if (turnNum > 0)
			{
				moveDirection = moveMemory[turnNum];      // 保存されている移動方向を設定		
				//playerAction = PlayerAction.NEXT;
				//moveFlag = true;
			}
		}
    }

    public void backs()
    {
		if (!stage.GetComponent<Stage>().CheckAutoMove())
		{
			if (turnNum > 0)
			{
				moveDirection = moveMemory[turnNum];      // 保存されている移動方向を設定
				//playerAction = PlayerAction.RETURN;
				//moveFlag = true;
			}
		}
    }

	// ★移動開始時に呼ばれる関数★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
	public void StartMove(int type) 
	{
		Debug.Log("type");
		Debug.Log(type);
		moveFlag = true;
		moveTimer = 0;
		switch(type)
		{
 			case 1:
				playerAction = PlayerAction.NEXT;

				// 現在の行動より後の動きを消去
				for (int i = turnNum+1 ; i < 100; i++)
				{
					moveMemory[i] = MoveDirection.NONE;
				}

				break;
			case 2:
				playerAction = PlayerAction.NEXT;
				break;
			case 3:
				playerAction = PlayerAction.RETURN;
				moveTimer = 0;
				break;
			default:
				Debug.Log("岩おかしい");
				break;
		}
		
				
		// 岩の内部ターンの経過
		if (playerAction == PlayerAction.NEXT) { turnNum++; }
		else if (playerAction == PlayerAction.RETURN) { turnNum--; }
	}

	public void FallCheck()
	{
		if(fallFlag)
		{
			moveFlag = true;
			moveTimer = 0;
			turnNum++;
		}
	}

	// 座標を動かす
	public void ArrayMove()
	{
		// 岩の座標を動かす
		if (playerAction == PlayerAction.NEXT)
		{
			if (moveMemory[turnNum] == MoveDirection.FRONT) { posZ++; }
			else if (moveMemory[turnNum] == MoveDirection.BACK) { posZ--; }
			else if (moveMemory[turnNum] == MoveDirection.LEFT) { posX--; }
			else if (moveMemory[turnNum] == MoveDirection.RIGHT) { posX++; }
			else if (moveMemory[turnNum] == MoveDirection.DOWN) { posY--; }
			else if (moveMemory[turnNum] == MoveDirection.UP) { posY++; }
		}
		else if (playerAction == PlayerAction.RETURN)
		{
			if (turnNum > 0)
			{
				if (moveMemory[turnNum+1] == MoveDirection.FRONT) { posZ--; }
				else if (moveMemory[turnNum+1] == MoveDirection.BACK) { posZ++; }
				else if (moveMemory[turnNum+1] == MoveDirection.LEFT) { posX++; }
				else if (moveMemory[turnNum+1] == MoveDirection.RIGHT) { posX--; }
				else if (moveMemory[turnNum+1] == MoveDirection.DOWN) { posY++; }
				else if (moveMemory[turnNum + 1] == MoveDirection.UP) { posY--; }
			}
		}

		// 時間を戻したなら、Stageの方でも岩を移動させる
		if (playerAction == PlayerAction.RETURN)
		{
			if (turnNum > 0)
			{
				if (moveMemory[turnNum + 1] == MoveDirection.FRONT) { stage.GetComponent<Stage>().GimmickReturn(posX, posY, posZ, 0, 0, -1, 1); }
				if (moveMemory[turnNum + 1] == MoveDirection.BACK) { stage.GetComponent<Stage>().GimmickReturn(posX, posY, posZ, 0, 0, 1, 1); }
				if (moveMemory[turnNum + 1] == MoveDirection.LEFT) { stage.GetComponent<Stage>().GimmickReturn(posX, posY, posZ, 1, 0, 0, 1); }
				if (moveMemory[turnNum + 1] == MoveDirection.RIGHT) { stage.GetComponent<Stage>().GimmickReturn(posX, posY, posZ, -1, 0, 0, 1); }
				if (moveMemory[turnNum + 1] == MoveDirection.UP) { stage.GetComponent<Stage>().GimmickReturn(posX, posY, posZ, 0, 1, 0, 1); }
				if (moveMemory[turnNum + 1] == MoveDirection.DOWN) { stage.GetComponent<Stage>().GimmickReturn(posX, posY, posZ, 0, -1, 0, 1); }
				Debug.Log("RockArrayReturn");
			}
		}
		fallFlag = false;
	}
}
