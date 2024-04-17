Shader "Unlit/EJToon"
{
    
    Properties
    { 
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("Main Texture",2d)="white"{}
        [Toggle] _useAlpha("UseAlpha",float)=0
    }

    SubShader
    {        
        
        Name "ForwardLit"
        
        Tags { 
            "RenderPipeline" = "UniversalRenderPipeline" 
            "LightMode" = "UniversalForward"
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            }

        Pass
        {            
            Cull off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

            struct VertexIn
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct VertexOut
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
           
   


            CBUFFER_START(UnityPerMaterial)
              
                half4 _BaseColor;
                float4 _MainTex_ST;
            CBUFFER_END

            

            

            VertexOut vert(VertexIn i)
            {
                VertexOut o;
                o.positionHCS = TransformObjectToHClip(i.positionOS.xyz);
                o.uv = i.uv.xy* _MainTex_ST.xy + _MainTex_ST.zw +_Time.x;
                return o;
            }

            half4 frag(VertexOut o) : SV_Target
            {
                half4 color = tex2D(_MainTex,o.uv) ;
                return color;
            }
            ENDHLSL
        }
    }
}


