namespace PongBrain.Base.Graphics.Shaders {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Runtime.InteropServices;

using Core;
using Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class AdsMaterial {
    /*-------------------------------------
     * NESTED CLASSES
     *-----------------------------------*/

    [StructLayout(LayoutKind.Sequential)]
    private struct AdsData {
        public Vector4 Ambient;
        public Vector4 Diffuse;
        public Vector4 Specular;
        public bool UseTexture;
    }

    /*-------------------------------------
     * NON-PUBLIC FIELDS
     *-----------------------------------*/


    private AdsData m_Data;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Color Ambient {
        get {
            return Color.FromVector4(m_Data.Ambient);
        }
        
        set {
            m_Data.Ambient = value.ToVector4();
            Shader.SetConstants(m_Data);
        }
    }

    public Color Diffuse {
        get {
            return Color.FromVector4(m_Data.Diffuse);
        }
        
        set {
            m_Data.Diffuse = value.ToVector4();
            Shader.SetConstants(m_Data);
        }
    }

    public IShader Shader { get; }

    public Color Specular {
        get {
            return Color.FromVector4(m_Data.Specular);
        }
        
        set {
            m_Data.Specular = value.ToVector4();
            Shader.SetConstants(m_Data);
        }
    }

    public bool UseTexture {
        get {
            return m_Data.UseTexture;
        }

        set {
            m_Data.UseTexture = value;
        }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public AdsMaterial(IShaderMgr shaderMgr=null) {
        if (shaderMgr == null) {
            shaderMgr = Game.Inst.Graphics.ShaderMgr;
        }

        Shader = shaderMgr.LoadPS<AdsData>("src/Shaders/HLSL/AdsMaterial.ps.hlsl");
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/
}

}
