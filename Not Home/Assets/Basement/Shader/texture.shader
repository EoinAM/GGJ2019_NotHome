Shader "Custom/texture"  //name of the Shader
{
	Properties//user inputed properties 
	{
		_BumpMap("Normal Map", 2D) = "bump" {}
		_Color("Diffuse Material Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Pass//pass foor pixle light sources.
		{
			Tags{ "LightMode" = "ForwardAdd" }// for per pixle light source  calculation.
			CGPROGRAM
			#pragma vertex vert   //specifies the vertex funtion as a vertex shader.
			#pragma fragment frag//specifies the fragment function asa fragment shader.
			#include "UnityCG.cginc"

			uniform sampler2D _BumpMap;
			uniform float4 _BumpMap_ST;
			uniform float4 _Color;//colour of object


			struct vertexInput //input variables  for vertexs.
			{
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};
			struct vertexOutput //output variables for vertexs.
			{
				float4 pos : SV_POSITION;
				float4 posWorld : TEXCOORD0;
				float4 tex : TEXCOORD1;
				float3 tangentWorld : TEXCOORD2;
				float3 normalWorld : TEXCOORD3;
				float3 binormalWorld : TEXCOORD4;
			};

			vertexOutput vert(vertexInput input)//output function for vertext calulations. 
			{
				vertexOutput output;//ouput. 

				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;

				output.tangentWorld = normalize(mul(modelMatrix, float4(input.tangent.xyz, 0.0)).xyz);//tangent for world.
				output.normalWorld = normalize(mul(float4(input.normal, 0.0), modelMatrixInverse).xyz);//normal fopr world.
				output.binormalWorld = normalize(cross(output.normalWorld, output.tangentWorld)* input.tangent.w);//ads normals for points on texture map.
				output.posWorld = mul(modelMatrix, input.vertex);//postion of ouput in world.
				output.tex = input.texcoord;//texture coordinate output.
				output.pos = UnityObjectToClipPos(input.vertex);//postion ouput.
				return output;//return ouput.
			}
			float4 frag(vertexOutput input) : COLOR // output for fragment calculations.
			{
				float3 lightDirection;//direction of light source. 
				float3 specularReflection;//size of light reflected. 
				float3 vertexToLightSource;//distance of light to vertex.
				float3 viewDirection;//view direction
				float3 normalDirection;//direction of normal. 
				float3x3 local2WorldTranspose;//transpose of object
				float3 localCoords;//local cords of object.
				float4 encodedNormal;// normals for bumps on texture map.
				float3 diffuseReflection;//colour refectuion size.

				encodedNormal = tex2D(_BumpMap,_BumpMap_ST.xy * input.tex.xy + _BumpMap_ST.zw);//encoded date from texture map.
				localCoords = float3(2.0 * encodedNormal.a - 1.0,2.0 * encodedNormal.g - 1.0, 0.0);// cordinates on map to object. 
				localCoords.z = sqrt(1.0 - dot(localCoords, localCoords));
				local2WorldTranspose = float3x3(input.tangentWorld,input.binormalWorld,input.normalWorld);//transpose of object in world.
				normalDirection = normalize(mul(localCoords, local2WorldTranspose));//normal direction of light.
				viewDirection = normalize(_WorldSpaceCameraPos - input.posWorld.xyz);//view direction.
				vertexToLightSource =_WorldSpaceLightPos0.xyz - input.posWorld.xyz;//vertex distance to light source.
				lightDirection = normalize(vertexToLightSource);//direction of light.
				diffuseReflection = _Color.rgb* max(0.0, dot(normalDirection, lightDirection));//colour over object dependin onlight. 
				specularReflection = pow(max(0.0, dot(reflect(-lightDirection, normalDirection),viewDirection)), 10);//refelction size and spread and colour. 

				return float4(diffuseReflection + specularReflection, 1.0);//return. 
			}
		ENDCG
		}
	}
}