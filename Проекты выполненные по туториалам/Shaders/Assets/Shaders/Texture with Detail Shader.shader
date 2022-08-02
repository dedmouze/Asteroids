Shader "Custom/Texture with Detail Shader"
{
	Properties
	{
		_Tint ("Tint", Color) = (1, 1, 1, 1)
		_MainTex ("Texture", 2D) = "white" {}
		_DetailTex("DetailTex", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex VertexProgram
			#pragma fragment FragmentProgram

			#include "UnityCG.cginc"

			float4 _Tint;
			sampler2D _MainTex, _DetailTex;
			float4 _MainTex_ST, _DetailTex_ST;

			struct Interpolators
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvDetail : TEXCOORD1;
			};

			struct VertexData
			{
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			Interpolators VertexProgram(VertexData v)
			{
				Interpolators i;
				i.position = UnityObjectToClipPos(v.position);
				//i.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
				i.uv = TRANSFORM_TEX(v.uv, _MainTex); // Macro ^
				i.uvDetail = TRANSFORM_TEX(v.uv, _DetailTex);
				return i;
			}
			float4 FragmentProgram(Interpolators i) : SV_TARGET
			{
				float4 color = tex2D(_MainTex, i.uv) * _Tint;
				color *= tex2D(_DetailTex, i.uvDetail) * unity_ColorSpaceDouble;
				return color;
			}
			
			ENDCG
		}
	}
}	