Shader "Custom/BlendedGlowingCross"
{
    Properties
    {
        _StartPoints ("Start Points", Vector) = (0, 0, 0, 0)
        _TimeSinceStart ("Time Since Start", Float) = 0
        _LineColor ("Line Color", Color) = (1, 0, 0, 1)
        _Thickness ("Line Thickness", Range(0.001, 0.1)) = 0.01
        _BlurAmount ("Blur Amount", Range(0.0, 1.0)) = 0.2
        _FadeTime ("Fade Out Time", Float) = 2.0
        _BlendFactor ("Blend Factor", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 screenUV : TEXCOORD0;
            };

            float4 _StartPoints[10];
            float _TimeSinceStart[10];
            float4 _LineColor;
            float _Thickness;
            float _BlurAmount;
            float _FadeTime;
            float _BlendFactor;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenUV = o.pos.xy / o.pos.w * 0.5 + 0.5;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float alpha = 0;
                float3 color = _LineColor.rgb;

                for (int j = 0; j < 10; j++)
                {
                    if (_TimeSinceStart[j] >= _FadeTime) continue;

                    float2 startPos = _StartPoints[j].xy;
                    float horizontalDist = abs(i.screenUV.y - startPos.y);
                    float verticalDist = abs(i.screenUV.x - startPos.x);

                    float lineAlpha = exp(-horizontalDist / _Thickness) + exp(-verticalDist / _Thickness);
                    lineAlpha *= smoothstep(1.0 - _BlurAmount, 1.0, 1.0 - max(horizontalDist, verticalDist));

                    float fadeFactor = 1.0 - (_TimeSinceStart[j] / _FadeTime);
                    lineAlpha *= fadeFactor;

                    alpha = max(alpha, lineAlpha);
                }

                // ブレンド率を適用
                alpha *= _BlendFactor;

                return fixed4(color, alpha);
            }
            ENDCG
        }
    }
}
