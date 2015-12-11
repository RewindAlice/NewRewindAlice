using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EXImage : MonoBehaviour {

    public GameObject GameMain;

    public Sprite exSprite_one;
    public Sprite exSprite_two;
    public Sprite exSprite_three;
    public Sprite exSprite_four;
    public Sprite exSprite_five;
    public Sprite exSprite_six;
    public Sprite exSprite_seven;
    public Sprite exSprite_eight;

    public Sprite exSprite_one_S;
    public Sprite exSprite_two_S;
    public Sprite exSprite_three_S;
    public Sprite exSprite_four_S;
    public Sprite exSprite_five_S;
    public Sprite exSprite_six_S;
    public Sprite exSprite_seven_S;
    public Sprite exSprite_eight_S;


    public bool Android;
	// Use this for initialization
	void Start () {
        GameMain = GameObject.Find("GameMain");
	}
	
	// Update is called once per frame
	void Update () {
	
        if(GameMain.GetComponent<GameMain>().tutorialFlag == true)
        {
            if(!Android)
            {
                if (GameMain.GetComponent<GameMain>().stageNumber == 1)
                {
                    switch (GameMain.GetComponent<GameMain>().tutorialTurn)
                    {
                        case 2:
                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_one;
                            break;
                        case 4:
                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_two;
                            break;
                        case 6:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_three;
                            break;
                        case 7:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_four;
                            break;
                    }
                }
                else if (GameMain.GetComponent<GameMain>().stageNumber == 2)
                {
                    switch (GameMain.GetComponent<GameMain>().tutorialTurn)
                    {
                        case 1:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_five;
                            break;
                        case 3:
                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_six;
                            break;
                        case 4:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_seven;
                            break;
                        case 6:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_eight;
                            break;
                    }
                }
            }
            else if(Android)
            {
                if (GameMain.GetComponent<GameMain>().stageNumber == 1)
                {
                    switch (GameMain.GetComponent<GameMain>().tutorialTurn)
                    {
                        case 2:
                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_one_S;
                            break;
                        case 4:
                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_two_S;
                            break;
                        case 6:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_three_S;
                            break;
                        case 7:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_four_S;
                            break;
                    }
                }
                else if (GameMain.GetComponent<GameMain>().stageNumber == 2)
                {
                    switch (GameMain.GetComponent<GameMain>().tutorialTurn)
                    {
                        case 1:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_five_S;
                            break;
                        case 3:
                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_six_S;
                            break;
                        case 4:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_seven_S;
                            break;
                        case 6:

                            GameObject.Find("EXImage").GetComponent<Image>().sprite = exSprite_eight_S;
                            break;
                    }
                }
            }
           

        }
	}
}
