# TIL
 

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