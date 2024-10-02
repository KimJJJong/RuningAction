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

            // ±âº»ÀûÀ¸·Î È­¸éÀÇ ÁÂÇ¥¸¦ ¹Þ¾Æ¿À´Â Á¤Á¡ ¼ÎÀÌ´õ
            v2f vert_img(float4 pos : POSITION, float2 uv : TEXCOORD0)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(pos);
                o.uv = uv;
                return o;
            }

            // ÈÄÃ³¸® °î¼± È¿°ú¸¦ Àû¿ëÇÏ´Â ÇÈ¼¿ ¼ÎÀÌ´õ
            float4 frag(v2f i) : SV_Target
            {
                // UV ÁÂÇ¥¸¦ 0.5 ±âÁØÀ¸·Î Á¤±ÔÈ­
                float2 uv = i.uv - 0.5;

                // °î¼± È¿°ú Àû¿ë (ÁÂ/¿ì ÈÖ¾îÁü)
                uv.x += uv.y * uv.y * _CurveAmount;

                // ´Ù½Ã ¿ø·¡ ÁÂÇ¥·Î º¹¿ø
                uv += 0.5;

                // ÅØ½ºÃ³¿¡¼­ »ùÇÃ¸µµÈ »ö»ó ¹ÝÈ¯
                float4 color = tex2D(_MainTex, uv);
                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
