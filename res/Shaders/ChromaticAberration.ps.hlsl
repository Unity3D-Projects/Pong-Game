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
    float Intensity;
};

tbuffer tbTextures {
    Texture2D Textures[1];
}

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

float4 tex2D(in Texture2D tex, in float2 texCoord) {
    return tex.Sample(TextureSampler, texCoord);
}

float3 chromaticAberration(in float2 x) {
    const int K = 2;

    float texWidth;
    float texHeight;

    Textures[0].GetDimensions(texWidth, texHeight);

    float2 y = 0.0;
    float2 r = Intensity / float2(texWidth, texHeight);

    float4 c = tex2D(Textures[0], x);
    float2 a = (c.r + c.g + c.b);

    for (int i = -K; i <= K; i++) {
        for (int j = -K; j <= K; j++) {
            if (i == 0 && j == 0) {
                continue;
            }

            float2 p = float2(i, j);
            c = Textures[0].Sample(TextureSampler, x + p*r);
            float2 b = (c.r + c.g + c.b);

            y += (b - a)*p / length(p);
        }
    }

    y /= (2*K + 1)*(2*K + 1);

    return y.rgg;
}


void main(in PS_INPUT psIn, out PS_OUTPUT psOut) {
    float4 c = Textures[0].Sample(TextureSampler, psIn.texCoord);
    float3 w = chromaticAberration(psIn.texCoord);

    psOut.color = c + float4(w.rgb, 0.0);
}
