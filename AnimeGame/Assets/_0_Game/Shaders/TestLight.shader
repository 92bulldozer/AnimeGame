Shader "Unlit/TestLight"
{
    Properties
    {
        _BaseMap("BaseMap",2D) = "white"{}
        _AmbientColor("Ambient Color",color) = (0,0,0,0)
        
        [PerRendererData] _AnimTimeInfo ("Animation Time Info", Vector) = (0.0, 0.0, 0.0, 0.0)
		[PerRendererData] _AnimTextures ("Animation Textures", 2DArray) = "" {}
		[PerRendererData] _AnimTextureIndex ("Animation Texture Index", float) = -1.0
		[PerRendererData] _AnimInfo ("Animation Info", Vector) = (0.0, 0.0, 0.0, 0.0)
		[PerRendererData] _AnimScalar ("Animation Scalar", Vector) = (1.0, 1.0, 1.0, 0.0)
		[PerRendererData] _CrossfadeAnimTextureIndex ("Crossfade Texture Index", float) = -1.0
		[PerRendererData] _CrossfadeAnimInfo ("Crossfade Animation Info", Vector) = (0.0, 0.0, 0.0, 0.0)
		[PerRendererData] _CrossfadeAnimScalar ("Crossfade Animation Scalar", Vector) = (1.0, 1.0, 1.0, 0.0)
		[PerRendererData] _CrossfadeStartTime ("Crossfade Start Time", float) = -1.0
		[PerRendererData] _CrossfadeEndTime ("Crossfade End Time", float) = -1.0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //1. 라이트용 인클루드를 받아옵니다 
            //#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : NORMAL;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _AmbientColor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                Light light = GetMainLight();

                float3 lightDir = normalize(light.direction); //1. 두 벡터를 노말라이트합시다 
                float3 normalWS = normalize(IN.normalWS); //1. 두 벡터를 노말라이트합시다 

                float NdotL = saturate(dot(lightDir, normalWS)) ; //2. NdotL 처리합시다
                float3 toonLight = NdotL > 0 ? light.color :_AmbientColor; 

                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, TRANSFORM_TEX(IN.uv, _BaseMap));
                color.rgb *= toonLight;
                return color; //3. 확인
            }
            ENDHLSL
        }
    }
}