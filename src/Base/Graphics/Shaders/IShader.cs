namespace PongBrain.Base.Graphics.Shaders {

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface IShader {
    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void SetShaderParam(string name, ref object data);
}

}
