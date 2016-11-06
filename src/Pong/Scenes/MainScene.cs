namespace PongBrain.Pong.Scenes {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;
using System.Drawing;

using Base.Components.AI;
using Base.Components.Input;
using Base.Components.Physical;
using Base.Core;
using Base.Input;
using Base.Messages;
using Base.Subsystems;

using AI.Trivial;
using Effects;
using Entities.Graphical;
using Entities.Mechanical;
using Subsystems;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class MainScene: Scene {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private bool m_AboutToScore;

    private Entity m_Ball;
    private Entity m_LeftPaddle;
    private int m_LeftScore;
    private Entity m_LeftScoreText;
    private Entity m_RightPaddle;
    private int m_RightScore;
    private Entity m_RightScoreText;


    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public MainScene() {
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

            new PongControlsSubsystem(),

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

        CreateLeftScoreText();
        CreateRightScoreText();
        
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

    private void CreateLeftScoreText() {
        m_LeftScoreText = new ScoreEntity(-0.96f, 0.68f, () => string.Format("P1 SCORE: {0}", m_LeftScore));

        AddEntity(m_LeftScoreText);
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

        m_RightPaddle.AddComponent(new BrainComponent {
            ThinkFunc = new TrivialAI(m_RightPaddle, ball).Think,
        });

        m_RightPaddle.GetComponent<PositionComponent>().X = 0.9f;

        AddEntity(m_RightPaddle);
    }

    private void CreateRightScoreText() {
        m_RightScoreText = new ScoreEntity(0.24f, 0.68f, () => string.Format("P2 SCORE: {0}", m_RightScore));

        AddEntity(m_RightScoreText);
    }

    private void SetupEffects() {
        Game.Inst.OnMessage<CollisionMessage>(msg => {
            var o = ((CollisionMessage)msg);

            if (o.Entity1 == m_Ball || o.Entity2 != null) {
                new BounceEffect(o.Entity1, 0.2f).Create();

                if (o.Entity2 != null) {
                    new BounceEffect(o.Entity2, 0.2f).Create();

                    var x = o.Entity1.GetComponent<PositionComponent>().X;
                    var y = o.Entity1.GetComponent<PositionComponent>().Y;

                    new ExplosionEffect(x, y).Create();
                }
            }
        });
    }

    private void Score(int player) {
        var ballPosition = m_Ball.GetComponent<PositionComponent>();
        var ballVelocity = m_Ball.GetComponent<VelocityComponent>();

        ballPosition.X = 0.0f;
        ballPosition.Y = 0.0f;

        ballVelocity.X = 0.0f;
        ballVelocity.Y = 0.0f;

        m_AboutToScore = false;

        if (player == 1) {
            m_LeftScore += 100;
        }
        else if (player == 2) {
            m_RightScore += 100;
        }

        Game.Inst.SetTimeout(() => {
            var random = new Random();

            var theta = 2.0f*(float)Math.PI*(float)random.NextDouble();
            var r = 0.9f+0.6f*(float)random.NextDouble();
        
            ballVelocity.X = (float)Math.Cos(theta)*r;
            ballVelocity.Y = (float)Math.Sin(theta)*r;
        }, 0.5f);
    }

    private void SolveCollisions() {
        // Clean up this method. Oh my lord.
        var lpb = m_LeftPaddle.GetComponent<PositionComponent>().Y - 0.5f*m_LeftPaddle.GetComponent<AxisAlignedBoxComponent>().Height;
        var lpt = m_LeftPaddle.GetComponent<PositionComponent>().Y + 0.5f*m_LeftPaddle.GetComponent<AxisAlignedBoxComponent>().Height;
        var lpl  = m_LeftPaddle.GetComponent<PositionComponent>().X - 0.5f*m_LeftPaddle.GetComponent<AxisAlignedBoxComponent>().Width;
        var lpr  = m_LeftPaddle.GetComponent<PositionComponent>().X + 0.5f*m_LeftPaddle.GetComponent<AxisAlignedBoxComponent>().Width;

        var bb = m_Ball.GetComponent<PositionComponent>().Y - 0.5f * m_Ball.GetComponent<AxisAlignedBoxComponent>().Height;
        var bt = m_Ball.GetComponent<PositionComponent>().Y + 0.5f*m_Ball.GetComponent<AxisAlignedBoxComponent>().Height;
        var bl  = m_Ball.GetComponent<PositionComponent>().X - 0.5f*m_Ball.GetComponent<AxisAlignedBoxComponent>().Width;
        var br  = m_Ball.GetComponent<PositionComponent>().X + 0.5f*m_Ball.GetComponent<AxisAlignedBoxComponent>().Width;

        var rpb = m_RightPaddle.GetComponent<PositionComponent>().Y - 0.5f*m_RightPaddle.GetComponent<AxisAlignedBoxComponent>().Height;
        var rpt = m_RightPaddle.GetComponent<PositionComponent>().Y + 0.5f*m_RightPaddle.GetComponent<AxisAlignedBoxComponent>().Height;
        var rpl  = m_RightPaddle.GetComponent<PositionComponent>().X - 0.5f*m_RightPaddle.GetComponent<AxisAlignedBoxComponent>().Width;
        var rpr  = m_RightPaddle.GetComponent<PositionComponent>().X + 0.5f*m_RightPaddle.GetComponent<AxisAlignedBoxComponent>().Width;

        if (bl < lpr) {
            if (bb < lpt && bt > lpb && !m_AboutToScore) {
                m_Ball.GetComponent<PositionComponent>().X = lpr + 0.5f*m_Ball.GetComponent<AxisAlignedBoxComponent>().Width;
                m_Ball.GetComponent<VelocityComponent>().X *= -1.0f;
                Game.Inst.PostMessage(new CollisionMessage(m_Ball, m_LeftPaddle));
            }
            else {
                m_AboutToScore = true;

                if (br < lpl) {
                    Score(2);
                }
            }
        }
        else if (br > rpl) {
            if (bb < rpt && bt > rpb && !m_AboutToScore) {
                m_Ball.GetComponent<PositionComponent>().X = rpl - 0.5f*m_Ball.GetComponent<AxisAlignedBoxComponent>().Width;
                m_Ball.GetComponent<VelocityComponent>().X *= -1.0f;
                Game.Inst.PostMessage(new CollisionMessage(m_Ball, m_RightPaddle));
            }
            else {
                m_AboutToScore = true;

                if (bl > rpr) {
                    Score(1);
                }
            }
        }
    }
}

}
