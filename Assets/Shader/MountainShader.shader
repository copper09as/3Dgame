// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ChinesePainting/MountainShader" 
{
	Properties 
	{
		_MainTex("Main Texture", 2D) = "white" {}

		[Header(OutLine)]
		// Stroke Color
		_StrokeColor ("Stroke Color", Color) = (0,0,0,1)
		// Noise Map
		_OutlineNoise ("Outline Noise Map", 2D) = "white" {}
		_Outline ("Outline", Range(0, 1)) = 0.1
		// Outside Noise Width
		_OutsideNoiseWidth ("Outside Noise Width", Range(1, 2)) = 1.3
		_MaxOutlineZOffset ("Max Outline Z Offset", Range(0,1)) = 0.5

		[Header(Interior)]
		_Ramp ("Ramp Texture", 2D) = "white" {}
		// Stroke Map
		_StrokeTex ("Stroke Noise Tex", 2D) = "white" {}
		_InteriorNoise ("Interior Noise Map", 2D) = "white" {}
		// Interior Noise Level
		_InteriorNoiseLevel ("Interior Noise Level", Range(0, 1)) = 0.15
		// Guassian Blur
		radius ("Guassian Blur Radius", Range(0,60)) = 30
        resolution ("Resolution", float) = 800
        hstep("HorizontalStep", Range(0,1)) = 0.5
        vstep("VerticalStep", Range(0,1)) = 0.5  

	}
    SubShader 
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}

		// the first outline pass
		Pass 
		{
			NAME "OUTLINE"
			Cull Front
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			float _Outline;
			float4 _StrokeColor;
			sampler2D _OutlineNoise;
			half _MaxOutlineZOffset;

			struct a2v 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			}; 
			
			struct v2f 
			{
			    float4 pos : SV_POSITION;
			};
			
			v2f vert (a2v v) 
			{
				// fetch Perlin noise map here to map the vertex
				// add some bias by the normal direction
	
				float4 burn = tex2Dlod(_OutlineNoise, v.vertex);

				v2f o = (v2f)0;
				float3 scaledir = mul((float3x3)UNITY_MATRIX_MV, v.normal);
				scaledir += 0.5;
				scaledir.z = 0.01;
				scaledir = normalize(scaledir);

				// camera space
				float4 position_cs = mul(UNITY_MATRIX_MV, v.vertex);
				position_cs /= position_cs.w;

				float3 viewDir = normalize(position_cs.xyz);
				float3 offset_pos_cs = position_cs.xyz + viewDir * _MaxOutlineZOffset;
                
                // unity_CameraProjection[1].y = fov/2
				float linewidth = -position_cs.z / unity_CameraProjection[1].y;
				linewidth = sqrt(linewidth);
				position_cs.xy = offset_pos_cs.xy + scaledir.xy * linewidth * burn.x * _Outline ;
				position_cs.z = offset_pos_cs.z;

				o.pos = mul(UNITY_MATRIX_P, position_cs);

				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target 
			{
				fixed4 c = _StrokeColor;
				return c;
			}
			ENDCG
		}
		
		// the second outline pass for random part, a little bit wider than last one
		Pass 
		{
			NAME "OUTLINE 2"
			Cull Front
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			float _Outline;
			float4 _StrokeColor;
			sampler2D _OutlineNoise;
			float _OutsideNoiseWidth;
			half _MaxOutlineZOffset;

			struct a2v 
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0; 
			}; 
			
			struct v2f 
			{
			    float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (a2v v) 
			{
				// fetch Perlin noise map here to map the vertex
				// add some bias by the normal direction
				float4 burn = tex2Dlod(_OutlineNoise, v.vertex);

				v2f o = (v2f)0;

				float3 scaledir = mul((float3x3)UNITY_MATRIX_MV, v.normal);
				scaledir += 0.5;
				scaledir.z = 0.01;
				scaledir = normalize(scaledir);

				float4 position_cs = mul(UNITY_MATRIX_MV, v.vertex);
				position_cs /= position_cs.w;

				float3 viewDir = normalize(position_cs.xyz);
				float3 offset_pos_cs = position_cs.xyz + viewDir * _MaxOutlineZOffset;

				float linewidth = -position_cs.z / unity_CameraProjection[1].y;
				linewidth = sqrt(linewidth);
				position_cs.xy = offset_pos_cs.xy + scaledir.xy * linewidth * burn.y * _Outline * 1.1 * _OutsideNoiseWidth ;
				position_cs.z = offset_pos_cs.z;

				o.pos = mul(UNITY_MATRIX_P, position_cs);

				o.uv = v.texcoord.xy;
				


				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target 
			{
				// clip random outline here
				fixed4 c = _StrokeColor;
				fixed3 burn = tex2D(_OutlineNoise, i.uv).rgb;
				if (burn.x > 0.5)
					discard;
				return c;
			}
			ENDCG
		}
		
		// the interior pass
		Pass
{
Name"INTERIOR"
    Tags
{"LightMode" = "ForwardBase"
}

Cull off

ZWrite Off

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
#include "UnityCG.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;

sampler2D _CameraDepthTexture;
sampler2D _CameraNormalsTexture;

float4 _Params;
float _BlurIntensity;
float _DepthSensitivity;
float _NormalSensitivity;
float _InteriorAlpha;

struct a2v
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 pos : SV_POSITION;
    float2 uv : TEXCOORD0;
    float2 screenUV : TEXCOORD1;
};

v2f vert(a2v v)
{
    v2f o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);


    o.screenUV = o.pos.xy / o.pos.w;
    o.screenUV = o.screenUV * 0.5 + 0.5;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float2 tc = i.uv;
    float hstep = _BlurIntensity / _ScreenParams.x;
    float vstep = _BlurIntensity / _ScreenParams.y;

    fixed4 sum = fixed4(0.0, 0.0, 0.0, 0.0);

        // 高斯模糊采样（彩色水墨感）
    sum += tex2D(_MainTex, float2(tc.x - 4.0 * hstep, tc.y - 4.0 * vstep)) * 0.0162162162;
    sum += tex2D(_MainTex, float2(tc.x - 3.0 * hstep, tc.y - 3.0 * vstep)) * 0.0540540541;
    sum += tex2D(_MainTex, float2(tc.x - 2.0 * hstep, tc.y - 2.0 * vstep)) * 0.1216216216;
    sum += tex2D(_MainTex, float2(tc.x - 1.0 * hstep, tc.y - 1.0 * vstep)) * 0.1945945946;
    sum += tex2D(_MainTex, float2(tc.x, tc.y)) * 0.2270270270;
    sum += tex2D(_MainTex, float2(tc.x + 1.0 * hstep, tc.y + 1.0 * vstep)) * 0.1945945946;
    sum += tex2D(_MainTex, float2(tc.x + 2.0 * hstep, tc.y + 2.0 * vstep)) * 0.1216216216;
    sum += tex2D(_MainTex, float2(tc.x + 3.0 * hstep, tc.y + 3.0 * vstep)) * 0.0540540541;
    sum += tex2D(_MainTex, float2(tc.x + 4.0 * hstep, tc.y + 4.0 * vstep)) * 0.0162162162;

    fixed4 col = sum;

        // 混合深度和法线信息来控制透明度（保留水墨韵染的透明感）
    float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.screenUV));
    float3 normal = tex2D(_CameraNormalsTexture, i.screenUV).rgb * 2 - 1;

    float normalDiff = saturate(dot(normal, float3(0, 0, 1)));
    float depthDiff = abs(depth - Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.screenUV)));

    float alpha = saturate((1 - depthDiff * _DepthSensitivity) * (normalDiff * _NormalSensitivity));
    col.a = alpha * _InteriorAlpha;

    return col;
}
    ENDCG
}

	}
	FallBack "Diffuse"
}
