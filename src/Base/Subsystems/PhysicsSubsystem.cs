namespace PongBrain.Base.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.Physical;
using Core;
using Math;
using Messages;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class PhysicsSubsystem: Subsystem {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    private Rectangle m_WorldBounds;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public PhysicsSubsystem(Rectangle worldBounds)
    {
        m_WorldBounds = worldBounds;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        var worldLeft   = m_WorldBounds.Left;
        var worldRight  = m_WorldBounds.Right;
        var worldBottom = m_WorldBounds.Bottom;
        var worldTop    = m_WorldBounds.Top;

        var entities = Game.Inst.Scene.GetEntities<VelocityComponent>();
        foreach (var entity in entities) {
            var body     = entity.GetComponent<BodyComponent>();
            var position = entity.GetComponent<PositionComponent>();
            var velocity = entity.GetComponent<VelocityComponent>();

            if (position == null) {
                continue;
            }

            if (body != null) {
                velocity.X -= velocity.X * body.LinearDrag * dt;
                velocity.Y -= velocity.Y * body.LinearDrag * dt;
            }

            position.X += velocity.X * dt;
            position.Y += velocity.Y * dt;

            var aab = entity.GetComponent<AxisAlignedBoxComponent>();

            if (aab != null) {
                var e = 1.0f;
                var f = 0.0f;

                if (body != null) {
                    e = body.Restitution;
                    f = body.Friction;
                }

                if (position.X < worldLeft + 0.5f*aab.Width) {
                    position.X = worldLeft + 0.5f*aab.Width;
                    velocity.X *= -e;
                    velocity.Y -= velocity.Y * f;

                    Game.Inst.PostMessage(new CollisionMessage(entity));
                }
                else if (position.X > worldRight - 0.5f*aab.Width) {
                    position.X = worldRight - 0.5f*aab.Width;
                    velocity.X *= -e;
                    velocity.Y -= velocity.Y * f;

                    Game.Inst.PostMessage(new CollisionMessage(entity));
                }

                if (position.Y < worldBottom + 0.5f*aab.Height) {
                    position.Y = worldBottom + 0.5f*aab.Height;
                    velocity.X -= velocity.X * f;
                    velocity.Y *= -e;

                    Game.Inst.PostMessage(new CollisionMessage(entity));
                }
                else if (position.Y > worldTop - 0.5f*aab.Height) {
                    position.Y = worldTop - 0.5f*aab.Height;
                    velocity.X -= velocity.X * f;
                    velocity.Y *= -e;

                    Game.Inst.PostMessage(new CollisionMessage(entity));
                }
            }
        }
    }
}

}
