#if !defined(LIGHTINGS_INCLUDE)
#define LIGHTINGS_INCLUDE

#include "UnityPBSLighting.cginc"
#include "AutoLight.cginc"

float4 _Tint;
float _Metallic;
float _Smoothness;

sampler2D _MainTex, _DetailTex;
float4 _MainTex_ST, _DetailTex_ST;

sampler2D _NormalMap, _DetailNormalMap;
float _BumpScale, _DetailBumpScale;

struct VertexData
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
    float2 uv : TEXCOORD0;
};
struct Interpolators
{
    float4 pos : SV_POSITION;
    float4 uv : TEXCOORD0;
    float3 normal : TEXCOORD1;
    
    #if defined(BINORMAL_PER_FRAGMENT)
        float4 tangent : TEXCOORD2;
    #else
        float3 tangent : TEXCOORD2;
        float3 binormal : TEXCOORD3;
    #endif
        
    float3 worldPos : TEXCOORD4;

    SHADOW_COORDS(5)
    /*#if defined(SHADOWS_SCREEN)
        float4 shadowCoordinates : TEXCOORD5;
    #endif*/
    
    #if defined(VERTEXLIGHT_ON)
        float3 vertexLightColor : TEXCOORD6;
    #endif
    //float2 uv : TEXCOORD0;
};

void ComputeVertexLightColor(inout Interpolators i)
{
    #if defined(VERTEXLIGHT_ON)
        i.vertexLightColor = Shade4PointLights(
            unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
            unity_LightColor[0].rgb, unity_LightColor[1].rgb,
            unity_LightColor[2].rgb, unity_LightColor[3].rgb,
            unity_4LightAtten0, i.worldPos, i.normal
        );
    #endif
    /*float3 lightPos = float3(unity_4LightPosX0.x, unity_4LightPosY0.x, unity_4LightPosZ0.x);
    float3 lightVec = lightPos - i.worldPos;
    float3 lightDir = normalize(lightVec);
    float ndotl = DotClamped(i.normal, lightDir);
    float attenuation = 1 / (1 + dot(lightVec, lightVec) * unity_4LightAtten0.x);
    i.vertexLightColor = unity_LightColor[0].rgb * ndotl * attenuation;*/
}

float3 CreateBinormal(float3 normal, float3 tangent, float binormalSign)
{
    return cross(normal, tangent.xyz) * (binormalSign * unity_WorldTransformParams);
}

Interpolators VertexProgram(VertexData v)
{
    Interpolators i;
    i.pos = UnityObjectToClipPos(v.vertex);
    i.worldPos = mul(unity_ObjectToWorld, v.vertex);
    i.normal = UnityObjectToWorldNormal(v.normal);
    //i.normal = mul(transpose(unity_WorldToObject), float4(v.normal, 0)); Unity object to world normal
    
    #if defined(BINORMAL_PER_FRAGMENT)
        i.tangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
    #else
        i.tangent = float3(UnityObjectToWorldDir(v.tangent.xyz));
        i.binormal = CreateBinormal(i.normal, i.tangent, v.tangent.w);
    #endif
    
    i.uv.xy = TRANSFORM_TEX(v.uv, _MainTex); // one UV variable for 2 different textures
    i.uv.zw = TRANSFORM_TEX(v.uv, _DetailTex); // XY channel for the main texture, ZW channel for the detail texture

    TRANSFER_SHADOW(i);
    /*i.shadowCoordinates.xy = (float2(i.position.x, -i.position.y) + i.position.w) * 0.5; 
    i.shadowCoordinates.zw = i.position.zw;*/
    
    ComputeVertexLightColor(i);
    return i;
}

UnityLight CreateLight(Interpolators i)
{
    UnityLight light;

    #if defined(POINT) || defined (POINT_COOKIE) || defined(SPOT)
        light.dir = normalize(_WorldSpaceLightPos0 - i.worldPos);
    #else
        light.dir = _WorldSpaceLightPos0;
    #endif
    
    UNITY_LIGHT_ATTENUATION(attenuation, i, i.worldPos);
    //float attenuation = tex2D(_ShadowMapTexture, i.shadowCoordinates.xy / i.position.w); Screen to space
    
    light.color = _LightColor0.rgb * attenuation;
    return light;
    /*float3 lightDir = _WorldSpaceLightPos0 - worldPos; (1) Unity light attenuation manually
    float attention = 1 / (1 + dot(lightDir, lightDir));*/
}
UnityIndirect CreateIndirectLight(Interpolators i)
{
    UnityIndirect indirectLight;
    indirectLight.diffuse = 0;
    indirectLight.specular = 0;
    
    #if defined(VERTEXLIGHT_ON)
        indirectLight.diffuse = i.vertexLightColor;
    #endif

    #if defined(FORWARD_BASE_PASS)
        indirectLight.diffuse += max(0, ShadeSH9(float4 (i.normal, 1)));
    #endif
    
    return indirectLight;
}

void InitializeFragmentNormal(inout Interpolators i)
{
    float3 mainNormal = UnpackScaleNormal(tex2D(_NormalMap, i.uv.xy), _BumpScale);
    float3 detailNormal = UnpackScaleNormal(tex2D(_DetailNormalMap, i.uv.zw), _DetailBumpScale);
    float3 tangentSpaceNormal = BlendNormals(mainNormal, detailNormal);
    
    #if defined(BINORMAL_PER_FRAGMENT)
        float3 binormal = CreateBinormal(i.normal, i.tangent.xyz, i.tangent.w);
    #else
        float3 binormal = i.binormal;
    #endif
    
    i.normal = normalize(
        tangentSpaceNormal.x * i.tangent +
        tangentSpaceNormal.y * binormal +
        tangentSpaceNormal.z * i.normal
        );

    /*i.normal = float3(mainNormal.xy + detailNormal.xy, mainNormal.z * detailNormal.z); (3) Blend normals (func have better optimisation)
    i.normal = normalize(i.normal);*/

    /*i.normal.xy = tex2D(_NormalMap, i.uv).wy * 2 - 1; (2) Unpack scale normal (func have better optimisation)
    i.normal.xy *= _BumpScale;
    i.normal.z = sqrt(1 - saturate(dot(i.normal.xy, i.normal.xy)));*/
    
    /*float2 du = float2(_HeightMap_TexelSize.x * 0.5, 0); (1) Without scales
    float u1 = tex2D(_HeightMap, i.uv - du);
    float u2 = tex2D(_HeightMap, i.uv + du);
    float2 dv = float2(_HeightMap_TexelSize.y * 0.5, 0);
    float v1 = tex2D(_HeightMap, i.uv - dv);
    float v2 = tex2D(_HeightMap, i.uv + dv);
    i.normal = float3(u1 - u2, 1, v1 - v2);*/
}

float4 FragmentProgram(Interpolators i): SV_TARGET
{
    InitializeFragmentNormal(i);
    
    float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
				
    float oneMinusReflectivity;
    float3 specularTint;
    float3 albedo = tex2D(_MainTex, i.uv.xy).rgb * _Tint.rgb;
    albedo *= tex2D(_DetailTex, i.uv.zw) * unity_ColorSpaceDouble;
    albedo = DiffuseAndSpecularFromMetallic(albedo, _Metallic, specularTint, oneMinusReflectivity);

    return UNITY_BRDF_PBS(albedo, specularTint, oneMinusReflectivity, _Smoothness,
        i.normal, viewDir, CreateLight(i), CreateIndirectLight(i));
    // //float3 reflectionDir = reflect(-lightDir, i.normal); (1) Only reflect
				
    //float3 halfVector = normalize(lightDir + viewDir); (2) Half vector method to calculate angle
				
    //float3 specular = specularTint.rgb * lightColor * pow(DotClamped(halfVector, i.normal), _Smoothness * 100); (3) Unity brdf pbs func (func more complex)
    //float3 diffuse = albedo * lightColor * DotClamped(lightDir, i.normal);
}
#endif