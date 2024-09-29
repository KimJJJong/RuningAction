Shader "Custom/CameraCurvedEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CurveAmount ("Curve Amount", Range(-1, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _CurveAmount;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // �⺻������ ȭ���� ��ǥ�� �޾ƿ��� ���� ���̴�
            v2f vert_img(float4 pos : POSITION, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(pos);
                o.uv = uv;
                return o;
            }

            // ��ó�� � ȿ���� �����ϴ� �ȼ� ���̴�
            float4 frag(v2f i) : SV_Target
            {
                // UV ��ǥ�� 0.5 �������� ����ȭ
                float2 uv = i.uv - 0.5;

                // � ȿ�� ���� (��/�� �־���)
                uv.x += uv.y * uv.y * _CurveAmount;

                // �ٽ� ���� ��ǥ�� ����
                uv += 0.5;

                // �ؽ�ó���� ���ø��� ���� ��ȯ
                float4 color = tex2D(_MainTex, uv);
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
