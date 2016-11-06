namespace PongBrain.Base.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class LifetimeSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        foreach (var entity in Game.Inst.GetEntities<LifetimeComponent>()) {
            var lifetime = entity.GetComponent<LifetimeComponent>();

            lifetime.Age += dt;

            if (lifetime.Age >= lifetime.Lifetime) {
                lifetime.EndOfLife?.Invoke();
                entity.Destroy();
            }
        }
    }
}

}
