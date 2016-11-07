namespace PongBrain.Base.Graphics {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Windows.Forms;

using Math;

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IGraphicsImpl {
    /*-------------------------------------
     * PROPERTIES
     *-----------------------------------*/

    string Name { get; }

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
