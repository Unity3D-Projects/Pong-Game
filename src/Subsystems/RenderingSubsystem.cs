namespace PongBrain.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Drawing;
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
    private Graphics m_FormGraphics;
    private readonly Form m_Form;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Color ClearColor { get; set; } = Color.Black;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public RenderingSubsystem(Form form) {
        m_Form = form;
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Cleanup() {
        base.Cleanup();

        m_BackBuffer.Dispose();
        m_BackBufferGraphics.Dispose();
        m_FormGraphics.Dispose();
    }

    public override void Draw(float dt) {
        base.Draw(dt);

        var g = m_BackBufferGraphics;

        g.Clear(ClearColor);

        var width  = m_Form.ClientRectangle.Width;
        var height = m_Form.ClientRectangle.Height;
        var cx     = width  / 2.0f;
        var cy     = height / 2.0f;
        var scale  = 0.5f*Math.Max(width, height);

        foreach (var entity in Game.Inst.GetEntities<CircleComponent>()) {
            var r = 0.0f;
            var x = 0.0f;
            var y = 0.0f;

            var circle   = entity.GetComponent<CircleComponent>();
            var position = entity.GetComponent<PositionComponent>();

            if (circle != null) {
                r = circle.Radius;
            }

            if (position != null) {
                x = position.X;
                y = position.Y;
            }

            r *= scale;
            x *= scale;
            y *= -scale;
            
            x += cx;
            y += cy;

            g.FillEllipse(Brushes.White, x-r, y-r, 2.0f*r, 2.0f*r);
        }

        foreach (var entity in Game.Inst.GetEntities<RectangleComponent>()) {
            var w = 0.0f;
            var h = 0.0f;
            var x = 0.0f;
            var y = 0.0f;

            var rectangle = entity.GetComponent<RectangleComponent>();
            var position  = entity.GetComponent<PositionComponent>();

            if (rectangle != null) {
                w = rectangle.Width;
                h = rectangle.Height;
            }

            if (position != null) {
                x = position.X;
                y = position.Y;
            }

            w *= scale;
            h *= scale;
            x *= scale;
            y *= -scale;
            
            x += cx;
            y += cy;

            g.FillRectangle(Brushes.White, x-0.5f*w, y-0.5f*h, w, h);
        }

        Present();
    }

    public override void Init() {
        base.Init();

        var width  = m_Form.ClientRectangle.Width;
        var height = m_Form.ClientRectangle.Height;

        m_BackBuffer         = new Bitmap(width, height);
        m_BackBufferGraphics = Graphics.FromImage(m_BackBuffer);
        m_FormGraphics       = m_Form.CreateGraphics();
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private void Present() {
        m_FormGraphics.DrawImageUnscaled(m_BackBuffer, 0, 0);
    }
}

}
