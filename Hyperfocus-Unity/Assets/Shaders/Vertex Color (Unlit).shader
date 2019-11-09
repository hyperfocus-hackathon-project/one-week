// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "HyperFocus/Vertex Color (Unlit)"
{
	Properties
	{
	}

	CGINCLUDE

	#include "UnityCG.cginc"

	ENDCG

	SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		Pass
		{
			Lighting Off
			CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#pragma target 5.0
#pragma only_renderers d3d11

			struct v2f
			{
				float4 pos : SV_POSITION0;
				half4 color : COLOR;
			};

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				return i.color;
			}
			ENDCG
		}
	}
		Fallback "VertexLit"
}