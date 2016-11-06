namespace PongBrain.Pong.Entities.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Drawing;

using Base.Components.Graphical;
using Base.Components.Physical;
using Base.Core;

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
