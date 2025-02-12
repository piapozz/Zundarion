using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CommonModule
{
    /// <summary>
    /// リストが空か否か
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="checkList"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(List<T> checkList)
    {
        return checkList == null || checkList.Count == 0;
    }

    /// <summary>
    /// キューが空か否か
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="checkQueue"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(Queue<T> checkQueue)
    {
        return checkQueue == null || checkQueue.Count == 0;
    }

    /// <summary>
    /// リスト1からリスト2に要素を移動させる
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sourceList"></param>
    /// <param name="targetList"></param>
    public static void ReinsertListMember<T>(ref List<T> sourceList, ref List<T> targetList)
    {
        if (IsEmpty(sourceList)) return;

        int targetNum = sourceList.Count - 1;
        T member = sourceList[targetNum];
        sourceList.RemoveAt(targetNum);
        targetList.Add(member);
    }

    /// <summary>
    /// リスト1からリスト2に指定した要素を移動させる
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sourceList"></param>
    /// <param name="targetList"></param>
    /// <param name="target"></param>
    public static void ReinsertListMember<T>(ref List<T> sourceList, ref List<T> targetList, T target)
    {
        if (IsEmpty(sourceList)) return;

        int targetNum = sourceList.IndexOf(target);
        if (targetNum < 0) return;

        T member = sourceList[targetNum];
        sourceList.RemoveAt(targetNum);
        targetList.Add(member);
    }

    #region WaitAction(sec)

    /// <summary>
    /// 指定した秒数後に関数を実行する
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask WaitAction(float sec, System.Action action)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        action();
    }

    public static async UniTask WaitAction(float sec, System.Action action, CancellationToken token)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        action();
    }

    /// <summary>
    /// 指定した秒数後に関数を実行する
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask WaitAction<T>(float sec, System.Action<T> action, T pram)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            pram != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        action(pram);
    }

    /// <summary>
    /// 指定した秒数後に関数を実行する
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask<T> WaitAction<T>(float sec, System.Func<T> action)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        return action();
    }

    /// <summary>
    /// 指定した秒数後に関数を実行する
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask<T> WaitAction<T>(float sec, System.Func<T, T> action, T pram)
    {
        float elapsedTime = 0.0f;
        while (action != null &&
            pram != null &&
            elapsedTime < sec)
        {
            elapsedTime += Time.deltaTime;
            await UniTask.DelayFrame(1);
        }
        return action(pram);
    }

    #endregion

    #region WaitAction(frame)

    /// <summary>
    /// 指定したフレームに関数を実行する
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask WaitAction(int frame, System.Action action)
    {
        float elapsedTime = 0;
        while (action != null &&
            elapsedTime < frame)
        {
            elapsedTime++;
            await UniTask.DelayFrame(1);
        }
        action();
    }

    /// <summary>
    /// 指定したフレームに関数を実行する
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask WaitAction<T>(int frame, System.Action<T> action, T pram)
    {
        float elapsedTime = 0;
        while (action != null &&
            pram != null &&
            elapsedTime < frame)
        {
            elapsedTime++;
            await UniTask.DelayFrame(1);
        }
        action(pram);
    }

    /// <summary>
    /// 指定したフレームに関数を実行する
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask<T> WaitAction<T>(int frame, System.Func<T> action)
    {
        float elapsedTime = 0;
        while (action != null &&
            elapsedTime < frame)
        {
            elapsedTime++;
            await UniTask.DelayFrame(1);
        }
        return action();
    }

    /// <summary>
    /// 指定したフレームに関数を実行する
    /// </summary>
    /// <param name="sec"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static async UniTask<T> WaitAction<T>(int frame, System.Func<T, T> action, T pram)
    {
        float elapsedTime = 0;
        while (action != null &&
            pram != null &&
            elapsedTime < frame)
        {
            elapsedTime++;
            await UniTask.DelayFrame(1);
        }
        return action(pram);
    }

    #endregion
}
