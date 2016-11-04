namespace PongBrain.Entities {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components;
using Components.Graphical;
using Components.Input;
using Components.Physical;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class PaddleEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public PaddleEntity() {
        AddComponents(
            new BodyComponent        { },
            new BoundingBoxComponent { Width=0.04f, Height=0.2f },
            new ControlsComponent    { },
            new RectangleComponent   { Width=0.04f, Height=0.2f },
            new PaddleInfoComponent  { },
            new PositionComponent    { X=-0.9f, Y=0.0f },
            new VelocityComponent    { X=0.0f, Y=-0.1f }
        );
    }
}

}
