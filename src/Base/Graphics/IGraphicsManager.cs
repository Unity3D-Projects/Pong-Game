namespace PongBrain.Base.Graphics {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Windows.Forms;

using Math;
using Shaders;
using Textures;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IGraphicsManager {
    /*-------------------------------------
     * PROPERTIES
     *-----------------------------------*/

    string Name { get; }

    IShader PixelShader { get; set; }

    IShaderManager Shader { get; }

    ITextureManager Texture { get; }

    IShader VertexShader { get; set; }

    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void BeginFrame();

    void Cleanup();

    void Clear(Color clearColor);

    void DrawTexture(ITexture texture, Matrix4x4 transform);

    void EndFrame();

    void Init(Form window);
}

}
