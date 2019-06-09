// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ShadowDepthDebug" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
            float4 _MainTex_ST;

            #include "UnityCG.cginc"

			struct vert_in {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			v2f vert (vert_in v) {
				v2f o;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				return o;
			}
			
			float4 frag (v2f i) : SV_Target {
				return tex2D(_MainTex, i.texcoord);
			}
			ENDCG
		}
	}
}
