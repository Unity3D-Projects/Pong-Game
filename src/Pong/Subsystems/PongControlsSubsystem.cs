namespace PongBrain.Pong.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Core;
using Base.Components.Input;
using Base.Components.Physical;
using Base.Math;
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
        var body       = entity.GetComponent<BodyComponent>();
        var paddleInfo = entity.GetComponent<PaddleInfoComponent>();

        if (controls.Controls.ContainsKey("Y")) {
            var newVelocity = new Vector2(body.Velocity.X, body.Velocity.Y + paddleInfo.Speed * controls.Controls["Y"] * dt);
            body.Velocity = newVelocity;
        }
    }
}

}
