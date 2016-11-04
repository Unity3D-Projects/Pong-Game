namespace PongBrain.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.Physical;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class PhysicsSubsystem: Subsystem {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    private float m_WorldBottom;
    private float m_WorldLeft;
    private float m_WorldRight;
    private float m_WorldTop;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public PhysicsSubsystem(float worldLeft,
                            float worldRight,
                            float worldBottom,
                            float worldTop)
    {
        m_WorldBottom = worldBottom;
        m_WorldLeft   = worldLeft;
        m_WorldRight  = worldRight;
        m_WorldTop    = worldTop;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        var worldLeft   = m_WorldLeft;
        var worldRight  = m_WorldRight;
        var worldBottom = m_WorldBottom;
        var worldTop    = m_WorldTop;

        foreach (var entity in Game.Inst.GetEntities<VelocityComponent>()) {
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

            var boundingBox = entity.GetComponent<BoundingBoxComponent>();

            if (boundingBox != null) {
                var e = 1.0f;
                var f = 0.0f;

                if (body != null) {
                    e = body.Restitution;
                    f = body.Friction;
                }

                if (position.X < worldLeft + 0.5f*boundingBox.Width) {
                    position.X = worldLeft + 0.5f*boundingBox.Width;
                    velocity.X *= -e;
                    velocity.Y -= velocity.Y * f;
                }
                else if (position.X > worldRight - 0.5f*boundingBox.Width) {
                    position.X = worldRight - 0.5f*boundingBox.Width;
                    velocity.X *= -e;
                    velocity.Y -= velocity.Y * f;
                }

                if (position.Y < worldBottom + 0.5f*boundingBox.Height) {
                    position.Y = worldBottom + 0.5f*boundingBox.Height;
                    velocity.X -= velocity.X * f;
                    velocity.Y *= -e;
                }
                else if (position.Y > worldTop - 0.5f*boundingBox.Height) {
                    position.Y = worldTop - 0.5f*boundingBox.Height;
                    velocity.X -= velocity.X * f;
                    velocity.Y *= -e;
                }

                foreach (var otherEntity in Game.Inst.GetEntities<BoundingBoxComponent>()) {
                    if (otherEntity.ID <= entity.ID) {
                        continue;
                    }

                    SolveIntersection(entity, otherEntity);
                }
            }
        }
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private void SolveIntersection(Entity e1, Entity e2) {
        var bb1 = e1.GetComponent<BoundingBoxComponent>();
        var bb2 = e2.GetComponent<BoundingBoxComponent>();

        var p1 = e1.GetComponent<PositionComponent>();
        var p2 = e2.GetComponent<PositionComponent>();

        var bb1w = bb1.Width;
        var bb1h = bb1.Height;
        var bb1l = p1.X - 0.5f*bb1w;
        var bb1r = p1.X + 0.5f*bb1w;
        var bb1b = p1.Y - 0.5f*bb1h;
        var bb1t = p1.Y + 0.5f*bb1h;

        var bb2w = bb2.Width;
        var bb2h = bb2.Height;
        var bb2l = p1.X - 0.5f*bb2w;
        var bb2r = p1.X + 0.5f*bb2w;
        var bb2b = p1.Y - 0.5f*bb2h;
        var bb2t = p1.Y + 0.5f*bb2h;
    }
}

}
