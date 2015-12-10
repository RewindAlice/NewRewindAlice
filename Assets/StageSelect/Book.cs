using UnityEngine;
using System.Collections;

public class Book : MonoBehaviour {

    private int fadeCount;
	// Use this for initialization
	void Start () {
        fadeCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
        fadeCount++;
        if(fadeCount == 30)
        {
            GetComponent<Animator>().SetBool("start", true);
        }
    }
}
