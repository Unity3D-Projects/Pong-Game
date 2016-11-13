namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Collections.Generic;

using Textures;

using SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal sealed class SharpDXTextureMgr: IDisposable, ITextureMgr {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private SharpDXGraphics m_Graphics;

    private List<SharpDXTexture> m_Textures = new List<SharpDXTexture>();
    
    private SharpDXTexture m_White;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public ITexture White {
        get {
            if (m_White == null) {
                m_White = CreateTexture(1, 1);

                var context = m_Graphics.Device.ImmediateContext;
                var data    = new byte[] { 255, 255, 255, 255 };

                context.UpdateSubresource(data, m_White.Texture);
            }

            return m_White;
        }
    }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTextureMgr(SharpDXGraphics graphics) {
        m_Graphics = graphics;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public SharpDXTexture CreateTexture(int width, int height) {
        var textureDescription = new Texture2DDescription {
            ArraySize         = 1,
            BindFlags         = BindFlags.ShaderResource,
            Format            = SharpDX.DXGI.Format.R16G16B16A16_Float,
            MipLevels         = 1,
            Width             = width,
            Height            = height
        };

        var device     = m_Graphics.Device;
        var gpuTexture = new Texture2D(device, textureDescription);
        var texture    = new SharpDXTexture(m_Graphics, gpuTexture, width, height);

        m_Textures.Add(texture);

        return texture;
    }

    public void Dispose() {
        foreach (var texture in m_Textures) {
            texture.Dispose();
        }

        m_Textures.Clear();
        m_Textures = null;

        m_White    = null;
        m_Graphics = null;
    }
}

}
