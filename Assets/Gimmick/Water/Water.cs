using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{
    private Renderer _renderer;
    private Vector2 offset;
    private GameObject pause;
    private Pause pauseScript;
	// 初期化
	void Start ()
    {
        pause = GameObject.Find("Pause");
        pauseScript = pause.GetComponent<Pause>();
        _renderer = GetComponent<Renderer>();
        offset = _renderer.sharedMaterial.mainTextureOffset;
	}
	
	// 更新
	void Update ()
    {
        if (pauseScript.pauseFlag == false)
        {
            offset.x = Mathf.Sin(Time.time * 2) * 0.02f;
            offset.y = Mathf.Repeat(offset.y - 0.002f, 1.0f);
            _renderer.sharedMaterial.mainTextureOffset = offset;
            _renderer.sharedMaterial.SetTextureOffset("_BumpMap", offset);
        }   
       
	}
}
