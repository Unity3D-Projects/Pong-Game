namespace PongBrain.Base.Graphics.Shaders {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal sealed class SharpDXRenderTarget: IRenderTarget {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public readonly D3D11.RenderTargetView View;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXRenderTarget(D3D11.RenderTargetView view) {
        View = view;
    }
}

}
