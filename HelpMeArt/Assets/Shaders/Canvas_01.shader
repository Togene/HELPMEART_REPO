// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Test/Canvas_01"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DynamicTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma glsl
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float _Smoothing;
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
				float4 wPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			sampler2D _DynamicTex;
			float4 _DynamicTex_ST;

			fixed4 dyno;

			v2f vert (appdata_full v)
			{
				v2f o;
				
				o.wPos =  mul(unity_ObjectToWorld, v.vertex);
				o.pos = mul(UNITY_MATRIX_MVP,   v.vertex);
				o.uv = v.texcoord;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 main = tex2D(_MainTex, i.uv);
				fixed4 dynoSmoke = tex2D(_DynamicTex, i.uv);

				fixed4 col = main;
				return col;
			}
			ENDCG
		}

	}
}
