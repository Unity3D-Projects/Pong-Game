namespace PongBrain.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components;
using Components.Input;
using Components.Physical;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class ControlsSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Update(float dt) {
        base.Update(dt);

        foreach (var entity in Game.Inst.GetEntities<ControlsComponent>()) {
            var controls   = entity.GetComponent<ControlsComponent>();
            var paddleInfo = entity.GetComponent<PaddleInfoComponent>();
            var velocity   = entity.GetComponent<VelocityComponent>();

            if (controls.Controls.ContainsKey("Y")) {
                velocity.Y += paddleInfo.Velocity * controls.Controls["Y"] * dt;
            }
        }
    }
}

}
