using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/PlayerAnimation")]
public class PlayerAnimation : ScriptableObject
{
    public string[] movePram;       // �ړ��A�j���[�V����
    public string[] attackPram;     // �A�^�b�N�A�j���[�V����
    public string[] parryPram;      // �p���B�A�j���[�V����
    public string[] statePram;      // �ړ��A�j���[�V����
    public string[] InterruptPram;  // ���荞�݃A�j���[�V����
}
