namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Runtime.InteropServices;

using Shaders;
using Textures;

using SharpDX;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXShader: IDisposable, IShader {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public D3D11.Buffer ConstantBuffer;

    public SharpDXGraphics Graphics;

    public D3D11.InputLayout InputLayout;

    public D3D11.DeviceChild Shader;

    public D3D11.ShaderResourceView[] Textures;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXShader(SharpDXGraphics   graphics,
                         D3D11.DeviceChild shader,
                         D3D11.InputLayout inputLayout,
                         Type              inputType)
    {
        Graphics    = graphics;
        InputLayout = inputLayout;
        Shader      = shader;

        if (inputType != null) {
            ConstantBuffer = CreateConstantBuffer(inputType);
        }
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        Graphics = null;

        if (ConstantBuffer != null) {
            ConstantBuffer.Dispose();
            ConstantBuffer = null;
        }

        if (InputLayout != null) {
            InputLayout.Dispose();
            InputLayout = null;
        }

        if (Shader != null) {
            Shader.Dispose();
            Shader = null;
        }
    }

    public void SetConstants<T>(T constants) where T: struct {
        const D3D11.MapMode  MODE  = D3D11.MapMode.WriteDiscard;
        const D3D11.MapFlags FLAGS = D3D11.MapFlags.None;

        var context = Graphics.Device.ImmediateContext;

        DataStream dataStream;
        context.MapSubresource(ConstantBuffer, MODE, FLAGS, out dataStream);
        dataStream.Write(constants);
        dataStream.Dispose();
        context.UnmapSubresource(ConstantBuffer, 0);
    }

    public void SetTextures(params ITexture[] textures) {
        var numTextures = textures.Length;

        Textures = new D3D11.ShaderResourceView[numTextures];
        for (var i = 0; i < numTextures; i++) {
            Textures[i] = ((SharpDXTexture)textures[i]).ShaderResource;
        }
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private D3D11.Buffer CreateConstantBuffer(Type type) {
        int size = Marshal.SizeOf(type);

        if ((size % 16) != 0) {
            size += 16 - (size % 16);
        }

        var bufferDescription = new D3D11.BufferDescription() {
            BindFlags      = D3D11.BindFlags.ConstantBuffer,
            CpuAccessFlags = D3D11.CpuAccessFlags.Write,
            SizeInBytes    = size,
            Usage          = D3D11.ResourceUsage.Dynamic,
        };

        var data   = new byte[size];
        var buffer = D3D11.Buffer.Create(Graphics.Device,
                                         ref data[0],
                                         bufferDescription);

        return buffer;
    }
}

}
