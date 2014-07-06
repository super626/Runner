Shader "Custom/Decal" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		LOD 200
		Blend SrcAlpha One
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
			    float  fogc : TEXCOORD1;
			};
			
			v2f vert (appdata v)
			{
			    v2f o;
			    float3 wpos = GetTurnPos(v.vertex, true);
			    
			   	o.pos = mul(UNITY_MATRIX_VP, float4(wpos, 1));
			   	
				o.uv = v.texcoord.xy;
				float dist = distance(_WorldSpaceCameraPos, wpos);
				o.fogc = smoothstep(35, 25, dist);
				
			    return o;
			}
			float4 frag (v2f i) : COLOR
			{
				float4 texCol = tex2D(_MainTex,i.uv);
			    float4 outp = texCol * _Color;
			    outp.rgb *= i.fogc;
			    return outp;
			}
			ENDCG
		}
	}

	FallBack "Unlit/Texture"
}
