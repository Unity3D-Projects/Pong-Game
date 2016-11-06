namespace PongBrain.Pong.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Core;

using Components;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class EffectsSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Draw(float dt) {
        base.Draw(dt);

        foreach (var entity in Game.Inst.Scene.GetEntities<EffectComponent>()) {
            var effect = entity.GetComponent<EffectComponent>();

            effect.Update?.Invoke(dt);
        }
    }
}

}
