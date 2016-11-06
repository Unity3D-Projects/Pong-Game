namespace PongBrain.Base.Graphics {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Drawing;

/*-------------------------------------
 * interfaces
 *-----------------------------------*/

public static class Texture {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/
    
    private static ITexture s_White;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public static ITexture White {
        get {
            if (s_White != null) {
                return s_White;
            }

            var bmp = new Bitmap(1, 1);
            bmp.SetPixel(0, 0, System.Drawing.Color.White);

            s_White = new GdiPlus.GdiPlusTexture(bmp);

            return s_White;
        }
    }
}

}
