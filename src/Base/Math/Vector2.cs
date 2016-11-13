namespace PongBrain.Base.Math {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
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

    public void Normalize() {
        var r = (float)Math.Sqrt(X*X + Y*Y);
        if (r > 0.0f) {
            X /= r;
            Y /= r;
        }
    }

    public float PerpDot(Vector2 a) {
        return X*a.Y - Y*a.X;
    }

    public Vector2 Perp() {
        return new Vector2(-Y, X);
    }

    /*-------------------------------------
     * OPERATORS
     *-----------------------------------*/

    public static Vector2 operator +(Vector2 a, Vector2 b) {
        return new Vector2(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator *(float a, Vector2 b) {
        return new Vector2(a*b.X, a*b.Y);
    }

    public static Vector2 operator *(Vector2 a, float b) {
        return new Vector2(a.X*b, a.Y*b);
    }
}

}
