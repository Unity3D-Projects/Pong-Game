namespace PongBrain.Pong.Entities.Mechanical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Components.Graphical;
using Base.Components.Input;
using Base.Components.Physical;
using Base.Core;

using Components;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class PaddleEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public PaddleEntity() {
        var g = Game.Inst.Graphics;

        var width  = 0.06f;
        var height = 0.38f;
        var quad   = g.TriMeshMgr.CreateQuad(width, height);

        AddComponents(
            new BodyComponent           { },
            new AxisAlignedBoxComponent { Width=width, Height=height },
            new ControlsComponent       { },
            new MotionBlurComponent     { },
            new PaddleInfoComponent     { },
            new PositionComponent       { X=0.0f, Y=0.0f },
            new TriMeshComponent        { TriMesh=quad },
            new VelocityComponent       { X=0.0f, Y=0.0f }
        );
    }
}

}
