Shader "Unlit/KeepTextAndWhite"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _TextColor("Text Color", Color) = (0.0, 0.0, 0.0, 1.0) // �۾� ���� (�⺻��: ������)
        _WhiteThreshold("White Threshold", Range(0, 1)) = 0.9 // ��� ����
        _ColorThreshold("Color Threshold", Range(0, 1)) = 0.1 // �۾� ����
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100

            Pass
            {
                ZWrite Off
                Blend SrcAlpha OneMinusSrcAlpha

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
                float4 _TextColor;
                float _WhiteThreshold;
                float _ColorThreshold;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 texColor = tex2D(_MainTex, i.uv);

                // �۾� ������� �Ÿ� ���
                float textDiff = distance(texColor.rgb, _TextColor.rgb);

                // ��� ���� (R, G, B ���� ��� ���� ���)
                float whiteLevel = (texColor.r + texColor.g + texColor.b) / 3.0;
                bool isWhite = whiteLevel > _WhiteThreshold;

                // �۾� �Ǵ� ����̸� ����, �׷��� ������ ����
                if (textDiff < _ColorThreshold || isWhite)
                {
                    texColor.a = 1.0; // �۾��� ����� ������
                }
                else
                {
                    texColor.a = 0.0; // ����� ����
                }

                return texColor;
            }
            ENDCG
        }
        }
}