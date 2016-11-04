namespace PongBrain.Components.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using System.Drawing;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class SpriteComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    
    public float ScaleX { get; set; } = 1.0f;

    public float ScaleY { get; set; } = 1.0f;

    public Bitmap Texture { get; set; }
}

}
