Shader "Custom/MapCurveShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 오브젝트의 원래 텍스처를 정의
        _CurveStrength ("Curve Strength", Range(0, 1)) = 0.8 // 곡률의 정도를 조절
        _CurveDirection ("Curve Direction", Range(-1, 1)) = 0.1 // 좌우 곡률 방향을 조정
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
                float2 uv : TEXCOORD0; // 텍스처 좌표
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex; // 텍스처 샘플링을 위한 변수를 선언
            float4 _MainTex_ST; // 텍스처 변환을 위한 매트릭스
            float _CurveStrength;
            float _CurveDirection;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // 화면 좌표로 변환
                float2 screenPos = o.vertex.xy / o.vertex.w;

                // 곡률 계산
                float curveAmount = _CurveStrength * screenPos.y * screenPos.y;
                screenPos.x += curveAmount * _CurveDirection;

                // 다시 클립 공간으로 변환
                o.vertex.xy = screenPos * o.vertex.w;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // 텍스처 좌표 변환
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 텍스처에서 색상을 샘플링
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor; // 텍스처의 색상을 유지하면서 렌더링
            }
            ENDCG
        }
    }
}
