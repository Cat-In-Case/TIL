# TIL
 
				//다른 카메라를 사용하여 다른 ZBufferParams가 필요한 경우 
                float4 _CustomZBufferCal_Fixed(float2 Far_Near)
                {
                    float4 ZBuffer;
                    //x = -1 + far / near
                    ZBuffer.x = -1 + (Far_Near.x / Far_Near.y);
                    //y = far / near
                    ZBuffer.y = 1;
                    //z = x / far
                    ZBuffer.z = ZBuffer.x / Far_Near.x;
                    //w = y / far
                    ZBuffer.w = 1 / Far_Near.x;

                    return ZBuffer;
                } 

                //GrabPass 사용시 ScreenPos를 가져오는 함수
				float4 ComputeGrabScreenPos(float4 pos)
                {
                    #if UNITY_UV_STARTS_AT_TOP
                    float scale = -1.0;
                    #else
                    float scale = 1.0;
                    #endif
                    float4 o = pos * 0.5f;
                    o.xy = float2(o.x, o.y * scale) + o.w;
                    o.xy *= 0.5;    
                    #ifdef UNITY_SINGLE_PASS_STEREO
                    o.xy = TransformStereoScreenSpaceTex(o.xy, pos.w);
                    #endif
                    o.zw = pos.zw;
                    return o;
                }

                //사용한 곳 
                //CustomGrabPass에서 GrabPassCamera의 SolidColor를 GrabTexture에서 마스킹하기 위한 용도
                float GetSubstractAlpha(float3 main, float3 color) 
                {
                    half3 delta = abs(main.xyz - color.xyz);
                    float alpha = 0;
                    if (length(delta) < 0.05)
                    {
                        alpha = 0;
                    }
                    else
                    {
                        alpha = 1;
                    }
                    return alpha;
                }


float3 getNormalFromTexture(Texture2D t, SamplerState s, float strength, float2 offset, float2 uv)
{
	offset = pow(offset, 3) * 0.1f;
    float2 offsetU = float2(uv.x + offset.x, uv.y);
    float2 offsetV = float2(uv.x, uv.y + offset.y);
    
    float normalSample = t.Sample(smp, uv).a;
    float uSample = t.Sample(s, offsetU).a;
    float vSample = t.Sample(s, offsetV).a;
    float3 va = float3(1.0f, 0.0f, (uSample - normalSample) * strength);
    float3 vb = float3(0.0f, 1.0f, (vSample - normalSample) * strength);
    
    //float3 normal = (cross(va, vb));
    float3 normal = normalize(cross(va, vb));
    return normal;
}


float3 voronoiNoise(float2 uv, float scale, float2 resolution, float animateOffset)
{
	float3 color;
	float2 st = uv * (resolution.x/resolution.y) * scale;
	float2 i_st = floor(st);
	float2 f_st = frac(st);
	
	float m_dist = 1;
	
	for (int y=-1;y<=1;y++)
	{
		for(int x=-1;x<=1;x++)
		{
			float2 neighbor = float2(float(x),float(y));
			i_st + neighbor);
			p = .5f + .5f * sin(animateOffset + 6.2381f * p);
			
			float2 diff = neighbor + p - f_st;
			float dist = length(diff);
			m_dist = min(m_dist, dist);
		}
	}
	//	draw the min distance
	color += m_dist;
	//	draw cell center
	return color;
}