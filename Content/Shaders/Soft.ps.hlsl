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
    Texture2D Textures[1];
}

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

float4 tex2D(in Texture2D tex, in float2 texCoord) {
    return tex.Sample(TextureSampler, texCoord);
}

float3 soft(in float2 texCoord) {
    const int K = 1;

    float texWidth;
    float texHeight;

    Textures[0].GetDimensions(texWidth, texHeight);

    float3 y = 0.0;
    float2 r = 1.0 / float2(texWidth, texHeight);

    for (int i = -K; i <= K; i++) {
        for (int j = -K; j <= K; j++) {
            float2 d = float2(i, j);

            y += tex2D(Textures[0], texCoord + r*d);
        }
    }

    y /= (2*K + 1)*(2*K + 1);

    return y;
}

void main(in PS_INPUT psIn, out PS_OUTPUT psOut) {
    float4 c = Textures[0].Sample(TextureSampler, psIn.texCoord);
    float3 x = soft(psIn.texCoord);

    psOut.color = 0.75*c + 0.25*float4(x.rgb, 0.0);
}
