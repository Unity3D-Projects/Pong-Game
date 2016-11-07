namespace PongBrain.Base.Graphics.GdiPlusImpl {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class GdiPlusGraphics: IGraphicsImpl {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private Bitmap m_BackBuffer;

    private Graphics m_BackBufferGraphics;

    private float m_HalfHeight;

    private float m_HalfWidth;

    private float m_Height;

    private float m_Width;

    private Form m_Window;

    private Graphics m_WindowGraphics;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public string Name {
        get { return "GDI+"; }
    }
    
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void BeginFrame() {
    }

    public void Cleanup() {
        m_BackBuffer.Dispose();
        m_BackBuffer = null;

        m_BackBufferGraphics.Dispose();
        m_BackBufferGraphics = null;

        m_WindowGraphics.Dispose();
        m_WindowGraphics = null;
    }

    public void Clear(Base.Graphics.Color clearColor) {
        var color = Color.FromArgb(clearColor.ToIntARGB());
        m_BackBufferGraphics.Clear(color);
    }

    public void DrawTexture(ITexture texture, Matrix4x4 transform) {
        var bmp = ((GdiPlusTexture)texture).m_Bitmap;
        var x   = m_HalfWidth  + m_HalfWidth  * transform.M41;
        var y   = m_HalfHeight - m_HalfHeight * transform.M42;
        var w   = m_Width  * transform.M11 / bmp.Width;
        var h   = m_Height * transform.M22 / bmp.Height;

        m_BackBufferGraphics.DrawImage(bmp, x - 0.5f*w, y - 0.5f*h, w, h);
    }

    public void EndFrame() {
        m_WindowGraphics.DrawImageUnscaled(m_BackBuffer, 0, 0);
    }

    public void Init(Form window) {
        m_Window = window;

        var width  = m_Window.ClientRectangle.Width;
        var height = m_Window.ClientRectangle.Height;

        m_BackBuffer         = new Bitmap(width, height);
        m_BackBufferGraphics = Graphics.FromImage(m_BackBuffer);
        m_WindowGraphics     = m_Window.CreateGraphics();
        
        var g1 = m_BackBufferGraphics;
        g1.CompositingQuality = CompositingQuality.HighSpeed;
        g1.InterpolationMode  = InterpolationMode.NearestNeighbor;
        g1.PixelOffsetMode    = PixelOffsetMode.HighSpeed;
        g1.SmoothingMode      = SmoothingMode.HighSpeed;

        var g2 = m_WindowGraphics;
        g2.CompositingQuality = CompositingQuality.HighSpeed;
        g2.InterpolationMode  = InterpolationMode.NearestNeighbor;
        g2.PixelOffsetMode    = PixelOffsetMode.HighSpeed;
        g2.SmoothingMode      = SmoothingMode.HighSpeed;

        m_Height     = m_Window.ClientRectangle.Height;
        m_Width      = m_Window.ClientRectangle.Width;
        m_HalfHeight = 0.5f*m_Height;
        m_HalfWidth  = 0.5f*m_Width;
    }
}

}
