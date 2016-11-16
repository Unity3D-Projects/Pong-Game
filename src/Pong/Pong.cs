namespace Pong {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.IO;
using System.Runtime.InteropServices;

using Base.Core;
using Base.Graphics;
using Base.Graphics.Shaders;
using Base.Graphics.Textures;
using Base.Math;
using Base.Sound;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

[StructLayout(LayoutKind.Sequential)]
public struct Afterimage {
    public float A;
    public int K;
}

public static class Pong {
    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/

    private static readonly Random s_Rnd = new Random();

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public static IRenderTarget[] RenderTargets { get; private set; }

    public static IShader PsAdsMaterial         { get; private set; }
    public static IShader PsAfterimage          { get; private set; }
    public static IShader PsAlphaBlend          { get; private set; }
    public static IShader PsBlend               { get; private set; }
    public static IShader PsChromaticAberration { get; private set; }
    public static IShader PsMotionBlur0         { get; private set; }
    public static IShader PsMotionBlur1         { get; private set; }
    public static IShader PsNoise               { get; private set; }
    public static IShader PsSoft                { get; private set; }

    public static IShader VsMotionBlur { get; private set; }

    public static ITexture TexSplash { get; private set; }

    public static ITriMesh TmUnitQuad { get; private set; }

    public static ISound SndHitPaddle { get; private set; }
    public static ISound SndHitWall   { get; private set; }
    public static ISound SndMusic     { get; private set; }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public static Afterimage AfterimageConstants(float a, int k) {
        return new Afterimage {
            A = a,
            K = k
        };
    }

    public static void Preload() {
        var g = Game.Inst.Graphics;
        var s = Game.Inst.Sound;

        RenderTargets = g.CreateRenderTargets(3);

        PsAdsMaterial         = LoadPS<AdsMaterial>("AdsMaterial"        );
        PsAfterimage          = LoadPS<Afterimage >("Afterimage"         );
        PsBlend               = LoadPS<float      >("Blend"              );
        PsChromaticAberration = LoadPS<float      >("ChromaticAberration");
        PsMotionBlur0         = LoadPS             ("MotionBlur0"        );
        PsMotionBlur1         = LoadPS             ("MotionBlur1"        );
        PsNoise               = LoadPS<uint       >("Noise"              );
        PsSoft                = LoadPS             ("Soft"               );

        VsMotionBlur = LoadVS<Matrix4x4>("MotionBlur");

        TexSplash = g.TextureMgr.Load("Content/Images/Splash.png");

        TmUnitQuad = g.TriMeshMgr.CreateQuad(2.0f, 2.0f);

        SndMusic     = s.Load("Content/Sounds/Music.wav"    );
        SndHitPaddle = s.Load("Content/Sounds/HitPaddle.wav");
        SndHitWall   = s.Load("Content/Sounds/HitWall.wav"  );

        Game.Inst.Window.Show();
    }

    public static float Rnd() {
        return (float)s_Rnd.NextDouble();
    }

    public static float Rnd(float min, float max) {
        return min + (max - min)*Rnd();
    }

    public static int Rnd(int min, int max) {
        return s_Rnd.Next(min, max+1);
    }

    public static float RndPitch(float min=-0.5f, float max=0.5f) {
        return Rnd(min, max);
    }

    /*-------------------------------------
     * NON-PUBLIC METHODS
     *-----------------------------------*/

    private static IShader LoadPS(string path, Type constantsType=null) {
        path = Path.Combine("Content/Shaders/", path + ".ps.hlsl");
        return Game.Inst.Graphics.ShaderMgr.LoadPS(path, constantsType);
    }

    private static IShader LoadPS<T>(string path) where T: struct {
        return LoadPS(path, typeof (T));
    }

    private static IShader LoadVS(string path, Type constantsType=null) {
        path = Path.Combine("Content/Shaders/", path + ".vs.hlsl");
        return Game.Inst.Graphics.ShaderMgr.LoadVS(path, constantsType);
    }

    private static IShader LoadVS<T>(string path) where T: struct {
        return LoadVS(path, typeof (T));
    }
}

}
