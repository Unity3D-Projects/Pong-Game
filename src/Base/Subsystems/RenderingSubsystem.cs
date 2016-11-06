namespace PongBrain.Base.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Components.Graphical;
using Components.Physical;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class RenderingSubsystem: Subsystem {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private Bitmap m_BackBuffer;
    private Graphics m_BackBufferGraphics;
    private Graphics m_WindowGraphics;
    private readonly Form m_Window;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Base.Graphics.Color ClearColor { get; set; } =
        Base.Graphics.Color.Black;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public RenderingSubsystem(Form window) {
        m_Window = window;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Cleanup() {
        base.Cleanup();

        m_BackBuffer.Dispose();
        m_BackBufferGraphics.Dispose();
        m_WindowGraphics.Dispose();
    }

    public override void Draw(float dt) {
        base.Draw(dt);

        var g = m_BackBufferGraphics;

        var clearColor = Color.FromArgb(ClearColor.ToInt());
        g.Clear(clearColor);

        var width  = m_Window.ClientRectangle.Width;
        var height = m_Window.ClientRectangle.Height;
        var cx     = 0.5f*width;
        var cy     = 0.5f*height;
        var scale  = 0.5f*Math.Max(width, height);

        foreach (var entity in Game.Inst.GetEntities<SpriteComponent>()) {
            var sprite   = entity.GetComponent<SpriteComponent>();
            var position = entity.GetComponent<PositionComponent>();

            var x = 0.0f;
            var y = 0.0f;
            var w = (float)sprite.Texture.Width;
            var h = (float)sprite.Texture.Height;

            if (position != null) {
                x = position.X;
                y = position.Y;
            }

            w *= scale * sprite.ScaleX;
            h *= scale * sprite.ScaleY;
            x *= scale;
            y *= -scale;
            
            x += cx;
            y += cy;

            x = (int)x;
            y = (int)y;
            w = (int)w;
            h = (int)h;

            // GDI interpolates source pixels to outside the texture by rounding
            // so multiply w and h by 2.... lmao
            var bmp = sprite.Texture.m_Bitmap;
            g.DrawImage(bmp, x-0.5f*w, y-0.5f*h, 2.0f*w, 2.0f*h);
        }

        foreach (var entity in Game.Inst.GetEntities<TextComponent>()) {
            var text     = entity.GetComponent<TextComponent>();
            var position = entity.GetComponent<PositionComponent>();

            var x = 0.0f;
            var y = 0.0f;

            if (position != null) {
                x = position.X;
                y = position.Y;
            }

            x *= scale;
            y *= -scale;
            
            x += cx;
            y += cy;

            x = (int)x;
            y = (int)y;

            var font = text.Font.m_Font;
            g.DrawString(text.Text(), font, Brushes.White, x, y);
        }

        Present();
    }

    public override void Init() {
        base.Init();

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

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private void Present() {
        m_WindowGraphics.DrawImageUnscaled(m_BackBuffer, 0, 0);
    }
}

}
