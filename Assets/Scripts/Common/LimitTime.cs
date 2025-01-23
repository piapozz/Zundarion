using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitTime : MonoBehaviour
{
    public float deleteTime = 0.1f;

    /// <summary>
    /// 制限時間を設定し、時間経過後にゲームオブジェクトを破壊します。
    /// </summary>
    /// <param name="time">制限時間（秒）</param>
    // 指定時間経過後に破壊処理を呼び出す
    public void OnEnable()
    {
        DestroyCountStart(deleteTime);
        //StartCoroutine(DestroyAfterTime(deleteTime));
    }

    // Coroutine で破壊処理
    private IEnumerator DestroyAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        LimitOver();
    }

    private async UniTask DestroyCountStart(float time)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        LimitOver();
    }

    // LimitOver メソッド
    private void LimitOver()
    {
        Destroy(gameObject); // オブジェクト破壊を行う
    }

}
