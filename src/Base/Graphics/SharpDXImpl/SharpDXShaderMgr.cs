namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Shaders;

using SharpDX.D3DCompiler;
using SharpDX.DXGI;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class ShaderMgr: IShaderMgr {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private readonly D3D11.Device m_Device;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public ShaderMgr(D3D11.Device device) {
        m_Device = device;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Cleanup() {
    }

    public void Init() {
    }

    public IShader LoadPS(string path) {
        using (var byteCode = ShaderBytecode.CompileFromFile(path, "main", "ps_4_0", ShaderFlags.Debug)) {
            var shader = new D3D11.PixelShader(m_Device, byteCode);

            return new SharpDXShader(shader);
        }
    }

    public IShader LoadVS(string path) {
        using (var byteCode = ShaderBytecode.CompileFromFile(path, "main", "vs_4_0", ShaderFlags.Debug)) {
            var shader = new D3D11.VertexShader(m_Device, byteCode);

            var inputElements = new D3D11.InputElement[] {
                new D3D11.InputElement("SV_POSITION", 0, Format.R32G32B32A32_Float,  0, 0),
                new D3D11.InputElement("TEXCOORD"   , 0, Format.R32G32_Float      , 16, 0)
            };

            var inputSignature = ShaderSignature.GetInputSignature(byteCode);
            var inputLayout = new D3D11.InputLayout(m_Device, inputSignature, inputElements);

            return new SharpDXShader(shader, inputLayout);
        }
    }
}

}
