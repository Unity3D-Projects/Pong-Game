namespace PongBrain.Pong.Entities.Mechanical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Components.Graphical;
using Base.Components.Input;
using Base.Components.Physical;
using Base.Core;
using Base.Math;
using Base.Math.Geom;

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

        var mass = 1.0f;
        var width  = 0.06f;
        var height = 0.38f;
        var quad   = g.TriMeshMgr.CreateQuad(width, height);

        AddComponents(
            new BodyComponent           { InvMoI     = MathUtil.RectInvMoI(mass, width, height),
                                          LinearDrag = 4.0f,
                                          InvMass     = 1.0f/mass,
                                          Shape      = Shape.Rectangle(width, height) },
            new ControlsComponent       { },
            new MotionBlurComponent     { },
            new PaddleInfoComponent     { },
            new TriMeshComponent        { TriMesh=quad }
        );
    }
}

}
