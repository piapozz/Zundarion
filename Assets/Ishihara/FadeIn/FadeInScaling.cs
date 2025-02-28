using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FadeInScaling : FadeInFont
{    [SerializeField]
    private TMP_Text _textMeshPro;
    [SerializeField]
    private float _scalingTime = 1f;

    [SerializeField]
    private AnimationCurve _scale;

    private List<Vector3[]> _startPosition;

    public async UniTask FadeIn(float value = 1)
    {
        _textMeshPro.ForceMeshUpdate();
        TMP_TextInfo textInfo = _textMeshPro.textInfo;
        _startPosition = new List<Vector3[]>();
        _textMeshPro.alpha = 1;

        for (int i = 0, max = textInfo.characterCount; i < max; i++)
        {
            TMP_CharacterInfo characterInfo = textInfo.characterInfo[i];
            if (!characterInfo.isVisible) continue;

            int materialIndex = characterInfo.materialReferenceIndex;
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;
            Vector3[] copiedVertices = (Vector3[])vertices.Clone();
            _startPosition.Add(copiedVertices);

            AnimateScaling(characterInfo, vertices, _startPosition[i], value);
        }

        await UniTask.DelayFrame(1);
    }

    private async UniTask AnimateScaling(TMP_CharacterInfo characterInfo, Vector3[] vertices, Vector3[] startPosition, float value = 1)
    {
        int vertexIndex = characterInfo.vertexIndex;
        float elapsedTime = 0.0f;
        Vector3[] goalPos = new Vector3[startPosition.Length];
        float maxOffset =(_scale.Evaluate(_scalingTime) * value);
        for (int i = 0, max = startPosition.Length; i < max; i++) 
        {
            goalPos[i] = maxOffset * startPosition[i];
        }

        while (_textMeshPro != null && elapsedTime < _scalingTime)
        {
            elapsedTime += 0.1f;

            for (int i = 0; i < 4; i++)
            {
                vertices[vertexIndex + i] = startPosition[vertexIndex + i] * Mathf.Lerp(_scale.Evaluate(0), maxOffset, elapsedTime);
            }
            _textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
            await UniTask.DelayFrame(1);

        }

        for (int i = 0; i < 4; i++)
        {
            vertices[vertexIndex + i] = startPosition[vertexIndex + i] * maxOffset;
        }
    }
}
