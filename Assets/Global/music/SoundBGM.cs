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

        if (Singleton<SoundPlayer>.instance.curBgmNull() == false)
        {
            Singleton<SoundPlayer>.instance.update();
        }
    }

}

