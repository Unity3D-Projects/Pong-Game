namespace PongBrain.Pong.Entities.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Components.Graphical;
using Base.Components.Physical;
using Base.Core;
using Base.Graphics;
using Base.Math;

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
            new PositionComponent   { X=x, Y=y },
            new MotionBlurComponent { },
            new TriMeshComponent    { Transform = Matrix4x4.Scale(width, height, 1.0f),
                                      TriMesh = s_Quad }
        );
    }
}

}
