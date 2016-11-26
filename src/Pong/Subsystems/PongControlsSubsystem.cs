namespace Pong.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using PrimusGE.Core;
using PrimusGE.Components.Input;
using PrimusGE.Components.Physical;
using PrimusGE.Math;
using PrimusGE.Subsystems;

using Components;
using Entities.Mechanical;

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
            var acc    = paddleInfo.Speed*controls.Controls["Y"]*dt;
            var newVel = new Vector2(body.Velocity.X, body.Velocity.Y + acc);
            body.Velocity = newVel;
        }

        if (controls.Controls.ContainsKey("Tilt")) {
            var paddle = (PaddleEntity)entity;
            paddle.Tilt = controls.Controls["Tilt"] * paddleInfo.TiltAngle;
        }
    }
}

}
