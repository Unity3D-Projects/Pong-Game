namespace PongBrain.Pong.Effects {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Base.Components;
using Base.Components.Physical;
using Base.Core;

using Components;
using Entities.Graphical;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class TrailEffect: Effect {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    private readonly Entity m_Entity;

    private readonly int m_NumParticles;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public TrailEffect(Entity entity, int numParticles=2): base(0.0f) {
        m_Entity       = entity;
        m_NumParticles = numParticles;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Begin() {
        base.Begin();

        var random = new Random();

        var p = m_Entity.GetComponent<BodyComponent>().Position;
        var v = m_Entity.GetComponent<BodyComponent>().Velocity;

        for (var i = 0; i < m_NumParticles; i++) {
            var x        = 0.08f*((float)random.NextDouble()-0.5f);
            var y        = 0.08f*((float)random.NextDouble()-0.5f);
            var size     = 0.01f + 0.01f*(float)random.NextDouble();
            var particle = new RectangleEntity(p.X + x, p.Y + y, size, size);
            var theta    = 2.0f*(float)Math.PI * (float)random.NextDouble();
            var r        = 0.05f + 0.1f*(float)random.NextDouble();
            var a        = 0.5f + 0.6f*(float)random.NextDouble();
            var w        = ((float)random.NextDouble()-0.5f)*2.0f*(float)Math.PI*4.0f;
            var vx       = 0.3f*v.X + (float)Math.Cos(theta)*r;
            var vy       = 0.3f*v.Y + (float)Math.Sin(theta)*r;
            var body     = particle.GetComponent<BodyComponent>();

            body.Angle = theta;
            body.AngularVelocity = w;
            body.Velocity = new Base.Math.Vector2(vx, vy);

            particle.AddComponents(
                new LifetimeComponent { Lifetime = a }
            );

            Game.Inst.Scene.AddEntity(particle);
        }

        if (!DisableAll) {
            Game.Inst.SetTimeout(() => Begin(), 0.1f);
        }
    }
}

}
