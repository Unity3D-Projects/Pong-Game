namespace Pong.Entities.Mechanical {

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
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float Tilt { get; set; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public PaddleEntity(float x, float y) {
        var g = Game.Inst.Graphics;

        var mass = 9.0f;
        var width  = 0.06f;
        var height = 0.38f;
        var quad   = g.TriMeshMgr.CreateQuad(width, height);

        BodyComponent body;

        AddComponents(
     body = new BodyComponent           { InvMoI     = MathUtil.RectInvMoI(mass, width, height),
                                          LinearDrag = 4.0f,
                                          InvMass    = mass > 0.0f ? 1.0f/mass : 0.0f,
                                          Position   = new Vector2(x, y),
                                          Shape      = Shape.Rectangle(width, height) },
            new ControlsComponent       { },
            new MotionBlurComponent     { },
            new PaddleInfoComponent     { },
            new TriMeshComponent        { TriMesh=quad }
        );

        body.DerivsFn = (state, derivs) => {
            derivs[0] = state[3];
            derivs[1] = state[4];
            derivs[2] = state[5];

            derivs[3] = -derivs[0] * body.LinearDrag + (x - state[0]) * 40.0f;
            derivs[4] = -derivs[1] * body.LinearDrag;
            derivs[5] = -derivs[2] * 5.0f + (Tilt - state[2]) * 40.0f;
        };
    }
}

}
