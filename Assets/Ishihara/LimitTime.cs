using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitTime : MonoBehaviour
{
    public void SetPeriod(float time)
    {
        // 制限時間後に破壊関数を呼ぶ
        Invoke("LimitOver", time);
    }

    // 時間制限が来たらこのオブジェクトを破壊する
    void LimitOver()
    {
        // 破壊
        Destroy(this);
    }
}
