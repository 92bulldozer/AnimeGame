// Made with Amplify Shader Editor v1.9.3.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "UID"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _Emission("Emission", Float) = 3

    }

    SubShader
    {
		LOD 0

        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

        Stencil
        {
        	Ref [_Stencil]
        	ReadMask [_StencilReadMask]
        	WriteMask [_StencilWriteMask]
        	Comp [_StencilComp]
        	Pass [_StencilOp]
        }


        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha, SrcAlpha One
        ColorMask [_ColorMask]

        
        Pass
        {
            Name "Default"
        CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            #include "UnityShaderVariables.cginc"


            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float4  mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
                
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

            uniform float _Emission;
            struct Gradient
            {
            	int type;
            	int colorsLength;
            	int alphasLength;
            	float4 colors[8];
            	float2 alphas[8];
            };
            
            Gradient NewGradient(int type, int colorsLength, int alphasLength, 
            float4 colors0, float4 colors1, float4 colors2, float4 colors3, float4 colors4, float4 colors5, float4 colors6, float4 colors7,
            float2 alphas0, float2 alphas1, float2 alphas2, float2 alphas3, float2 alphas4, float2 alphas5, float2 alphas6, float2 alphas7)
            {
            	Gradient g;
            	g.type = type;
            	g.colorsLength = colorsLength;
            	g.alphasLength = alphasLength;
            	g.colors[ 0 ] = colors0;
            	g.colors[ 1 ] = colors1;
            	g.colors[ 2 ] = colors2;
            	g.colors[ 3 ] = colors3;
            	g.colors[ 4 ] = colors4;
            	g.colors[ 5 ] = colors5;
            	g.colors[ 6 ] = colors6;
            	g.colors[ 7 ] = colors7;
            	g.alphas[ 0 ] = alphas0;
            	g.alphas[ 1 ] = alphas1;
            	g.alphas[ 2 ] = alphas2;
            	g.alphas[ 3 ] = alphas3;
            	g.alphas[ 4 ] = alphas4;
            	g.alphas[ 5 ] = alphas5;
            	g.alphas[ 6 ] = alphas6;
            	g.alphas[ 7 ] = alphas7;
            	return g;
            }
            
            float4 SampleGradient( Gradient gradient, float time )
            {
            	float3 color = gradient.colors[0].rgb;
            	UNITY_UNROLL
            	for (int c = 1; c < 8; c++)
            	{
            	float colorPos = saturate((time - gradient.colors[c-1].w) / ( 0.00001 + (gradient.colors[c].w - gradient.colors[c-1].w)) * step(c, (float)gradient.colorsLength-1));
            	color = lerp(color, gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), gradient.type));
            	}
            	#ifndef UNITY_COLORSPACE_GAMMA
            	color = half3(GammaToLinearSpaceExact(color.r), GammaToLinearSpaceExact(color.g), GammaToLinearSpaceExact(color.b));
            	#endif
            	float alpha = gradient.alphas[0].x;
            	UNITY_UNROLL
            	for (int a = 1; a < 8; a++)
            	{
            	float alphaPos = saturate((time - gradient.alphas[a-1].y) / ( 0.00001 + (gradient.alphas[a].y - gradient.alphas[a-1].y)) * step(a, (float)gradient.alphasLength-1));
            	alpha = lerp(alpha, gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), gradient.type));
            	}
            	return float4(color, alpha);
            }
            

            
            v2f vert(appdata_t v )
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                

                v.vertex.xyz +=  float3( 0, 0, 0 ) ;

                float4 vPosition = UnityObjectToClipPos(v.vertex);
                OUT.worldPosition = v.vertex;
                OUT.vertex = vPosition;

                float2 pixelSize = vPosition.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (v.vertex.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                OUT.texcoord = v.texcoord;
                OUT.mask = float4(v.vertex.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN ) : SV_Target
            {
                //Round up the alpha color coming from the interpolator (to 1.0/256.0 steps)
                //The incoming alpha could have numerical instability, which makes it very sensible to
                //HDR color transparency blend, when it blends with the world's texture.
                const half alphaPrecision = half(0xff);
                const half invAlphaPrecision = half(1.0/alphaPrecision);
                IN.color.a = round(IN.color.a * alphaPrecision)*invAlphaPrecision;

                float2 appendResult10_g3 = (float2(1.0 , 1.0));
                float2 temp_output_11_0_g3 = ( abs( (IN.texcoord.xy*2.0 + -1.0) ) - appendResult10_g3 );
                float2 break16_g3 = ( 1.0 - ( temp_output_11_0_g3 / fwidth( temp_output_11_0_g3 ) ) );
                float2 appendResult10_g4 = (float2(0.9 , 0.9));
                float2 temp_output_11_0_g4 = ( abs( (IN.texcoord.xy*2.0 + -1.0) ) - appendResult10_g4 );
                float2 break16_g4 = ( 1.0 - ( temp_output_11_0_g4 / fwidth( temp_output_11_0_g4 ) ) );
                Gradient gradient36 = NewGradient( 0, 7, 2, float4( 1, 0, 0.1680102, 0.002944991 ), float4( 1, 0.4462366, 0, 0.1617609 ), float4( 1, 0.9233207, 0, 0.320592 ), float4( 0.4223243, 1, 0, 0.4941176 ), float4( 0, 1, 0.9413087, 0.6588235 ), float4( 0, 0.7770243, 1, 0.8294194 ), float4( 0, 0.4755411, 1, 1 ), 0, float2( 1, 0 ), float2( 1, 1 ), 0, 0, 0, 0, 0, 0 );
                float2 texCoord32 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
                float cos26 = cos( ( _Time.y * 2.0 ) );
                float sin26 = sin( ( _Time.y * 2.0 ) );
                float2 rotator26 = mul( texCoord32 - float2( 0.5,0.5 ) , float2x2( cos26 , -sin26 , sin26 , cos26 )) + float2( 0.5,0.5 );
                float4 temp_output_34_0 = ( ( saturate( min( break16_g3.x , break16_g3.y ) ) - saturate( min( break16_g4.x , break16_g4.y ) ) ) * SampleGradient( gradient36, rotator26.x ) );
                

                half4 color = ( _Emission * temp_output_34_0 );

                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                color.rgb *= color.a;

                return color;
            }
        ENDCG
        }
    }
    CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19302
Node;AmplifyShaderEditor.TimeNode;27;388.8455,796.1005;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;459.045,984.9005;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;641.045,878.0009;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;32;481.1448,664.8009;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RotatorNode;26;763.2449,706.4003;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;23;469.0575,214.0759;Inherit;True;Rectangle;-1;;3;6b23e0c975270fb4084c354b2c83366a;0;3;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;24;479.0575,437.0759;Inherit;True;Rectangle;-1;;4;6b23e0c975270fb4084c354b2c83366a;0;3;1;FLOAT2;0,0;False;2;FLOAT;0.9;False;3;FLOAT;0.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientNode;36;978.9034,634.6049;Inherit;False;0;7;2;1,0,0.1680102,0.002944991;1,0.4462366,0,0.1617609;1,0.9233207,0,0.320592;0.4223243,1,0,0.4941176;0,1,0.9413087,0.6588235;0,0.7770243,1,0.8294194;0,0.4755411,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;25;760.0575,326.0759;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GradientSampleNode;37;1261.558,645.8663;Inherit;True;2;0;OBJECT;;False;1;FLOAT;0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;1087.771,398.1036;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;44;1357.069,154.2571;Inherit;False;Property;_Emission;Emission;0;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;43;1267.19,453.0058;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;1478.489,338.017;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;1704.563,209.2361;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;1927.437,367.1009;Float;False;True;-1;2;ASEMaterialInspector;0;3;UID;5056123faa0c79b47ab6ad7e8bf059a4;True;Default;0;0;Default;2;True;True;3;1;False;;10;False;;8;5;False;;1;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;True;True;True;True;True;0;True;_ColorMask;False;False;False;False;False;False;False;True;True;0;True;_Stencil;255;True;_StencilReadMask;255;True;_StencilWriteMask;0;True;_StencilComp;0;True;_StencilOp;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;0;True;unity_GUIZTestMode;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;28;0;27;2
WireConnection;28;1;29;0
WireConnection;26;0;32;0
WireConnection;26;2;28;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;37;0;36;0
WireConnection;37;1;26;0
WireConnection;34;0;25;0
WireConnection;34;1;37;0
WireConnection;38;0;34;0
WireConnection;38;1;43;0
WireConnection;45;0;44;0
WireConnection;45;1;34;0
WireConnection;0;0;45;0
ASEEND*/
//CHKSM=90B7DE8CEED068826FDB0BBB1A0BACBB184E263A