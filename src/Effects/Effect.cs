namespace PongBrain.Effects {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Collections.Generic;

using Components;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public abstract class Effect {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private float m_Time;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/
    
    public float Duration { get; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public Effect(float duration=1.0f) {
        Duration = duration;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/
    
    public virtual void Begin() {
    }

    public void Create() {
        var entity = new Entity();

        entity.AddComponents(new EffectComponent {
            Update = dt => {
                var x = 0.0f;

                m_Time += dt;
                if (Duration > 0.0f) {
                    x = m_Time / Duration;
                }

                Update(x);
            } 
        });

        Begin();

        if (Duration > 0.0f) {
            entity.AddComponent(new LifetimeComponent { EndOfLife = End,
                                                        Lifetime  = Duration });
            Game.Inst.AddEntity(entity);
        }

    }

    public virtual void End() {
    }

    public virtual void Update(float x) {
    }
}

}
