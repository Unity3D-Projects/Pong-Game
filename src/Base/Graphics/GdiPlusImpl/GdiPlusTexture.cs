namespace PongBrain.Base.Graphics.GdiPlusImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Drawing;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class GdiPlusTexture: ITexture {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/
    
    internal readonly Bitmap m_Bitmap;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int Height {
        get { return m_Bitmap.Height; }
    }

    public int Width {
        get { return m_Bitmap.Width; }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public GdiPlusTexture(Bitmap bmp) {
        m_Bitmap = bmp;
    }
}

}
