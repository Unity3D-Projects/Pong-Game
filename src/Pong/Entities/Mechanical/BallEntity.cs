namespace PongBrain.Pong.Entities.Mechanical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Base.Components.Graphical;
using Base.Components.Physical;
using Base.Core;
using Base.Math;
using Base.Math.Geom;

using Components;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class BallEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public BallEntity() {
        var g = Game.Inst.Graphics;

        var mass   = 1.0f;
        var radius = 0.04f;
        var shape  = Shape.Rectangle(2.0f*radius, 2.0f*radius);

        BodyComponent body;

        AddComponents(
            new BallInfoComponent   { Radius      = radius },
     body = new BodyComponent       { InvMoI      = MathUtil.RectInvMoI(mass, 2.0f*radius, 2.0f*radius),
                                      InvMass     = 1.0f/mass,
                                      LinearDrag  = 0.0f,
                                      Restitution = 1.0f,
                                      Shape       = shape },
            new MotionBlurComponent { },
            new TriMeshComponent    { TriMesh = g.TriMeshMgr.FromShape(shape) }
        );

        body.DerivsFn = (state, derivs) => {
            derivs[0] = state[3];
            derivs[1] = state[4];
            derivs[2] = state[5];

            derivs[3] = Math.Sign(derivs[0]);
            derivs[4] = derivs[1] * 0.2f;
            derivs[5] = -derivs[2];
        };
    }
}

}
