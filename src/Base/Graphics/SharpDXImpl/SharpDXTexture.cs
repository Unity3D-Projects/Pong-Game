namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using SharpDX.Direct3D11;

using Textures;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal sealed class SharpDXTexture: ITexture {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public readonly Texture2D Texture;

    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private readonly int m_Height;

    private readonly int m_Width;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int Height {
        get { return m_Height; }
    }

    public int Width {
        get { return m_Width; }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTexture(Texture2D texture, int width, int height) {
        Texture = texture;
        
        m_Height = height;
        m_Width  = width;
    }
}

}
