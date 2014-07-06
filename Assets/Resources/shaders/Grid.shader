Shader "Custom/Grid" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
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
    			float4 color : COLOR0;
			};
			
			struct v2f {
			    float4  pos : SV_POSITION;
			    float2  uv : TEXCOORD0;
			    fixed3  cc : COLOR;
			    float  fogc : TEXCOORD1;
			};
			
			v2f vert (appdata v)
			{
			    v2f o;
			    float3 wpos = GetTurnPos(v.vertex, true);
			    
			   	o.pos = mul(UNITY_MATRIX_VP, float4(wpos, 1));
			   	
				o.uv = v.texcoord.xy;
				float3 tone = 1.0 - 0.5 * (float3(sin(wpos.x * 0.1 + wpos.z * 0.03), sin(wpos.x * 0.1 + wpos.z * 0.057 + 10), cos(wpos.z * 0.1)) + 1); 
				float3 c = 1 + float4(tone, 1) * v.color.a - v.color.rgb;  
				float dist = distance(_WorldSpaceCameraPos, wpos);
				o.fogc = smoothstep(35, 25, dist);
				o.cc = c;
				
			    return o;
			}
			float4 frag (v2f i) : COLOR
			{
				float4 texCol = tex2D(_MainTex,i.uv);
			    float4 outp = float4(i.cc * (1 - texCol), texCol.a);
			    outp.rgb *= i.fogc;
			    return outp;
			}
			ENDCG
		}
	}

	FallBack "Unlit/Texture"
}
