﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ShowDepth"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	CGINCLUDE
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
		sampler2D _SSCollTex;
    	sampler2D _CameraGBufferTexture2;
    	sampler2D _DepthNormalBack,_DepthNormalFront;
    	sampler2D_float _CameraDepthTexture;
		float4 _MainTex_ST;
		
		v2f vert (appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
			return o;
		}
		
		fixed4 frag (v2f i) : SV_Target
		{
			// sample the texture
			half4 c = tex2D(_DepthNormalBack, i.uv);
			half4 c1 = tex2D(_DepthNormalFront, i.uv);
			return lerp(c, c1, frac(_Time.y)>0.5);
		}
	ENDCG
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100 ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			ENDCG
		}
	}
}
