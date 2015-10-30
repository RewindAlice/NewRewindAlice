using UnityEngine;

using UnityEngine.UI;
using System.Collections;

public class StageNumberLeft : MonoBehaviour {

    private int stageNumber;

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
	void Start () {
        //イメージを取得
        image = gameObject.GetComponent<Image>();

        //ステージナンバー取得(三ケタで、Story(1)かchalenge(2)か、左側の数字、右側の数字)
        stageNumber = PlayerPrefs.GetInt("STAGE_NUM");
        if (stageNumber == 1)
        {
            stageNumber = 111;
        }
        else if (stageNumber == 2)
        {
            stageNumber = 112;
        }
        else if (stageNumber == 3)
        {
            stageNumber = 113;
        }
        else if (stageNumber == 4)
        {
            stageNumber = 114;
        }
        else if (stageNumber == 5)
        {
            stageNumber = 115;
        }
        else if (stageNumber == 6)
        {
            stageNumber = 121;
        }
        else if (stageNumber == 7)
        {
            stageNumber = 122;
        }
        else if (stageNumber == 8)
        {
            stageNumber = 123;
        }
        else if (stageNumber == 9)
        {
            stageNumber = 124;
        }
        else if (stageNumber == 10)
        {
            stageNumber = 125;
        }
        else if (stageNumber == 11)
        {
            stageNumber = 131;
        }
        else if (stageNumber == 12)
        {
            stageNumber = 132;
        }
        else if (stageNumber == 13)
        {
            stageNumber = 133;
        }
        else if (stageNumber == 14)
        {
            stageNumber = 134;
        }
        else if (stageNumber == 15)
        {
            stageNumber = 135;
        }

        else if (stageNumber == 16)
        {
            stageNumber = 141;
        }
        else if (stageNumber == 17)
        {
            stageNumber = 142;
        }
        else if (stageNumber == 18)
        {
            stageNumber = 143;
        }
        else if (stageNumber == 19)
        {
            stageNumber = 144;
        }
        else if (stageNumber == 20)
        {
            stageNumber = 145;
        }
        else if (stageNumber == 21)
        {
            stageNumber = 151;
        }
        else if (stageNumber == 22)
        {
            stageNumber = 152;
        }
        else if (stageNumber == 23)
        {
            stageNumber = 153;
        }
        else if (stageNumber == 24)
        {
            stageNumber = 154;
        }
        else if (stageNumber == 25)
        {
            stageNumber = 201;
        }


        //stageNumber = 222;

        stageNumber = stageNumber % 100;

        if (stageNumber / 10 == 0)
        {
            image.sprite = zeroSprite;
        }
        else if (stageNumber / 10 == 1)
        {
            image.sprite = oneSprite;
        }
        else if (stageNumber / 10 == 2)
        {
            image.sprite = twoSprite;
        }
        else if (stageNumber / 10 == 3)
        {
            image.sprite = threeSprite;
        }
        else if (stageNumber / 10 == 4)
        {
            image.sprite = fourSprite;
        }
        else if (stageNumber / 10 == 5)
        {
            image.sprite = fiveSprite;
        }
        else if (stageNumber / 10 == 6)
        {
            image.sprite = sixSprite;
        }
        else if (stageNumber / 10 == 7)
        {
            image.sprite = sevenSprite;
        }
        else if (stageNumber / 10 == 8)
        {
            image.sprite = eightSprite;
        }
        else if (stageNumber / 10 == 9)
        {
            image.sprite = nineSprite;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
