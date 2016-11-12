namespace PongBrain.Base.Math {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Runtime.InteropServices;

/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

[StructLayout(LayoutKind.Sequential)]
public struct Vector2 {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float X;
    public float Y;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Vector2(float x=0.0f, float y=0.0f) {
        X = x;
        Y = y;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public float Dot(Vector2 v) {
        return X*v.X + Y*v.Y;
    }
}

}
