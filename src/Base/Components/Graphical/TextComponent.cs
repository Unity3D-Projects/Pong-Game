namespace PongBrain.Base.Components.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System;
using System.Drawing;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class TextComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public Font Font { get; set; } = SystemFonts.DefaultFont;

    public Func<string> Text { get; set; }
}

}
