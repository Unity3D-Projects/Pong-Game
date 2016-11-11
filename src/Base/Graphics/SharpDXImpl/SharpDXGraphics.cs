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
using Shaders;
using Textures;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class SharpDXGraphics: IGraphicsManager {
    private D3D11.Buffer m_ShaderParams;
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private SharpDXShader m_DefaultPixelShader;

    private SharpDXRenderTarget m_DefaultRenderTarget;

    private SharpDXShader m_DefaultVertexShader;

    private D3D11.Device m_Device;

    private D3D11.DeviceContext m_DeviceContext;

    private SharpDXShader m_PixelShader;

    private D3D11.Buffer m_Quad;

    private SharpDXRenderTarget m_RenderTarget;

    private SharpDXShaderManager m_ShaderManager;

    private SwapChain m_SwapChain;

    private SharpDXTextureManager m_TextureManager;

    private SharpDXShader m_VertexShader;

    private Form m_Window;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public string Name {
        get { return "SharpDX"; }
    }

    public IShader PixelShader {
        get { return m_PixelShader; }
        set {
            var shader = (SharpDXShader)value;

            if (shader == null) {
                shader = m_DefaultPixelShader;
            }

            if (shader == m_PixelShader) {
                return;
            }

            m_DeviceContext.PixelShader.Set((D3D11.PixelShader)shader.Shader);
            m_PixelShader = shader;
        }
    }

    public IRenderTarget RenderTarget {
        get { return m_RenderTarget; }
        set {
            var renderTarget = (SharpDXRenderTarget)value;

            if (renderTarget == null) {
                renderTarget = m_DefaultRenderTarget;
            }

            if (renderTarget == m_RenderTarget) {
                return;
            }

            m_DeviceContext.OutputMerger.SetRenderTargets(renderTarget.View);
            m_RenderTarget = renderTarget;
        }
    }

    public IShaderManager Shader {
        get { return m_ShaderManager; }
    }

    public ITextureManager Texture {
        get { return m_TextureManager; }
    }

    public IShader VertexShader {
        get { return m_VertexShader; }
        set {
            var shader = (SharpDXShader)value;

            if (shader == null) {
                shader = m_DefaultVertexShader;
            }

            if (shader == m_VertexShader) {
                return;
            }

            m_DeviceContext.VertexShader.Set((D3D11.VertexShader)shader.Shader);
            m_DeviceContext.InputAssembler.InputLayout = shader.InputLayout;

            m_VertexShader = shader;
        }
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void BeginFrame() {
    }

    public void Cleanup() {
        m_DeviceContext.PixelShader.Set(null);
        m_DeviceContext.VertexShader.Set(null);

        m_ShaderManager.Cleanup();
        m_ShaderManager = null;

        m_TextureManager.Cleanup();
        m_TextureManager = null;
        
        m_Quad.Dispose();
        m_Quad = null;

        m_SwapChain.Dispose();
        m_SwapChain = null;

        m_Device.Dispose();
        m_Device = null;

        m_DeviceContext.Dispose();
        m_DeviceContext = null;
    }

    public void Clear(Graphics.Color clearColor) {
        var color = new Color(clearColor.ToIntABGR());
        m_DeviceContext.ClearRenderTargetView(m_RenderTarget.View, color);
    }

    public void CreateRenderTarget(int width, int height) {

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

        m_TextureManager = new SharpDXTextureManager(m_Device);

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
            m_DefaultRenderTarget = new SharpDXRenderTarget(new D3D11.RenderTargetView(m_Device, backBuffer));
        }

        RenderTarget = m_DefaultRenderTarget;

        m_DeviceContext.Rasterizer.SetViewport(new Viewport(0, 0, width, height));
    }

    private void InitShaders() {
        m_ShaderManager  = new SharpDXShaderManager(m_Device);

        m_DefaultPixelShader  = (SharpDXShader)Shader.LoadPixelShader("src/Shaders/DX/DefaultPS.hlsl");
        m_DefaultVertexShader = (SharpDXShader)Shader.LoadVertexShader("src/Shaders/DX/DefaultVS.hlsl");

        PixelShader  = m_DefaultPixelShader;
        VertexShader = m_DefaultVertexShader;
    }
}

}
