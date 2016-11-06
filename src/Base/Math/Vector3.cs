namespace PongBrain.Base.Math {

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public struct Vector3 {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public static readonly Vector3 Zero = new Vector3(0.0f, 0.0f, 0.0f);

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float X { get; }
    public float Y { get; }
    public float Z { get; }

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
