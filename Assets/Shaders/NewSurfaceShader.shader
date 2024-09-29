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

            // 기본적으로 화면의 좌표를 받아오는 정점 셰이더
            v2f vert_img(float4 pos : POSITION, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(pos);
                o.uv = uv;
                return o;
            }

            // 후처리 곡선 효과를 적용하는 픽셀 셰이더
            float4 frag(v2f i) : SV_Target
            {
                // UV 좌표를 0.5 기준으로 정규화
                float2 uv = i.uv - 0.5;

                // 곡선 효과 적용 (좌/우 휘어짐)
                uv.x += uv.y * uv.y * _CurveAmount;

                // 다시 원래 좌표로 복원
                uv += 0.5;

                // 텍스처에서 샘플링된 색상 반환
                float4 color = tex2D(_MainTex, uv);
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
