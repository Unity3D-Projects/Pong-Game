namespace PongBrain.Base.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Components.Physical;
using Core;
using Math;
using Math.Calc;
using Messages;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal struct CollisionInfo {
    public bool IsColliding;
    public Vector2 Normal;
    public Entity EntityA;
    public Vector2 Contact;
}

public class PhysicsSubsystem: Subsystem {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private Rectangle m_WorldBounds;

    /*-------------------------------------
     * PUBIC PROPERTIES
     *-----------------------------------*/

    public IOdeSolver Solver = new EulerOdeSolver();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public PhysicsSubsystem(Rectangle worldBounds) {
        m_WorldBounds = worldBounds;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        var collisions = new List<CollisionInfo>();
        var entities   = Game.Inst.Scene.GetEntities<BodyComponent>();

        foreach (var entity in entities) {
            var body = entity.GetComponent<BodyComponent>();
            Array.Copy(body.State, body.PrevState, body.State.Length);
        }

        var a = 1;
        var b = 1;

        while (dt > 1.0/1000000.0f) {
            float x = (float)a/b;

            foreach (var entity in entities) {
                var body = entity.GetComponent<BodyComponent>();

                if (body.IsStatic) {
                    continue;
                }

                Array.Copy(body.PrevState, body.State, body.State.Length);
                Solver.Solve(entity.GetComponent<BodyComponent>(), dt*x, body.DerivsFn);
            }

            if (FindCollisions(entities, collisions) > 0) {
                if ((b <<= 1) == (1 << 8)) {
                    ResolveCollisions(collisions);

                    foreach (var entity in entities) {
                        var body = entity.GetComponent<BodyComponent>();

                        if (body.IsStatic) {
                            continue;
                        }

                        body.PrevState[3] = body.State[3];
                        body.PrevState[4] = body.State[4];
                        body.PrevState[5] = body.State[5];
                    }

                    a = b = 1;
                }
            }
            else {
                foreach (var entity in entities) {
                    var body = entity.GetComponent<BodyComponent>();

                    if (body.IsStatic) {
                        continue;
                    }

                    Array.Copy(body.State, body.PrevState, body.State.Length);
                }

                dt -= dt*x;
                a <<= 1;
            }
        }
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    private CollisionInfo FindBodyWorldCollision(Entity entity) {
        var body  = entity.GetComponent<BodyComponent>();
        var shape = body.Shape;
        
        if (shape == null) {
            return default (CollisionInfo);
        }

        int numContacts = 0;
        var ci = new CollisionInfo();

        ci.EntityA = entity;

        Matrix2x2 rot = Matrix2x2.RotateZ(body.Angle);

        foreach (var p in shape.Points) {
            Vector2 localPos = rot*p;
            Vector2 worldPos = localPos + body.Position;

            bool f = false;

            if (worldPos.X < m_WorldBounds.Left  ) { f = true; ci.Normal += new Vector2( 1.0f,  0.0f); }
            if (worldPos.X > m_WorldBounds.Right ) { f = true; ci.Normal += new Vector2(-1.0f,  0.0f); }
            if (worldPos.Y < m_WorldBounds.Bottom) { f = true; ci.Normal += new Vector2( 0.0f,  1.0f); }
            if (worldPos.Y > m_WorldBounds.Top   ) { f = true; ci.Normal += new Vector2( 0.0f, -1.0f); }

            if (f) {
                numContacts++;
                ci.Contact += localPos;
            }
        }

        if (numContacts > 0) {
            ci.IsColliding = true;
            ci.Contact *= 1.0f / numContacts;
            ci.Normal.Normalize();
        }

        return ci;
    }

    private int FindCollisions(ICollection<Entity> bodies, IList<CollisionInfo> collisions) {
        collisions.Clear();
        foreach (var body in bodies) {
            var ci = FindBodyWorldCollision(body);
            if (ci.IsColliding) {
                collisions.Add(ci);
            }
        }

        return collisions.Count;
    }

    private void ResolveCollisions(ICollection<CollisionInfo> collisions) {
        foreach (var ci in collisions) {
            var a = ci.EntityA.GetComponent<BodyComponent>();

            Vector2 vel = ci.Contact.Perp();

            vel *= a.AngularVelocity;
            vel += a.Velocity;

            var invMass = a.InvMass;
            var invMoI  = (float)Math.Pow(ci.Contact.PerpDot(ci.Normal), 2.0f);

            float j = -(1.0f + a.Restitution) * vel.Dot(ci.Normal);
                  j /= invMass + invMoI * a.InvMoI;

            Vector2 impulse = j*ci.Normal;
            
            a.ApplyImpulse(impulse, ci.Contact);

            Game.Inst.PostMessage(new CollisionMsg(ci.EntityA, null, ci.Contact, ci.Normal));
        }
    }
}

}
