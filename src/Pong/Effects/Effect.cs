namespace Pong.Effects {
    
/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using PrimusGE.Components;
using PrimusGE.Core;

using Components;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public abstract class Effect {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private float m_Time;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/
    
    public float Duration { get; }

    public static bool DisableAll { get; set; }

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
        if (DisableAll) {
            return;
        }

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
            Game.Inst.Scene.AddEntity(entity);
        }

    }

    public virtual void End() {
    }

    public virtual void Update(float x) {
    }
}

}
