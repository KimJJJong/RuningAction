Shader "Custom/MapCurveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // ������Ʈ�� ���� �ؽ�ó�� ����
        _CurveStrength ("Curve Strength", Range(0, 1)) = 0.8 // ����� ������ ����
        _CurveDirection ("Curve Direction", Range(-1, 1)) = 0.1 // �¿� ��� ������ ����
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0; // �ؽ�ó ��ǥ
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex; // �ؽ�ó ���ø��� ���� ������ ����
            float4 _MainTex_ST; // �ؽ�ó ��ȯ�� ���� ��Ʈ����
            float _CurveStrength;
            float _CurveDirection;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // ȭ�� ��ǥ�� ��ȯ
                float2 screenPos = o.vertex.xy / o.vertex.w;

                // ��� ���
                float curveAmount = _CurveStrength * screenPos.y * screenPos.y;
                screenPos.x += curveAmount * _CurveDirection;

                // �ٽ� Ŭ�� �������� ��ȯ
                o.vertex.xy = screenPos * o.vertex.w;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // �ؽ�ó ��ǥ ��ȯ
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // �ؽ�ó���� ������ ���ø�
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor; // �ؽ�ó�� ������ �����ϸ鼭 ������
            }
            ENDCG
        }
    }
}
