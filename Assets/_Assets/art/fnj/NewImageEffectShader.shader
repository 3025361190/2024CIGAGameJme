Shader "Custom/WhiteOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth ("Outline Width", Range(0, 0.1)) = 0.01
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }
        ZWrite On
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha
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
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Calculate distance to nearest opaque pixel
                float2 dxy = 1.0 / _ScreenParams.xy * _OutlineWidth;
                float alpha = tex2D(_MainTex, i.uv + float2(-dxy.x, 0)).a;
                alpha = min(alpha, tex2D(_MainTex, i.uv + float2(dxy.x, 0)).a);
                alpha = min(alpha, tex2D(_MainTex, i.uv + float2(0, -dxy.y)).a);
                alpha = min(alpha, tex2D(_MainTex, i.uv + float2(0, dxy.y)).a);

                // If the current pixel is opaque and one of the adjacent pixels is transparent,
                // then it's an edge pixel
                if (col.a == 1 && alpha < 1)
                {
                    col.rgb = _OutlineColor.rgb;
                    col.a = 1;
                }
                else
                {
                    col.rgb = col.rgb * col.a + (1 - col.a) * _OutlineColor.rgb;
                    col.a = 1;
                }
                return col;
            }
            ENDCG
        }
    }
}

