namespace PongBrain.Base.Graphics {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Drawing;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class Texture {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/
    
    internal readonly Bitmap m_Bitmap;

    private static Texture s_White;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int Height {
        get { return m_Bitmap.Height; }
    }

    public int Width {
        get { return m_Bitmap.Width; }
    }

    public static Texture White {
        get {
            if (s_White != null) {
                return s_White;
            }

            var bmp = new Bitmap(1, 1);
            bmp.SetPixel(0, 0, System.Drawing.Color.White);

            s_White = new Texture(bmp);

            return s_White;
        }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Texture(Bitmap bmp) {
        m_Bitmap = bmp;
    }
}

}
