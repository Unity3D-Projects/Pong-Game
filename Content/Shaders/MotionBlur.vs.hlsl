/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

struct VS_INPUT {
    float4 pos     : POSITION0;
    float2 texCoord: TEXCOORD0;
};

struct VS_OUTPUT {
    float4 pos      : POSITION0;
    float4 prevPos  : POSITION1;
    float4 screenPos: SV_POSITION;
    float2 texCoord : TEXCOORD0;
};

/*-------------------------------------
 * GLOBALS
 *-----------------------------------*/

cbuffer cbConstants {
    float4x4 ModelViewProj;
}

cbuffer cbConstants: register(b1) {
    float4x4 PrevModelViewProj;
};

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

void main(in VS_INPUT vsIn, out VS_OUTPUT vsOut) {
    vsOut.pos       = mul(ModelViewProj    , vsIn.pos);
    vsOut.prevPos   = mul(PrevModelViewProj, vsIn.pos);
    vsOut.screenPos = vsOut.pos;
    vsOut.texCoord  = vsIn.texCoord;
}
