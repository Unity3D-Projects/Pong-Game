namespace PongBrain.Base.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.Input;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public abstract class ControlsSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        foreach (var entity in Game.Inst.GetEntities<ControlsComponent>()) {
            var controls = entity.GetComponent<ControlsComponent>();

            UpdateControls(entity, controls, dt);
        }
    }

    protected abstract void UpdateControls(Entity            entity,
                                           ControlsComponent controls,
                                           float             dt);
}

}
