namespace PongBrain.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class EffectsSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Draw(float dt) {
        base.Draw(dt);

        foreach (var entity in Game.Inst.GetEntities<EffectComponent>()) {
            var effect = entity.GetComponent<EffectComponent>();

            effect.Update?.Invoke(dt);
        }
    }
}

}
