Shader "Custom/ScreenLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightSource ("_Light Source", Vector) = (0.5, 0.5, 0, 0)  // �����̈ʒu
        _GlowColor ("_Glow Color", Color) = (1,1,0,1)  // ���̐F
        _LineWidth ("_Line Width", Float) = 0.005  // ���̕��i�������l���g�p�j
        _IntensityFactor ("_Intensity Factor", Float) = 1.0  // ���x�̒���
        _SmoothFactor ("_Smooth Factor", Float) = 10.0  // ���炩���̒����i��������j
        _BlurFactor ("_Blur Factor", Float) = 4.0  // �ڂ₯�̋��x
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

                // �G�̈ʒu����c�������ɐ����������߂̋����v�Z
                float distX = abs(uv.x - _LightSource.x);  // �������̋���
                float distY = abs(uv.y - _LightSource.y);  // �c�����̋���

                // ��ʒ[�܂Ő����������߂ɁAX����Y���̍ő�l���擾
                // smoothstep �͈̔͂𔽓]�����邱�ƂŁA���S�Ɍ������قǐ����ׂ��A�[�ɍs���قǑ����Ȃ�
                float lineX = smoothstep(_LineWidth, 0.0, distX);  // �������i���S�Ɍ������قǍׂ��Ȃ�j
                float lineY = smoothstep(_LineWidth, 0.0, distY);  // �c�����i���l�j

                // ���̋��x�i�������Əc�����̍ő�l���g�p�j
                float intensity = max(lineX, lineY);
                
                // �ڂ₯���ʂ̋���
                // �ڂ₯����苭���Ȃ�悤�ɂ��邽�߂ɁA2��̌v�Z��ǉ�
                float blur = exp(-pow(intensity, 2) * _BlurFactor);  // �ڂ₯�鋭�x�����Z

                // �������S���牓������ɂ�ċ��x���������A�ڂ₯�������Ȃ�
                intensity = exp(pow(intensity, 2) * _SmoothFactor) * blur;  // ���]�������x����

                // ���̐F�Ƌ��x�����Z
                float4 texColor = tex2D(_MainTex, uv);
                float4 finalColor = texColor + (_GlowColor * (1.0 - intensity) * _IntensityFactor);

                return finalColor;
            }
            ENDCG
        }
    }
}
