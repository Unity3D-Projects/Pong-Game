namespace PongBrain.Base.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;

using Components.Graphical;
using Components.Physical;
using Core;
using Graphics;
using Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class RenderingSubsystem: Subsystem {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Color ClearColor { get; set; } = Color.Black;

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public RenderingSubsystem() {
    }

    /*-------------------------------------
     * PUBLIC METHODS
     *-----------------------------------*/

    public override void Cleanup() {
        base.Cleanup();
    }

    public override void Draw(float dt) {
        base.Draw(dt);

        Game.Inst.Graphics.Clear(ClearColor);

        var width = 640;// m_Window.ClientRectangle.Width;
        var height = 480;// m_Window.ClientRectangle.Height;
        var cx     = 0.5f*width;
        var cy     = 0.5f*height;
        var scale  = 0.5f*Math.Max(width, height);

        var entities = Game.Inst.Scene.GetEntities<SpriteComponent>();
        foreach (var entity in entities) {
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
           // var bmp = sprite.Texture.m_Bitmap;
            //g.DrawImage(bmp, x-0.5f*w, y-0.5f*h, 2.0f*w, 2.0f*h);
            var transform = Matrix33.Translate(x, y) * Matrix33.Scale(w, h);
            Game.Inst.Graphics.DrawTexture(sprite.Texture, transform);
        }

        foreach (var entity in Game.Inst.Scene.GetEntities<TextComponent>()) {
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
            //g.DrawString(text.Text(), font, Brushes.White, x, y);
        }

        Game.Inst.Graphics.SwapBuffers();
    }

    public override void Init() {
        base.Init();

        Game.Inst.Graphics.Init(Game.Inst.Window);
    }
}

}
