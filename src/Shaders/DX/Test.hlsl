/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

float4 main(float2 p: TEXCOORD0) : SV_TARGET {
    return float4(cos(p.x*10.0), sin(p.y*10.0), 1.0, 1.0);
}
