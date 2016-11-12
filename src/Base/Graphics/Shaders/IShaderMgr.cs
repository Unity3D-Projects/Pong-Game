namespace PongBrain.Base.Graphics.Shaders {

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IShaderMgr {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    IShader LoadPS(string path);
    IShader LoadVS(string path);
}

}
