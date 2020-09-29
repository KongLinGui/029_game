Shader "Hidden/Radar Effect" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_RampTex ("Base (RGB)", 2D) = "grayscaleRamp" {}
}

SubShader {
	Pass {
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }
				
CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest 
#include "UnityCG.cginc"

uniform sampler2D _MainTex;
uniform float m_startRing;
uniform float m_width;

uniform int m_nomRings = 3;
uniform float m_ringBorder;
uniform float4 m_colorBonus;
fixed4 frag (v2f_img i) : COLOR
{
	fixed4 original = tex2D(_MainTex, i.uv);
	float2 centre = float2(0.5,0.5f);
	fixed4 output = original;
	fixed2 vec = i.uv - centre;
	float d0 = length(i.uv - centre);
	float angle;
	float startRing=0;
	float endRing=0;
	
	if(d0 < 0.5)
	{
		 angle = atan2(vec.y,vec.x) * 57.2957795;
		 startRing = m_startRing;
		 endRing = startRing + m_width;
		 for(int i=0; i<3; i++)
		 {
			if(d0 > startRing && d0 < startRing+m_width)
			{
				output += m_colorBonus.rgba;
			}
			startRing += m_ringBorder;
		}
	}else{
		output.rgb=0;
	}
	
	output.a = original.a;
	return output;
}

ENDCG

	}
}

Fallback off

}