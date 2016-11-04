namespace PongBrain.Effects {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Components.Graphical;
using Core;

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

    public BounceEffect(Entity entity) {
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
    
    public override void Update(float x) {
        base.Update(x);

        var t = 2.0f * (float)Math.PI * x;

        var scaleX = m_ScaleX * 0.1f*(float)Math.Sin(t*3.0f);
        var scaleY = m_ScaleY * 0.1f*(float)Math.Sin(t*5.0f);

        var sprite = m_Entity.GetComponent<SpriteComponent>();
        if (sprite != null) {
            sprite.ScaleX += scaleX;
            sprite.ScaleY += scaleY;
        }
    }
}

}
