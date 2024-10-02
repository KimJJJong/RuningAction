Shader "Custom/CurvedWorld"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CurveAmount ("Curve Amount", Float) = 0.05 // �¿� Ŀ���� ����
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
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
            float _CurveAmount;

            // Vertex Shader
            v2f vert (appdata v)
            {
                v2f o;
                
                // �⺻���� ���� -> Ŭ�� ��ȯ
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // Ŀ�� ȿ�� �߰�
                float curveFactor = o.vertex.x * o.vertex.x * _CurveAmount;
                o.vertex.y += curveFactor; // Ŀ�� ������ ���� �¿�� �ְ� ����
                
                o.uv = v.uv;
                return o;
            }

            // Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                // �ؽ�ó ����
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
