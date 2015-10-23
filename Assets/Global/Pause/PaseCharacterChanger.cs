using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class PaseCharacterChanger : MonoBehaviour {

    public Sprite alice_On;
    public Sprite aliceSmile_On;
    public Sprite aliceAmazing_On;
    
    //イメージ変更用
    public Image image;


    private int charaNum;
	// Use this for initialization
	void Start () {
        charaNum = 0;
        //オブジェクトのImageを入手
        image = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        

        if (charaNum == 0)
        {
            image.sprite = alice_On;
        }
        else if (charaNum == 1)
        {
            image.sprite = aliceSmile_On;
        }
        else if (charaNum == 2)
        {
            image.sprite = aliceAmazing_On;
        }
        else
        {
            image.sprite = alice_On;
        }
	}

    public void CharacterChange()
    {
        charaNum = Random.Range(0, 3);
    }

}
