namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class SharpDXTexture: ITexture {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/
    
    internal readonly object m_Texture;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int Height {
        get { return 0; }
    }

    public int Width {
        get { return 0; }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTexture(object texture) {
        m_Texture = texture;
    }
}

}
