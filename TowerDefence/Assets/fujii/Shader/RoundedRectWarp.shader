Shader "Custom/test"
{
	Properties
	{
		//_MainTex("Texture", 2D) = "white" {}
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_OutLineColor("OutLineColor",Color) = (1,1,1,1)
		_OutLineWidth("OutLineWidth",Range(0,1)) = 0.04

	}
		SubShader
		{
			Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
			}
			LOD 100
			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}
			Cull Off
			Lighting Off
			ZTest[unity_GUIZTestMode]

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass {
				Cull Back
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ SOFT_EDGE

				#include "UnityCG.cginc"

				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float2 normal : NORMAL;
					float4 color : COLOR;
				};

				struct v2f {
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float4 color : COLOR;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _OutLineWidth;

				v2f vert(appdata v) {
					v2f o;
					v.vertex += float4(v.normal * _OutLineWidth, 0, 0) - float4(5000 * _WorldSpaceLightPos0.xyz, 0);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.color = v.color;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = i.color;
					return col;
				}
					
				ENDCG
			}

		}
}


