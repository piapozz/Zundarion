using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FadeInBounce : FadeInFont
{
    [SerializeField]
    private TMP_Text _textMeshPro;

    private int _delayFrame = 5;
    private float _bounceTime = 1f;
    private float _bounceHeight = 1;

    [SerializeField]
    private AnimationCurve _positionX;
    [SerializeField]
    private AnimationCurve _positionY;

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

            AnimateBounce(characterInfo, vertices, _startPosition[i]);
           
            await UniTask.DelayFrame(60 - _delayFrame);
        }

        await UniTask.DelayFrame(1);
    }

    private async UniTask AnimateBounce(TMP_CharacterInfo characterInfo, Vector3[] vertices, Vector3[] startPosition)
    {
        int vertexIndex = characterInfo.vertexIndex;
        float elapsedTime = 0.0f;

        while (_textMeshPro != null && elapsedTime < _bounceTime)
        {
            elapsedTime += Time.deltaTime;
            float offsetX = _positionX.Evaluate(elapsedTime) * _bounceHeight;
            float offsetY = _positionY.Evaluate(elapsedTime) * _bounceHeight;

            for (int i = 0; i < 4; i++)
            {
                vertices[vertexIndex + i] = startPosition[vertexIndex + i] + new Vector3(offsetX, offsetY, 0);
            }
            _textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);

            await UniTask.DelayFrame(1);
        }
    }
}