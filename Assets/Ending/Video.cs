using UnityEngine;
using System.Collections;

public class Video : MonoBehaviour {

    private Renderer renderer;
    //public MovieTexture movie;
    public int count = 0;
    public bool flag = false;

    public bool Android;
    // Use this for initialization
    void Start()
    {
        flag = false;
        CameraFade.StartAlphaFade(Color.black, true, 1.0f, 0.5f);
        renderer = GetComponent<Renderer>();
        //renderer.material.mainTexture = movie as MovieTexture;
        //movie.Play();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Android)
        {
            count++;

            if ((count >= 420) && (flag == false))
            {
                flag = true;
                //movie.Stop();
                CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });
                //Application.LoadLevel("StageSelectScene");
            }
        }
       
    }
}
