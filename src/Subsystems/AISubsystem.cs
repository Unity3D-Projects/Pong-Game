namespace PongBrain.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.AI;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class AISubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        foreach (var entity in Game.Inst.GetEntities<BrainComponent>()) {
            var brain = entity.GetComponent<BrainComponent>();

            if (brain == null) {
                continue;
            }

            var t = brain.ThinkTimer + dt;

            var invThinkRate = 1.0f / brain.ThinkRate;
            while (t >= invThinkRate) {
                brain.ThinkFunc?.Invoke(dt);
                t -= invThinkRate;
            }

            brain.ThinkTimer = t;
        }
    }
}

}
