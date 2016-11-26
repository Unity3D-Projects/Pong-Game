namespace Pong.Entities.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using PrimusGE.Components.Graphical;
using PrimusGE.Components.Physical;
using PrimusGE.Core;
using PrimusGE.Graphics;
using PrimusGE.Math;

using Components;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class RectangleEntity: Entity {
    /*-------------------------------------
     * FIELDS
     *-----------------------------------*/

    private static readonly ITriMesh s_Quad = Game.Inst.Graphics.TriMeshMgr.CreateQuad(1.0f, 1.0f);

    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public RectangleEntity(float x, float y, float width, float height) {
        AddComponents(
            new BodyComponent       { Position = new Vector2(x, y) },
            new MotionBlurComponent { },
            new TriMeshComponent    { Transform = Matrix4x4.Scale(width, height, 1.0f),
                                      TriMesh = s_Quad }
        );
    }
}

}
