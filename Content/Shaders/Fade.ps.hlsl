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
    float Fade;
};

tbuffer tbTextures {
    Texture2D Textures[2];
}

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

float4 tex2D(in Texture2D tex, in float2 texCoord) {
    return tex.Sample(TextureSampler, texCoord);
}

void main(in PS_INPUT psIn, out PS_OUTPUT psOut) {
    float4 a = Textures[0].Sample(TextureSampler, psIn.texCoord);
    float4 b = Textures[1].Sample(TextureSampler, psIn.texCoord);

    psOut.color = (1.0f - Fade)*a + Fade*b;
}
