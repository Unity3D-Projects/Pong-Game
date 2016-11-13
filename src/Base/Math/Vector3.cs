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

    /*-------------------------------------
     * OPERATORS
     *-----------------------------------*/

    public static Vector3 operator +(Vector3 a, Vector3 b) {
        return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    }

    public static Vector3 operator *(float a, Vector3 b) {
        return new Vector3(a*b.X, a*b.Y, a*b.Z);
    }

    public static Vector3 operator *(Vector3 a, float b) {
        return new Vector3(a.X*b, a.Y*b, a.Z*b);
    }
}

}
