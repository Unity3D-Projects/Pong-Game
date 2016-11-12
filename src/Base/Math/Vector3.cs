namespace PongBrain.Base.Math {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Runtime.InteropServices;

/*-------------------------------------
 * STRUCTS
 *-----------------------------------*/

[StructLayout(LayoutKind.Sequential)]
public struct Vector3 {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float X;
    public float Y;
    public float Z;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Vector3(float x=0.0f, float y=0.0f, float z=0.0f) {
        X = x;
        Y = y;
        Z = z;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public float Dot(Vector3 v) {
        return X*v.X + Y*v.Y + Z*v.Z;
    }
}

}
