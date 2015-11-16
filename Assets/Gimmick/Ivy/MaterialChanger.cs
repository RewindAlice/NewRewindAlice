using UnityEngine;
using System.Collections;

public class MaterialChanger : MonoBehaviour {

    public Material material;
	// Use this for initialization
	void Start () {
        material = GetComponent<Material>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void changeMaterialBrown()
    {

        GetComponent<Renderer>().material.color = new Color(1.0f, 0.7f, 0.3f, 1.0f);
        
    }

    public void changeMaterialOrizinal()
    {
        GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
