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
        var quad   = g.TriMeshMgr.CreateQuad(2.0f*radius, 2.0f*radius);

        AddComponents(
            new BallInfoComponent        { Radius = radius },
            new BodyComponent            { AngularVelocity = 2.0f*(float)Math.PI*1.0f,
                                           InvMoI          = MathUtil.RectInvMoI(mass, 2.0f*radius, 2.0f*radius),
                                           InvMass         = 1.0f/mass,
                                           LinearDrag      = 0.0f,
                                           Restitution     = 1.0f,
                                           Shape           = Shape.Rectangle(2.0f*radius, 2.0f*radius) },
            new MotionBlurComponent      { },
            new TriMeshComponent         { TriMesh = quad }
        );
    }
}

}
