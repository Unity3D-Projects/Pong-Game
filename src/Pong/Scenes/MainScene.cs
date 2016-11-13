namespace PongBrain.Pong.Scenes {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Base.Components.AI;
using Base.Components.Graphical;
using Base.Components.Input;
using Base.Components.Physical;
using Base.Core;
using Base.Graphics;
using Base.Graphics.Shaders;
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
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private bool m_AboutToScore;

    private Entity m_Ball;
    private Entity m_LeftPaddle;
    private int m_LeftScore;
    private Entity m_LeftScoreText;
    private Entity m_RightPaddle;
    private int m_RightScore;
    private Entity m_RightScoreText;

    private GraphicsSubsystem m_GraphicsSubsystem;

    private Rectangle m_WorldBounds;


    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Init() {
        SetupSubsystems();

        base.Init();

        SetupDrawing();

        SetupEffects();

        CreateBall();
        CreateLeftPaddle();
        CreateRightPaddle(m_Ball);

        CreateLeftScoreText();
        CreateRightScoreText();
        
        CreateNet();

        Score(0);


    }

    public override void Update(float dt) {
        base.Update(dt);

        SolveCollisions();
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
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

        m_LeftPaddle.GetComponent<BodyComponent>().Position = new Vector2(-0.9f, 0.0f);

        AddEntity(m_LeftPaddle);
    }

    private void CreateLeftScoreText() {
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

            m_RightPaddle.GetComponent<BodyComponent>().Position = new Vector2(0.9f, 0.0f);

        AddEntity(m_RightPaddle);
    }

    private void CreateRightScoreText() {
        //AddEntity(m_RightScoreText);
    }

    private void SetupDrawing() {


        var g = Game.Inst.Graphics;


        var random = new Random();
        var renderTargets = g.CreateRenderTargets(3);

        var adsMaterial = new AdsMaterial();
        var defDrawFunc = m_GraphicsSubsystem.DrawFunc;
        Action draw = () => {
            g.RenderTarget = renderTargets[0];
            g.PixelShader  = adsMaterial.Shader;
            g.VertexShader = null;

            adsMaterial.Ambient = new Color(0.1f, 0.1f, 0.1f, 1.0f);

            defDrawFunc();
        };

        var motionBlurPS0 = g.ShaderMgr.LoadPS("src/Shaders/HLSL/MotionBlur0.ps.hlsl");
        var motionBlurPS1 = g.ShaderMgr.LoadPS("src/Shaders/HLSL/MotionBlur1.ps.hlsl");
        var motionBlurVS  = g.ShaderMgr.LoadVS<Matrix4x4>("src/Shaders/HLSL/MotionBlur.vs.hlsl");
        motionBlurPS1.SetTextures(renderTargets[0], renderTargets[1]);
        Action drawMotionBlur = () => {
            g.RenderTarget = renderTargets[1];
            g.PixelShader  = motionBlurPS0;
            g.VertexShader = motionBlurVS;

            g.RenderTarget.Clear(Color.Black);

            var entities = Game.Inst.Scene.GetEntities<MotionBlurComponent>();
            foreach (var entity in entities) {
                var motionBlur = entity.GetComponent<MotionBlurComponent>();
                var triMesh    = entity.GetComponent<TriMeshComponent>();
                var transform  = m_GraphicsSubsystem.CalcTransform(entity);

                transform.Transpose();

                if (motionBlur.PrevTransform.M11 == 0.0f) {
                    motionBlur.PrevTransform = transform;
                }

                motionBlurVS.SetConstants(motionBlur.PrevTransform);

                g.DrawTriMesh(triMesh.TriMesh, transform);

                motionBlur.PrevTransform = transform;
            }

            g.ApplyPostFX(renderTargets[2], motionBlurPS1);
        };

        var chromaticAberration = g.ShaderMgr.LoadPS<float>("src/Shaders/HLSL/ChromaticAberration.ps.hlsl");
        chromaticAberration.SetTextures(renderTargets[2]);
        chromaticAberration.SetConstants(0.85f);
        Action drawChromaticAberration = () => {
            g.ApplyPostFX(renderTargets[0], chromaticAberration);
        };

        var bloom = g.ShaderMgr.LoadPS("src/Shaders/HLSL/Bloom.ps.hlsl");
        bloom.SetTextures(renderTargets[0]);
        Action drawBloom = () => {
            g.ApplyPostFX(renderTargets[1], chromaticAberration);
        };

        var noise = g.ShaderMgr.LoadPS<uint>("src/Shaders/HLSL/Noise.ps.hlsl");
        noise.SetTextures(renderTargets[1]);
        Action drawNoise = () => {
            g.RenderTarget = null;
            g.ApplyPostFX(null, noise);
        };

        m_GraphicsSubsystem.DrawFunc = () => {
            noise.SetConstants((uint)random.Next(1, 999));

            draw();
            drawMotionBlur();
            drawBloom();
            drawNoise();
        };

    }

    private void SetupEffects() {
        Game.Inst.OnMessage<CollisionMsg>(msg => {
            var o = ((CollisionMsg)msg);

            if (o.EntityA == m_Ball || o.EntityB != null) {
                new CameraShakeEffect(m_GraphicsSubsystem).Create();
                new BounceEffect(o.EntityA, 0.2f).Create();

                if (o.EntityB != null) {
                    new BounceEffect(o.EntityB, 0.2f).Create();

                    new ExplosionEffect(o.Contact).Create();
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

        var clearColor = new Color(0xffffffff);// new Color(0xff708090);

        m_WorldBounds = new Rectangle(wt, wr, wb, wl);

        SetSubsystems(
            new LifetimeSubsystem(),

            new AISubsystem(),
            new InputSubsystem(),

            new PongControlsSubsystem(),

            new PhysicsSubsystem(m_WorldBounds),

            new EffectsSubsystem(),
            m_GraphicsSubsystem = new GraphicsSubsystem() { ClearColor=clearColor },
            new PerformanceInfoSubsystem()
        );
    }

    private void Score(int player) {
        var ode = m_Ball.GetComponent<BodyComponent>();
        var ballPosition = m_Ball.GetComponent<BodyComponent>().Position;
        var ballVelocity = m_Ball.GetComponent<BodyComponent>().Velocity;

        ode.Position = new Vector2(0.0f, 0.0f);
        ode.Velocity = new Vector2(0.0f, 0.0f);

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
        
            ode.Velocity = new Vector2((float)Math.Cos(theta)*r*d, (float)Math.Sin(theta)*r);
        }, 0.65f);
    }

    private void SolveCollisions() {
        // Clean up this method. Oh my lord.
        /*var lpb = m_LeftPaddle.GetComponent<PositionComponent>().Y - 0.5f*m_LeftPaddle.GetComponent<AxisAlignedBoxComponent>().Height;
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
        }*/
    }
}

}
