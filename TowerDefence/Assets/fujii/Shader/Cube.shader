Shader "Custom/Cube"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Width("Width",Range(0,10)) = 1
		_AlphaGrad("AlphaGrad",Range(0,10)) = 1
		_MainColor("MainColor",Color) = (1,1,1,1)
	}
		SubShader
		{
			Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
			LOD 100
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				#define PI 3.141592
				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _Width;
				float _AlphaGrad;
				fixed4 _MainColor;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float d = 2 * max(abs(0.5 - i.uv.x),abs(0.5 - i.uv.y));
					float w = 1 - pow(d, _Width);
					float a = pow(d, _AlphaGrad);
					fixed4 col = _MainColor * fixed4(w,w, 1,a);
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}
		}
}
