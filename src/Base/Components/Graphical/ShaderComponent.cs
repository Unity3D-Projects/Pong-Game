namespace PongBrain.Base.Components.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Graphics.Shaders;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class ShaderComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public IShader PixelShader { get; set; }

    public IShader VertexShader { get; set; }
}

}
