using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitTime : MonoBehaviour
{
    public void SetPeriod(float time)
    {
        // �������Ԍ�ɔj��֐����Ă�
        Invoke("LimitOver", time);
    }

    // ���Ԑ����������炱�̃I�u�W�F�N�g��j�󂷂�
    void LimitOver()
    {
        // �j��
        Destroy(this);
    }
}
