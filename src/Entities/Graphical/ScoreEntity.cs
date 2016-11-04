namespace PongBrain.Entities.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Drawing;

using Components.Graphical;
using Components.Physical;
using Core;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public class ScoreEntity: Entity {
    /*-------------------------------------
     * CONSTRUCTORS
     *-----------------------------------*/

    public ScoreEntity(float x, float y, Func<string> textFunc) {
        AddComponents(
            new TextComponent     { Font = new Font("Segoe UI", 22.0f, FontStyle.Bold),
                                    Text = textFunc },
            new PositionComponent { X=x, Y=y }
        );
    }
}

}
