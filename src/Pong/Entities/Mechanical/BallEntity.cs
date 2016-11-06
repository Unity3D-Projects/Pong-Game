namespace PongBrain.Pong.Entities.Mechanical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Components.Graphical;
using Base.Components.Physical;
using Base.Core;
using Base.Graphics;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class BallEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public BallEntity() {
        AddComponents(
            new AxisAlignedBoxComponent { Width=0.06f, Height=0.06f },
            new SpriteComponent         { ScaleX=0.06f,
                                          ScaleY =0.06f,
                                          Texture =Textures.White },
            new PositionComponent       { X=0.0f, Y=0.0f },
            new VelocityComponent       { X=1.1f, Y=1.1f }
        );
    }
}

}
