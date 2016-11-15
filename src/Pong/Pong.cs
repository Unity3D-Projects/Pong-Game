namespace PongBrain.Pong {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.IO;

using Base.Core;
using Base.Graphics;
using Base.Graphics.Shaders;
using Base.Graphics.Textures;
using Base.Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public static class Pong {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public static Random Random { get; } = new Random();

    public static IRenderTarget[] RenderTargets { get; private set; }

    public static IShader BloomPS               { get; private set; }
    public static IShader FadePS                { get; private set; }
    public static IShader ChromaticAberrationPS { get; private set; }
    public static IShader MotionBlur0PS         { get; private set; }
    public static IShader MotionBlur1PS         { get; private set; }
    public static IShader MotionBlurVS          { get; private set; }
    public static IShader NoisePS               { get; private set; }

    public static ITexture SplashTex { get; private set; }

    public static ITriMesh UnitQuad { get; private set; }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static void Preload() {
        var g = Game.Inst.Graphics;

        RenderTargets = g.CreateRenderTargets(2);

        BloomPS               = LoadPS           ("Bloom.ps.hlsl"              );
        FadePS                = LoadPS<float    >("Fade.ps.hlsl"               );
        ChromaticAberrationPS = LoadPS<float    >("ChromaticAberration.ps.hlsl");
        MotionBlur0PS         = LoadPS           ("MotionBlur0.ps.hlsl"        );
        MotionBlur1PS         = LoadPS           ("MotionBlur1.ps.hlsl"        );
        MotionBlurVS          = LoadVS<Matrix4x4>("MotionBlur.vs.hlsl"         );
        NoisePS               = LoadPS<uint     >("Noise.ps.hlsl"              );

        SplashTex = g.TextureMgr.Load("Content/Images/Splash.png");

        UnitQuad = g.TriMeshMgr.CreateQuad(2.0f, 2.0f);

        Game.Inst.Window.Show();

    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    private static IShader LoadPS(string path, Type constantsType=null) {
        path = Path.Combine("Content/Shaders/", path);
        return Game.Inst.Graphics.ShaderMgr.LoadPS(path, constantsType);
    }

    private static IShader LoadPS<T>(string path) where T: struct {
        return LoadPS(path, typeof (T));
    }

    private static IShader LoadVS(string path, Type constantsType=null) {
        path = Path.Combine("Content/Shaders/", path);
        return Game.Inst.Graphics.ShaderMgr.LoadVS(path, constantsType);
    }

    private static IShader LoadVS<T>(string path) where T: struct {
        return LoadVS(path, typeof (T));
    }
}

}
