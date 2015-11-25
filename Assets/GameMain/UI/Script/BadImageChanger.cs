using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BadImageChanger : MonoBehaviour {

    //キャラクター
    public Sprite toumei;
    public Sprite peke;
    //イメージ変更用
    public Image image;

    public bool endFlag;


    public GameObject alice;
    public Player player;
	// Use this for initialization
	void Start () {
        //オブジェクトのImageを入手
        image = gameObject.GetComponent<Image>();
        endFlag = false;
        alice = GameObject.Find("Alice");
        player = alice.GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {

        endFlag = player.gameOverFlag;  //調整必要
        
        if(endFlag == true)
        {
            image.sprite = peke;
        }
        else if(endFlag == false)
        {
            image.sprite = toumei;
        }
	
	}
}
