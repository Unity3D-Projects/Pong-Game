namespace PongBrain.Pong.Effects {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Base.Components;
using Base.Components.Physical;
using Base.Core;

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

        var p = m_Entity.GetComponent<PositionComponent>();
        var v = m_Entity.GetComponent<VelocityComponent>();

        for (var i = 0; i < m_NumParticles; i++) {
            var size     = 0.005f + 0.005f*(float)random.NextDouble();
            var particle = new RectangleEntity(p.X, p.Y, size, size);
            var velocity = particle.AddComponent(new VelocityComponent());
            var theta    = 2.0f*(float)Math.PI * (float)random.NextDouble();
            var r        = 0.05f + 0.1f*(float)random.NextDouble();
            var a        = 0.5f + 0.6f*(float)random.NextDouble();

            velocity.X = 0.3f*v.X + (float)Math.Cos(theta)*r;
            velocity.Y = 0.3f*v.Y + (float)Math.Sin(theta)*r;

            particle.AddComponent(new LifetimeComponent { Lifetime = a });

            Game.Inst.Scene.AddEntity(particle);
        }

        if (!DisableAll) {
            Game.Inst.SetTimeout(() => Begin(), 0.1f);
        }
    }
}

}
