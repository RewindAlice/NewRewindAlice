using UnityEngine;
using System.Collections;

public class BlockMaterialChanger : MonoBehaviour {


    public Material[] matelials;
    public GameObject Effect;
    public GameObject[] HoleEffect;
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


    public void EffectChange(int Number)
    {
        // タッチした画面座標からワールド座標へ変換
        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);

        pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y+1, gameObject.transform.position.z);

        Effect = (GameObject)Instantiate(HoleEffect[Number-1], pos, Quaternion.identity);
    }
}
