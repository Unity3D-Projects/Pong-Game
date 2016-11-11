namespace PongBrain.Pong.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Core;
using Base.Components.Input;
using Base.Components.Physical;
using Base.Subsystems;

using Components;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class PongControlsSubsystem: ControlsSubsystem {
    /*-------------------------------------
     * PROTECTED METHODS
     *-----------------------------------*/

    protected override void UpdateControls(Entity            entity,
                                           ControlsComponent controls,
                                           float             dt)
    {
        var paddleInfo = entity.GetComponent<PaddleInfoComponent>();
        var velocity   = entity.GetComponent<VelocityComponent>();

        if (controls.Controls.ContainsKey("Y")) {
            velocity.Y += paddleInfo.Speed * controls.Controls["Y"] * dt;
        }
    }
}

}
