/*
 * @file CharacterData.cs
 * @brief キャラクターのステータスの初期値を決める
 * @author sein
 * @date 2025/1/17
 */

using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;        // キャラクター名
    public float health;                // 体力
    public float strength;              // 攻撃力
    public float defence;               // 防御力
    public float speed;                 // 速さ
}