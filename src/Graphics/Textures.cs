namespace PongBrain.Graphics {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Drawing;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public static class Textures {
    private static Bitmap s_White;

    public static Bitmap White {
        get {
            if (s_White != null) {
                return s_White;
            }

            var bmp = new Bitmap(1, 1);
            bmp.SetPixel(0, 0, Color.White);

            s_White = bmp;

            return bmp;
        }
    }
}

}
