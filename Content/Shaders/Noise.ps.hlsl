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

float avg(in float2 x) {
    const int K = 1;

    float texWidth;
    float texHeight;

    Textures[0].GetDimensions(texWidth, texHeight);

    float y = 0.0;
    float2 r = 1.0 / float2(texWidth, texHeight);

    for (int i = -K; i <= K; i++) {
        for (int j = -K; j <= K; j++) {
            y += noise(x+r*float2(i, j));
        }
    }

    y /= (2*K + 1)*(2*K + 1);

    return y;
}

float4 tex2D(in Texture2D tex, in float2 texCoord) {
    return tex.Sample(TextureSampler, texCoord);
}

void main(in PS_INPUT psIn, out PS_OUTPUT psOut) {
    float4 a = 0.3*noise(psIn.texCoord) + 0.7*avg(psIn.texCoord);
    float4 c = tex2D(Textures[0], psIn.texCoord);
    float  x = 0.18 - (0.16/3.0)*(c.r + c.g + c.b);

    psOut.color = x*a + (1.0 - x)*c;
}
