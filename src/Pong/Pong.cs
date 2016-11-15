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
using Base.Sound;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public static class Pong {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public static Random Rnd { get; } = new Random();

    public static IRenderTarget[] RenderTargets { get; private set; }

    public static IShader PsBloom               { get; private set; }
    public static IShader PsFade                { get; private set; }
    public static IShader PsChromaticAberration { get; private set; }
    public static IShader PsMotionBlur0         { get; private set; }
    public static IShader PsMotionBlur1         { get; private set; }
    public static IShader PsNoise               { get; private set; }

    public static IShader VsMotionBlur          { get; private set; }

    public static ITexture TexSplash { get; private set; }

    public static ITriMesh TmQuad { get; private set; }

    public static ISound SndMusic     { get; private set; }
    public static ISound SndPaddleHit { get; private set; }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static void Preload() {
        var g = Game.Inst.Graphics;
        var s = Game.Inst.Sound;

        RenderTargets = g.CreateRenderTargets(2);

        PsBloom               = LoadPS           ("Bloom.ps.hlsl"              );
        PsFade                = LoadPS<float    >("Fade.ps.hlsl"               );
        PsChromaticAberration = LoadPS<float    >("ChromaticAberration.ps.hlsl");
        PsMotionBlur0         = LoadPS           ("MotionBlur0.ps.hlsl"        );
        PsMotionBlur1         = LoadPS           ("MotionBlur1.ps.hlsl"        );
        PsNoise               = LoadPS<uint     >("Noise.ps.hlsl"              );

        VsMotionBlur          = LoadVS<Matrix4x4>("MotionBlur.vs.hlsl"         );

        TexSplash = g.TextureMgr.Load("Content/Images/Splash.png");

        TmQuad = g.TriMeshMgr.CreateQuad(2.0f, 2.0f);

        SndMusic     = s.Load("Content/Sounds/Music.wav");
        SndPaddleHit = s.Load("Content/Sounds/PaddleHit.wav");

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
