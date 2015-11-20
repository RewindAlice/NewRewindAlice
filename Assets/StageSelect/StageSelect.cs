using UnityEngine;
using System.Collections;

using System.IO;    // ファイル入出力
using System.Text;

public class StageSelect : MonoBehaviour
{
	const int BUTTON_W = 200;
	const int BUTTON_H = 20;

	const int STAMP_NUM = 35;
	const float STAMP_X = -1;
	const float STAMP_Y1 = 2.8f;
	const float STAMP_Y2 = 1.9f;
	const float STAMP_Y3 = 0.9f;
	const float STAMP_Y4 = 0;
	const float STAMP_Y5 = -0.9f;
	const float STAMP_Y6 = -2;
	const float STAMP_Y7 = -3;
	const float STAMP_Z = -5;

    //BGMフェード調整
    const float   BGMFadeTime = 60.0f;
    private float BGMTimer;
    private bool  BGMDeleter;
	public enum STAGE
	{
		STAGE_1,
		STAGE_2,
		STAGE_3,
		STAGE_4,
		STAGE_5,
	}

	public enum Chapter
	{
		CHAPTER_1,
		CHAPTER_2,
		CHAPTER_3,
		CHAPTER_4,
		CHAPTER_5,
		CHAPTER_6,
		CHAPTER_7,
	}

	public STAGE stage;
	public Chapter chapter;
	public bool startFlag;

	public bool resetFlag;

	public GameObject book;
	public GameObject stage1Text;
	public GameObject stage2Text;
	public GameObject stage3Text;
	public GameObject stage4Text;
	public GameObject stage5Text;
	public GameObject stage1Picture;
	public GameObject stage2Picture;
	public GameObject stage3Picture;
	public GameObject stage4Picture;
	public GameObject stage5Picture;

	public GameObject selectIcon;       // セレクトアイコン
	public GameObject stampAlice;       // スタンプ（アリス）
	public GameObject stampRabbit;      // スタンプ（ウサギ）
	public GameObject stampSister;      // スタンプ（姉）
	public GameObject stampCheshire;    // スタンプ（チェシャ猫）
	public GameObject stampHatter;      // スタンプ（帽子屋）
	public GameObject stampCaterpillar; // スタンプ（イモムシ）
	public GameObject stampCardSpade;   // スタンプ（トランプ兵　スペード）
	public GameObject stampCardHeart;   // スタンプ（トランプ兵　ハート）
	public GameObject stampQueen;       // スタンプ（ハートの女王）

	public GameObject icon;
	public GameObject[] stamp = new GameObject[STAMP_NUM];
	public bool[] clearFlag = new bool[STAMP_NUM];

	public int count;
	public bool drawFlag;
	public int drawCount;

	public bool selectFlag;
	public bool keyFlag;

	// 初期化
	void Start()
	{
        BGMTimer = 0;
        BGMDeleter = false;
		selectFlag = false;
		keyFlag = false;
		CameraFade.StartAlphaFade(Color.black, true, 1.0f, 0.5f);

		switch (PlayerPrefs.GetInt("STAGE_SELECT_STAGE_NUM"))
		{
			case 1: stage = STAGE.STAGE_1; break;
			case 2: stage = STAGE.STAGE_2; break;
			case 3: stage = STAGE.STAGE_3; break;
			case 4: stage = STAGE.STAGE_4; break;
			case 5: stage = STAGE.STAGE_5; break;
		}
		chapter = Chapter.CHAPTER_1;
		startFlag = false;

		resetFlag = false;

		count = 0;
		drawCount = 0;
		drawFlag = false;

		// ファイルが存在する
		if (File.Exists("Assets/StageSelect/SaveClearData.txt")) { print("FILE"); }
		ReadFile(); // ファイルの読み込み

		CreateIcon();
		CreateStamp();  // スタンプの生成
        Singleton<SoundPlayer>.instance.playBGM("bgm002", 1.0f);
	}

	// 更新
	void Update()
	{
		count++;
        Singleton<SoundPlayer>.instance.update();
		
		if (count >= 50 && drawFlag == false)
		{
			drawFlag = true;
			book.GetComponent<Animator>().SetBool("FirstPageFlag", true);
		}

		if (drawFlag == true)
		{
			drawCount++;
		}

		if (drawCount > 50)
		{
			DrawStagePicture();
		}

		if (drawCount > 100)
		{
			icon.GetComponent<SpriteRenderer>().enabled = true;
			SetIconPosition();
			DrawStamp();
		}
		else
		{
			icon.GetComponent<SpriteRenderer>().enabled = false;

			for (int num = 0; num < STAMP_NUM; num++)
			{
				stamp[num].GetComponent<SpriteRenderer>().enabled = false;
			}
		}

		SetData();
		ResetData();

		float HorizontalKeyInput = Input.GetAxis("HorizontalKey");
		float VerticalKeyInput = Input.GetAxis("VerticalKey");

		if ((-0.3f < HorizontalKeyInput) && (HorizontalKeyInput < 0.3f) && (-0.3f < VerticalKeyInput) && (VerticalKeyInput < 0.3f))
		{
			keyFlag = false;
		}

		if ((selectFlag == false) && (keyFlag == false) && (drawCount > 100))
		{
			// 矢印左を押したら
			if ((Input.GetKeyDown(KeyCode.LeftArrow)) || ((HorizontalKeyInput < -0.8f) && (-0.5f < VerticalKeyInput) && (VerticalKeyInput < 0.5f)))
			{
				keyFlag = true;
				TurnThePageReturn();
			}
			// 矢印右を押したら
			if ((Input.GetKeyDown(KeyCode.RightArrow)) || ((HorizontalKeyInput > 0.8f) && (-0.5f < VerticalKeyInput) && (VerticalKeyInput < 0.5f)))
			{
				keyFlag = true;
				TurnThePageNext();
			}

			if ((Input.GetKeyDown(KeyCode.UpArrow)) || ((VerticalKeyInput < -0.7f)))
			{
				keyFlag = true;
				SelectChapterUp();
			}

			if ((Input.GetKeyDown(KeyCode.DownArrow)) || ((VerticalKeyInput > 0.7f)))
			{
				keyFlag = true;
				SelectChapterDown();
			}

			if ((Input.GetKeyDown(KeyCode.W)) ||
				(Input.GetKeyDown(KeyCode.Joystick1Button0)) ||
				(Input.GetKeyDown(KeyCode.Joystick1Button1)) ||
				(Input.GetKeyDown(KeyCode.Joystick1Button2)) ||
				(Input.GetKeyDown(KeyCode.Joystick1Button3)) ||
				(Input.GetKeyDown(KeyCode.Joystick1Button7)))
			{
                Singleton<SoundPlayer>.instance.stopBGM(1.0f);
                BGMDeleter = true;
				
			}
            if(BGMDeleter)
            {
                BGMTimer++;
            }
             //BGMが止まったら
           if(BGMTimer> BGMFadeTime)
           {
                Singleton<SoundPlayer>.instance.BGMPlayerDelete();
                JumpSelectStage();
           }
		}
	}
	// ページをめくる（次ページへ）
	void TurnThePageNext()
	{
		if (selectFlag == false)
		{
			switch (stage)
			{
				case STAGE.STAGE_1:
					stage = STAGE.STAGE_2;
					chapter = Chapter.CHAPTER_1;
					drawCount = 0;
					break;
				case STAGE.STAGE_2:
					stage = STAGE.STAGE_3;
					chapter = Chapter.CHAPTER_1;
					drawCount = 0;
					break;
				case STAGE.STAGE_3:
					stage = STAGE.STAGE_4;
					chapter = Chapter.CHAPTER_1;
					drawCount = 0;
					break;
				case STAGE.STAGE_4:
					stage = STAGE.STAGE_5;
					chapter = Chapter.CHAPTER_1;
					drawCount = 0;
					break;
			}
		}
	}

	// ページをめくる（前ページへ）
	void TurnThePageReturn()
	{
		if (selectFlag == false)
		{
			switch (stage)
			{
				case STAGE.STAGE_2:
					stage = STAGE.STAGE_1;
					chapter = Chapter.CHAPTER_1;
					drawCount = 0;
					break;
				case STAGE.STAGE_3:
					stage = STAGE.STAGE_2;
					chapter = Chapter.CHAPTER_1;
					drawCount = 0;
					break;
				case STAGE.STAGE_4:
					stage = STAGE.STAGE_3;
					chapter = Chapter.CHAPTER_1;
					drawCount = 0;
					break;
				case STAGE.STAGE_5:
					stage = STAGE.STAGE_4;
					chapter = Chapter.CHAPTER_1;
					drawCount = 0;
					break;
			}
		}
	}

	// チャプター選択（上）
	void SelectChapterUp()
	{
		switch (chapter)
		{
			case Chapter.CHAPTER_2: chapter = Chapter.CHAPTER_1; break;
			case Chapter.CHAPTER_3: chapter = Chapter.CHAPTER_2; break;
			case Chapter.CHAPTER_4: chapter = Chapter.CHAPTER_3; break;
			case Chapter.CHAPTER_5: chapter = Chapter.CHAPTER_4; break;
			case Chapter.CHAPTER_6: chapter = Chapter.CHAPTER_5; break;
			case Chapter.CHAPTER_7: chapter = Chapter.CHAPTER_6; break;
		}
	}

	// チャプター選択（下）
	void SelectChapterDown()
	{
		switch (chapter)
		{
			case Chapter.CHAPTER_1: chapter = Chapter.CHAPTER_2; break;
			case Chapter.CHAPTER_2: chapter = Chapter.CHAPTER_3; break;
			case Chapter.CHAPTER_3: chapter = Chapter.CHAPTER_4; break;
			case Chapter.CHAPTER_4: chapter = Chapter.CHAPTER_5; break;
			case Chapter.CHAPTER_5: chapter = Chapter.CHAPTER_6; break;
			case Chapter.CHAPTER_6: chapter = Chapter.CHAPTER_7; break;
		}
	}

	// 選択されたステージに移動
	void JumpSelectStage()
	{
      
		    selectFlag = true;
		    switch (stage)
		    {
			    case STAGE.STAGE_1:
				    switch (chapter)
				    {
					    case Chapter.CHAPTER_1: PlayerPrefs.SetInt("STORY_NUM", 11); break;
                        case Chapter.CHAPTER_2: PlayerPrefs.SetInt("STAGE_NUM", 1); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_3: PlayerPrefs.SetInt("STAGE_NUM", 2); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_4: PlayerPrefs.SetInt("STAGE_NUM", 3); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_5: PlayerPrefs.SetInt("STAGE_NUM", 4); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_6: PlayerPrefs.SetInt("STAGE_NUM", 5); PlayerPrefs.SetInt("CHALLENGE", 0); break;
					    case Chapter.CHAPTER_7: PlayerPrefs.SetInt("STORY_NUM", 12); break;
				    }
				    break;
			    case STAGE.STAGE_2:
				    switch (chapter)
				    {
					    case Chapter.CHAPTER_1: PlayerPrefs.SetInt("STORY_NUM", 21); break;
                        case Chapter.CHAPTER_2: PlayerPrefs.SetInt("STAGE_NUM", 6); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_3: PlayerPrefs.SetInt("STAGE_NUM", 7); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_4: PlayerPrefs.SetInt("STAGE_NUM", 8); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_5: PlayerPrefs.SetInt("STAGE_NUM", 9); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_6: PlayerPrefs.SetInt("STAGE_NUM", 10); PlayerPrefs.SetInt("CHALLENGE", 0); break;
					    case Chapter.CHAPTER_7: PlayerPrefs.SetInt("STORY_NUM", 22); break;
				    }
				    break;
			    case STAGE.STAGE_3:
				    switch (chapter)
				    {
					    case Chapter.CHAPTER_1: PlayerPrefs.SetInt("STORY_NUM", 31); break;
                        case Chapter.CHAPTER_2: PlayerPrefs.SetInt("STAGE_NUM", 11); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_3: PlayerPrefs.SetInt("STAGE_NUM", 12); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_4: PlayerPrefs.SetInt("STAGE_NUM", 13); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_5: PlayerPrefs.SetInt("STAGE_NUM", 14); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_6: PlayerPrefs.SetInt("STAGE_NUM", 15); PlayerPrefs.SetInt("CHALLENGE", 0); break;
					    case Chapter.CHAPTER_7: PlayerPrefs.SetInt("STORY_NUM", 32); break;
				    }
				    break;
			    case STAGE.STAGE_4:
				    switch (chapter)
				    {
					    case Chapter.CHAPTER_1: PlayerPrefs.SetInt("STORY_NUM", 41); break;
                        case Chapter.CHAPTER_2: PlayerPrefs.SetInt("STAGE_NUM", 16); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_3: PlayerPrefs.SetInt("STAGE_NUM", 17); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_4: PlayerPrefs.SetInt("STAGE_NUM", 18); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_5: PlayerPrefs.SetInt("STAGE_NUM", 19); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_6: PlayerPrefs.SetInt("STAGE_NUM", 20); PlayerPrefs.SetInt("CHALLENGE", 0); break;
					    case Chapter.CHAPTER_7: PlayerPrefs.SetInt("STORY_NUM", 42); break;
				    }
				    break;
			    case STAGE.STAGE_5:
				    switch (chapter)
				    {
					    case Chapter.CHAPTER_1: PlayerPrefs.SetInt("STORY_NUM", 51); break;
                        case Chapter.CHAPTER_2: PlayerPrefs.SetInt("STAGE_NUM", 21); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_3: PlayerPrefs.SetInt("STAGE_NUM", 22); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_4: PlayerPrefs.SetInt("STAGE_NUM", 23); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_5: PlayerPrefs.SetInt("STAGE_NUM", 24); PlayerPrefs.SetInt("CHALLENGE", 0); break;
                        case Chapter.CHAPTER_6: PlayerPrefs.SetInt("STORY_NUM", 52); break;
					    case Chapter.CHAPTER_7: PlayerPrefs.SetInt("STORY_NUM", 53); break;
				    }
				    break;
		    }

		    switch (chapter)
		    {
			    // ストーリーへ
			    case Chapter.CHAPTER_1:
			    case Chapter.CHAPTER_7:
				    JumpScene();
				    CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StoryMainScene"); });
                    Application.LoadLevel("AdventureMainScene");
				    break;

			    // ゲームへ
			    case Chapter.CHAPTER_2:
			    case Chapter.CHAPTER_3:
			    case Chapter.CHAPTER_4:
			    case Chapter.CHAPTER_5:
			    case Chapter.CHAPTER_6:
				    if ((stage == STAGE.STAGE_5) && (chapter == Chapter.CHAPTER_6))
				    {
					    JumpScene();
                        CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("AdventureMainScene"); });
                       
				    }
                    else if ((stage == STAGE.STAGE_1) && (chapter == Chapter.CHAPTER_2 || chapter == Chapter.CHAPTER_3))
                    {
                        JumpScene();
                        CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("TutorialMainScene"); });
                        
                    }
				    else
				    {
					    JumpScene();
					    CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("GameMainScene"); });
                        Application.LoadLevel("GameMainScene");
				    }
				    break;
		    }
       
	}

	void DrawStagePicture()
	{
		switch (stage)
		{
			case STAGE.STAGE_1:
				stage1Text.GetComponent<Animator>().SetBool("DrawFlag", true);
				stage2Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage1Picture.GetComponent<Animator>().SetBool("DrawFlag", true);
				stage2Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				break;
			case STAGE.STAGE_2:
				stage1Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Text.GetComponent<Animator>().SetBool("DrawFlag", true);
				stage3Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage1Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Picture.GetComponent<Animator>().SetBool("DrawFlag", true);
				stage3Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				break;
			case STAGE.STAGE_3:
				stage1Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Text.GetComponent<Animator>().SetBool("DrawFlag", true);
				stage4Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage1Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Picture.GetComponent<Animator>().SetBool("DrawFlag", true);
				stage4Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				break;
			case STAGE.STAGE_4:
				stage1Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Text.GetComponent<Animator>().SetBool("DrawFlag", true);
				stage5Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage1Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Picture.GetComponent<Animator>().SetBool("DrawFlag", true);
				stage5Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				break;
			case STAGE.STAGE_5:
				stage1Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Text.GetComponent<Animator>().SetBool("DrawFlag", true);
				stage1Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Picture.GetComponent<Animator>().SetBool("DrawFlag", true);
				break;
			default:
				stage1Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Text.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage1Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage2Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage3Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage4Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				stage5Picture.GetComponent<Animator>().SetBool("DrawFlag", false);
				break;
		}
	}

	void CreateIcon()
	{
		icon = GameObject.Instantiate(selectIcon, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		icon.transform.localEulerAngles = new Vector3(0, 90, 0);
	}

	void CreateStamp()
	{
		stamp[0] = GameObject.Instantiate(stampAlice, new Vector3(STAMP_X, STAMP_Y1, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[1] = GameObject.Instantiate(stampSister, new Vector3(STAMP_X, STAMP_Y2, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[2] = GameObject.Instantiate(stampSister, new Vector3(STAMP_X, STAMP_Y3, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[3] = GameObject.Instantiate(stampSister, new Vector3(STAMP_X, STAMP_Y4, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[4] = GameObject.Instantiate(stampSister, new Vector3(STAMP_X, STAMP_Y5, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[5] = GameObject.Instantiate(stampSister, new Vector3(STAMP_X, STAMP_Y6, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[6] = GameObject.Instantiate(stampRabbit, new Vector3(STAMP_X, STAMP_Y7, STAMP_Z), Quaternion.identity) as GameObject;

		stamp[7] = GameObject.Instantiate(stampAlice, new Vector3(STAMP_X, STAMP_Y1, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[8] = GameObject.Instantiate(stampCheshire, new Vector3(STAMP_X, STAMP_Y2, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[9] = GameObject.Instantiate(stampCheshire, new Vector3(STAMP_X, STAMP_Y3, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[10] = GameObject.Instantiate(stampCheshire, new Vector3(STAMP_X, STAMP_Y4, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[11] = GameObject.Instantiate(stampCheshire, new Vector3(STAMP_X, STAMP_Y5, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[12] = GameObject.Instantiate(stampCheshire, new Vector3(STAMP_X, STAMP_Y6, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[13] = GameObject.Instantiate(stampRabbit, new Vector3(STAMP_X, STAMP_Y7, STAMP_Z), Quaternion.identity) as GameObject;

		stamp[14] = GameObject.Instantiate(stampAlice, new Vector3(STAMP_X, STAMP_Y1, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[15] = GameObject.Instantiate(stampHatter, new Vector3(STAMP_X, STAMP_Y2, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[16] = GameObject.Instantiate(stampHatter, new Vector3(STAMP_X, STAMP_Y3, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[17] = GameObject.Instantiate(stampHatter, new Vector3(STAMP_X, STAMP_Y4, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[18] = GameObject.Instantiate(stampHatter, new Vector3(STAMP_X, STAMP_Y5, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[19] = GameObject.Instantiate(stampHatter, new Vector3(STAMP_X, STAMP_Y6, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[20] = GameObject.Instantiate(stampRabbit, new Vector3(STAMP_X, STAMP_Y7, STAMP_Z), Quaternion.identity) as GameObject;

		stamp[21] = GameObject.Instantiate(stampAlice, new Vector3(STAMP_X, STAMP_Y1, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[22] = GameObject.Instantiate(stampCaterpillar, new Vector3(STAMP_X, STAMP_Y2, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[23] = GameObject.Instantiate(stampCaterpillar, new Vector3(STAMP_X, STAMP_Y3, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[24] = GameObject.Instantiate(stampCaterpillar, new Vector3(STAMP_X, STAMP_Y4, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[25] = GameObject.Instantiate(stampCaterpillar, new Vector3(STAMP_X, STAMP_Y5, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[26] = GameObject.Instantiate(stampCaterpillar, new Vector3(STAMP_X, STAMP_Y6, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[27] = GameObject.Instantiate(stampRabbit, new Vector3(STAMP_X, STAMP_Y7, STAMP_Z), Quaternion.identity) as GameObject;

		stamp[28] = GameObject.Instantiate(stampAlice, new Vector3(STAMP_X, STAMP_Y1, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[29] = GameObject.Instantiate(stampCardSpade, new Vector3(STAMP_X, STAMP_Y2, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[30] = GameObject.Instantiate(stampCardSpade, new Vector3(STAMP_X, STAMP_Y3, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[31] = GameObject.Instantiate(stampCardHeart, new Vector3(STAMP_X, STAMP_Y4, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[32] = GameObject.Instantiate(stampCardHeart, new Vector3(STAMP_X, STAMP_Y5, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[33] = GameObject.Instantiate(stampQueen, new Vector3(STAMP_X, STAMP_Y6, STAMP_Z), Quaternion.identity) as GameObject;
		stamp[34] = GameObject.Instantiate(stampAlice, new Vector3(STAMP_X, STAMP_Y7, STAMP_Z), Quaternion.identity) as GameObject;

		for (int num = 0; num < STAMP_NUM; num++)
		{
			stamp[num].transform.localEulerAngles = new Vector3(0, 90, 0);
		}
	}

	void DrawStamp()
	{
		switch (stage)
		{
			case STAGE.STAGE_1:
				for (int num = 0; num < STAMP_NUM; num++)
				{
					if (0 <= num && num <= 6)
					{
						if (clearFlag[num] == true) { stamp[num].GetComponent<SpriteRenderer>().enabled = true; }
						else { stamp[num].GetComponent<SpriteRenderer>().enabled = false; }
					}
					else
					{
						stamp[num].GetComponent<SpriteRenderer>().enabled = false;
					}
				}
				break;
			case STAGE.STAGE_2:
				for (int num = 0; num < STAMP_NUM; num++)
				{
					if (7 <= num && num <= 13)
					{
						if (clearFlag[num] == true) { stamp[num].GetComponent<SpriteRenderer>().enabled = true; }
						else { stamp[num].GetComponent<SpriteRenderer>().enabled = false; }
					}
					else
					{
						stamp[num].GetComponent<SpriteRenderer>().enabled = false;
					}
				}
				break;
			case STAGE.STAGE_3:
				for (int num = 0; num < STAMP_NUM; num++)
				{
					if (14 <= num && num <= 20)
					{
						if (clearFlag[num] == true) { stamp[num].GetComponent<SpriteRenderer>().enabled = true; }
						else { stamp[num].GetComponent<SpriteRenderer>().enabled = false; }
					}
					else
					{
						stamp[num].GetComponent<SpriteRenderer>().enabled = false;
					}
				}
				break;
			case STAGE.STAGE_4:
				for (int num = 0; num < STAMP_NUM; num++)
				{
					if (21 <= num && num <= 27)
					{
						if (clearFlag[num] == true) { stamp[num].GetComponent<SpriteRenderer>().enabled = true; }
						else { stamp[num].GetComponent<SpriteRenderer>().enabled = false; }
					}
					else
					{
						stamp[num].GetComponent<SpriteRenderer>().enabled = false;
					}
				}
				break;
			case STAGE.STAGE_5:
				for (int num = 0; num < STAMP_NUM; num++)
				{
					if (28 <= num && num <= 34)
					{
						if (clearFlag[num] == true) { stamp[num].GetComponent<SpriteRenderer>().enabled = true; }
						else { stamp[num].GetComponent<SpriteRenderer>().enabled = false; }
					}
					else
					{
						stamp[num].GetComponent<SpriteRenderer>().enabled = false;
					}
				}
				break;
		}
	}

	void WriteFile()
	{
		FileStream fStream;
		fStream = new FileStream("Assets/StageSelect/SaveClearData.txt", FileMode.Create, FileAccess.Write);
		Encoding utf8Enc = Encoding.GetEncoding("UTF-8");
		StreamWriter writer = new StreamWriter(fStream, utf8Enc);

		for (int num = 0; num < STAMP_NUM; num++)
		{
			if (clearFlag[num] == true) { writer.WriteLine("○"); }
			else { writer.WriteLine("×"); }
		}
		writer.Close();
	}

	void ReadFile()
	{
		FileStream fStream;
        fStream = new FileStream("Assets/StageSelect/SaveClearData.txt", FileMode.Open, FileAccess.Read);
		StreamReader reader = new StreamReader(fStream);

		if (reader != null)
		{
			for (int num = 0; num < STAMP_NUM; num++)
			{
				string str = reader.ReadLine();

				if (str == "○") { clearFlag[num] = true; }
				else { clearFlag[num] = false; }
			}
			reader.Close();
		}
	}

	void ResetData()
	{
		if (resetFlag == true)
		{
			PlayerPrefs.SetInt("STAMP_NUM", 0);
			resetFlag = false;

			for (int num = 0; num < STAMP_NUM; num++)
			{
				clearFlag[num] = false;
			}
			WriteFile();
		}
	}

	void SetData()
	{
		int num = PlayerPrefs.GetInt("STAMP_NUM");

		if (num != 0)
		{
			clearFlag[num - 1] = true;
			WriteFile();
		}
	}

	void SetIconPosition()
	{
		switch (chapter)
		{
			case Chapter.CHAPTER_1: icon.transform.position = new Vector3(-1, STAMP_Y1, -1.5f); break;
			case Chapter.CHAPTER_2: icon.transform.position = new Vector3(-1, STAMP_Y2, -1.5f); break;
			case Chapter.CHAPTER_3: icon.transform.position = new Vector3(-1, STAMP_Y3, -1.5f); break;
			case Chapter.CHAPTER_4: icon.transform.position = new Vector3(-1, STAMP_Y4, -1.5f); break;
			case Chapter.CHAPTER_5: icon.transform.position = new Vector3(-1, STAMP_Y5, -1.5f); break;
			case Chapter.CHAPTER_6: icon.transform.position = new Vector3(-1, STAMP_Y6, -1.5f); break;
			case Chapter.CHAPTER_7: icon.transform.position = new Vector3(-1, STAMP_Y7, -1.5f); break;
		}
	}

	void JumpScene()
	{
		int stageNum = 0;

		switch (stage)
		{
			case STAGE.STAGE_1: stageNum = 1; break;
			case STAGE.STAGE_2: stageNum = 2; break;
			case STAGE.STAGE_3: stageNum = 3; break;
			case STAGE.STAGE_4: stageNum = 4; break;
			case STAGE.STAGE_5: stageNum = 5; break;
		}

		PlayerPrefs.SetInt("STAGE_SELECT_STAGE_NUM", stageNum);
	}
}

