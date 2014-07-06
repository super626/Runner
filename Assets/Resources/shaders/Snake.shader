Shader "Custom/Snake" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Common.cginc"
			sampler2D _MainTex;
			float4 _Color;
			
			struct appdata {
    			float4 vertex : POSITION;
    			float2 texcoord : TEXCOORD0;

			};
			
			struct v2f {
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			};
			
			v2f vert (appdata v)
			{
			    v2f o;
			    
			    float3 wpos = GetTurnPos(v.vertex, false);
			    
			   	o.pos = mul(UNITY_MATRIX_VP, float4(wpos, 1));
			   	
				o.uv = v.texcoord.xy;
				
			    return o;
			}
			float4 frag (v2f i) : COLOR
			{
				float4 texCol = tex2D(_MainTex,i.uv);
			    float4 outp = texCol * _Color;
			    return outp;
			}
			ENDCG
		}
	}

	FallBack "Unlit/Texture"
}
