#if false

namespace PongBrain.Base.Graphics.GdiPlusImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Drawing;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class GdiPlusTextureManager: ITextureManager {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/
    
    private GdiPlusTexture m_White;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public ITexture White {
        get {
            if (m_White != null) {
                return m_White;
            }

            var bmp = new Bitmap(1, 1);
            bmp.SetPixel(0, 0, Color.White);

            m_White = new GdiPlusTexture(bmp);

            return m_White;
        }
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Cleanup() {
        if (m_White != null) {
            m_White.m_Bitmap.Dispose();
            m_White = null;
        }
    }

    public void Init() {
    }
}

}

#endif
