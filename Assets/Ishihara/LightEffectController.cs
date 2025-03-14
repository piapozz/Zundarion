using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static CartoonFX.ExpressionParser;

public class LightEffectController : MonoBehaviour
{
    public Material crossMaterial;
    private Vector4[] startPoints = new Vector4[10];
    private float[] timeSinceStart = new float[10];

    void Update()
    {
        //for (int i = 0; i < startPoints.Length; i++)
        //{
        //    if (timeSinceStart[i] < 100f)
        //        timeSinceStart[i] += Time.deltaTime;
        //}

        //crossMaterial.SetVectorArray("_StartPoints", startPoints);
        //crossMaterial.SetFloatArray("_TimeSinceStart", timeSinceStart);

        //// クリックしたら新しい十字光を追加
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePos = Input.mousePosition;
        //    AddCross(mousePos);
        //}
    }

    public void AddCross(Vector3 screenPos)
    {
        for (int i = 0; i < startPoints.Length; i++)
        {
            if (timeSinceStart[i] >= 100f) // 空きスロットに追加
            {
                startPoints[i] = new Vector4(screenPos.x / Screen.width, screenPos.y / Screen.height, 0, 0);
                timeSinceStart[i] = 0;
                break;
            }
        }
    }

}
