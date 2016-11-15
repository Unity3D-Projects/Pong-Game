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

    private float m_Time;


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
        
        CreateNet();

        RespawnBall(1.4f);
    }

    public override void Draw(float dt) {
        base.Draw(dt);

        m_Time += dt;

        if (m_Time > 1.0f) {
            m_Time = 1.0f;
        }
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
        m_LeftPaddle = new PaddleEntity(-0.9f, 0.0f);

        var controls = m_LeftPaddle.GetComponent<ControlsComponent>().Controls;

        m_LeftPaddle.AddComponent(new InputComponent {
            KeyMap = {
                { Key.Down, () => {
                    controls["Y"] = -1.0f;
                } },
                { Key.Up, () => {
                    controls["Y"] = 1.0f;
                } },
                { Key.Q, () => {
                    controls["Tilt"] = 1.0f;
                } },
                { Key.E, () => {
                    controls["Tilt"] = -1.0f;
                } }
            },
            ResetControls = () => {
                controls["Tilt"] = 0.0f;
                controls["Y"   ] = 0.0f;
            }
        });

        AddEntity(m_LeftPaddle);
    }

    private void CreateNet() {
        var entities = new List<Entity>();

        var numRectangles = 18;

        var width   = (float)Game.Inst.Window.ClientRectangle.Width;
        var height  = (float)Game.Inst.Window.ClientRectangle.Height;
        var aspect  = height / width;
        var spacing = 2.0f*aspect / numRectangles;

        var y = -aspect + 0.5f*spacing;
        for (var i = 0; i < numRectangles; i++) {
            var rectangle = new RectangleEntity(0.0f, y, 0.01f, 0.03f);
            entities.Add(rectangle);

            y += spacing;
        }

        AddEntities(entities.ToArray());
    }

    private void CreateRightPaddle(Entity ball) {
        m_RightPaddle = new PaddleEntity(0.9f, 0.0f);

        m_RightPaddle.AddComponent(new BrainComponent {
            ThinkFunc = new TrivialAI(m_RightPaddle, ball).Think,
        });

        AddEntity(m_RightPaddle);
    }

    private void RespawnBall(float delay=0.85f) {
        var body = m_Ball.GetComponent<BodyComponent>();

        body.Angle           = 0.0f;
        body.AngularVelocity = 0.0f;
        body.Position        = new Vector2(0.0f, 0.0f);
        body.Velocity        = new Vector2(0.0f, 0.0f);

        var r = Pong.Random;

        Game.Inst.SetTimeout(() => {
            var theta         = 0.9f*(float)Math.PI*((float)r.NextDouble() - 0.45f);
            var speed         = 0.2f + 0.3f*(float)r.NextDouble();
            var direction     = Math.Sign((float)r.NextDouble() - 0.5f);
            var spinDirection = Math.Sign((float)r.NextDouble() - 0.5f);

            var n = 40;
            var v = (1.0f/n)*new Vector2((float)Math.Cos(theta)*speed*direction,
                                         (float)Math.Sin(theta)*speed*2.0f);
            var w = (1.0f/n)*3.0f*(0.15f*(float)r.NextDouble() + 0.7f)*2.0f*(float)Math.PI*spinDirection;

            for (var i = 0; i < n; i++) {
                Game.Inst.SetTimeout(() => {
                    body.AngularVelocity += w;
                    body.Velocity        += v;
                }, 0.5f*i/n);
            }
        }, delay);
    }

    private void SetupDrawing() {
        var g = Game.Inst.Graphics;

        Pong.ChromaticAberrationPS.SetTextures(Pong.RenderTargets[0]);
        Pong.BloomPS              .SetTextures(Pong.RenderTargets[0]);
        Pong.FadePS               .SetTextures(g.TextureMgr.White, Pong.RenderTargets[0]);
        Pong.MotionBlur1PS        .SetTextures(Pong.RenderTargets[0], Pong.RenderTargets[1]);
        Pong.NoisePS              .SetTextures(Pong.RenderTargets[0]);

        var adsMaterial = new AdsMaterial();
        var defDrawFunc = m_GraphicsSubsystem.DrawFunc;

        Action drawMotionBlur = () => {
            g.RenderTarget = Pong.RenderTargets[1];
            g.PixelShader  = Pong.MotionBlur0PS;
            g.VertexShader = Pong.MotionBlurVS;

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

                Pong.MotionBlurVS.SetConstants(motionBlur.PrevTransform);
                g.DrawTriMesh(triMesh.TriMesh, transform);
                motionBlur.PrevTransform = transform;
            }

            g.ApplyPostFX(Pong.RenderTargets[0], Pong.MotionBlur1PS);
        };

        m_GraphicsSubsystem.DrawFunc = () => {
            g.RenderTarget = Pong.RenderTargets[0];
            g.PixelShader  = adsMaterial.Shader;
            g.VertexShader = g.DefaultVertexShader;

            adsMaterial.Ambient = new Color(0.1f, 0.1f, 0.1f, 1.0f);

            defDrawFunc();

            Pong.ChromaticAberrationPS.SetConstants(0.85f                         );
            Pong.NoisePS              .SetConstants((uint)Pong.Random.Next(1, 999));

            g.ApplyPostFX(Pong.RenderTargets[0], Pong.ChromaticAberrationPS);
            g.ApplyPostFX(Pong.RenderTargets[0], Pong.BloomPS              );

            drawMotionBlur();

            if (m_Time < 1.0f) {
                var fade = (float)Math.Pow(m_Time, 3.0);
                Pong.FadePS.SetConstants(fade);
                g.ApplyPostFX(Pong.RenderTargets[0], Pong.FadePS);
            }

            g.ApplyPostFX(g.ScreenRenderTarget, Pong.NoisePS);
        };

    }

    private void SetupEffects() {
        Game.Inst.OnMessage<CollisionMsg>(msg => {
            var o = ((CollisionMsg)msg);

            if (o.EntityA is BallEntity && o.EntityB == null) {
                if (o.Normal == new Vector2(1.0f, 0.0f)) {
                    Score(2);
                }
                else if (o.Normal == new Vector2(-1.0f, 0.0f)) {
                    Score(1);
                }
                else {
                    new CameraShakeEffect(m_GraphicsSubsystem).Create();
                }

                return;
            }

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
        var wl     = -1.2f;
        var wr     = 1.2f;
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
            m_GraphicsSubsystem = new GraphicsSubsystem() { ClearColor=clearColor }

#if DEBUG
            ,new PerformanceInfoSubsystem()
#endif
        );
    }

    private void Score(int player) {
        m_AboutToScore = false;

        if (player == 1) {
            m_LeftScore += 100;
        }
        else if (player == 2) {
            m_RightScore += 100;
        }

        RespawnBall();
    }
}

}
