namespace PongBrain.Pong.Scenes {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Base.Components.AI;
using Base.Components.Input;
using Base.Components.Physical;
using Base.Core;
using Base.Graphics;
using Base.Input;
using Base.Math;
using Base.Messages;
using Base.Subsystems;

using AI.Trivial;
using Components;
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

    private GraphicsSubsystem m_RenderingSubsystem;

    private Rectangle m_WorldBounds;


    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Init() {
        SetupSubsystems();

        SetupEffects();

        CreateBall();
        CreateLeftPaddle();
        CreateRightPaddle(m_Ball);

        CreateLeftScoreText();
        CreateRightScoreText();
        
        CreateNet();

        Score(0);

        base.Init();

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

        new TrailEffect(m_Ball).Create();
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

        var numRectangles = 18;

        var w      = (float)Game.Inst.Window.ClientRectangle.Width;
        var h      = (float)Game.Inst.Window.ClientRectangle.Height;
        var aspect = h / w;
        var dY = 2.0f*aspect / numRectangles;

        var y = -aspect + 0.5f*dY;
        for (var i = 0; i < numRectangles; i++) {
            var rectangle = new RectangleEntity(0.0f, y, 0.01f, 0.03f);
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
                new CameraShakeEffect(m_RenderingSubsystem).Create();
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

    private void SetupSubsystems() {
        var w      = (float)Game.Inst.Window.ClientRectangle.Width;
        var h      = (float)Game.Inst.Window.ClientRectangle.Height;
        var aspect = h / w;
        var wl     = -1.0f;
        var wr     = 1.0f;
        var wb     = -aspect;
        var wt     = aspect;
        
        var clearColor = new Color(0xff708090);

        m_WorldBounds = new Rectangle(wt, wr, wb, wl);

        SetSubsystems(
            new LifetimeSubsystem(),

            new AISubsystem(),
            new InputSubsystem(),

            new PongControlsSubsystem(),

            new PhysicsSubsystem(m_WorldBounds),

            new EffectsSubsystem(),
            m_RenderingSubsystem = new GraphicsSubsystem() { ClearColor=clearColor },
            new FpsCounterSubsystem()
        );
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

            var theta = 0.6f*(float)Math.PI*(float)random.NextDouble()-0.3f*(float)Math.PI;
            var r = 0.9f+0.6f*(float)random.NextDouble();
            var d = Math.Sign(random.Next(0, 2)-0.5f);
        
            ballVelocity.X = (float)Math.Cos(theta)*r*d;
            ballVelocity.Y = (float)Math.Sin(theta)*r;
        }, 0.65f);
    }

    private void SolveCollisions() {
        // Clean up this method. Oh my lord.
        var lpb = m_LeftPaddle.GetComponent<PositionComponent>().Y - 0.5f*m_LeftPaddle.GetComponent<AxisAlignedBoxComponent>().Height;
        var lpt = m_LeftPaddle.GetComponent<PositionComponent>().Y + 0.5f*m_LeftPaddle.GetComponent<AxisAlignedBoxComponent>().Height;
        var lpl  = m_LeftPaddle.GetComponent<PositionComponent>().X - 0.5f*m_LeftPaddle.GetComponent<AxisAlignedBoxComponent>().Width;
        var lpr  = m_LeftPaddle.GetComponent<PositionComponent>().X + 0.5f*m_LeftPaddle.GetComponent<AxisAlignedBoxComponent>().Width;

        var bb = m_Ball.GetComponent<PositionComponent>().Y -  m_Ball.GetComponent<BallInfoComponent>().Radius;
        var bt = m_Ball.GetComponent<PositionComponent>().Y +  m_Ball.GetComponent<BallInfoComponent>().Radius;
        var bl  = m_Ball.GetComponent<PositionComponent>().X - m_Ball.GetComponent<BallInfoComponent>().Radius;
        var br  = m_Ball.GetComponent<PositionComponent>().X + m_Ball.GetComponent<BallInfoComponent>().Radius;

        var rpb = m_RightPaddle.GetComponent<PositionComponent>().Y - 0.5f*m_RightPaddle.GetComponent<AxisAlignedBoxComponent>().Height;
        var rpt = m_RightPaddle.GetComponent<PositionComponent>().Y + 0.5f*m_RightPaddle.GetComponent<AxisAlignedBoxComponent>().Height;
        var rpl  = m_RightPaddle.GetComponent<PositionComponent>().X - 0.5f*m_RightPaddle.GetComponent<AxisAlignedBoxComponent>().Width;
        var rpr  = m_RightPaddle.GetComponent<PositionComponent>().X + 0.5f*m_RightPaddle.GetComponent<AxisAlignedBoxComponent>().Width;

        if (bt > m_WorldBounds.Top) {
            m_Ball.GetComponent<PositionComponent>().Y = m_WorldBounds.Top - m_Ball.GetComponent<BallInfoComponent>().Radius;
            m_Ball.GetComponent<VelocityComponent>().Y *= -1.0f;
                Game.Inst.PostMessage(new CollisionMessage(m_Ball));
        }
        else if (bb < m_WorldBounds.Bottom) {
            m_Ball.GetComponent<PositionComponent>().Y = m_WorldBounds.Bottom + m_Ball.GetComponent<BallInfoComponent>().Radius;
            m_Ball.GetComponent<VelocityComponent>().Y *= -1.0f;
            Game.Inst.PostMessage(new CollisionMessage(m_Ball));
        }

        if (bl < lpr) {
            if (bb < lpt && bt > lpb && !m_AboutToScore) {
                var v = m_Ball.GetComponent<VelocityComponent>();
                var s = 4.0f * (float)Math.Sqrt(v.X*v.X+v.Y*v.Y);
                m_Ball.GetComponent<PositionComponent>().X = lpr + m_Ball.GetComponent<BallInfoComponent>().Radius;
                m_Ball.GetComponent<VelocityComponent>().X *= -1.05f;
                m_Ball.GetComponent<VelocityComponent>().Y += s * (m_Ball.GetComponent<PositionComponent>().Y - m_LeftPaddle.GetComponent<PositionComponent>().Y);
                Game.Inst.PostMessage(new CollisionMessage(m_Ball, m_LeftPaddle));
            }
            else {
                m_AboutToScore = true;

                if (br < m_WorldBounds.Left-0.1f) {
                    Score(2);
                }
            }
        }
        else if (br > rpl) {
            if (bb < rpt && bt > rpb && !m_AboutToScore) {
                var v = m_Ball.GetComponent<VelocityComponent>();
                var s = 4.0f * (float)Math.Sqrt(v.X*v.X+v.Y*v.Y);
                m_Ball.GetComponent<PositionComponent>().X = rpl - m_Ball.GetComponent<BallInfoComponent>().Radius;
                m_Ball.GetComponent<VelocityComponent>().X *= -1.05f;
                m_Ball.GetComponent<VelocityComponent>().Y += s * (m_Ball.GetComponent<PositionComponent>().Y - m_RightPaddle.GetComponent<PositionComponent>().Y);
                Game.Inst.PostMessage(new CollisionMessage(m_Ball, m_RightPaddle));
            }
            else {
                m_AboutToScore = true;

                if (bl > m_WorldBounds.Right+0.1f) {
                    Score(1);
                }
            }
        }
    }
}

}
