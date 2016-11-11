namespace PongBrain.Base.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

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

    public Matrix4x4 ViewTransform { get; set; } = Matrix4x4.Identity();

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

        var g = Game.Inst.Graphics;

        g.BeginFrame();
        g.Clear(ClearColor);

        DrawSprites(g);

        g.EndFrame();
    }

    public override void Init() {
        base.Init();
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private void DrawSprites(IGraphicsImpl g) {
        var entities = Game.Inst.Scene.GetEntities<SpriteComponent>();
        foreach (var entity in entities) {
            var sprite   = entity.GetComponent<SpriteComponent>();
            var position = entity.GetComponent<PositionComponent>();

            var x = 0.0f;
            var y = 0.0f;
            var w = sprite.Texture.Width  * sprite.ScaleX;
            var h = sprite.Texture.Height * sprite.ScaleY;

            if (position != null) {
                x = position.X;
                y = position.Y;
            }

            var transform = ViewTransform
                          * Matrix4x4.Translate(x, y, 0.0f)
                          * Matrix4x4.Scale    (w, h, 1.0f);

            transform.Transpose();
            g.DrawTexture(sprite.Texture, transform);
        }
    }
}

}
