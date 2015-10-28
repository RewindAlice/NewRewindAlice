using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoryPause : MonoBehaviour
{

    public bool pauseFlag = false;
    public bool keyFlag;
    public int selectMode = 0;
    public int timer = 0;
    public int selectTimer = 0;
    public bool menuSelectFlag = false;

    const int RETURN_GAME = 0;
    //const int RESTART = 1;
    const int RETURN_SELECT = 1;


    public GameObject backGorund;
    public GameObject backButton;
    //public GameObject returnButton;
    
    public GameObject stageSelectButton;
    public GameObject character;
    public GameObject selecticon;
    private PauseImageManager pauseImageManager1;
    private PauseImageManager pauseImageManager2;
    //private PauseImageManager pauseImageManager3;
    private PauseImageManager pauseImageManager4;
    private PauseImageManager pauseImageManager5;
    private PauseImageManager pauseImageManager6;
    private PaseCharacterChanger changer;
    //テキストの制御
    public GameObject textController;
    private TextController text;
    //キャラ名の制御
    public GameObject nameTextController;
    private NameTextController name;

    void Start()
    {
        keyFlag = false;
        pauseFlag = false;
        pauseImageManager1 = backGorund.GetComponent<PauseImageManager>();
        pauseImageManager2 = backButton.GetComponent<PauseImageManager>();
        //pauseImageManager3 = returnButton.GetComponent<PauseImageManager>();
        pauseImageManager4 = stageSelectButton.GetComponent<PauseImageManager>();
        pauseImageManager5 = character.GetComponent<PauseImageManager>();
        pauseImageManager6 = selecticon.GetComponent<PauseImageManager>();
       
        changer = pauseImageManager5.GetComponent<PaseCharacterChanger>();

        textController = GameObject.Find("SentenceTextController");
        text = textController.GetComponent<TextController>();
        name = nameTextController.GetComponent<NameTextController>();
    }

    void Update()
    {
        
        float HorizontalKeyInput = Input.GetAxis("HorizontalKey");

        float VerticalKeyInput = Input.GetAxis("VerticalKey");
        if (pauseFlag)
        {
            if (menuSelectFlag == false)
            {

                if ((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.Joystick1Button7)) && (selectTimer == 0))
                {
                    
                    pauseImageManager1.GetComponent<Image>().enabled = false;
                    pauseImageManager2.GetComponent<Image>().enabled = false;
                    //pauseImageManager3.GetComponent<Image>().enabled = false;
                    pauseImageManager4.GetComponent<Image>().enabled = false;
                    pauseImageManager5.GetComponent<Image>().enabled = false;
                    pauseImageManager6.GetComponent<Image>().enabled = false;
                    EscapePause();
                    changer.CharacterChange();
                    pauseFlag = false;
                }
            }
            // メニューが選択されている場合
            else if (menuSelectFlag)
            {

                // 一定の時間が経過しているなら、選択中のメニューを返す
                if (selectTimer > 15)
                {
                
                    switch (selectMode)
                    {
                        case RETURN_GAME:
                            pauseImageManager1.GetComponent<Image>().enabled = false;
                            pauseImageManager2.GetComponent<Image>().enabled = false;
                            //pauseImageManager3.GetComponent<Image>().enabled = false;
                            pauseImageManager4.GetComponent<Image>().enabled = false;
                            pauseImageManager5.GetComponent<Image>().enabled = false;
                            pauseImageManager6.GetComponent<Image>().enabled = false;
                            EscapePause();

                            pauseFlag = false;
                            break;
                        //case RESTART:
                        //    //EscapePause();
                        //    EscapePause();
                        //    Application.LoadLevel("GameMainScene");
                        //    //CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("GameMainScene"); });
                        //    break;
                        case RETURN_SELECT:
                            //EscapePause();
                            EscapePause();
                            Application.LoadLevel("StageSelectScene");
                            // CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });
                            // CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });

                            break;
                        default:
                            break;
                    }
                }
                // 経過していないなら、選択してからのタイマーをプラス
                else
                    selectTimer++;
            }

            // メニュー選択がされていない場合
            if (menuSelectFlag == false)
            {
                if ((-0.3f < HorizontalKeyInput) && (HorizontalKeyInput < 0.3f) && (-0.3f < VerticalKeyInput) && (VerticalKeyInput < 0.3f))
                {
                    keyFlag = false;
                }
                if (keyFlag == false)
                {
                    // 上入力でメニューを上に
                    if (((Input.GetKeyDown(KeyCode.UpArrow)) || ((VerticalKeyInput < -0.7f))) && (selectMode > RETURN_GAME))
                    {
                        selectMode--;
                        keyFlag = true;
                    }
                    // 下入力でメニューを下に
                    else if (((Input.GetKeyDown(KeyCode.DownArrow)) || ((VerticalKeyInput > +0.7f))) && (selectMode < RETURN_SELECT))
                    {
                        selectMode++;
                        keyFlag = true;
                    }
                    // 決定キーでメニューを選択
                    else if ((Input.GetKeyDown(KeyCode.Space)) ||
								(Input.GetKeyDown(KeyCode.W)) ||
                                (Input.GetKeyDown(KeyCode.Joystick1Button0)) ||
                                (Input.GetKeyDown(KeyCode.Joystick1Button1)) ||
                                (Input.GetKeyDown(KeyCode.Joystick1Button2)) ||
                                (Input.GetKeyDown(KeyCode.Joystick1Button3)) ||
                                (Input.GetKeyDown(KeyCode.Joystick1Button7)))
                    {
                        menuSelectFlag = true;
                        //SOUND_Play(Sound_PauseMenu);
                    }
                }
            }
            timer++; // タイマーをプラス
        }
        else
        {
            if (((Input.GetKeyDown(KeyCode.Escape)) || (Input.GetKeyDown(KeyCode.Joystick1Button7))))
            {
                text.stopText = true;//ストーリーのテキストを止める
                name.stopText = true;//名前のテキストを止める
                pauseImageManager1.GetComponent<Image>().enabled = true;
                pauseImageManager2.GetComponent<Image>().enabled = true;
                //pauseImageManager3.GetComponent<Image>().enabled = true;
                pauseImageManager4.GetComponent<Image>().enabled = true;
                pauseImageManager5.GetComponent<Image>().enabled = true;
                pauseImageManager6.GetComponent<Image>().enabled = true;
                Initialize();
            }

        }

        if (selectMode == RETURN_GAME)
        {
            pauseImageManager6.transform.localPosition = new Vector3(180, 180, 0);
        }
        //else if (selectMode == RESTART)
        //{
        //    pauseImageManager6.transform.localPosition = new Vector3(80, 0, 0);

        //}
        else if (selectMode == RETURN_SELECT)
        {
            pauseImageManager6.transform.localPosition = new Vector3(180, -100, 0);

        }
    }

    void Initialize()
    {
        pauseFlag = true;
        Time.timeScale = 0;
        selectMode = RETURN_GAME;
        timer = 0;
        selectTimer = 0;
        menuSelectFlag = false;
    }

    void EscapePause()
    {
        pauseFlag = false;
        Time.timeScale = 1;
        timer = 0;
        selectTimer = 0;
        menuSelectFlag = false;
        text.stopText = false;//ストーリーのテキストを動かす
        name.stopText = false;//名前のテキストを動かす;
    }
}
