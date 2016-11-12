/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

struct VS_INPUT {
    float4 Position: SV_POSITION;
    float2 TexCoord: TEXCOORD0;
};

struct VS_OUTPUT {
    float4 Position: SV_POSITION;
    float2 TexCoord: TEXCOORD0;
};

/*-------------------------------------
 * GLOBALS
 *-----------------------------------*/

float4x4 g_Transform;

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

VS_OUTPUT main(VS_INPUT v) {
    VS_OUTPUT r;

    r.Position = mul(g_Transform, v.Position);
    r.TexCoord = v.TexCoord;

    return r;
}
