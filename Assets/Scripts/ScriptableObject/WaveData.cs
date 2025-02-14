/*
 * @file WaveData.cs
 * @brief �E�F�[�u�̃f�[�^
 * @author sakakura
 * @date 2025/1/22
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/Stage/WaveData")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class GenerateCharacterData
    {
        public GameObject characterPrefab;

        public int generateAnchorNum;
    }

    public GenerateCharacterData[] generateCharacterData;
}
