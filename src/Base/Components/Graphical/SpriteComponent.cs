namespace PongBrain.Base.Components.Graphical {

/*-------------------------------------
 * USINGS
 *-----------------------------------*/

using Graphics;

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class SpriteComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float ScaleX { get; set; } = 1.0f;

    public float ScaleY { get; set; } = 1.0f;

    public Texture Texture { get; set; }
}

}
