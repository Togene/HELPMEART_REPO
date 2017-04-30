// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Test/Smoke"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_StampTex ("Stamp Texture", 2D) = "white" {}
		_Pixels("Number of Pixels", Float) = 1028
		_Dissipation("Rate of Disperstion", Range(0,1)) = 4
		_Minimum("Minimum Dissipation Size", Range(0,1)) = 0.003
		_Transmission("Transmission", Vector) = (1,1,1,1)
		_SmokeCentre("Smoke Point", Vector) = (0,0,0,0)
		_SmokeRaduis("Smoke Size", Range(0,1)) = 0.0089
		_PaintColor("ColorOfPaint", Color) = (1,1,1,1)
		_ContactPointsLength("Number of Contact Points", Float) = 1.0
		[KeywordEnum(Draw, NoDraw)] _DRAWMode ("Drawing Mode", Float) = 1.0
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
			#pragma shader_feature _DRAWMode_DRAW _DRAWMode_NODRAW

			uniform int _ContactPointsLength;
			float3 _Array[6];
			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _StampTex;
			float4 _StampTex_ST;
			uniform half4 _Transmission;
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
				o.wPos =  mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_TARGET
			{

				fixed2 uv = (i.uv * _Pixels) / _Pixels;
			//
			////Neighboring cells
			half s = 1 / _Pixels;
			//
			float cl = tex2D(_MainTex, uv + fixed2(-s, 0)).a;	// Centre Left
			float tc = tex2D(_MainTex, uv + fixed2(-0, -s)).a;	// Top Centre
			float cc = tex2D(_MainTex, uv + fixed2(0, 0)).a;	// Centre Centre
			float bc = tex2D(_MainTex, uv + fixed2(0, +s)).a;	// Bottom Centre
			float cr = tex2D(_MainTex, uv + fixed2(+s, 0)).a;	// Centre Right

			float factor =
				_Dissipation *
				(
				(
					cl * _Transmission.x +
					tc * _Transmission.y +
					bc * _Transmission.z +
					cr * _Transmission.w
					)
					- (_Transmission.x + _Transmission.y + _Transmission.z + _Transmission.w) * cc
					);

			//Minimum Flow
			if(factor >= -_Minimum && factor < 0.0)
			factor = -_Minimum;

			cc += factor;

			float3 wPos = i.wPos;

			for (int i = 0; i < _ContactPointsLength; i++)
			{
				if (distance(wPos, _Array[i].xy) < _SmokeRaduis)
				{
				cc = 1;
				_PaintColor.rgb = _PaintColor.rgb;
				//discard;
				}
			}

			//if(stamp.a < .9)
			//discard;


			return 
				float4(_PaintColor.rgb * cc, cc);
			}
			ENDCG
		}
	}
}
