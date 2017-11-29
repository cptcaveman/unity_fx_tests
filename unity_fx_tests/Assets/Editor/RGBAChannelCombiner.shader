Shader "Hidden/RGBAChannelCombiner"
{
	Properties
	{
		_MainTex ("Tex for R Channel", 2D) = "black" {}
		_TexG("Tex for G Channel", 2D) = "black" {}
		_TexB("Tex for B Channel", 2D) = "black" {}
		_TexA("Tex for A Channel", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _TexG;
			sampler2D _TexB;
			sampler2D _TexA;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed r = tex2D(_MainTex, i.uv).r;
				fixed g = tex2D(_TexG, i.uv).r;
				fixed b = tex2D(_TexB, i.uv).r;
				fixed a = tex2D(_TexA, i.uv).r;
				
				return fixed4(r, g, b, a);
			}
			ENDCG
		}
	}
}
