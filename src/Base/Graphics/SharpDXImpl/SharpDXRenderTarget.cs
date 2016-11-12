namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Shaders;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXRenderTarget: SharpDXTexture, IRenderTarget {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public readonly D3D11.RenderTargetView View;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXRenderTarget(SharpDXGraphics        graphics,
                               D3D11.Texture2D        texture,
                               int                    width,
                               int                    height,
                               D3D11.RenderTargetView view)
        : base(graphics, texture, width, height)
    {
        View = view;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Clear(Color clearColor) {
        var context = Graphics.Device.ImmediateContext;
        var color = new SharpDX.Color(clearColor.ToIntABGR());
        context.ClearRenderTargetView(View, color);
    }
}

}
