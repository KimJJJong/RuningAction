Shader "Custom/CurvedWorld"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CurveAmount ("Curve Amount", Float) = 0.05 // 좌우 커브의 강도
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
                
                // 기본적인 월드 -> 클립 변환
                o.vertex = UnityObjectToClipPos(v.vertex);
                
                // 커브 효과 추가
                float curveFactor = o.vertex.x * o.vertex.x * _CurveAmount;
                o.vertex.y += curveFactor; // 커브 정도에 따라 좌우로 휘게 만듦
                
                o.uv = v.uv;
                return o;
            }

            // Fragment Shader
            fixed4 frag (v2f i) : SV_Target
            {
                // 텍스처 적용
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
