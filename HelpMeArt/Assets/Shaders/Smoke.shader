﻿
Shader "Test/Smoke"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_StampTex ("Stamp Texture", 2D) = "white" {}
		_Pixels("Number of Pixels", Float) = 1028
		_Dissipation("Rate of Disperstion", Range(0,1)) = 4
		_Minimum("Minimum Dissipation Size", Range(0,1)) = 0.003

		_SmokeCentre("Smoke Point", Vector) = (0,0,0,0)
		_SmokeRaduis("Smoke Size", Range(0,1)) = 0.0089
		_PaintColor("ColorOfPaint", Color) = (1,1,1,1)
		_ContactPointsLength("Number of Contact Points", Float) = 1.0
	}
	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform int _ContactPointsLength;
			float3 _Array[3];
			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _StampTex;
			float4 _StampTex_ST;

			uniform float _Pixels;
			uniform float _Dissipation;
			uniform float _Minimum;

			uniform float _SmokeRaduis;
			uniform float2 _SmokeCentre;
			uniform float4 _PaintColor;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 wPos : TEXCOORD1;
			};

			v2f vert (appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.wPos =  mul(unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{

				fixed2 uv = round(i.uv * _Pixels) / _Pixels;

				//Neighboring cells
			    half s = 1 / _Pixels;

				float cl = tex2D(_MainTex, uv + fixed2(-s, 0)).a; // F[ x - 1, y ] : Centre Left
				float tc = tex2D(_MainTex, uv + fixed2(0, -s)).a; // F[ x, y - 1 ] : Top Left
				float cc = tex2D(_MainTex, uv + fixed2(0,  0)).a; // F[ x,  y    ] : Centre Centre
				float bc = tex2D(_MainTex, uv + fixed2(0, +s)).a; //F[ x, y + 1] : Bottom Centre
				float cr = tex2D(_MainTex, uv + fixed2(+s, 0)).a; // F[ x, y] : Centre Right

				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 stamp =  tex2D(_StampTex, (i.uv.xy / _SmokeRaduis - ((_SmokeCentre / _SmokeRaduis) - .5)));

				//Diffusion step (HAWT HAWT HAWT)
				float factor = _Dissipation * (.1 * (cl + tc + bc + cr) - cc ) ;

				//Minimum Flow
				if(factor >= -_Minimum && factor < 0.0)
				factor = -_Minimum;

				cc += factor;
				stamp += factor;

				float3 wPos = i.wPos;

				for(int i = 0; i < _ContactPointsLength; i++)
				{
					if(distance(wPos, _Array[i].xy) < _SmokeRaduis)
					{
					cc = 1 / distance(wPos, _Array[i].xy); 
					_PaintColor =1 / distance(wPos, _Array[i].xy);
					//discard;
					}
				}
			
				//if(stamp.a < .9)
				//discard;


				return _PaintColor * float4(cc, cc, cc, cc);
			}
			ENDCG
		}
	}
}
