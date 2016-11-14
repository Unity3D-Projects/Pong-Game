namespace PongBrain.Base.Math.Geom {

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class Shape {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Vector2[] Points { get; set; }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static Shape Rectangle(float width, float height) {
        var hh = 0.5f*height;
        var hw = 0.5f*width;

        var points = new [] {
            new Vector2( hw,  hh),
            new Vector2(-hw,  hh),
            new Vector2(-hw, -hh),
            new Vector2( hw, -hh)
        };
        
        return new Shape { Points = points };
    }
}

}
