namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using SharpDX.D3DCompiler;
using SharpDX.DXGI;

using Shaders;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal sealed class SharpDXShaderManager: IShaderManager {
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

    public SharpDXShaderManager(D3D11.Device device) {
        m_Device = device;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Cleanup() {
    }

    public void Init() {
    }

    public IShader LoadPixelShader(string path) {
        using (var byteCode = ShaderBytecode.CompileFromFile(path, "main", "ps_4_0", ShaderFlags.Debug)) {
            var shader = new D3D11.PixelShader(m_Device, byteCode);

            return new SharpDXShader(shader);
        }
    }

    public IShader LoadVertexShader(string path) {
        using (var byteCode = ShaderBytecode.CompileFromFile(path, "main", "vs_4_0", ShaderFlags.Debug)) {
            var shader = new D3D11.VertexShader(m_Device, byteCode);

            var inputElements = new D3D11.InputElement[] {
                new D3D11.InputElement("POSITION", 0, Format.R32G32_Float, 0)
            };

            var inputSignature = ShaderSignature.GetInputSignature(byteCode);
            var inputLayout = new D3D11.InputLayout(m_Device, inputSignature, inputElements);

            return new SharpDXShader(shader, inputLayout);
        }
    }
}

}
