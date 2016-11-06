namespace PongBrain.Pong.Effects {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Base.Components.Graphical;
using Base.Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class BounceEffect: Effect {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    private readonly Entity m_Entity;

    private float m_ScaleX = 1.0f;

    private float m_ScaleY = 1.0f;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public BounceEffect(Entity entity, float duration=1.0f): base(duration) {
        m_Entity = entity;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Begin() {
        base.Begin();

        var sprite = m_Entity.GetComponent<SpriteComponent>();
        if (sprite != null) {
            m_ScaleX = sprite.ScaleX;
            m_ScaleY = sprite.ScaleY;
        }
    }

    public override void End() {
        base.End();

        /*var sprite = m_Entity.GetComponent<SpriteComponent>();
        if (sprite != null) {
            sprite.ScaleX = m_ScaleX;
            sprite.ScaleY = m_ScaleY;
        }*/
    }

    public override void Update(float x) {
        base.Update(x);

        var t = 2.0f * (float)Math.PI * x;

        var scaleX = m_ScaleX * 0.1f*(float)Math.Sin(t*1.0f);
        var scaleY = m_ScaleY * 0.1f*(float)Math.Sin(t*1.0f);

        var sprite = m_Entity.GetComponent<SpriteComponent>();
        if (sprite != null) {
            sprite.ScaleX += scaleX;
            sprite.ScaleY += scaleY;
        }
    }
}

}
