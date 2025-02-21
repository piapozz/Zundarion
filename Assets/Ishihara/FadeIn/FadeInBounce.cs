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

    [SerializeField]
    private int _delayFrame = 5;
    [SerializeField]
    private float _bounceTime = 1f;
    [SerializeField]
    private float _bounceHeight = 1;
    [SerializeField]
    private float _scaleUpValue = 1;

    [SerializeField]
    private AnimationCurve _scale;
    [SerializeField]
    private AnimationCurve _positionX;
    [SerializeField]
    private AnimationCurve _positionY;
    [SerializeField]
    private AnimationCurve _alpha;

    private List<Vector3[]> _startPosition;

    public async UniTask FadeIn()
    {
        if (_textMeshPro == null) return;

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

            Color32[] colors = textInfo.meshInfo[materialIndex].colors32;

            UniTask task = UniTask.WhenAll(
                AnimateScaling(characterInfo, _startPosition[i]),
                AnimateBounce(characterInfo, vertices, _startPosition[i])
                //AnimateAlpha(characterInfo, colors)
            );

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

    private async UniTask AnimateScaling(TMP_CharacterInfo characterInfo, Vector3[] startPosition)
    {
        int vertexIndex = characterInfo.vertexIndex;
        float elapsedTime = 0.0f;
        Vector3[] savePosition = (Vector3[])startPosition.Clone();

        while (_textMeshPro != null && elapsedTime < _bounceTime)
        {
            elapsedTime += Time.deltaTime;
            float offset = _scale.Evaluate(elapsedTime) * _scaleUpValue;

            for (int i = 0; i < 4; i++)
            {
                startPosition[i] = savePosition[i] * offset;
            }

            await UniTask.DelayFrame(1);
        }
    }

    private async UniTask AnimateAlpha(TMP_CharacterInfo characterInfo, Color32[] color32)
    {
        int vertexIndex = characterInfo.vertexIndex;
        float elapsedTime = 0.0f;

        while (_textMeshPro != null && elapsedTime < _bounceTime)
        {
            elapsedTime += Time.deltaTime;
            byte newAlpha = (byte)(_alpha.Evaluate(elapsedTime));

            for (int i = 0; i < 4; i++)
            {
                color32[vertexIndex + i] = new Color32(color32[vertexIndex + i].r, color32[vertexIndex + i].g, color32[vertexIndex + i].b, newAlpha);
            }
            _textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            await UniTask.DelayFrame(1);
        }

        for (int i = 0; i < 4; i++)
        {
            color32[vertexIndex + i] = new Color32(color32[vertexIndex + i].r, color32[vertexIndex + i].g, color32[vertexIndex + i].b, 1);
        }
    }
}