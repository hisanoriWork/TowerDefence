﻿Shader "Custom/OutLineShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Radius("Radius px", Float) = 10
		_Width("Width px", Float) = 100
		_Height("Height px", Float) = 100

		[Toggle(SOFT_EDGE)] _SoftEdge("Soft edge", Float) = 0

		[HideInInspector]  _StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector]  _Stencil("Stencil ID", Float) = 0
		[HideInInspector]  _StencilOp("Stencil Operation", Float) = 0
		[HideInInspector]  _StencilWriteMask("Stencil Write Mask", Float) = 255
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
		
		Pass{
			Cull Back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ SOFT_EDGE

			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Radius;
			float _Width;
			float _Height;

			v2f vert(appdata v) {
				v2f o;
				//v.vertex += float4(v.normal * _OutLineWidth, 0);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			float MakeEdge(float from, float to, fixed t) {
				fixed r = max(_Radius, 2);
				t = saturate(t * (r - t));
				return lerp(from, to, t);
			}

			float2 GetRadiusToPointVector(float2 pixel, float2 halfRes, float radius) {
				float2 firstQuadrant = abs(pixel);
				float2 radiusToPoint = firstQuadrant - (halfRes - radius);
				radiusToPoint = max(radiusToPoint, 0.0);
				return radiusToPoint;
			}

			float SoftRounded(float2 pixel, float2 halfRes, float radius) {
				float2 v = GetRadiusToPointVector(pixel, halfRes, radius);
				float alpha = 1.0 - length(v) / radius;
				alpha = MakeEdge(0, 1, alpha);
				return alpha;
			}

			float HardRounded(float2 pixel, float2 halfRes, float radius) {
				float2 v = GetRadiusToPointVector(pixel, halfRes, radius);
				float alpha = 1.0 - floor(length(v) / radius);
				return alpha;
			}

			fixed4 frag(v2f i) : SV_Target {
				float2 uvInPixel = (i.uv - 0.5) * float2(_Width, _Height);
				float2 halfRes = float2(_Width, _Height) * 0.5;

				#ifdef SOFT_EDGE
				float alpha = SoftRounded(uvInPixel, halfRes, _Radius);
				#else
				float alpha = HardRounded(uvInPixel, halfRes, _Radius);
				#endif
				float2 v = GetRadiusToPointVector(uvInPixel, halfRes, _Radius);
				float2 foldedV = float2(max(v.x, v.y), min(v.x, v.y));
				if (foldedV.y > 0.0) {
					float l = length(float2(_Radius, foldedV.y * _Radius / foldedV.x));
					float2 newUvInPixel = uvInPixel + ((l / _Radius) - 1.0) * sign(uvInPixel) * v;
					i.uv = (newUvInPixel / float2(_Width, _Height)) + 0.5;
				}
				fixed4 col = tex2D(_MainTex, i.uv) * i.color;
				col.a *= alpha;
				return col;
			}
			ENDCG
		}
	}
}


