namespace PongBrain.Pong.Entities.Mechanical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

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
        var g = Game.Inst.Graphics;

        var radius = 0.04f;
        var quad   = g.TriMeshMgr.CreateQuad(2.0f*radius, 2.0f*radius);

        AddComponents(
            new AngularVelocityComponent { W=2.0f*(float)Math.PI*1.0f },
            new BallInfoComponent        { Radius=radius },
            new PositionComponent        { },
            new MotionBlurComponent      { },
            new RotationComponent        { },
            new TriMeshComponent         { TriMesh=quad },
            new VelocityComponent        { }
        );
    }
}

}
