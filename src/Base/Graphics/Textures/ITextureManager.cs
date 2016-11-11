namespace PongBrain.Base.Graphics.Textures {

/*-------------------------------------
 * INTERFACES
 *-----------------------------------*/

public interface ITextureManager {
    /*-------------------------------------
     * PROPERTIES
     *-----------------------------------*/

    ITexture White { get; }

    /*-------------------------------------
     * METHODS
     *-----------------------------------*/

    void Cleanup();
    void Init();
}

}
