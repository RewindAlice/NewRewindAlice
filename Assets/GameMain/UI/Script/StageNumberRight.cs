using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageNumberRight : MonoBehaviour {

    private int stageNumber;
    private int challengeFlag;

    public Sprite zeroSprite;
    public Sprite oneSprite;
    public Sprite twoSprite;
    public Sprite threeSprite;
    public Sprite fourSprite;
    public Sprite fiveSprite;
    public Sprite sixSprite;
    public Sprite sevenSprite;
    public Sprite eightSprite;
    public Sprite nineSprite;
    public Image image;
    // Use this for initialization
    void Start()
    {
        //イメージを取得
        image = gameObject.GetComponent<Image>();

        stageNumber = PlayerPrefs.GetInt("STAGE_NUM");
        challengeFlag = PlayerPrefs.GetInt("CHALLENGE");

        if(challengeFlag == 0)
        {
            if(stageNumber%5 == 1)
            {
                image.sprite = oneSprite;
            }
            else if(stageNumber%5 == 2)
            {
                image.sprite = twoSprite;
            }
            else if (stageNumber % 5 == 3)
            {
                image.sprite = threeSprite;
            }
            else if (stageNumber % 5 == 4)
            {
                image.sprite = fourSprite;
            }
            else if (stageNumber % 5 == 0)
            {
                image.sprite = fiveSprite;
            }
        }
        else if(challengeFlag == 1)
        {
            stageNumber = stageNumber % 10;

            if (stageNumber == 0)
            {
                image.sprite = zeroSprite;
            }
            else if (stageNumber == 1)
            {
                image.sprite = oneSprite;
            }
            else if (stageNumber == 2)
            {
                image.sprite = twoSprite;
            }
            else if (stageNumber == 3)
            {
                image.sprite = threeSprite;
            }
            else if (stageNumber == 4)
            {
                image.sprite = fourSprite;
            }
            else if (stageNumber == 5)
            {
                image.sprite = fiveSprite;
            }
            else if (stageNumber == 6)
            {
                image.sprite = sixSprite;
            }
            else if (stageNumber == 7)
            {
                image.sprite = sevenSprite;
            }
            else if (stageNumber == 8)
            {
                image.sprite = eightSprite;
            }
            else if (stageNumber == 9)
            {
                image.sprite = nineSprite;
            }
        }       
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
