using UnityEngine;
using System.Collections;


public class SoundBGM : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) == true)
        {
            //SEトリガー
            Debug.Log("");
            ////SEを重ならないようにする
            Singleton<SoundPlayer>.instance.playBGM("Gbgm01", 2.0f,false);
        }
        if (Singleton<SoundPlayer>.instance.curBgmNull() == false)
        {
            Singleton<SoundPlayer>.instance.update();
        }
        if (Input.GetKeyDown(KeyCode.I) == true)
        {
            Singleton<SoundPlayer>.instance.PlaySE("se001");
        }
        if (Input.GetKeyDown(KeyCode.U) == true)
        {
            Singleton<SoundPlayer>.instance.stopBGM(2.0f);
        }
        if (Input.GetKeyDown(KeyCode.Y) == true)
        {
            Singleton<SoundPlayer>.instance.BGMPlayerDelete();
        }
    }

}

