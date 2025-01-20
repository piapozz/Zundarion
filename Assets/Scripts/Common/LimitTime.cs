using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitTime : MonoBehaviour
{
    public float deleteTime = 0.1f;

    /// <summary>
    /// �������Ԃ�ݒ肵�A���Ԍo�ߌ�ɃQ�[���I�u�W�F�N�g��j�󂵂܂��B
    /// </summary>
    /// <param name="time">�������ԁi�b�j</param>
    // �w�莞�Ԍo�ߌ�ɔj�󏈗����Ăяo��
    public void OnEnable()
    {
        StartCoroutine(DestroyAfterTime(deleteTime));
    }

    // Coroutine �Ŕj�󏈗�
    private IEnumerator DestroyAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        LimitOver();
    }

    // LimitOver ���\�b�h
    private void LimitOver()
    {
        Destroy(gameObject); // �I�u�W�F�N�g�j����s��
    }

}
