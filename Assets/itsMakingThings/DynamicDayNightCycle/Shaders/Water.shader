// @itsmakingthings

Shader "Gareth/Water"
{
    Properties
    {
        _WaterColor ("Water Color", Color) = (0, 0.5, 0.7, 0.7)
        _FoamColor ("Foam Color", Color) = (1, 1, 1, 1)
        _FoamTexture ("Foam Texture", 2D) = "white" {}
        _WaveSpeed ("Wave Speed", Range(0, 5)) = 1
        _WaveHeight ("Wave Height", Range(0, 1)) = 0.05
        _WaveFrequency ("Wave Frequency", Range(0.5, 100)) = 3
        _FoamTilingX ("Foam Tiling X", Range(0.01, 20)) = 1
        _FoamTilingZ ("Foam Tiling Z", Range(0.01, 20)) = 1
        _FoamBlend ("Foam Blend", Range(0.1, 10)) = 1
        _FoamAnimationAmount ("Foam Animation", Range(0, 2)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _FoamTexture;
            float4 _WaterColor, _FoamColor;
            float _WaveSpeed, _WaveHeight, _WaveFrequency;
            float _FoamTilingX, _FoamTilingZ, _FoamBlend, _FoamAnimationAmount;

            v2f vert (appdata_t v)
            {
                v2f o;
                float timeOffset = _Time.y * _WaveSpeed;

                // Scale wave height properly
                float wave = sin(v.vertex.x * _WaveFrequency + v.vertex.z * _WaveFrequency + timeOffset) * (_WaveHeight * 0.01);
                v.vertex.y += wave;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Apply animation movement to foam
                float2 animatedFoamOffset = sin(i.worldPos.xz * 0.5 + _Time.y) * _FoamAnimationAmount;

                // Manually apply X and Z tiling + animation
                float2 foamUV = float2(i.worldPos.x * _FoamTilingX, i.worldPos.z * _FoamTilingZ) + animatedFoamOffset;

                // Sample the foam texture
                float foamFactor = tex2D(_FoamTexture, foamUV).r * (1.0 / _FoamBlend);

                // Blend foam with water
                float4 finalColor = lerp(_WaterColor, _FoamColor, foamFactor);

                return float4(finalColor.rgb, _WaterColor.a);
            }
            ENDCG
        }
    }
}
