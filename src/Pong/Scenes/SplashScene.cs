namespace PongBrain.Pong.Scenes {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Base.Core;
using Base.Math;
using Base.Subsystems;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class SplashScene: Scene {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private float m_Time;

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Init() {
#if DEBUG
        SetSubsystems(new PerformanceInfoSubsystem());
#endif

        base.Init();

        Pong.Preload();

        var g = Game.Inst.Graphics;

        Pong.ChromaticAberrationPS.SetTextures(Pong.RenderTargets[0]);
        Pong.BloomPS              .SetTextures(Pong.RenderTargets[0]);
        Pong.FadePS               .SetTextures(g.TextureMgr.White, Pong.RenderTargets[0]);
        Pong.NoisePS              .SetTextures(Pong.RenderTargets[0]);
    }

    public override void Draw(float dt) {
        base.Draw(dt);

        var g = Game.Inst.Graphics;

        var fadeVal = Math.Min(0.5f*m_Time*m_Time, 1.0f);

        Pong.ChromaticAberrationPS.SetConstants(0.85f*fadeVal                 );
        Pong.FadePS               .SetConstants(fadeVal*fadeVal               );
        Pong.NoisePS              .SetConstants((uint)Pong.Random.Next(1, 999));

        g.BeginFrame();

        if (m_Time < 3.0f) {
            g.RenderTarget = Pong.RenderTargets[0];
            g.PixelShader  = g.DefaultPixelShader;
            g.PixelShader.SetTextures(Pong.SplashTex);

            g.DrawTriMesh(Pong.UnitQuad, Matrix4x4.Identity());
            g.ApplyPostFX(Pong.RenderTargets[0], Pong.ChromaticAberrationPS);
        }
        else {
            Pong.FadePS.SetConstants(0.86f);
        }

        g.ApplyPostFX(Pong.RenderTargets[0], Pong.FadePS );
        g.ApplyPostFX(Pong.RenderTargets[0], Pong.BloomPS);
        g.ApplyPostFX(g.ScreenRenderTarget , Pong.NoisePS);

        g.EndFrame();

        m_Time += dt;

        if (m_Time > 3.5f) {
            Game.Inst.LeaveScene();
            Game.Inst.EnterScene(new MainScene());
        }
    }

}

}
