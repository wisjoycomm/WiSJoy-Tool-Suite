Shader "WiSdom/BlendShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" { }
        _Color ("Color", Color) = (1, 1, 1, 1)

        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 0
        [Enum(UnityEngine.Rendering.BlendOp)] _Opp ("Operation", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100
        Blend [_SrcBlend][_DstBlend]
        BlendOp [_Opp]

        // blend
        // source = whatever this shader outputs
        // destination = whatever is inthe background

        // source * fsource + destination * fdestination
        // white * 0.7 + Background * (1 - 0.7)
        // We see 70% of the source and 30% of the background

        // SrcAlpha = fsource
        // OneMinusSrcAlpha = fdestination

        // SrcAlpha = 1
        // OneMinusSrcAlpha = 0

        // Additive
        // Blend SrcAlpha One
        // source * fsource + destination * fdestination
        // source * 1 + destination * 1
        // We see 100% of the source and 100% of the background

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
            fixed4 _Color;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;
                return col;
            }
            ENDCG
        }
    }
}
