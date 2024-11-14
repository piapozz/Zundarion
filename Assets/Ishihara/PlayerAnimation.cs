using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player/PlayerAnimation")]
public class PlayerAnimation : ScriptableObject
{
    public string[] movePram;       // 移動アニメーション
    public string[] attackPram;     // アタックアニメーション
    public string[] parryPram;      // パリィアニメーション
    public string[] statePram;      // 移動アニメーション
    public string[] InterruptPram;  // 割り込みアニメーション
}
