using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageNumberFlame : MonoBehaviour {

    private int stageNumber;

    public Sprite storySprite;
    public Sprite challengeSprite;

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

        if(stageNumber/100 ==1)
        {
            image.sprite = storySprite;
        }
        else if(stageNumber / 100 == 2)
        {
            image.sprite = challengeSprite;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
