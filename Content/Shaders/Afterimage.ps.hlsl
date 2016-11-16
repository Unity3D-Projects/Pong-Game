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
    float Afterimage;
    int   Blur;
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

float4 average(Texture2D tex, in float2 texCoord) {
    float texWidth;
    float texHeight;

    tex.GetDimensions(texWidth, texHeight);

    float4 color = 0.0;

    float2 r = 1.0 / float2(texWidth, texHeight);
    for (int i = -Blur; i <= Blur; i++) {
        for (int j = -Blur; j <= Blur; j++) {
            float2 d = float2(i, j);

            color += tex2D(tex, texCoord + r*d);
        }
    }

    color /= (2*Blur + 1)*(2*Blur + 1);

    return color;
}

void main(in PS_INPUT psIn, out PS_OUTPUT psOut) {
    float4 a = tex2D(Textures[0], psIn.texCoord);
    float4 b;

    if (Blur == 0) {
        b = tex2D(Textures[1], psIn.texCoord);
    }
    else {
        b = average(Textures[1], psIn.texCoord);
    }

    psOut.color = (1.0 - Afterimage)*a + Afterimage*b;
}
