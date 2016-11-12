/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

struct PS_INPUT {
    float4 Position: SV_POSITION;
    float2 TexCoord: TEXCOORD0;
};

/*-------------------------------------
 * CONSTANTS
 *-----------------------------------*/

SamplerState TextureSampler {
    AddressU = Clamp;
    AddressV = Clamp;
    Filter   = MIN_MAG_MIP_LINEAR;
};

/*-------------------------------------
 * GLOBALS
 *-----------------------------------*/

Texture2D g_Texture;

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

float4 main(PS_INPUT v): SV_TARGET {
    return g_Texture.Sample(TextureSampler, v.TexCoord);
}
