namespace PongBrain.Base.Graphics.Shaders {

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IShaderManager {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void Cleanup();
    void Init();
    IShader LoadPixelShader(string path);
    IShader LoadVertexShader(string path);
}

}
