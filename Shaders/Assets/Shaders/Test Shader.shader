// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Test Shader"
{
    Properties 
    {
		_Tint ("Tint", Color) = (1, 1, 1, 1)
        _MainTex("MainTexture", 2D) = "white" {}
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

            sampler2D _MainTex;

            struct Interpolators
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            struct VertexDate
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            Interpolators VertexProgram(VertexDate v)
            {
                Interpolators i;
                i.position = UnityObjectToClipPos(v.position);
                i.uv = v.uv;
                return i;
            }
            float4 FragmentProgram(Interpolators i) : SV_TARGET
            {
                float tex = tex2D(_MainTex, i.uv);
                return tex;
            }
            ENDCG
        }
    }
}
