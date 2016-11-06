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
     * METHODS
     *-----------------------------------*/

    void Cleanup();
    void Clear(Color clearColor);
    void DrawTexture(ITexture texture, float x, float y);
    void DrawTexture(ITexture texture, Matrix33 transform);
    void Init(Form window);
    void SwapBuffers();
}

}
