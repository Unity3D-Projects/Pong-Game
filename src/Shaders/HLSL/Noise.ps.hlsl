/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

struct PS_INPUT {
    float4 pos      : POSITION0;
    float4 screenPos: SV_POSITION;
    float2 texCoord : TEXCOORD0;
};

struct PS_OUTPUT {
    float4 color: SV_TARGET;
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

cbuffer cbConstants: register(b1) {
    uint Seed;
};

tbuffer tbTextures {
    Texture2D Textures[1];
}

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

float noise(in float2 x) {
    float2 k = float2(23.1406926327792690, 2.6651441426902251);
    return cos(fmod(123456789.0 + Seed, 256.0 * dot(x, k)));
}

void main(in PS_INPUT psIn, out PS_OUTPUT psOut) {
    float4 a = noise(psIn.texCoord);
    float4 c = Textures[0].Sample(TextureSampler, psIn.texCoord);
    float  x = 0.20 - (0.18/3.0)*(c.r + c.g + c.b);

    psOut.color = x*a + (1.0 - x)*c;
}
