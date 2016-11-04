namespace PongBrain.Entities.Mechanical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.Graphical;
using Components.Physical;
using Core;
using Graphics;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class BallEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public BallEntity() {
        AddComponents(
            new BoundingBoxComponent { Width=0.06f, Height=0.06f },
            new SpriteComponent      { ScaleX=0.06f,
                                       ScaleY =0.06f,
                                       Texture =Textures.White },
            new PositionComponent    { X=0.0f, Y=0.0f },
            new VelocityComponent    { X=0.7f, Y=0.7f }
        );
    }
}

}
