Shader "Cool Shaders/PIXELATE"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_TexelSize("Texel Size", int) = 128 
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
			int _TexelSize;

            fixed4 frag(v2f i) : SV_Target
            {
                // use different value for y based on aspect ratio to get square pixels
                int texelSizeY = _TexelSize * (_ScreenParams.y / _ScreenParams.x);
                float2 newUv = float2(0,0);
                newUv.x = floor(i.uv.x * _TexelSize) / _TexelSize;
                newUv.y = floor(i.uv.y * texelSizeY) / texelSizeY;
                fixed4 col = tex2D(_MainTex, newUv);
                
                return col;
            }
            ENDCG
        }
    }
}
