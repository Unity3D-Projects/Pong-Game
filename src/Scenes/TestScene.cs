namespace PongBrain.Scenes {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using AI;
using Components.AI;
using Components.Input;
using Components.Physical;
using Core;
using Entities;
using Input;
using Subsystems;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class TestScene: Scene {
    public TestScene() {
        var w      = (float)Game.Inst.Window.ClientRectangle.Width;
        var h      = (float)Game.Inst.Window.ClientRectangle.Height;
        var aspect = h / w;
        var wl     = -1.0f;
        var wr     = 1.0f;
        var wb     = -aspect;
        var wt     = aspect;

        SetSubsystems(
            new AISubsystem(),
            new InputSubsystem(),

            new ControlsSubsystem(),

            new PhysicsSubsystem(wl, wr, wb, wt),
            new RenderingSubsystem(Game.Inst.Window),
            new FpsCounterSubsystem()
        );
    }

    public override void Init() {
        base.Init();

        var ball = new BallEntity();

        var paddle1 = new PaddleEntity();

        var controls = paddle1.GetComponent<ControlsComponent>().Controls;
        paddle1.AddComponent(new InputComponent {
            KeyMap = {
                { Key.Down, () => {
                    controls["Y"] = -1.0f;
                } },
                { Key.Up, () => {
                    controls["Y"] = 1.0f;
                } }
            },
            ResetControls = () => {
                controls["Y"] = 0.0f;
            }
        });

        var paddle2 = new PaddleEntity();
        var ai = new SimplePaddleAI(ball, paddle2);
        paddle2.AddComponent(new BrainComponent {
            ThinkFunc = ai.Think,
        });
        paddle2.GetComponent<PositionComponent>().X = 0.9f;


        AddEntities(paddle1, paddle2, ball);
    }
}

}
