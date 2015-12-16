using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RightStageNumber : MonoBehaviour {

    public Sprite zeroSprite;
    public Sprite oneSprite;
    public Sprite sixSprite;
    public Sprite sevenSprite;
    public Image image;
    public int stageNumber;
	// Use this for initialization
	void Start () {
        stageNumber = PlayerPrefs.GetInt("STORY_NUM");

	}
	
	// Update is called once per frame
	void Update () 
    {
        if(stageNumber % 10 == 1)
        {
            image.sprite = zeroSprite;
        }
        else if(stageNumber% 10 == 2)
        {
            image.sprite = sixSprite;
        }
        else if(stageNumber % 10 == 3)
        {
            image.sprite = sevenSprite;
        }
	}
}
