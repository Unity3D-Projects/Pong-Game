namespace PongBrain.Scenes {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Collections.Generic;
using System.Drawing;

using AI.Trivial;
using Components.AI;
using Components.Input;
using Components.Physical;
using Core;
using Effects;
using Entities.Graphical;
using Entities.Mechanical;
using Input;
using Messaging.Messages;
using Subsystems;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class GameScene: Scene {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private Entity m_Ball;
    private Entity m_LeftPaddle;
    private Entity m_RightPaddle;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public GameScene() {
        var clearColor = Color.SlateGray;

        var w      = (float)Game.Inst.Window.ClientRectangle.Width;
        var h      = (float)Game.Inst.Window.ClientRectangle.Height;
        var aspect = h / w;
        var wl     = -1.0f;
        var wr     = 1.0f;
        var wb     = -aspect;
        var wt     = aspect;

        SetSubsystems(
            new LifetimeSubsystem(),

            new AISubsystem(),
            new InputSubsystem(),

            new ControlsSubsystem(),

            new PhysicsSubsystem(wl, wr, wb, wt),

            new EffectsSubsystem(),
            new RenderingSubsystem(Game.Inst.Window) { ClearColor=clearColor },
            new FpsCounterSubsystem()
        );
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Init() {
        base.Init();

        SetupEffects();

        CreateBall();
        CreateLeftPaddle();
        CreateRightPaddle(m_Ball);

        CreateNet();
    }

    public override void Update(float dt) {
        base.Update(dt);

        SolveCollisions();
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private void CreateBall() {
        m_Ball = new BallEntity();

        AddEntity(m_Ball);
    }

    private void CreateLeftPaddle() {
        m_LeftPaddle = new PaddleEntity();

        var controls = m_LeftPaddle.GetComponent<ControlsComponent>().Controls;

        m_LeftPaddle.AddComponent(new InputComponent {
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

        m_LeftPaddle.GetComponent<PositionComponent>().X = -0.9f;

        AddEntity(m_LeftPaddle);
    }

    private void CreateNet() {
        var entities = new List<Entity>();

        var numRectangles = 8;
        var margin        = 0.1f;

        var w      = (float)Game.Inst.Window.ClientRectangle.Width;
        var h      = (float)Game.Inst.Window.ClientRectangle.Height;
        var aspect = h / w;
        var dY = 2.0f*aspect / numRectangles;

        var y = -aspect + 0.5f*dY;
        for (var i = 0; i < numRectangles; i++) {
            var rectangle = new RectangleEntity(0.0f, y, 0.04f, 0.04f);
            entities.Add(rectangle);

            y += dY;
        }

        AddEntities(entities.ToArray());
    }

    private void CreateRightPaddle(Entity ball) {
        m_RightPaddle = new PaddleEntity();

        var ai = new TrivialPaddleAI(m_RightPaddle, ball);

        m_RightPaddle.AddComponent(new BrainComponent {
            ThinkFunc = ai.Think,
        });

        m_RightPaddle.GetComponent<PositionComponent>().X = 0.9f;

        AddEntity(m_RightPaddle);
    }

    private void SetupEffects() {
        return;
        Game.Inst.OnMessage<CollisionMessage>(msg => {
            var entity = ((CollisionMessage)msg).Entity1;
            new BounceEffect(entity).Create();
        });
    }

    private void Score(int player) {
        var ballPosition = m_Ball.GetComponent<PositionComponent>();

        ballPosition.X = 0.0f;
        ballPosition.Y = 0.0f;
    }

    private void SolveCollisions() {
        var lpb = m_LeftPaddle.GetComponent<PositionComponent>().Y - 0.5f*m_LeftPaddle.GetComponent<BoundingBoxComponent>().Height;
        var lpt = m_LeftPaddle.GetComponent<PositionComponent>().Y + 0.5f*m_LeftPaddle.GetComponent<BoundingBoxComponent>().Height;
        var lpr  = m_LeftPaddle.GetComponent<PositionComponent>().X + 0.5f*m_LeftPaddle.GetComponent<BoundingBoxComponent>().Width;

        var bb = m_Ball.GetComponent<PositionComponent>().Y - 0.5f * m_Ball.GetComponent<BoundingBoxComponent>().Height;
        var bt = m_Ball.GetComponent<PositionComponent>().Y + 0.5f*m_Ball.GetComponent<BoundingBoxComponent>().Height;
        var bl  = m_Ball.GetComponent<PositionComponent>().X - 0.5f*m_Ball.GetComponent<BoundingBoxComponent>().Width;
        var br  = m_Ball.GetComponent<PositionComponent>().X + 0.5f*m_Ball.GetComponent<BoundingBoxComponent>().Width;

        var rpb = m_RightPaddle.GetComponent<PositionComponent>().Y - 0.5f*m_RightPaddle.GetComponent<BoundingBoxComponent>().Height;
        var rpt = m_RightPaddle.GetComponent<PositionComponent>().Y + 0.5f*m_RightPaddle.GetComponent<BoundingBoxComponent>().Height;
        var rpl  = m_RightPaddle.GetComponent<PositionComponent>().X - 0.5f*m_RightPaddle.GetComponent<BoundingBoxComponent>().Width;

        if (bl < lpr) {
            if (bb < lpt && bt > lpb) {
                m_Ball.GetComponent<PositionComponent>().X = lpr + 0.5f*m_Ball.GetComponent<BoundingBoxComponent>().Width;
                m_Ball.GetComponent<VelocityComponent>().X *= -1.0f;
                Game.Inst.PostMessage(new CollisionMessage(m_LeftPaddle, m_Ball));
            }
            else {
                Score(1);
            }
        }
        else if (br > rpl) {
            if (bb < rpt && bt > rpb) {
                m_Ball.GetComponent<PositionComponent>().X = rpl - 0.5f*m_Ball.GetComponent<BoundingBoxComponent>().Width;
                m_Ball.GetComponent<VelocityComponent>().X *= -1.0f;
                Game.Inst.PostMessage(new CollisionMessage(m_RightPaddle, m_Ball));
            }
            else {
                Score(2);
            }
        }
    }
}

}
