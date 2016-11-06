namespace PongBrain.Pong.Entities.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Base.Components.Graphical;
using Base.Components.Physical;
using Base.Core;
using Base.Graphics;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class RectangleEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public RectangleEntity(float x, float y, float width, float height) {
        AddComponents(
            new SpriteComponent   { ScaleX  = width,
                                    ScaleY  = height,
                                    Texture = Textures.White },
            new PositionComponent { X=x, Y=y }
        );
    }
}

}
