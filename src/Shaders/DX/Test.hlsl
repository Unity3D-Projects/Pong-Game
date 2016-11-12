/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

struct PS_INPUT {
    float4 Position: SV_POSITION;
    float2 TexCoord: TEXCOORD0;
};

/*-------------------------------------
 * FUNCTIONS
 *-----------------------------------*/

float4 main(PS_INPUT v): SV_TARGET {
    return float4(1.0f, 0.0f, 0.0f, 1.0f);
}
