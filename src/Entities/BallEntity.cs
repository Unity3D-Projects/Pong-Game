namespace PongBrain.Entities {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.Graphical;
using Components.Physical;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class BallEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public BallEntity() {
        var r = 0.03f;

        AddComponents(
            new BoundingBoxComponent { Width=2.0f*r, Height=2.0f*r },
            new CircleComponent      { Radius=r },
            new PositionComponent    { X=0.0f, Y=0.0f },
            new VelocityComponent    { X=0.5f, Y=0.5f }
        );
    }
}

}
