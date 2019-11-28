﻿Shader "Custom/HandShader"
{
	Properties{
		_MainColor("_Color",Color) = (0,0,0,0)
		_RimColor("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower("Rim Power", Range(0.5,8.0)) = 3.0
	}

		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
			LOD 200

			Pass{
				ZWrite On
				ColorMask A
			}

			CGPROGRAM
			#pragma surface surf Standard alpha:fade 
			#pragma target 3.0

			struct Input {
			   float3 viewDir;
			};

			fixed4 _MainColor;
			float4 _RimColor;
			float _RimPower;

			void surf(Input IN, inout SurfaceOutputStandard o) {
				o.Albedo = _MainColor;
				o.Alpha = _MainColor.a;
				//o.Alpha = 1.0;
				half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Emission = _RimColor.rgb * pow(rim, _RimPower);
			}
			ENDCG
	}
		FallBack "Diffuse"
}

