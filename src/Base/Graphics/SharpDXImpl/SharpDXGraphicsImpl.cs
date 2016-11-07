namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Windows.Forms;

using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.DXGI;

using Core;
using Math;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class SharpDXGraphicsImpl: IGraphicsImpl {
    private D3D11.Buffer m_ShaderParams;
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private D3D11.Device m_Device;

    private D3D11.DeviceContext m_DeviceContext;

    private D3D11.PixelShader m_PixelShader;

    private D3D11.Buffer m_Quad;

    private D3D11.RenderTargetView m_RenderTargetView;

    private SwapChain m_SwapChain;

    private D3D11.VertexShader m_VertexShader;

    private Form m_Window;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public string Name {
        get { return "SharpDX"; }
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void BeginFrame() {
    }

    public void Cleanup() {
        m_Quad.Dispose();
        m_Quad = null;

        m_DeviceContext.PixelShader.Set(null);
        m_PixelShader.Dispose();
        m_PixelShader = null;

        m_DeviceContext.VertexShader.Set(null);
        m_VertexShader.Dispose();
        m_VertexShader = null;

        m_RenderTargetView.Dispose();
        m_RenderTargetView = null;

        m_SwapChain.Dispose();
        m_SwapChain = null;

        m_Device.Dispose();
        m_Device = null;

        m_DeviceContext.Dispose();
        m_DeviceContext = null;
    }

    public void Clear(Graphics.Color clearColor) {
        var color = new Color(clearColor.ToIntABGR());
        m_DeviceContext.ClearRenderTargetView(m_RenderTargetView, color);
    }

    public void DrawTexture(ITexture texture, Matrix4x4 transform) {
        m_DeviceContext.UpdateSubresource(ref transform, m_ShaderParams);
        m_DeviceContext.Draw(4, 0);
    }

    public void EndFrame() {
        m_SwapChain.Present(1, PresentFlags.None);
    }

    public void Init(Form window) {
        m_Window = window;

        InitDevice();
        InitShaders();

        CreateQuad();

        var desc = new D3D11.BufferDescription(64, D3D11.ResourceUsage.Default, D3D11.BindFlags.ConstantBuffer, D3D11.CpuAccessFlags.None, D3D11.ResourceOptionFlags.None, 0);
        var o = Matrix4x4.Identity();
       
        m_ShaderParams = D3D11.Buffer.Create(m_Device, ref o, desc);
        m_DeviceContext.VertexShader.SetConstantBuffer(0, m_ShaderParams);
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private void CreateQuad() {
        var vertices = new Vector2[] {
            new Vector2(-0.5f, -0.5f),
            new Vector2(-0.5f,  0.5f),
            new Vector2( 0.5f, -0.5f),
            new Vector2( 0.5f,  0.5f),
        };

        m_Quad = D3D11.Buffer.Create(m_Device, D3D11.BindFlags.VertexBuffer, vertices);
        m_DeviceContext.InputAssembler.SetVertexBuffers(0, new D3D11.VertexBufferBinding(m_Quad, Utilities.SizeOf<Vector2>(), 0));
        m_DeviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
    }

    private void InitDevice() {
        var width  = m_Window.ClientRectangle.Width;
        var height = m_Window.ClientRectangle.Height;

        var refreshRate = new Rational(60, 1);
        var modeDesc = new ModeDescription(width, height, refreshRate, Format.R8G8B8A8_UNorm);

        var swapChainDesc = new SwapChainDescription() {
            BufferCount       = 1,
            IsWindowed        = true,
            ModeDescription   = modeDesc,
            SampleDescription = new SampleDescription(1, 0),
            OutputHandle      = Game.Inst.Window.Handle,
            Usage             = Usage.RenderTargetOutput
        };

        D3D11.Device.CreateWithSwapChain(DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc, out m_Device, out m_SwapChain);
        m_DeviceContext = m_Device.ImmediateContext;

        using (var backBuffer = m_SwapChain.GetBackBuffer<D3D11.Texture2D>(0)) {
            m_RenderTargetView = new D3D11.RenderTargetView(m_Device, backBuffer);
        }

        m_DeviceContext.OutputMerger.SetRenderTargets(m_RenderTargetView);
        m_DeviceContext.Rasterizer.SetViewport(new Viewport(0, 0, width, height));
    }

    private void InitShaders() {
        using (var byteCode = ShaderBytecode.CompileFromFile("src/Shaders/DX/DefaultPS.hlsl", "main", "ps_4_0", ShaderFlags.Debug)) {
            m_PixelShader = new D3D11.PixelShader(m_Device, byteCode);
        }

        using (var byteCode = ShaderBytecode.CompileFromFile("src/Shaders/DX/DefaultVS.hlsl", "main", "vs_4_0", ShaderFlags.Debug)) {
            m_VertexShader = new D3D11.VertexShader(m_Device, byteCode);

            var inputElements = new D3D11.InputElement[] {
                new D3D11.InputElement("POSITION", 0, Format.R32G32_Float, 0)
            };
            var inputSignature = ShaderSignature.GetInputSignature(byteCode);
            var inputLayout = new D3D11.InputLayout(m_Device, inputSignature, inputElements);
            m_DeviceContext.InputAssembler.InputLayout = inputLayout;
        }

        m_DeviceContext.VertexShader.Set(m_VertexShader);
        m_DeviceContext.PixelShader.Set(m_PixelShader);
    }
}

}
