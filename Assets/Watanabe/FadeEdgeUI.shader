Shader "Custom/FadeEdgeUI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // UIのテクスチャ
        _Color ("Color Tint", Color) = (1,1,1,1) // UIの色を変更可能に
        _FadeRange ("Fade Range", Range(0, 1)) = 0.5 // フェード開始位置
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off Lighting Off ZWrite Off // UI用設定
        
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
                fixed4 color : COLOR; // UIのColorを取得
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _FadeRange;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color; // UIのColorを適用
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color; // UIの色を適用

                // 0.5 から離れるほど透明度が増す（中央を残し、端を透明に）
                float fade = smoothstep(_FadeRange, 1.0, abs(i.uv.x - 0.5) * 2);

                col.a *= (1.0 - fade); // 透明度を逆転

                return col;
            }
            ENDCG
        }
    }
}
