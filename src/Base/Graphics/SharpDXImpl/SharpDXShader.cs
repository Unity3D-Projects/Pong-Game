namespace PongBrain.Base.Graphics.Shaders {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal sealed class SharpDXShader: IShader {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public readonly D3D11.InputLayout InputLayout;

    public readonly D3D11.DeviceChild Shader;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXShader(D3D11.DeviceChild shader,
                         D3D11.InputLayout inputLayout=null)
    {
        InputLayout = inputLayout;
        Shader      = shader;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void SetShaderParam(string name, ref object data) {

    }
}

}
