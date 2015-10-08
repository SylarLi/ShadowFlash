Shader "Sylar/GaussianBlur" 
{
	Properties 
	{
		_MainTex("Input", 2D) = "white" {}
		_Blur("Blur", float) = 1
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

		Pass
		{
			// Vertical

			CGPROGRAM

 			#pragma vertex vert
 			#pragma fragment frag
 			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _MainTex_ST;
			float4 _MainTex_TexelSize;
			float _Blur;

	 		struct appdata
	 		{
	 			float4 vertex : POSITION;
	 			float2 texcoord : TEXCOORD0;
	 		};

	 		struct v2f
	 		{
	 			float4 pos : POSITION;
	 			float2 uv : TEXCOORD0;
	 		};

	 		v2f vert(appdata i)
	 		{
	 			v2f o;
	 			o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
	 			o.uv = TRANSFORM_TEX(i.texcoord, _MainTex);
	 			return o;
	 		}

	 		static const float weights[5] = { 0.2270270270, 0.1945945946, 0.1216216216, 0.0540540541, 0.0162162162 };
	 		#define TexOffset(x, y) (tex2D(_MainTex, i.uv + float2(x, y)))

	 		fixed4 frag(v2f i) : Color
	 		{
	 			fixed4 sum = TexOffset(0, 0) * weights[0];
	 			for (int s = 1; s < 5; s++)
	 			{
 					sum += TexOffset(_Blur / _MainTex_TexelSize.x * s, 0) * weights[s];
 					sum += TexOffset(-_Blur / _MainTex_TexelSize.x * s, 0) * weights[s];
	 			}
	 			return sum;
	 		}

	 		ENDCG
		}
	}

	Fallback "Diffuse"
}