namespace PongBrain.Pong.Entities.Mechanical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Components.Graphical;
using Base.Components.Physical;
using Base.Core;

using Components;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class BallEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public BallEntity() {
        var radius = 0.03f;

        AddComponents(
            new BallInfoComponent { Radius=radius },
            new SpriteComponent   { ScaleX  = 2.0f*radius,
                                    ScaleY  = 2.0f*radius,
                                    Shader  = Game.Inst.Graphics.Shader.LoadPixelShader("src/Shaders/DX/Test.hlsl"),
                                    Texture = Game.Inst.Graphics.Texture.White },
            new PositionComponent { X=0.0f, Y=0.0f },
            new VelocityComponent { X=0.0f, Y=0.0f }

        );
    }
}

}
