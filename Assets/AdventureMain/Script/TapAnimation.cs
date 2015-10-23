using UnityEngine;
using System.Collections;

public class TapAnimation : MonoBehaviour
{

    private int count;
    private int Limitcount;

    private Vector3 MoveMinasPoint = new Vector3(0.0f, -0.5f, 0f);
    private Vector3 MovePulusPoint = new Vector3(0.0f, +0.5f, 0f);
    // Use this for initialization
    void Start()
    {
        count = 0;
        //MovePoint = transform.localPosition;  // 形状位置を保持
    }

    // Update is called once per frame
    void Update()
    {

        count++;
        if (count < 31)
        {
            transform.localPosition += MoveMinasPoint;  // 形状位置を保持
        }
        else if (count < 61)
        {
            transform.localPosition += MovePulusPoint;  // 形状位置を保持
        }
        else
        {
            count = 0;
        }


        //transform.position += MovePoint;
    }
}
