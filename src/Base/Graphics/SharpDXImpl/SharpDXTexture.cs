namespace PongBrain.Base.Graphics.SharpDXImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Textures;

using SharpDX.Direct3D;

using D3D11 = SharpDX.Direct3D11;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

internal class SharpDXTexture: IDisposable, ITexture {
    /*-------------------------------------
     * PUBLIC FIELDS
     *-----------------------------------*/

    public SharpDXGraphics Graphics;


    public D3D11.Texture2D Texture;

    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private D3D11.ShaderResourceView m_ShaderResource;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public int Height { get; private set; }

    public D3D11.ShaderResourceView ShaderResource {
        get {
            if (m_ShaderResource == null) {
                m_ShaderResource = CreateShaderResource();
            }

            return m_ShaderResource;
        }
    }

    public int Width { get; private set; }

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public SharpDXTexture(SharpDXGraphics graphics,
                          D3D11.Texture2D texture,
                          int             width,
                          int             height)
    {
        Graphics = graphics;
        Texture  = texture;
        
        Height = height;
        Width  = width;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Dispose() {
        if (m_ShaderResource != null) {
            m_ShaderResource.Dispose();
            m_ShaderResource = null;
        }

        if (Texture != null) {
            Texture.Dispose();
            Texture = null;
        }

        Graphics = null;

        Width  = 0;
        Height = 0;
    }

    /*-------------------------------------
     * PRIVATE
     *-----------------------------------*/

    private D3D11.ShaderResourceView CreateShaderResource() {
        var description = new D3D11.ShaderResourceViewDescription {
            Dimension = ShaderResourceViewDimension.Texture2D,
            Format    = Texture.Description.Format,
            Texture2D = new D3D11.ShaderResourceViewDescription.Texture2DResource {
                MipLevels       = 1,
                MostDetailedMip = 0
            },
        };

        return new D3D11.ShaderResourceView(Graphics.Device, Texture, description);
    }
}

}
