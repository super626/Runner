float _TurnX;

float3 GetTurnPos(float4 pos, bool bTurnY) {
	float3 wpos = mul(_Object2World, pos);
    float dist = wpos.z - _WorldSpaceCameraPos.z;
    if (bTurnY)
    {
        float cd = clamp(dist - 15, 0, 50) * (1.57 / 50);
	    wpos.y -= sin(cd * cd) * 7;
	}
	float cd2 = clamp(dist, 3, 50) * (1.57 / 50);
	wpos.x -= sin(cd2 * cd2) * _TurnX;
    
    return wpos;
}