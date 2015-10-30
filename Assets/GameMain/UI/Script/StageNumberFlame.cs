using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StageNumberFlame : MonoBehaviour {

    private int challengeFlag;

    public Sprite storySprite;
    public Sprite challengeSprite;

    public Image image;
	// Use this for initialization
	void Start () {

        //イメージを取得
        image = gameObject.GetComponent<Image>();
        
        challengeFlag = PlayerPrefs.GetInt("CHALLENGE");

        if(challengeFlag==0)
        {
            image.sprite = storySprite;
        }
        else if (challengeFlag == 0)
        {
            image.sprite = challengeSprite;
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
