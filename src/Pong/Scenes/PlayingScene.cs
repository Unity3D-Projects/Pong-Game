namespace Pong.Scenes {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using PrimusGE.Components.AI;
using PrimusGE.Components.Graphical;
using PrimusGE.Components.Input;
using PrimusGE.Components.Physical;
using PrimusGE.Core;
using PrimusGE.Graphics;
using PrimusGE.Graphics.Animation;
using PrimusGE.Graphics.Shaders;
using PrimusGE.Input;
using PrimusGE.Math;
using PrimusGE.Messages;
using PrimusGE.Subsystems;

using AI.Trivial;
using Components;
using Effects;
using Entities.Graphical;
using Entities.Mechanical;
using Subsystems;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class PlayingScene: Scene {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private List<Entity> m_Balls = new List<Entity>();

    private Entity m_LeftPaddle;
    private Entity m_RightPaddle;

    private Tweening m_FadeTweening;

    private GraphicsSubsystem m_GraphicsSubsystem;

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
        CreateRightPaddle(m_Balls[0]);
        
        CreateNet();

        Respawn(1.4f);
        
        m_FadeTweening = new Tweening((a, b, x) => (float)Math.Pow(x, 3.0));
    }

    public override void Draw(float dt) {
        base.Draw(dt);

        if (m_FadeTweening != null) {
            m_FadeTweening.X += dt;
        }
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    private void CreateBall() {
        var ball = new BallEntity();

        new BallTrailEffect(ball).Create();

        m_Balls.Add(ball);
        AddEntity(ball);
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

    private void FadeOutIn() {
        m_FadeTweening = new Tweening((a, b, x) => 1.0f - (float)Math.Pow(Math.Sin(Math.PI * x), 4.0));
    }

    private void Respawn(float delay=0.85f) {
        var leftPaddle  = m_LeftPaddle .GetComponent<BodyComponent>();
        var rightPaddle = m_RightPaddle.GetComponent<BodyComponent>();

        /*leftPaddle.Angle           = 0.0f;
        leftPaddle.AngularVelocity = 0.0f;
        leftPaddle.Position        = new Vector2(-0.9f, 0.0f);
        leftPaddle.Velocity        = new Vector2(0.0f, 0.0f);

        rightPaddle.Angle           = 0.0f;
        rightPaddle.AngularVelocity = 0.0f;
        rightPaddle.Position        = new Vector2(0.9f, 0.0f);
        rightPaddle.Velocity        = new Vector2(0.0f, 0.0f);*/

        foreach (var entity in m_Balls) {
            var ball = entity.GetComponent<BodyComponent>();

            ball.IsStatic        = false;
            ball.Angle           = 0.0f;
            ball.AngularVelocity = 0.0f;
            ball.Position        = new Vector2(0.0f, 0.0f);
            ball.Velocity        = new Vector2(0.0f, 0.0f);

            Game.Inst.SetTimeout(() => {
                var theta         = 0.9f*(float)Math.PI*(Pong.Rnd() - 0.45f);
                var speed         = 0.2f + 0.3f*Pong.Rnd();
                var direction     = Math.Sign(Pong.Rnd() - 0.5f);
                var spinDirection = Math.Sign(Pong.Rnd() - 0.5f);

                var n = 40;
                var v = (1.0f/n)*new Vector2((float)Math.Cos(theta)*speed*direction,
                                             (float)Math.Sin(theta)*speed*2.0f);
                var w = (1.0f/n)*3.0f*(0.15f*Pong.Rnd() + 0.7f)*2.0f*(float)Math.PI*spinDirection;

                for (var i = 0; i < n; i++) {
                    Game.Inst.SetTimeout(() => {
                        ball.AngularVelocity += w;
                        ball.Velocity        += v;
                    }, 0.5f*i/n);
                }
            }, delay);
        }
    }

    private void SetupDrawing() {
        var g = Game.Inst.Graphics;

        var defMaterial = new AdsMaterial { 
            Ambient = new Vector4(0.1f, 0.1f, 0.1f, 1.0f)
        };

        Pong.PsAfterimage         .SetTextures(Pong.RenderTargets[0], Pong.RenderTargets[2]);
        Pong.PsBlend              .SetTextures(g.TextureMgr.White   , Pong.RenderTargets[0]);
        Pong.PsChromaticAberration.SetTextures(Pong.RenderTargets[0]                       );
        Pong.PsMotionBlur1        .SetTextures(Pong.RenderTargets[0], Pong.RenderTargets[1]);
        Pong.PsNoise              .SetTextures(Pong.RenderTargets[2]                       );
        Pong.PsSoft               .SetTextures(Pong.RenderTargets[0]                       );

        Pong.PsAdsMaterial        .SetConstants(defMaterial     );
        Pong.PsAfterimage         .SetConstants(Pong.AfterimageConstants(0.45f, 1));
        Pong.PsChromaticAberration.SetConstants(0.85f           );

        Pong.RenderTargets[2].Clear(new Color(1.0f, 1.0f, 1.0f, 1.0f));

        var defDrawFunc = m_GraphicsSubsystem.DrawFunc;

        Action drawMotionBlur = () => {
            g.RenderTarget = Pong.RenderTargets[1];
            g.PixelShader  = Pong.PsMotionBlur0;
            g.VertexShader = Pong.VsMotionBlur;

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

                Pong.VsMotionBlur.SetConstants(motionBlur.PrevTransform);
                g.DrawTriMesh(triMesh.TriMesh, transform);
                motionBlur.PrevTransform = transform;
            }

            g.ApplyPostFX(Pong.RenderTargets[0], Pong.PsMotionBlur1);
        };

        m_GraphicsSubsystem.DrawFunc = () => {
            Pong.PsNoise.SetConstants(Pong.Rnd(1, 999));

            g.RenderTarget = Pong.RenderTargets[0];
            g.PixelShader  = Pong.PsAdsMaterial;
            g.VertexShader = g.DefaultVertexShader;

            defDrawFunc();

            g.ApplyPostFX(Pong.RenderTargets[0], Pong.PsChromaticAberration);
            g.ApplyPostFX(Pong.RenderTargets[0], Pong.PsSoft              );

            drawMotionBlur();

            if (m_FadeTweening != null) {
                Pong.PsBlend.SetConstants(m_FadeTweening.Calc());
                g.ApplyPostFX(Pong.RenderTargets[0], Pong.PsBlend);

                if (m_FadeTweening.IsDone) {
                    m_FadeTweening = null;
                }
            }

            g.ApplyPostFX(Pong.RenderTargets[2], Pong.PsAfterimage);
            g.ApplyPostFX(g.ScreenRenderTarget , Pong.PsNoise);
        };

    }

    private void SetupEffects() {
        Game.Inst.OnMessage<CollisionMsg>(msg => {
            var o = ((CollisionMsg)msg);

            if (o.EntityA is BallEntity && o.EntityB == null) {
                if (o.Normal == new Vector2(1.0f, 0.0f)) {
                    o.EntityA.GetComponent<BodyComponent>().IsStatic = true;

                    Score(2);
                }
                else if (o.Normal == new Vector2(-1.0f, 0.0f)) {
                    o.EntityA.GetComponent<BodyComponent>().IsStatic = true;

                    Score(1);
                }
                else {
                    new CameraShakeEffect(m_GraphicsSubsystem).Create();
                    Pong.SndHitWall.Play(Pong.RndPitch());
                }

                return;
            }

            if (o.EntityA is BallEntity || o.EntityB != null) {
                new CameraShakeEffect(m_GraphicsSubsystem).Create();
                new BounceEffect(o.EntityA, 0.2f).Create();

                if (o.EntityB != null) {
                    new BounceEffect(o.EntityB, 0.2f).Create();
                    new ExplosionEffect(o.Contact).Create();

                    Pong.SndHitPaddle.Play(Pong.RndPitch());
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

        var clearColor  = new Color(0xffffffff);// new Color(0xff708090);
        var worldBounds = new Rectangle(wt, wr, wb, wl);

        SetSubsystems(
            new LifetimeSubsystem(),

            new AISubsystem(),
            new InputSubsystem(),

            new PongControlsSubsystem(),

            new PhysicsSubsystem(worldBounds),

            new EffectsSubsystem(),
            m_GraphicsSubsystem = new GraphicsSubsystem() { ClearColor=clearColor }

#if DEBUG
            ,new PerformanceInfoSubsystem()
#endif
        );
    }

    private void Score(int player) {

        FadeOutIn();

        Game.Inst.SetTimeout(() => {
            Respawn();
        }, 0.5f);
    }
}

}
