using UnityEngine;
using System.Collections;

public class MaterialChanger : MonoBehaviour {

    public Material material;

    private Renderer renderer;
    public bool drawFlag;       // ギミックの表示フラグ

	// Use this for initialization
	void Start ()
    {
        material = GetComponent<Material>();

        renderer = GetComponent<Renderer>();
        drawFlag = true;                        // ギミックの初期表示フラグを真に
	}
	
	// Update is called once per frame
	void Update ()
    {
        // 描画フラグが
        if (drawFlag)
        {
            // 真なら
            renderer.enabled = true;    // ギミックを表示
        }
        else
        {
            // 偽なら
            renderer.enabled = false;   // ギミックを非表示
        }
	}

    // 初期状態
    public void changeMaterialOrizinal()
    {
        GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        drawFlag = true;
    }

    // 枯れた状態
    public void changeMaterialBrown()
    {
        GetComponent<Renderer>().material.color = new Color(1.0f, 0.7f, 0.3f, 1.0f);
        drawFlag = true;
    }

    // 枯れた状態
    public void changeMaterialErase()
    {
        drawFlag = false;
    }
}
