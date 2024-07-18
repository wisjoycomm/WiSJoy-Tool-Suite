Shader "WiSdom/TextureSample"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" { }
        _AnimateXY ("AnimateXY", Vector) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata // Object Data or Mesh

            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f // vertex to fragment, pass data from vertex to fragment shader

            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _AnimateXY;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv += frac(_AnimateXY.xy + float2(sin(_Time.y), 0));
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uvs = i.uv;
                fixed4 textureColor = tex2D(_MainTex, uvs);
                return textureColor;
            }
            ENDCG
        }
    }
}
