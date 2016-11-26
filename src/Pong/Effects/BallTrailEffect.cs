namespace Pong.Effects {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using PrimusGE.Components;
using PrimusGE.Components.Physical;
using PrimusGE.Core;
using PrimusGE.Math;

using Entities.Graphical;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class BallTrailEffect: Effect {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    private readonly Entity m_Ball;

    private readonly int m_NumParticles;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public BallTrailEffect(Entity ball, int numParticles=2): base(0.0f) {
        m_Ball       = ball;
        m_NumParticles = numParticles;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Begin() {
        base.Begin();

        SpawnParticles();
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    private void SpawnParticles() {
        if (!DisableAll) {
            Game.Inst.SetTimeout(() => Begin(), 0.1f);
        }

        var ball = m_Ball.GetComponent<BodyComponent>();

        if (ball.IsStatic) {
            return;
        }

        var p = ball.Position;
        var v = ball.Velocity;

        for (var i = 0; i < m_NumParticles; i++) {
            var x        = 0.08f*(Pong.Rnd()-0.5f);
            var y        = 0.08f*(Pong.Rnd()-0.5f);
            var size     = 0.01f + 0.01f*Pong.Rnd();
            var particle = new RectangleEntity(p.X + x, p.Y + y, size, size);
            var theta    = 2.0f*(float)Math.PI * Pong.Rnd();
            var r        = 0.05f + 0.1f*Pong.Rnd();
            var a        = 0.5f + 0.6f*Pong.Rnd();
            var w        = (Pong.Rnd()-0.5f)*2.0f*(float)Math.PI*4.0f;
            var vx       = 0.3f*v.X + (float)Math.Cos(theta)*r;
            var vy       = 0.3f*v.Y + (float)Math.Sin(theta)*r;
            var body     = particle.GetComponent<BodyComponent>();

            body.Angle = theta;
            body.AngularVelocity = w;
            body.Velocity = new Vector2(vx, vy);

            particle.AddComponents(
                new LifetimeComponent { Lifetime = a },
                new Components.GlowComponent()
            );

            Game.Inst.Scene.AddEntity(particle);
        }
    }
}

}
