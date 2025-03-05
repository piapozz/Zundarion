Shader "Custom/FadeEdgeUI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // UI�̃e�N�X�`��
        _Color ("Color Tint", Color) = (1,1,1,1) // UI�̐F��ύX�\��
        _FadeRange ("Fade Range", Range(0, 1)) = 0.5 // �t�F�[�h�J�n�ʒu
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off Lighting Off ZWrite Off // UI�p�ݒ�
        
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
                fixed4 color : COLOR; // UI��Color���擾
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
                o.color = v.color * _Color; // UI��Color��K�p
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color; // UI�̐F��K�p

                // 0.5 ���痣���قǓ����x�������i�������c���A�[�𓧖��Ɂj
                float fade = smoothstep(_FadeRange, 1.0, abs(i.uv.x - 0.5) * 2);

                col.a *= (1.0 - fade); // �����x���t�]

                return col;
            }
            ENDCG
        }
    }
}
