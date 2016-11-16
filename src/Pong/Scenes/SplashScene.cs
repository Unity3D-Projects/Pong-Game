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

        Pong.SndMusic.Play();

        var g = Game.Inst.Graphics;

        Pong.PsChromaticAberration.SetTextures(Pong.RenderTargets[0]);
        Pong.PsSoft              .SetTextures(Pong.RenderTargets[0]);
        Pong.PsBlend               .SetTextures(g.TextureMgr.White, Pong.RenderTargets[0]);
        Pong.PsNoise              .SetTextures(Pong.RenderTargets[0]);
    }

    public override void Draw(float dt) {
        base.Draw(dt);

        var g = Game.Inst.Graphics;

        var fadeVal = Math.Min(0.5f*m_Time*m_Time, 1.0f);

        Pong.PsChromaticAberration.SetConstants(0.85f*fadeVal   );
        Pong.PsBlend               .SetConstants(fadeVal*fadeVal );
        Pong.PsNoise              .SetConstants(Pong.Rnd(1, 999));

        g.BeginFrame();

        if (m_Time < 3.0f) {
            g.RenderTarget = Pong.RenderTargets[0];
            g.PixelShader  = g.DefaultPixelShader;
            g.PixelShader.SetTextures(Pong.TexSplash);

            g.DrawTriMesh(Pong.TmUnitQuad, Matrix4x4.Identity());
            g.ApplyPostFX(Pong.RenderTargets[0], Pong.PsChromaticAberration);
        }
        else {
            Pong.PsBlend.SetConstants(0.86f);
        }

        g.ApplyPostFX(Pong.RenderTargets[0], Pong.PsBlend );
        g.ApplyPostFX(Pong.RenderTargets[0], Pong.PsSoft);
        g.ApplyPostFX(g.ScreenRenderTarget , Pong.PsNoise);

        g.EndFrame();

        m_Time += dt;

        if (m_Time > 3.5f) {
            Game.Inst.LeaveScene();
            Game.Inst.EnterScene(new PlayingScene());
        }
    }

}

}
