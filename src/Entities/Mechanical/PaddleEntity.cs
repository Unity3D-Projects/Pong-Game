namespace PongBrain.Entities.Mechanical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components;
using Components.Graphical;
using Components.Input;
using Components.Physical;
using Core;
using Graphics;

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
            new BoundingBoxComponent { Width=0.06f, Height=0.28f },
            new ControlsComponent    { },
            new SpriteComponent      { ScaleX=0.06f,
                                       ScaleY =0.28f,
                                       Texture =Textures.White },
            new PaddleInfoComponent  { },
            new PositionComponent    { X=0.0f, Y=0.0f },
            new VelocityComponent    { X=0.0f, Y=0.0f }
        );
    }
}

}
