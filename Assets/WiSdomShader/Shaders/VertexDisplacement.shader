Shader "WiSdom/VertexDisplacement"
{
    Properties
    {
        
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)] _Opp ("Operation", Float) = 0

        _MainTex ("Texture", 2D) = "white" { }
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
                float2 uv : TEXCOORD0;
            };
            

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert(appdata v)
            {
                v2f o;
                o.uv = v.uv;
                float xMod = tex2Dlod(_MainTex, float4(v.uv, 0.0, 1.0));
                xMod = xMod * 2.0 - 1.0;
                // o.uv = o.uv * 2.0 - 5.0;
                o.uv.x = sin(xMod * 2.0 - _Time.y);
                float3 vert = v.vertex;
                vert.y = o.uv.x;
                o.uv.x = o.uv.x + 0.5;
                o.vertex = UnityObjectToClipPos(vert);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return fixed4(i.uv.x, 0.0, 0.0, 1.0);
            }
            ENDCG
        }
    }
}
