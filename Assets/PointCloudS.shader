Shader "Unlit/PointCloudS"
{
	Properties
	{
		
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f
            {
                float4 col : COLOR0;
                float4 vertex : SV_POSITION;
            };

            StructuredBuffer<float3> PointPos;
            StructuredBuffer<float4> PointCol;
            
            v2f vert (uint id : SV_VertexID)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(float4(PointPos[id], 1));
                o.col = PointCol[id];
                return o;
            }
 
            fixed4 frag (v2f i) : SV_Target
            {
                return i.col;
            }
			ENDCG
		}
	}
}
