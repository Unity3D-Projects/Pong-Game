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

tbuffer tbTextures {
    Texture2D Textures[2];
}

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

float4 tex2D(in Texture2D tex, in float2 texCoord) {
    return tex.Sample(TextureSampler, texCoord);
}

float2 sampledVelocity(in float2 uv) {
    float texWidth;
    float texHeight;

    Textures[1].GetDimensions(texWidth, texHeight);

    float2 r = 2.0 / float2(texWidth, texHeight);
    float2 v = 0.0;

    for (int i = -4; i <= 4; i++) {
        for (int j = -4; j <= 4; j++) {
            v += tex2D(Textures[1], uv + float2(i, j)*r).rg;
        }
    }

    v *= 1.0/81.0;

    return v;
}

void main(in PS_INPUT psIn, out PS_OUTPUT psOut) {
    float texWidth;
    float texHeight;

    Textures[0].GetDimensions(texWidth, texHeight);

    float2 texelSize = 1.0 / float2(texWidth, texHeight);
    float2 velocity = 1.4 * sampledVelocity(psIn.texCoord);
    float speed = length(velocity / texelSize);
    int numSamples = clamp(int(speed), 1, 32);

    float4 color = 0.0;

    for (int i = 0; i < numSamples; i++) {
        float a = float(i) / float(numSamples);
        float2 offset = velocity * (a - 0.5);
        color += tex2D(Textures[0], psIn.texCoord + offset);
    }

    color /= float(numSamples);
    color.a = 1.0;

    psOut.color = color;
}
