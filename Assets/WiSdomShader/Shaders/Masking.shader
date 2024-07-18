Shader "WiSdom/Masking"
{
    Properties
    {
        
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
        [Enum(UnityEngine.Rendering.BlendOp)] _Opp ("Operation", Float) = 0

        _MainTex ("Texture", 2D) = "white" { }
        _MaskTex ("Mask Texture", 2D) = "white" { }
        _Reveal ("Reveal", Range(0, 1)) = 1
        _Feather ("Feather", Range(0, 1)) = 0.1

        _ErodeColor ("Erode Color", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        Blend [_SrcBlend][_DstBlend]
        BlendOp [_Opp]


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;
            float _Reveal, _Feather;
            float4 _ErodeColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.zw = TRANSFORM_TEX(v.uv, _MaskTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv.xy);
                fixed4 mask = tex2D(_MaskTex, i.uv.zw);
                // float revealAmount = smoothstep(mask.r - _Feather,mask.r, _Reveal);
                // float revealAmount = step(mask.r, _Reveal);
                // float reavealSin = sin(_Time.y * 0.5 * 2) * 0.5 + 0.5;
                float revealAmountTop = step(mask.r, _Reveal + _Feather);
                float revealAmountBottom = step(mask.r, _Reveal - _Feather);
                float revealDifferece = revealAmountTop - revealAmountBottom;
                float3 finalColor = lerp(col.rgb, _ErodeColor, revealDifferece);
                // return fixed4(revealDifferece.xxx, 1);
                return fixed4(finalColor, col.a * revealAmountTop);
            }
            ENDCG
        }
    }
}
