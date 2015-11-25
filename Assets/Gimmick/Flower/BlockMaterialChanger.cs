using UnityEngine;
using System.Collections;

public class BlockMaterialChanger : MonoBehaviour {


    public Material[] matelials;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void MatelialChange(int Number)
    {
        GetComponent<MeshRenderer>().material = matelials[Number-1];
    }
}
