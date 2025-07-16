Shader "Custom/WavyHealthBar"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", Float) = 2
        _WaveStrength ("Wave Strength", Float) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _WaveSpeed;
            float _WaveStrength;

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
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float strengthFactor = 0.0;

                if (i.uv.x > 0.4 && i.uv.x < 0.6)
                    strengthFactor = 1.0;
                else if ((i.uv.x > 0.25 && i.uv.x <= 0.4) || (i.uv.x >= 0.6 && i.uv.x < 0.75))
                    strengthFactor = 0.5;
                else
                    strengthFactor = 0.0;

                float wave = sin(i.uv.y * 20 + _Time.y * _WaveSpeed) * _WaveStrength * strengthFactor;
                float2 distortedUV = i.uv + float2(wave, 0);

                return tex2D(_MainTex, distortedUV);
            }
            ENDCG
        }
    }
}