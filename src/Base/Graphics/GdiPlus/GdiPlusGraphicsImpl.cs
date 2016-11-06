namespace PongBrain.Base.Graphics.GdiPlus {

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

public class GdiPlusGraphicsImpl: IGraphicsImpl {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private Bitmap m_BackBuffer;
    private Graphics m_BackBufferGraphics;
    private Graphics m_WindowGraphics;
    private Form m_Window;
    
    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public void Cleanup() {
        m_BackBuffer.Dispose();
        m_BackBufferGraphics.Dispose();
        m_WindowGraphics.Dispose();
    }

    public void Clear(Base.Graphics.Color clearColor) {
        var color = Color.FromArgb(clearColor.ToInt());
        m_BackBufferGraphics.Clear(color);
    }

    public void DrawTexture(ITexture texture, float x, float y) {
        var bmp = ((GdiPlusTexture)texture).m_Bitmap;

        m_BackBufferGraphics.DrawImage(bmp, x, y);
    }

    public void DrawTexture(ITexture texture, Matrix33 transform) {
        var bmp = ((GdiPlusTexture)texture).m_Bitmap;

        var tx = transform.X.Z;
        var ty = transform.Y.Z;
        var sx = transform.X.X;
        var sy = transform.Y.Y;
        var w  = 2.0f*sx*bmp.Width;
        var h  = 2.0f*sy*bmp.Height;

        m_BackBufferGraphics.DrawImage(bmp, tx - 0.5f*w, ty - 0.5f*h, w, h);
    }

    public void Init(Form window) {
        m_Window = window;

        var width  = m_Window.ClientRectangle.Width;
        var height = m_Window.ClientRectangle.Height;

        m_BackBuffer         = new Bitmap(width, height);
        m_BackBufferGraphics = Graphics.FromImage(m_BackBuffer);
        m_WindowGraphics     = m_Window.CreateGraphics();
        
        var g = m_BackBufferGraphics;
        g.CompositingQuality = CompositingQuality.HighSpeed;
        g.InterpolationMode  = InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode    = PixelOffsetMode.HighSpeed;
        g.SmoothingMode      = SmoothingMode.HighSpeed;

        g = m_WindowGraphics;
        g.CompositingQuality = CompositingQuality.HighSpeed;
        g.InterpolationMode  = InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode    = PixelOffsetMode.HighSpeed;
        g.SmoothingMode      = SmoothingMode.HighSpeed;
    }

    public void SwapBuffers() {
        m_WindowGraphics.DrawImageUnscaled(m_BackBuffer, 0, 0);
    }
}

}
