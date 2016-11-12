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
        var radius = 0.04f;

        AddComponents(
            new AngularVelocityComponent { W=2.0f*(float)Math.PI*2.0f },
            new BallInfoComponent        { Radius=radius },
            new PositionComponent        { X=0.0f, Y=0.0f },
            new RotationComponent        { },
            new ShaderComponent          { PixelShader=Game.Inst.Graphics.ShaderMgr.LoadPS("src/Shaders/DX/Test.hlsl") },
            new TriMeshComponent         { TriMesh=Game.Inst.Graphics.TriMeshMgr.CreateQuad(2.0f*radius, 2.0f*radius) },
            new VelocityComponent        { X=0.0f, Y=0.0f }
        );
    }
}

}
