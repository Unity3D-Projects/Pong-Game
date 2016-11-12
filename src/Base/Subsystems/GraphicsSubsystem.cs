namespace PongBrain.Base.Subsystems {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Collections.Generic;

using Components.Graphical;
using Components.Physical;
using Core;
using Graphics;
using Math;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class GraphicsSubsystem: Subsystem {
    /*-------------------------------------
     * PRIVATE FIELDS
     *-----------------------------------*/

    private ITriMesh m_Quad;

    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Color ClearColor { get; set; } = Color.Black;

    public Matrix4x4 ViewTransform { get; set; } = Matrix4x4.Identity();

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public GraphicsSubsystem() {
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
        g.RenderTarget.Clear(ClearColor);

        DrawTriMeshes(Game.Inst.Scene.GetEntities<TriMeshComponent>());
        DrawSprites(Game.Inst.Scene.GetEntities<SpriteComponent>());

        g.EndFrame();
    }

    public override void Init() {
        base.Init();

        m_Quad = Game.Inst.Graphics.TriMeshMgr.CreateQuad(1.0f, 1.0f);
    }

    /*-------------------------------------
     * PRIVATE METHODS
     *-----------------------------------*/

    private void DrawSprites(IEnumerable<Entity> entities) {
        var g = Game.Inst.Graphics;

        foreach (var entity in entities) {
            var sprite   = entity.GetComponent<SpriteComponent>();
            var position = entity.GetComponent<PositionComponent>();
            var rotation = entity.GetComponent<RotationComponent>();

            var a = 0.0f;
            var x = 0.0f;
            var y = 0.0f;
            var w = sprite.Texture.Width  * sprite.ScaleX;
            var h = sprite.Texture.Height * sprite.ScaleY;

            if (position != null) {
                x = position.X;
                y = position.Y;
            }

            if (rotation != null) {
                a = rotation.Angle;
            }

            var transform = ViewTransform
                          * Matrix4x4.Translate(x, y, sprite.LayerDepth)
                          * Matrix4x4.Scale    (w, h, 1.0f)
                          * Matrix4x4.RotateZ  (a);

            transform.Transpose();

            g.DrawTriMesh(m_Quad, transform);
        }
    }

    private void DrawTriMeshes(IEnumerable<Entity> entities) {
        var g = Game.Inst.Graphics;

        foreach (var entity in entities) {
            var triMesh  = entity.GetComponent<TriMeshComponent>();
            var position = entity.GetComponent<PositionComponent>();
            var rotation = entity.GetComponent<RotationComponent>();
            var shader   = entity.GetComponent<ShaderComponent>();

            var a = 0.0f;
            var x = 0.0f;
            var y = 0.0f;

            if (position != null) {
                x = position.X;
                y = position.Y;
            }

            if (rotation != null) {
                a = rotation.Angle;
            }

            var transform = ViewTransform
                          * Matrix4x4.Translate(x, y, 0.0f)
                          * Matrix4x4.RotateZ  (a);

            transform.Transpose();

            if (shader != null) {
                var oldPS = g.PixelShader;
                var oldVS = g.VertexShader;

                g.PixelShader  = shader.PixelShader;
                g.VertexShader = shader.VertexShader;

                g.DrawTriMesh(triMesh.TriMesh, transform);

                g.PixelShader  = oldPS;
                g.VertexShader = oldVS;
            }
            else {
                g.DrawTriMesh(triMesh.TriMesh, transform);
            }
        }
    }
}

}
