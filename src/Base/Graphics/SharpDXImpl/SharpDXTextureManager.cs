namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Drawing;
using SharpDX.Direct3D11;

using Textures;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal sealed class SharpDXTextureManager: ITextureManager {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private readonly Device m_Device;
    
    private SharpDXTexture m_White;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public ITexture White {
        get {
            if (m_White != null) {
                return m_White;
            }

            /*var bmp = new Bitmap(1, 1);
            bmp.SetPixel(0, 0, Color.White);

            var texDesc = new Texture2DDescription {
                ArraySize = 1,
                Format    = SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                MipLevels = 1,
                SampleDescription = new SharpDX.DXGI.SampleDescription(1, 0),
                Width  = bmp.Width,
                Height = bmp.Height
            };*/

            //var tex = new Texture2D(m_Device, texDesc);

            m_White = new SharpDXTexture(null, 1, 1);

            return m_White;
        }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTextureManager(Device device) {
        m_Device = device;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Cleanup() {
        // FIXME: Dont check if m_Texture is null
        if (m_White != null && m_White.Texture != null) {
            m_White.Texture.Dispose();
            m_White = null;
        }
    }

    public void Init() {
    }
}

}
