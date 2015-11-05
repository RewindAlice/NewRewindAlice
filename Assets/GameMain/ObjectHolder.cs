using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AliceActionNotifer
{
    public GameObject[] gimmickArray;   // ギミックを入れておく配列

    public GameObject[] GimmickArray
    {
        set { gimmickArray = value; }
    }

    // ★アリスが進んだ時★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void NotiferNext(int aliceMove)
    {
        for(int i = 0; i < gimmickArray.Length; i++)
        {
            gimmickArray[i].GetComponent<BaseGimmick>().OnAliceMoveNext(aliceMove);
        }
    }

    // ★アリスが戻った時★〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓〓
    public void NotiferReturn(int aliceMove)
    {
        for (int i = 0; i < gimmickArray.Length; i++)
        {
            gimmickArray[i].GetComponent<BaseGimmick>().OnAliceMoveReturn(aliceMove);
        }
    }
}