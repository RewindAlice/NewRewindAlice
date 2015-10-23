using UnityEngine;
using System.Collections;

public class SEManager : MonoBehaviour {
   
    public AudioClip audioClip1_1;
    public AudioClip audioClip1_2;
    public AudioClip audioClip2_1;
    public AudioClip audioClip2_2;
    public AudioClip audioClip3_1;
    public AudioClip audioClip3_2;
    public AudioClip audioClip4_1;
    public AudioClip audioClip4_2;
    public AudioClip audioClip5_1;
    public AudioClip audioClip5_2;
    public AudioClip audioClip5_3;

    public AudioSource audioSource;


	// Use this for initialization
	void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {

       
       
	}

    public void SEChanger(int stageNum, int clickCount)
    {
        if(stageNum == 11 && clickCount == 8)
        {
            audioSource.clip = audioClip1_1;
            audioSource.Play();
        }
    }

    public void SEStop()
    {
       audioSource.Stop();
        
    }
}
