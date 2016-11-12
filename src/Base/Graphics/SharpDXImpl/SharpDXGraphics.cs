namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Runtime.InteropServices;
using System.Windows.Forms;

using Core;
using Math;
using Shaders;
using Textures;

using SharpDX;
using SharpDX.Direct3D;
using SharpDX.DXGI;

using D3D11   = SharpDX.Direct3D11;
using Vector2 = Math.Vector2;
using Vector3 = Math.Vector2;
using Vector4 = Math.Vector4;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class SharpDXGraphics: IGraphicsMgr {
    private D3D11.Buffer m_ShaderParams;
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private ITexture m_CurTexture;

    private SharpDXShader m_DefaultPixelShader;

    private SharpDXRenderTarget m_DefaultRenderTarget;

    private SharpDXShader m_DefaultVertexShader;

    public D3D11.Device Device;

    private D3D11.DeviceContext m_DeviceContext;

    private SharpDXShader m_PixelShader;

    private SharpDXRenderTarget m_RenderTarget;

    private ShaderMgr m_ShaderMgr;

    private SwapChain m_SwapChain;

    private SharpDXTextureMgr m_TextureMgr;

    private SharpDXTriMeshMgr m_TriMeshMgr;

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

    public IShaderMgr ShaderMgr {
        get { return m_ShaderMgr; }
    }

    public ITextureMgr TextureMgr {
        get { return m_TextureMgr; }
    }

    public ITriMeshMgr TriMeshMgr {
        get { return m_TriMeshMgr; }
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

        m_ShaderMgr.Cleanup();
        m_ShaderMgr = null;
        
        m_SwapChain.Dispose();
        m_SwapChain = null;

        Device.Dispose();
        Device = null;

        m_DeviceContext.Dispose();
        m_DeviceContext = null;

        //--------

        m_TextureMgr.Dispose();
        m_TextureMgr = null;

        m_TriMeshMgr.Dispose();
        m_TriMeshMgr = null;
    }

    public IRenderTarget CreateRenderTarget() {
        var width  = m_Window.ClientRectangle.Width;
        var height = m_Window.ClientRectangle.Height;

        var textureDescription = new D3D11.Texture2DDescription {
            ArraySize         = 1,
            BindFlags         = D3D11.BindFlags.RenderTarget | D3D11.BindFlags.ShaderResource,
            Format            = Format.R8G8B8A8_UNorm,
            MipLevels         = 1,
            SampleDescription = new SampleDescription(1, 0),
            Width             = width,
            Height            = height,
        };

        var texture      = new D3D11.Texture2D(Device, textureDescription);
        var renderTarget = new D3D11.RenderTargetView(Device, texture);

        return new SharpDXRenderTarget(this, texture, width, height, renderTarget);
    }

    public void DrawTriMesh(ITriMesh triMesh, Matrix4x4 transform) {
        var context        = Device.ImmediateContext;
        var inputAssembler = context.InputAssembler;
        var vbBinding      = ((SharpDXTriMesh)triMesh).Binding;

        inputAssembler.SetVertexBuffers(0, vbBinding);
        inputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleStrip;

        context.UpdateSubresource(ref transform, m_ShaderParams);

        context.Draw(triMesh.NumVerts, 0);

    }

    public void EndFrame() {
        m_SwapChain.Present(1, PresentFlags.None);
    }

    public void Init(Form window) {
        m_Window = window;

        InitDevice();
        InitShaders();

        m_TextureMgr = new SharpDXTextureMgr(this);
        m_TriMeshMgr = new SharpDXTriMeshMgr(this);

        var desc = new D3D11.BufferDescription(64, D3D11.ResourceUsage.Default, D3D11.BindFlags.ConstantBuffer, D3D11.CpuAccessFlags.None, D3D11.ResourceOptionFlags.None, 0);
        var o = Matrix4x4.Identity();
       
        m_ShaderParams = D3D11.Buffer.Create(Device, ref o, desc);
        m_DeviceContext.VertexShader.SetConstantBuffer(0, m_ShaderParams);
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

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

        D3D11.Device.CreateWithSwapChain(DriverType.Hardware, D3D11.DeviceCreationFlags.None, swapChainDesc, out Device, out m_SwapChain);
        m_DeviceContext = Device.ImmediateContext;

        var backBuffer = m_SwapChain.GetBackBuffer<D3D11.Texture2D>(0);
            m_DefaultRenderTarget = new SharpDXRenderTarget(this, backBuffer, width, height, new D3D11.RenderTargetView(Device, backBuffer));

        RenderTarget = m_DefaultRenderTarget;

        m_DeviceContext.Rasterizer.SetViewport(new Viewport(0, 0, width, height));
    }

    private void InitShaders() {
        m_ShaderMgr  = new ShaderMgr(Device);

        m_DefaultPixelShader  = (SharpDXShader)ShaderMgr.LoadPS("src/Shaders/DX/DefaultPS.hlsl");
        m_DefaultVertexShader = (SharpDXShader)ShaderMgr.LoadVS("src/Shaders/DX/DefaultVS.hlsl");

        PixelShader  = m_DefaultPixelShader;
        VertexShader = m_DefaultVertexShader;
    }
}

}
