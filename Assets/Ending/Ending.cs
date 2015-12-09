using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ending : MonoBehaviour {

    public Sprite Ending01;
    public Sprite Ending02;
    public Sprite Ending03;
    public Sprite Ending04;

    public Image image;

    public int count;

    public bool Android;
	// Use this for initialization
	void Start () {

        CameraFade.StartAlphaFade(Color.black, true, 1.0f, 0.5f);
        image = gameObject.GetComponent<Image>();
        count = 0;

        if(!Android)
        {
            gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
    void Update()
    {
        if (Android)
        {
            count++;
            if (count == 45)
            {
                CameraFade.StartAlphaFade(Color.white, false, 0.7f, 0.0f, () => { image.sprite = Ending02; });
            }
            else if (count == 90)
            {
                CameraFade.StartAlphaFade(Color.white, false, 0.7f, 0.0f, () => { image.sprite = Ending03; });
            }

            else if (count == 135)
            {
                CameraFade.StartAlphaFade(Color.white, false, 1.7f, 0.0f, () => { image.sprite = Ending04; });
            }

            if (count == 200)
            {
                CameraFade.StartAlphaFade(Color.black, false, 1.0f, 0.5f, () => { Application.LoadLevel("StageSelectScene"); });
            }
        }
    }
}
