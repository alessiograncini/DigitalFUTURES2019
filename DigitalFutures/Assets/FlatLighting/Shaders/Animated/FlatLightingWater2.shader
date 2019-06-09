// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "FlatLighting/Animated/Water2" {
  	Properties {
  		_LightNegativeX ("Light -X", Color) = (1,1,1,1)
		_LightNegative2X ("Light2 -X", Color) = (1,1,1,1)
		_GradientOriginOffsetNegativeX ("Gradient Width", Float) = 3.0
		_GradientWidthNegativeX ("Gradient Offset", Float) = 0.0

		_LightPositiveX ("Light X", Color) = (1,1,1,1)
		_LightPositive2X ("Light2 X", Color) = (1,1,1,1)
		_GradientOriginOffsetPositiveX ("Gradient Width", Float) = 3.0
		_GradientWidthPositiveX ("Gradient Offset", Float) = 0.0

		_LightNegativeZ ("Light -Z", Color) = (1,1,1,1)
		_LightNegative2Z ("Light2 -Z", Color) = (1,1,1,1)
		_GradientOriginOffsetNegativeZ ("Gradient Width", Float) = 3.0
		_GradientWidthNegativeZ ("Gradient Offset", Float) = 0.0

		_LightPositiveZ ("Light Z", Color) = (1,1,1,1)
		_LightPositive2Z ("Light2 Z", Color) = (1,1,1,1)
		_GradientOriginOffsetPositiveZ ("Gradient Width", Float) = 3.0
		_GradientWidthPositiveZ ("Gradient Offset", Float) = 0.0


		_LightNegativeY ("Light -Y", Color) = (1,1,1,1)
		_LightNegative2Y ("Light2 -Y", Color) = (1,1,1,1)
		_GradientOriginOffsetNegativeY ("Gradient Width", Float) = 3.0
		_GradientWidthNegativeY ("Gradient Offset", Float) = 0.0

		_LightPositiveY ("Light Y", Color) = (1,1,1,1)
		_LightPositive2Y ("Light2 Y", Color) = (1,1,1,1)
		_GradientOriginOffsetPositiveY ("Gradient Width", Float) = 3.0
		_GradientWidthPositiveY ("Gradient Offset", Float) = 0.0

		_MainTex("Main Texture", 2D) = "white" {}
		_LightAndTextureMix("Texture Power", Range(0.0, 1.0)) = 1.0

		_GradienColorGoal ("Gradient Goal Color", Color) = (1,1,1,1)
		_GradientBlending ("Gradient Blending" , Range(0.0, 1.0)) = 0.0
		_GradientUnitAxis ("Gradient Axis", Vector) = (0,1,0,0)
		_GradientWidth ("Gradient Width", Float) = 3.0
		_GradientOffset ("Gradient Offset", Float) = 0.0

		_UVChannel ("Lightmap UV Channel", Int) = 0
		_CustomLightmap ("Lightmap (Greyscale)", 2D) = "white" {}
		_ShadowTint ("Shadow Tint", Color) = (1,1,1,1)
		_ShadowBoost ("Shadow Boost", Range(0.0, 1.0)) = 0.0

		_Ambient_Light ("Ambient Light", Color) = (1,1,1,1)

		_BlendedLightColor ("Blended Light Color", Color) = (1,1,1,1)
		_BlendedLightIntensities ("Blended Light Intensities", Vector) = (0,1,0,0)
		[Toggle] _BlendedLightSmoothness ("Blended Light Smoothness", Float) = 0.0

		_VertexAnimSpeed ("Movement Speed", Vector) = (0,0,0,0)
 		_VertexAnimRepeat ("Movement Repeat", Vector) = (0,0,0,0)
 		_VertexAnimScale ("Movement Scale", Vector) = (0,0,0,0)
 		_NormalAnimSpeed("Flow Speed", Vector) = (0,0,0,0)
 		_NormalAnimRepeat ("Flow Repeat", Vector) = (0,0,0,0)
 		_NormalAnimScale ("Flow Scale", Vector) = (0,0,0,0)
		_Alpha ("Alpha", Range(0.0,1.0)) = 1.0
	}
	SubShader {

		Tags { "Queue"="Transparent" 
				"IgnoreProjector"="True" 
				"RenderType"="Transparent" 
			    "FlatLightingTag"="FlatLighting/FlatLightingWater2"
		}

		LOD 100

		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha 

		Pass {
			CGPROGRAM
			#pragma vertex vertWater
			#pragma fragment fragWater
			#pragma target 3.0

			#pragma shader_feature __ FL_VERTEX_COLOR
			#pragma shader_feature FL_COLORS_WORLD FL_COLORS_LOCAL
			#pragma shader_feature FL_SYMETRIC_COLORS_ON FL_SYMETRIC_COLORS_OFF
			#pragma shader_feature __ FL_GRADIENT_AXIS_ON_X
			#pragma shader_feature __ FL_GRADIENT_AXIS_ON_Y
			#pragma shader_feature __ FL_GRADIENT_AXIS_ON_Z
			#pragma shader_feature __ FL_AMBIENT_LIGHT
			#pragma shader_feature __ FL_GRADIENT_LOCAL FL_GRADIENT_WORLD
			#pragma shader_feature __ FL_DIRECTIONAL_LIGHT
			#pragma shader_feature __ FL_SPOT_LIGHT
			#pragma shader_feature __ FL_POINT_LIGHT
			#pragma shader_feature __ FL_BLEND_LIGHT_SOURCES
			#pragma shader_feature __ FL_RECEIVESHADOWS
			#pragma shader_feature __ FL_LIGHTMAPPING FL_UNITY_LIGHTMAPPING
			#pragma shader_feature __ FL_MAIN_TEXTURE

			#include "../cginc/FlatLightingCommon.cginc"
			
			uniform half3 _VertexAnimSpeed;
			uniform half3 _VertexAnimRepeat;
			uniform half3 _VertexAnimScale;
			uniform half3 _NormalAnimSpeed;
			uniform half3 _NormalAnimRepeat;
			uniform half3 _NormalAnimScale;
			uniform fixed _Alpha;

			#define FL_ALPHA


			fl_vertex_input calculate_water(fl_vertex_input v)
			{
				float4 temp = mul(unity_ObjectToWorld, v.vertex);
				float4 tempWorld = float4(temp.x, temp.y, temp.z, temp.w);

				float speedX = (_Time.y * _VertexAnimSpeed.x);
				float speedY = (_Time.y * _VertexAnimSpeed.y);
				float speedZ = (_Time.y * _VertexAnimSpeed.z);

				tempWorld.y = (temp.y + 
						(
							 (sin((speedY + (_VertexAnimRepeat.y * temp.z))) * _VertexAnimScale.y)
						)
					);

				tempWorld.x = (temp.x + 
						(
							(sin((speedX + (_VertexAnimRepeat.x * temp.y))) * _VertexAnimScale.x)
						)
					);

				tempWorld.z = (temp.z + 
						(
							(sin((speedZ + (_VertexAnimRepeat.z * temp.y))) * _VertexAnimScale.z)
						)
					);

				v.vertex.xyz = (mul(unity_WorldToObject, tempWorld).xyz);


				float speedNormalX = (_Time.y * _NormalAnimSpeed.x);
				float speedNormalY = (_Time.y * _NormalAnimSpeed.y);
				float speedNormalZ = (_Time.y * _NormalAnimSpeed.z);

				float4 newNormal = float4(0, 0, 0, 1);
				newNormal.xyz = v.normal.xyz;
				newNormal.x = (newNormal.x + (sin((speedNormalX + (_NormalAnimRepeat.x *  v.vertex.x))) * _NormalAnimScale.x));
				newNormal.y = (newNormal.y + (sin((speedNormalY + (_NormalAnimRepeat.y *  v.vertex.y))) * _NormalAnimScale.y));
  				newNormal.z = (newNormal.z + (sin((speedNormalZ + (_NormalAnimRepeat.z *  v.vertex.z))) * _NormalAnimScale.y));

  				v.normal = newNormal;

  				return v;
			}

			fl_vertex2fragment vertWater(fl_vertex_input v) {
				v = calculate_water(v);
				return FLvertex(v);
			}

			fixed4 fragWater(fl_vertex2fragment i) : SV_Target {
				fixed4 color = FLfragment(i);
				color.a *= _Alpha;

				return color;
			}
			
			ENDCG
		}
	}

	CustomEditor "FlatLightingWater2ShaderEditor"
}
