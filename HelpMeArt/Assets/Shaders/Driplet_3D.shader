Shader "Eugene/Test/Driplet_3D"
{
	Properties
	{
		_Color ("Droplit Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Cull Off
		 Fog { Mode Off }
		Pass
		{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag		
			#include "UnityCG.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 posWorld : TEXCOORD1;
				float4 screenPosition : TEXCOORD2;
				float3 worldDirection : TEXCOORD3;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			uniform float maxDepth;
			sampler2D _CameraDepthTexture;
			fixed4 _CameraDepthTexture_TexelSize;

			uniform float4 _Color;
			v2f vert (appdata_base v)
			{
				v2f o;
				o.worldDirection = mul(unity_ObjectToWorld, v.vertex).xyz - _WorldSpaceCameraPos;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				o.posWorld = mul(unity_WorldToObject, v.vertex);
				o.screenPosition = o.vertex;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//Compute projective scaling factor
				float perspectiveDivide = 1.0f / i.screenPosition.w;

				//Scale our view ray to unit depth
				float3 direction = i.worldDirection * perspectiveDivide;

				//calculate our Uv within the screen (for reading depth buffer)
				float2 screenUV = (i.screenPosition.xy * perspectiveDivide) * 0.5 + 0.5;
			    screenUV = UnityStereoTransformScreenSpaceTex(screenUV); 
				screenUV.y = 1.0f - screenUV.y;

				//Read depth, linearizing into theworldspace units.
				fixed depth =  LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, screenUV))).x;

				//Advance by depth along our view ray drom the camera position
				//this is the worldspace coordinate of the corresponding fragment
				//we retrieved from the depth buffer
				float3 worldspace = direction * depth + _WorldSpaceCameraPos;

				//Draw a worldspace tartan pattern over the scene to demeonstrate
				float4 tartan = float4(frac((worldspace)), 1.0);

				half3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;

				fixed4 lightDir = fixed4(
				normalize(lerp(_WorldSpaceLightPos0.xyz, fragmentToLightSource, _WorldSpaceLightPos0.w)),
				lerp(1.0, 1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w)
				);

				float3 N; 

				N.xy = i.uv * 2.0 - 1.0;
				float2 r2 = dot(N.xy, N.xy);

				if(r2.x > (1.0) || r2.y > (1.0)) discard;
				N.z = -sqrt(1.0 - r2);

				//Calculate depth
				float4 pixelPos = float4(_WorldSpaceCameraPos + N * .05, 1.0);
				float4 clipSpacePos = i.vertex;
				// apply fog
				float diffuse = max(0.0, dot(N, lightDir));
				//_Color
				return float4(worldspace.rgb, 1.0);
			}
			ENDCG
		}
	}
}
