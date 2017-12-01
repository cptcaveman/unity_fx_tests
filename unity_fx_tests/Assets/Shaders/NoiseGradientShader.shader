Shader "Custom/NoiseGradientShader" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_GradientMask("Gradient Influence for Noise 2D", 2D) = "white" {}
		_DistortionTex("Distortion Texture", 2D) = "white" {}
		_DistortionStrength("Slider for Distortion strength", Range(0,1)) = .5
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _GradientMask;
		sampler2D _DistortionTex;
		fixed _DistortionStrength;

		struct Input {
			fixed2 uv_MainTex;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			fixed gradientMask = tex2D(_GradientMask, IN.uv_MainTex);
			float2 timeOffset = float2(_Time.x, _Time.x);
			fixed2 noiseOffset = tex2D(_DistortionTex, IN.uv_MainTex + timeOffset).xy - .5 * 2;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex + noiseOffset * gradientMask * _DistortionStrength) * _Color;
			clip(c.a - .001);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
