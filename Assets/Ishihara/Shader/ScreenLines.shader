Shader "Custom/ScreenLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightSource ("_Light Source", Vector) = (0.5, 0.5, 0, 0)  // 光源の位置
        _GlowColor ("_Glow Color", Color) = (1,1,0,1)  // 光の色
        _LineWidth ("_Line Width", Float) = 0.005  // 線の幅（小さい値を使用）
        _IntensityFactor ("_Intensity Factor", Float) = 1.0  // 強度の調整
        _SmoothFactor ("_Smooth Factor", Float) = 10.0  // 滑らかさの調整（強くする）
        _BlurFactor ("_Blur Factor", Float) = 4.0  // ぼやけの強度
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _LightSource;
            float4 _GlowColor;
            float _LineWidth;
            float _IntensityFactor;
            float _SmoothFactor;
            float _BlurFactor;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // 敵の位置から縦横方向に線を引くための距離計算
                float distX = abs(uv.x - _LightSource.x);  // 横方向の距離
                float distY = abs(uv.y - _LightSource.y);  // 縦方向の距離

                // 画面端まで線を引くために、X軸とY軸の最大値を取得
                // smoothstep の範囲を反転させることで、中心に向かうほど線が細く、端に行くほど太くなる
                float lineX = smoothstep(_LineWidth, 0.0, distX);  // 横方向（中心に向かうほど細くなる）
                float lineY = smoothstep(_LineWidth, 0.0, distY);  // 縦方向（同様）

                // 線の強度（横方向と縦方向の最大値を使用）
                float intensity = max(lineX, lineY);
                
                // ぼやけ効果の強調
                // ぼやけがより強くなるようにするために、2乗の計算を追加
                float blur = exp(-pow(intensity, 2) * _BlurFactor);  // ぼやける強度を加算

                // 線が中心から遠ざかるにつれて強度が増加し、ぼやけが薄くなる
                intensity = exp(pow(intensity, 2) * _SmoothFactor) * blur;  // 反転した強度減衰

                // 光の色と強度を加算
                float4 texColor = tex2D(_MainTex, uv);
                float4 finalColor = texColor + (_GlowColor * (1.0 - intensity) * _IntensityFactor);

                return finalColor;
            }
            ENDCG
        }
    }
}
