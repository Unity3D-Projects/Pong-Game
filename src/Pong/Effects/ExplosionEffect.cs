namespace Pong.Effects {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Base.Components;
using Base.Components.Physical;
using Base.Core;
using Base.Math;

using Entities.Graphical;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class ExplosionEffect: Effect {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private readonly int m_NumParticles;

    private readonly float m_X;
    private readonly float m_Y;

    private static readonly Random s_Random = new Random();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public ExplosionEffect(float x, float y, int numParticles=10): base(0.0f) {
        m_X = x;
        m_Y = y;

        m_NumParticles = numParticles;
    }

    public ExplosionEffect(Vector2 p, int numParticles=10)
        : this(p.X, p.Y, numParticles)
    { }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Begin() {
        base.Begin();

        for (var i = 0; i < m_NumParticles; i++) {
            var size     = 0.02f + 0.02f*(float)s_Random.NextDouble();
            var particle = new RectangleEntity(m_X, m_Y, size, size);
            var theta    = 2.0f*(float)Math.PI * (float)s_Random.NextDouble();
            var r        = 0.4f + 0.4f*(float)s_Random.NextDouble();
            var a        = 0.2f + 0.2f*(float)s_Random.NextDouble();
            var w        = ((float)s_Random.NextDouble()-0.5f)*2.0f*(float)Math.PI*6.0f;
            var vx       = (float)Math.Cos(theta)*r;
            var vy       = (float)Math.Sin(theta)*r;
            var body     = particle.GetComponent<BodyComponent>();

            body.Angle = theta;
            body.AngularVelocity = w;
            body.Velocity = new Base.Math.Vector2(vx, vy);

            particle.AddComponents(
                new LifetimeComponent { Lifetime = a }
            );

            Game.Inst.Scene.AddEntity(particle);
        }
    }
}

}
