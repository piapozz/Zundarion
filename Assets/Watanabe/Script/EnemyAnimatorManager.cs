using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorManager : MonoBehaviour
{
    private Animator animator;



    public void SetTrigger(string triggerName)
    {
        animator?.SetTrigger(triggerName);
    }

    public void SetBool(string boolName, bool value)
    {
        animator?.SetBool(boolName, value);
    }
}
