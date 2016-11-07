/*-------------------------------------
 * GLOBALS
 *-----------------------------------*/

float4x4 g_Model;

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

float4 main(float4 p: POSITION) : SV_POSITION {
    float4 v = mul(g_Model, p);
    return v;
}
