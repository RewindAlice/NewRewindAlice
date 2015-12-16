using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeftStageNum : MonoBehaviour {

    public Sprite oneSprite;
    public Sprite twoSprite;
    public Sprite threeSprite;
    public Sprite fourSprite;
    public Sprite fiveSprite;



    public int stageNumber;
    public Image image;
	// Use this for initialization
	void Start ()
    {

        stageNumber = PlayerPrefs.GetInt("STORY_NUM");
	}
	
	// Update is called once per frame
	void Update () 
    {
        //会話パートプロローグ/エピローグ
        if(stageNumber/10 == 1)
        {
            image.sprite = oneSprite;
        }
        else if( stageNumber/10 == 2)
        {
            image.sprite = twoSprite;
        }
        else if(stageNumber/10 == 3)
        {
            image.sprite = threeSprite;
        }
        else if(stageNumber/10 == 4)
        {
            image.sprite = fourSprite;
        }
        else if(stageNumber /10 == 5)
        {
            image.sprite = fiveSprite;
        }
	
	}
}
