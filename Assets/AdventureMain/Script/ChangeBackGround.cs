using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.Audio;

public class ChangeBackGround : MonoBehaviour
{

    //一章
    //木陰
    public Sprite Stage11;
    //不思議な森
    public Sprite Stage11_1;
    //家が中心に映った森
    public Sprite Stage12;
    //二章
    //不思議な家の中(家具の大きさが様々かわいらしい綺麗な)
    public Sprite Stage21;
    //森(道がある？)
    public Sprite Stage22;
    //三章
    //森(道がある？)
    public Sprite Stage31;
    //お茶会
    public Sprite Stage32;
    //四章
    //近道みたいな(気が生い茂っているような)
    public Sprite Stage41;
    //イモムシなどが住んでいそうな森
    public Sprite Stage42;
    //いじめ
    public Sprite Stage42_2;
    //五章
    //庭園
    public Sprite Stage51;
    //城が映った庭園
    public Sprite Stage52;
    //城の中庭園
    public Sprite Stage53;
    //木陰
    //public Sprite Forest;


    private int stageNum;
    public Image image;


    public GameObject sentenceTextController;

    private FlagManager flagManager;
   

  

    // Use this for initialization
    void Start()
    {

        image = gameObject.GetComponent<Image>();
        stageNum = PlayerPrefs.GetInt("STORY_NUM");


        
       
        if (stageNum == 11)
        {
            image.sprite = Stage11;
            
        }
        else if (stageNum == 12)
        {
            image.sprite = Stage12;
        }
        else if (stageNum == 21)
        {
            image.sprite = Stage21;
        }
        else if (stageNum == 22)
        {
            image.sprite = Stage22;

        }
        else if (stageNum == 31)
        {
            image.sprite = Stage31;
            
        }
        else if (stageNum == 32)
        {
            image.sprite = Stage32;
        }
        else if (stageNum == 41)
        {
            image.sprite = Stage41;
            
        }
        else if (stageNum == 42)
        {
            image.sprite = Stage42;
            
        }
        else if (stageNum == 51)
        {
            image.sprite = Stage51;
            
        }
        else if (stageNum == 52)
        {
            image.sprite = Stage52;
            
        }
        else if (stageNum == 53)
        {
            image.sprite = Stage53;
            
        }
        else
        {
            image.sprite = Stage11;
        }

        flagManager = sentenceTextController.GetComponent<FlagManager>();
       
    }

    // Update is called once per frame
    void Update()
    {

        if (stageNum == 11 && (flagManager.clickCounter == 9))
        {
            image.sprite = Stage11_1;
        }

    }
}
