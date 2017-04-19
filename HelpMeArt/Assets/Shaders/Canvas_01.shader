// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

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

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
		
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			sampler2D _DynamicTex;
			float4 _DynamicTex_ST;

			float4 blend(float4 A, float4 B)
			{
			   float4 C;
			   C.a = A.a + (1 - A.a) * B.a;
			   C.rgb = (1 / C.a) * (A.a * A.rgb + (1 - A.a) * B.a * B.rgb);
			   return C;
			}

			v2f vert (appdata_full v)
			{
				v2f o;
				
				//o.wPos =  mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture 
				fixed4 dynoSmoke = tex2D(_DynamicTex, i.uv);
				fixed4 main = tex2D(_MainTex, i.uv);

				return blend(dynoSmoke, main);
			}
			ENDCG
		}

	}
}
