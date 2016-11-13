/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

struct PS_INPUT {
    float4 pos      : POSITION0;
    float4 prevPos  : POSITION1;
    float4 screenPos: SV_POSITION;
    float2 texCoord : TEXCOORD0;
};

struct PS_OUTPUT {
    float4 color: SV_TARGET;
};

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

void main(in PS_INPUT psIn, out PS_OUTPUT psOut) {
    float2 d = (psIn.pos.xy/psIn.pos.w) - (psIn.prevPos.xy/psIn.prevPos.w);

    psOut.color = float4(d.xy, 0.0, 0.0);
}
