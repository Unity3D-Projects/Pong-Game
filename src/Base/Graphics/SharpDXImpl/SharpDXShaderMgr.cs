namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Shaders;

using SharpDX.D3DCompiler;
using SharpDX.DXGI;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXShaderMgr: IDisposable, IShaderMgr {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public SharpDXGraphics Graphics;

    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private List<SharpDXShader> m_Shaders = new List<SharpDXShader>();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXShaderMgr(SharpDXGraphics graphics) {
        Graphics = graphics;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        foreach (var shader in m_Shaders) {
            shader.Dispose();
        }

        m_Shaders.Clear();
        m_Shaders = null;

        Graphics = null;
    }

    public IShader LoadPS<T>(string path) where T: struct {
        return LoadPS(path, typeof (T));
    }

    public IShader LoadPS(string path, Type inputType=null) {
        using (var byteCode = ShaderBytecode.CompileFromFile(path, "main", "ps_5_0", ShaderFlags.Debug)) {
            var gpuShader = new D3D11.PixelShader(Graphics.Device, byteCode);

            var shader = new SharpDXShader(Graphics, gpuShader, null, inputType);

            m_Shaders.Add(shader);

            return shader;
        }
    }

    public IShader LoadVS<T>(string path) where T: struct {
        return LoadVS(path, typeof (T));
    }

    public IShader LoadVS(string path, Type inputType=null) {
        using (var byteCode = ShaderBytecode.CompileFromFile(path, "main", "vs_5_0", ShaderFlags.Debug)) {
            var gpuShader = new D3D11.VertexShader(Graphics.Device, byteCode);

            var inputElements = new D3D11.InputElement[] {
                new D3D11.InputElement("POSITION", 0, Format.R32G32B32A32_Float,  0, 0),
                new D3D11.InputElement("TEXCOORD", 0, Format.R32G32_Float      , 16, 0)
            };

            var inputSignature = ShaderSignature.GetInputSignature(byteCode);
            var inputLayout = new D3D11.InputLayout(Graphics.Device, inputSignature, inputElements);

            var shader = new SharpDXShader(Graphics, gpuShader, inputLayout, inputType);

            m_Shaders.Add(shader);

            return shader;
        }
    }
}

}
