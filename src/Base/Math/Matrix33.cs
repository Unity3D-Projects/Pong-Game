namespace PongBrain.Base.Math {

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public struct Matrix33 {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Vector3 X { get; }
    public Vector3 Y { get; }
    public Vector3 Z { get; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Matrix33(Vector3 x, Vector3 y, Vector3 z) {
        X = x;
        Y = y;
        Z = z;
    }

    public Matrix33(float xx, float xy, float xz,
                    float yx, float yy, float yz,
                    float zx, float zy, float zz)
        : this(new Vector3(xx, xy, xz),
               new Vector3(yx, yy, yz),
               new Vector3(zx, zy, zz))
    {
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static Matrix33 Identity() {
        return new Matrix33(1.0f, 0.0f, 0.0f,
                            0.0f, 1.0f, 0.0f,
                            0.0f, 0.0f, 1.0f);
    }

    public static Matrix33 Scale(float x, float y) {
        var m = new Matrix33(x   , 0.0f, 0.0f,
                             0.0f, y   , 0.0f,
                             0.0f, 0.0f, 1.0f);

        return m;
    }

    public static Matrix33 Translate(float x, float y) {
        var m = new Matrix33(1.0f, 0.0f, x,
                             0.0f, 1.0f, y,
                             0.0f, 0.0f, 1.0f);

        return m;
    }

    public Matrix33 Transpose() {
        return new Matrix33();
    }

    /*-------------------------------------
     * OPERATORS
     *-----------------------------------*/

    public static Matrix33 operator *(Matrix33 a, Matrix33 b) {
        var xx = a.X.X*b.X.X+a.X.Y*b.Y.X+a.X.Z*b.Z.X;
        var xy = a.X.X*b.X.Y+a.X.Y*b.Y.Y+a.X.Z*b.Z.Y;
        var xz = a.X.X*b.X.Z+a.X.Y*b.Y.Z+a.X.Z*b.Z.Z;
        var yx = a.Y.X*b.X.X+a.Y.Y*b.Y.X+a.Y.Z*b.Z.X;
        var yy = a.Y.X*b.X.Y+a.Y.Y*b.Y.Y+a.Y.Z*b.Z.Y;
        var yz = a.Y.X*b.X.Z+a.Y.Y*b.Y.Z+a.Y.Z*b.Z.Z;
        var zx = a.Z.X*b.X.X+a.Z.Y*b.Y.X+a.Z.Z*b.Z.X;
        var zy = a.Z.X*b.X.Y+a.Z.Y*b.Y.Y+a.Z.Z*b.Z.Y;
        var zz = a.Z.X*b.X.Z+a.Z.Y*b.Y.Z+a.Z.Z*b.Z.Z;

        return new Matrix33(xx, xy, xz, yx, yy, yz, zx, zy, zz);
    }

    public static Vector3 operator *(Matrix33 m, Vector3 v) {
        var x = m.X.X*v.X + m.X.Y*v.Y + m.X.Z*v.Z;
        var y = m.Y.X*v.X + m.Y.Y*v.Y + m.Y.Z*v.Z;
        var z = m.Z.X*v.X + m.Z.Y*v.Y + m.Z.Z*v.Z;

        return new Vector3(x, y, z);
    }
}

}
