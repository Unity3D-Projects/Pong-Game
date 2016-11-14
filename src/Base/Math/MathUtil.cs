namespace PongBrain.Base.Math {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public static class MathUtil {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static float RectInvMoI(float mass, float width, float height) {
        if (mass == 0.0f) {
            return 0.0f;
        }

        var moi = (mass/12.0f)*(width*width + height*height);

        return 1.0f/moi;
    }
}

}
