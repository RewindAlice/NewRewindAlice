using UnityEngine;

using UnityEngine.UI;
using System.Collections;

public class StageNumberLeft : MonoBehaviour {

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
	void Start () {
        //イメージを取得
        image = gameObject.GetComponent<Image>();

        //ステージナンバー取得(三ケタで、Story(1)かchalenge(2)か、左側の数字、右側の数字)
        stageNumber = PlayerPrefs.GetInt("STAGE_NUM");
        challengeFlag = PlayerPrefs.GetInt("CHALLENGE");

        if(challengeFlag == 0)
        {
            if (stageNumber < 6)
            {
                image.sprite = oneSprite;
            }
            else if (stageNumber < 11)
            {
                image.sprite = twoSprite;
            }
            else if (stageNumber < 16)
            {
                image.sprite = threeSprite;
            }
            else if (stageNumber < 21)
            {
                image.sprite = fourSprite;
            }
            else if (stageNumber < 25)
            {
                image.sprite = fiveSprite;
            }
        }
        else if(challengeFlag > 0)
        {
            if (challengeFlag / 10 == 0)
            {
                image.sprite = zeroSprite;
            }
            else if (challengeFlag / 10 == 1)
            {
                image.sprite = oneSprite;
            }
            else if (challengeFlag / 10 == 2)
            {
                image.sprite = twoSprite;
            }
            else if (challengeFlag / 10 == 3)
            {
                image.sprite = threeSprite;
            }
            else if (challengeFlag / 10 == 4)
            {
                image.sprite = fourSprite;
            }
            else if (challengeFlag / 10 == 5)
            {
                image.sprite = fiveSprite;
            }
            else if (challengeFlag / 10 == 6)
            {
                image.sprite = sixSprite;
            }
            else if (challengeFlag / 10 == 7)
            {
                image.sprite = sevenSprite;
            }
            else if (challengeFlag / 10 == 8)
            {
                image.sprite = eightSprite;
            }
            else if (challengeFlag / 10 == 9)
            {
                image.sprite = nineSprite;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
