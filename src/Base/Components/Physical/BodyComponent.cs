namespace PongBrain.Base.Components.Physical {

/*-------------------------------------
 * CLASSES
 *-----------------------------------*/

public sealed class BodyComponent {
    /*-------------------------------------
     * PUBLIC PROPERTIES
     *-----------------------------------*/

    public float Friction    { get; set; } = 0.5f;
    public float LinearDrag  { get; set; } = 4.0f;
    public float Mass        { get; set; } = 1.0f;
    public float Restitution { get; set; } = 0.7f;
}

}
