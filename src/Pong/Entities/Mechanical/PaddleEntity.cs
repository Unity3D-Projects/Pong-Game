namespace PongBrain.Pong.Entities.Mechanical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Components.Graphical;
using Base.Components.Input;
using Base.Components.Physical;
using Base.Core;
using Base.Graphics;

using Components;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class PaddleEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public PaddleEntity() {
        AddComponents(
            new BodyComponent           { },
            new AxisAlignedBoxComponent { Width=0.06f, Height=0.28f },
            new ControlsComponent       { },
            new SpriteComponent         { ScaleX=0.06f,
                                          ScaleY =0.28f,
                                          Texture =Textures.White },
            new PaddleInfoComponent     { },
            new PositionComponent       { X=0.0f, Y=0.0f },
            new VelocityComponent       { X=0.0f, Y=0.0f }
        );
    }
}

}
