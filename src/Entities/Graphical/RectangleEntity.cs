namespace PongBrain.Entities.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Components.Graphical;
using Components.Physical;
using Core;
using Graphics;

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
